using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    class Cargar_archivo
    {
        private static List<string> Lista_texto;

        public static void Cargar_Lista_Texto()
        {
            string sline;
            Lista_texto = new List<string>();
            OpenFileDialog Myfile = new OpenFileDialog
            {
                Filter = "Archivo de muros de concreto|*.dmc",
                Title = "Abrir archivo"
            };
            Myfile.ShowDialog();
            Listas_Programa.Ruta_archivo = Myfile.FileName;
            try
            {

                int FinPunto = 0;
                int FinSla = 0;
                string Ruta_Carpeta = "";

                for (int n = 0; n < (Myfile.FileName.Length); n++) { if (Myfile.FileName.Substring(n, 1) == ".") { FinPunto = n; } }
                for (int n = FinPunto; n >= 0; n--) { if (Myfile.FileName.Substring(n, 1) == @"\") { FinSla = n; break; } }
                for (int n = 0; n < FinSla; n++) { Ruta_Carpeta = Ruta_Carpeta + (Myfile.FileName.Substring(n, 1)); }

                Listas_Programa.Ruta_Carpeta = Ruta_Carpeta;
            }
            catch
            {


            }




            try
            {
                StreamReader Lector = new StreamReader(Myfile.FileName);
                do
                {
                    sline = Lector.ReadLine();
                    Lista_texto.Add(sline);

                } while (!(sline == null));

                Lector.Close();
                Cargar_Capacidad();
                Generar_muros();
                Cargar_Fuerzas();
                Cargar_Cortante();
                Cargar_flexural_Stress();
                Cargar_resumen();
            }
            catch
            {

            }
        }
        private static void Cargar_Capacidad()
        {
            int inicio, Fin;
            inicio = Lista_texto.FindIndex(x => x.Contains("0.Capacidad")) + 2;
            Fin = Lista_texto.FindIndex(x => x.Contains("1.Wall Forces")) - 2;
            string[] Vector_Texto = null;

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');

            }
            Listas_Programa.Capacidad = Vector_Texto[0];

        }

        private static void Generar_muros()
        {
            int inicio, Fin;
            string[] Vector_Texto;
            Muro Muro_i;

            inicio = Lista_texto.FindIndex(x => x.Contains("2.Wall Geometry")) + 2;
            Fin = Lista_texto.FindIndex(x => x.Contains("3.Shear Design")) - 2;

            if (Listas_Programa.Lista_Muros != null)
            {
                Listas_Programa.Lista_Muros.Clear();
            }
            else
            {
                Listas_Programa.Lista_Muros = new List<Muro>();
            }

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');
                Muro_i = new Muro
                {
                    Story = Vector_Texto[0],
                    Pier = Vector_Texto[1],
                    lw = Convert.ToSingle(Vector_Texto[2]),
                    bw = Convert.ToSingle(Vector_Texto[3]),
                    Fc = Convert.ToSingle(Vector_Texto[4]),
                    hw = Convert.ToSingle(Vector_Texto[5]),
                    h_acumulado = Convert.ToSingle(Vector_Texto[6]),
                    Rho_l_Inicial = Convert.ToDouble(Vector_Texto[7]),
                    Rho_l_Def = Convert.ToDouble(Vector_Texto[7])
                };
                Listas_Programa.Lista_Muros.Add(Muro_i);
            }


        }

        private static void Cargar_Fuerzas()
        {
            int inicio, Fin, indice;
            string[] Vector_Texto;

            inicio = Lista_texto.FindIndex(x => x.Contains("1.Wall Forces")) + 2;
            Fin = Lista_texto.FindIndex(x => x.Contains("2.Wall Geometry")) - 2;

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');
                indice = Listas_Programa.Lista_Muros.FindIndex(x => x.Story == Vector_Texto[0] & x.Pier == Vector_Texto[1]);
                Listas_Programa.Lista_Muros[indice].Load.Add(Vector_Texto[2]);
                Listas_Programa.Lista_Muros[indice].Loc.Add(Vector_Texto[3]);
                Listas_Programa.Lista_Muros[indice].P.Add(Convert.ToDouble(Vector_Texto[4]));
                Listas_Programa.Lista_Muros[indice].V2.Add(Convert.ToDouble(Vector_Texto[5]));
                Listas_Programa.Lista_Muros[indice].V3.Add(Convert.ToDouble(Vector_Texto[6]));
                Listas_Programa.Lista_Muros[indice].M2.Add(Convert.ToDouble(Vector_Texto[7]));
                Listas_Programa.Lista_Muros[indice].M3.Add(Convert.ToDouble(Vector_Texto[8]));
            }
        }

        private static void Cargar_Cortante()
        {
            int inicio, Fin, indice;
            string[] Vector_Texto;

            inicio = Lista_texto.FindIndex(x => x.Contains("3.Shear Design")) + 2;
            Fin = Lista_texto.FindIndex(x => x.Contains("4.Flexural Stress")) - 2;

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');
                indice = Listas_Programa.Lista_Muros.FindIndex(x => x.Story == Vector_Texto[0] & x.Pier == Vector_Texto[1]);

                if (Listas_Programa.Lista_Muros[indice].Phi_Vc == null)
                {
                    Listas_Programa.Lista_Muros[indice].Phi_Vc = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Phi_Vn_Max2 = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Phi_Vs = new List<double>();
                    Listas_Programa.Lista_Muros[indice].pt_definitivo = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Cortinas = new List<int>();
                    Listas_Programa.Lista_Muros[indice].Error_Cortante = new List<string>();
                }

                Listas_Programa.Lista_Muros[indice].Phi_Vc.Add(Convert.ToDouble(Vector_Texto[10]));
                Listas_Programa.Lista_Muros[indice].Phi_Vn_Max1 = Convert.ToDouble(Vector_Texto[11]);
                Listas_Programa.Lista_Muros[indice].Phi_Vn_Max2.Add(Convert.ToDouble(Vector_Texto[12]));
                Listas_Programa.Lista_Muros[indice].Phi_Vs.Add(Convert.ToDouble(Vector_Texto[13]));
                Listas_Programa.Lista_Muros[indice].Phi_Vs_Max = Convert.ToDouble(Vector_Texto[14]);
                Listas_Programa.Lista_Muros[indice].Pt_max = Convert.ToDouble(Vector_Texto[15]);
                Listas_Programa.Lista_Muros[indice].pt_definitivo.Add(Convert.ToDouble(Vector_Texto[16]));
                Listas_Programa.Lista_Muros[indice].Cortinas.Add(Convert.ToInt16(Vector_Texto[18]));
                Listas_Programa.Lista_Muros[indice].Error_Cortante.Add(Vector_Texto[19]);

            }
        }

        private static void Cargar_flexural_Stress()
        {
            int inicio, Fin, indice;
            string[] Vector_Texto;

            inicio = Lista_texto.FindIndex(x => x.Contains("4.Flexural Stress")) + 2;
            Fin = Lista_texto.FindIndex(x => x.Contains("5.Reporte")) - 2;

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');
                indice = Listas_Programa.Lista_Muros.FindIndex(x => x.Story == Vector_Texto[0] & x.Pier == Vector_Texto[1]);

                if (Listas_Programa.Lista_Muros[indice].Fa == null)
                {
                    Listas_Programa.Lista_Muros[indice].Fa = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Fv = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Sigma_Max = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Sigma_Min = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Relacion = new List<double>();
                    Listas_Programa.Lista_Muros[indice].C_def = new List<double>();
                    Listas_Programa.Lista_Muros[indice].L_Conf = new List<double>();
                    Listas_Programa.Lista_Muros[indice].Error_Flexion = new List<string>();

                }

                Listas_Programa.Lista_Muros[indice].Fa.Add(Convert.ToDouble(Vector_Texto[9]));
                Listas_Programa.Lista_Muros[indice].Fv.Add(Convert.ToDouble(Vector_Texto[10]));
                Listas_Programa.Lista_Muros[indice].Sigma_Max.Add(Convert.ToDouble(Vector_Texto[11]));
                Listas_Programa.Lista_Muros[indice].Sigma_Min.Add(Convert.ToDouble(Vector_Texto[12]));
                Listas_Programa.Lista_Muros[indice].Relacion.Add(Convert.ToDouble(Vector_Texto[13]));
                Listas_Programa.Lista_Muros[indice].C_def.Add(Convert.ToDouble(Vector_Texto[14]));
                Listas_Programa.Lista_Muros[indice].L_Conf.Add(Convert.ToDouble(Vector_Texto[15]));
                Listas_Programa.Lista_Muros[indice].Error_Flexion.Add(Vector_Texto[16]);

            }

        }

        private static void Cargar_resumen()
        {
            int inicio, Fin;
            string[] Vector_Texto;
            List<List<string>> Auxiliar = new List<List<string>>();
            List<string> Muros_distintos;
            Muros_Consolidados Muro_i;

            inicio = Lista_texto.FindIndex(x => x.Contains("5.Reporte")) + 2;

            try
            {
                Fin = Lista_texto.FindIndex(x => x.Contains("6.Datos de Refuerzo Adicional")) - 2;
            }
            catch
            {
                Fin = Lista_texto.FindIndex(x => x.Contains("Fin")) - 2;
            }

            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Listas_Programa.Muros_Consolidados_Listos.Clear();
            }
            else
            {
                Listas_Programa.Muros_Consolidados_Listos = new List<Muros_Consolidados>();
            }

            for (int i = inicio; i <= Fin; i++)
            {
                Vector_Texto = Lista_texto[i].Split('\t');
                Auxiliar.Add(Vector_Texto.ToList());
            }

            Muros_distintos = Auxiliar.Select(x => x[1]).Distinct().ToList();

            for (int i = 0; i < Muros_distintos.Count; i++)
            {
                List<List<string>> Auxiliar_2 = Auxiliar.FindAll(x => x[1] == Muros_distintos[i]).ToList();

                Muro_i = new Muros_Consolidados();
                Muro_i.Pier_name = Muros_distintos[i];
                Muro_i.Stories.AddRange(Auxiliar_2.Select(x => x[0]));
                Muro_i.lw.AddRange(Auxiliar_2.Select(x => Convert.ToSingle(x[2])));
                Muro_i.Bw.AddRange(Auxiliar_2.Select(x => Convert.ToSingle(x[3])));
                Muro_i.fc.AddRange(Auxiliar_2.Select(x => Convert.ToSingle(x[4])));
                Muro_i.Rho_T.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[5])));
                Muro_i.Rho_l.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[6])));
                Muro_i.Malla.AddRange(Auxiliar_2.Select(x => x[7]));
                Muro_i.C_Def.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[8])));
                Muro_i.Lebe_Izq.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[9])));
                Muro_i.Lebe_Der.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[10])));
                Muro_i.Lebe_Centro.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[11])));
                Muro_i.Zc_Izq.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[12])));
                Muro_i.Zc_Der.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[13])));
                Muro_i.Hw.AddRange(Auxiliar_2.Select(x => Convert.ToSingle(x[14])));
                Muro_i.Est_ebe.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[15])));
                Muro_i.Sep_ebe.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[16])));
                Muro_i.Est_Zc.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[17])));
                Muro_i.Sep_Zc.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[18])));
                Muro_i.As_Long.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[19])));
                Muro_i.ramas_izq.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[20])));
                Muro_i.ramas_der.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[21])));
                Muro_i.ramas_centro.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[22])));
                Muro_i.As_htal.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[23])));
                Muro_i.Ref_htal.AddRange(Auxiliar_2.Select(x => x[24]));
                Muro_i.Capas_htal.AddRange(Auxiliar_2.Select(x => Convert.ToInt32(x[25])));
                Muro_i.sep_htal.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[26])));
                Muro_i.As_Htal_Total.AddRange(Auxiliar_2.Select(x => Convert.ToDouble(x[27])));

                Listas_Programa.Muros_Consolidados_Listos.Add(Muro_i);
            }

        }
    }
}
