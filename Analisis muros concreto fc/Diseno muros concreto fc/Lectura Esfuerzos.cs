using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public class Esfuerzos
    {
        public string Label, Story, Load;
        public double S22;

    }

    class Lectura_Esfuerzos
    {

        public static void Cargar_Esfuerzos()
        {

            OpenFileDialog Myfile = new OpenFileDialog();
            string sline;
            StreamReader Lector;
            List<string> Lineas_CSV = new List<string>();
            string[] Vector_Texto;

            Esfuerzos Esfuerzo_i;
            Listas_Programa.Lista_Esfuerzos = new List<Esfuerzos>();

            Myfile.Filter = "Esfuerzos en muros |*.CSV";
            Myfile.Title = "Abrir archivo con esfuerzos en muros";
            Myfile.ShowDialog();

            try
            {
                Lector = new StreamReader(Myfile.FileName);

                do
                {
                    sline = Lector.ReadLine();
                    Lineas_CSV.Add(sline);

                } while (!(sline == null));
                Lector.Close();

                for (int i = 3; i < Lineas_CSV.Count - 1; i++)
                {
                    if (Lineas_CSV[i].Contains("Wall") == true)
                    {
                        Vector_Texto = Lineas_CSV[i].Split(';');
                        Esfuerzo_i = new Esfuerzos
                        {
                            Label = Vector_Texto[1],
                            Story = Vector_Texto[0],
                            Load = Vector_Texto[6],
                            S22 = Convert.ToDouble(Vector_Texto[14])
                        };
                        Listas_Programa.Lista_Esfuerzos.Add(Esfuerzo_i);
                    }

                }

            }
            catch
            {

            }
        }

        public static void Enviar_a_Shells(List<Shells_Prop> Shells_piso)
        {
            int Pos;
            for (int i = 0; i < Shells_piso.Count; i++)
            {
                List<Esfuerzos> Esf_Aux_i = Listas_Programa.Lista_Esfuerzos.FindAll(x => x.Label == Shells_piso[i].Label & x.Story == Shells_piso[i].Story);
                Shells_piso[i].S22 = Esf_Aux_i.Min(x => x.S22);
                Pos = Esf_Aux_i.FindIndex(x => x.S22 == Shells_piso[i].S22);
                Shells_piso[i].Loads = Esf_Aux_i[Pos].Load;
            }

        }

    }
}
