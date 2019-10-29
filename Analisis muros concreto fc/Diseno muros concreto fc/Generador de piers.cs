using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public class Geometria_Piers
    {
        public string Pier, Story;
        public float bw, lw, hw, Fc, h_acumulado;
    }

    public class Fuerza_Piers
    {
        public string Pier, Story, Load, Loc;
        public double P, V2, V3, M2, M3;
    }

    public class Datos_Diseno
    {
        public string Pier, Story, Loc, Pier_Leg;
        public double Spacing, Rho_l;
    }

    internal class Generador_de_piers
    {
        private static List<Geometria_Piers> Lista_Geometria;
        private static List<Fuerza_Piers> Datos_Fuerza;
        private static List<Datos_Diseno> Lista_Diseno;

        public static void Lector_Geometria()
        {
            OpenFileDialog Myfile = new OpenFileDialog();
            string sline;
            StreamReader Lector;
            List<string> Lineas_Geometria = new List<string>();
            Geometria_Piers Geometria_i;
            string[] Vector_Texto;

            Lista_Geometria = new List<Geometria_Piers>();
            Myfile.Filter = "Datos de geometria muros |*.CSV";
            Myfile.Title = "Abrir archivo con la geometria de los muros";
            Myfile.ShowDialog();

            try
            {
                Lector = new StreamReader(Myfile.FileName);

                do
                {
                    sline = Lector.ReadLine();
                    Lineas_Geometria.Add(sline);
                } while (!(sline == null));
                Lector.Close();

                for (int i = 3; i < Lineas_Geometria.Count - 1; i++)
                {
                    Vector_Texto = Lineas_Geometria[i].Split(';');

                    Geometria_i = new Geometria_Piers
                    {
                        Story = Vector_Texto[0],
                        Pier = Vector_Texto[1],
                        lw = Convert.ToSingle(Vector_Texto[5]),
                        bw = Convert.ToSingle(Vector_Texto[6]),
                        Fc = Listas_Programa.Lista_Materiales[Listas_Programa.Lista_Materiales.FindIndex(x => x.Nombre == Vector_Texto[9])].Fc,
                        hw = Convert.ToSingle(Vector_Texto[15]) - Convert.ToSingle(Vector_Texto[12]),
                        h_acumulado = Convert.ToSingle(Vector_Texto[15])
                    };
                    Lista_Geometria.Add(Geometria_i);
                }
            }
            catch { }
        }

        public static void Lector_Fuerza()
        {
            OpenFileDialog Myfile = new OpenFileDialog();
            string sline;
            StreamReader Lector;
            List<string> Lineas_Fuerza = new List<string>();
            Fuerza_Piers Fuerza_i;
            string[] Vector_Texto;

            Datos_Fuerza = new List<Fuerza_Piers>();
            Myfile.Filter = "Datos de fuerza de muros |*.CSV";
            Myfile.Title = "Abrir archivo con la fuerza de los muros";
            Myfile.ShowDialog();

            try
            {
                Lector = new StreamReader(Myfile.FileName);
                do
                {
                    sline = Lector.ReadLine();
                    Lineas_Fuerza.Add(sline);
                } while (!(sline == null));
                Lector.Close();

                for (int i = 3; i < Lineas_Fuerza.Count - 1; i++)
                {
                    Vector_Texto = Lineas_Fuerza[i].Split(';');
                    Fuerza_i = new Fuerza_Piers
                    {
                        Story = Vector_Texto[0],
                        Pier = Vector_Texto[1],
                        Load = Vector_Texto[2],
                        Loc = Vector_Texto[3],
                        P = Convert.ToDouble(Vector_Texto[4]),
                        V2 = Math.Abs(Convert.ToDouble(Vector_Texto[5])),
                        V3 = Math.Abs(Convert.ToDouble(Vector_Texto[6])),
                        M2 = Math.Abs(Convert.ToDouble(Vector_Texto[8])),
                        M3 = Math.Abs(Convert.ToDouble(Vector_Texto[9]))
                    };
                    Datos_Fuerza.Add(Fuerza_i);
                }
            }
            catch { }
        }

        public static void Lectura_Diseno()
        {
            OpenFileDialog Myfile = new OpenFileDialog();
            string sline;
            StreamReader Lector;
            List<string> Lineas_diseno = new List<string>();
            string[] Vector_Texto;
            Datos_Diseno diseno_i;

            Lista_Diseno = new List<Datos_Diseno>();
            Myfile.Filter = "Datos de diseño a flexo-compresión de etabs |*.CSV";
            Myfile.Title = "Abrir archivo con el diseño a flexo-compresión de los muros";
            Myfile.ShowDialog();

            try
            {
                Lector = new StreamReader(Myfile.FileName);
                do
                {
                    sline = Lector.ReadLine();
                    Lineas_diseno.Add(sline);
                } while (!(sline == null));
                Lector.Close();

                for (int i = 3; i < Lineas_diseno.Count - 1; i++)
                {
                    Vector_Texto = Lineas_diseno[i].Split(';');
                    diseno_i = new Datos_Diseno
                    {
                        Story = Vector_Texto[0],
                        Pier = Vector_Texto[1],
                        Loc = Vector_Texto[2],
                        Spacing = Convert.ToDouble(Vector_Texto[6]) * 100,
                        Rho_l = Convert.ToDouble(Vector_Texto[7]) / 100,
                        Pier_Leg = Vector_Texto[9]
                    };

                    Lista_Diseno.Add(diseno_i);
                }
            }
            catch { }
        }

        public static void Recopilar_info()
        {
            Muro Muro_i;
            Listas_Programa.Lista_Muros = new List<Muro>();
            for (int i = 0; i < Lista_Geometria.Count; i++)
            {
                Muro_i = new Muro
                {
                    Pier = Lista_Geometria[i].Pier,
                    Story = Lista_Geometria[i].Story,
                    bw = Lista_Geometria[i].bw * 100,
                    lw = Lista_Geometria[i].lw * 100,
                    dw = 0.80f * Lista_Geometria[i].lw * 100,
                    hw = Lista_Geometria[i].hw * 100,
                    h_acumulado = Lista_Geometria[i].h_acumulado * 100,
                    Fc = Lista_Geometria[i].Fc / 10
                };


                List<Fuerza_Piers> Aux_fuerzas = Datos_Fuerza.FindAll(x => x.Pier == Muro_i.Pier & x.Story == Muro_i.Story);
                for (int j = 0; j < Aux_fuerzas.Count; j++)
                {
                    Muro_i.Load.Add(Aux_fuerzas[j].Load);
                    Muro_i.Loc.Add(Aux_fuerzas[j].Loc);
                    Muro_i.P.Add(Aux_fuerzas[j].P);
                    Muro_i.V2.Add(Aux_fuerzas[j].V2);
                    Muro_i.V3.Add(Aux_fuerzas[j].V3);
                    Muro_i.M2.Add(Aux_fuerzas[j].M2);
                    Muro_i.M3.Add(Aux_fuerzas[j].M3);
                }
                Muro_i.Calc_pc();

                Muro_i.Shells_Muro = Listas_Programa.Lista_shells.FindAll(x => x.Pier == Muro_i.Pier & x.Story == Muro_i.Story);
                List<Datos_Diseno> Aux_2 = Lista_Diseno.FindAll(x => x.Pier == Muro_i.Pier & x.Story == Muro_i.Story);
                Muro_i.Rho_l_Inicial = Aux_2.Max(x => x.Rho_l);
                Muro_i.Spacing = Aux_2.Max(x => x.Spacing);
                Listas_Programa.Lista_Muros.Add(Muro_i);
            }
        }
    }
}