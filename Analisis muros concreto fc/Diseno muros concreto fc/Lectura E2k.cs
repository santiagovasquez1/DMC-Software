using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public class Material
    {
        public string Nombre;
        public float Fc;
    }

    class Area_Assigns
    {
        public string Label, Pier, Story, Section;
    }

    public class Lectura_E2k
    {
        private static List<Area_Assigns> Lista_Areas_Aux;

        public static void Cargar_E2k()
        {

            OpenFileDialog Myfile = new OpenFileDialog();
            string sline;
            int Inicio, Fin, indice;
            List<string> Lineas_E2k = new List<string>();
            string[] Vector_Texto;
            StreamReader Lector;
            Puntos_Modelo Puntoi;
            Shells_Prop Shell_i;
            int pos;
            Area_Assigns Area_i;
            Material Material_i;

            Listas_Programa.Lista_Materiales = new List<Material>();
            Listas_Programa.Lista_Puntos = new List<Puntos_Modelo>();
            Listas_Programa.Lista_shells = new List<Shells_Prop>();
            Lista_Areas_Aux = new List<Area_Assigns>();

            Myfile.Filter = "Archivo de Etabs |*.$ET";
            Myfile.Title = "Abrir Modelo";
            Myfile.ShowDialog();

            if (Myfile.FileName != "")
            {
                try
                {
                    Lector = new StreamReader(Myfile.FileName);
                    do
                    {
                        sline = Lector.ReadLine();
                        Lineas_E2k.Add(sline);

                    } while (!(sline == null));

                    Lector.Close();

                    Inicio = Lineas_E2k.FindIndex(x => x.Contains("$ MATERIAL PROPERTIES")) + 1;
                    Fin = Lineas_E2k.FindIndex(x => x.Contains("$ REBAR DEFINITIONS")) - 1;

                    for (int i = Inicio; i < Fin; i++)
                    {
                        if (Lineas_E2k[i].Contains("FC") == true)
                        {
                            Vector_Texto = Lineas_E2k[i].Split();
                            if (Vector_Texto.Length == 10)
                            {
                                Material_i = new Material
                                {
                                    Nombre = Texto_sub(Vector_Texto, 4, 34),
                                    Fc = Convert.ToSingle(Vector_Texto[9])
                                };
                                Listas_Programa.Lista_Materiales.Add(Material_i);
                            }
                        }

                    }

                    Inicio = Lineas_E2k.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
                    Fin = Lineas_E2k.FindIndex(x => x.Contains("$ LINE CONNECTIVITIES")) - 1;

                    for (int i = Inicio; i < Fin; i++)
                    {

                        Vector_Texto = Lineas_E2k[i].Split();
                        pos = Vector_Texto[3].LastIndexOf((char)34);
                        Puntoi = new Puntos_Modelo
                        {
                            Label = Vector_Texto[3].Substring(1, pos - 1),
                            Xc = Convert.ToDouble(Vector_Texto[5]),
                            Yc = Convert.ToDouble(Vector_Texto[6])
                        };
                        Listas_Programa.Lista_Puntos.Add(Puntoi);
                    }

                    Inicio = Lineas_E2k.FindIndex(x => x.Contains("$ AREA ASSIGNS")) + 1;
                    Fin = Lineas_E2k.FindIndex(x => x.Contains("$ LOAD PATTERNS")) - 1;

                    for (int i = Inicio; i < Fin; i++)
                    {
                        if (Lineas_E2k[i].Contains("PIER") == true)
                        {
                            Vector_Texto = Lineas_E2k[i].Split();
                            Area_i = new Area_Assigns

                            {
                                Label = Texto_sub(Vector_Texto, 4, 34),
                                Story = Texto_sub(Vector_Texto, 6, 34),
                                Section = Texto_sub(Vector_Texto, 9, 34),
                                Pier = Texto_sub(Vector_Texto, 13, 34)
                            };
                            Lista_Areas_Aux.Add(Area_i);
                        }
                    }

                    Inicio = Lineas_E2k.FindIndex(x => x.Contains("$ AREA CONNECTIVITIES")) + 1;
                    try
                    {
                        Fin = Lineas_E2k.FindIndex(x => x.Contains("$ GROUPS")) - 1;
                    }
                    catch
                    {
                        Fin = Lineas_E2k.FindIndex(x => x.Contains("$ POINT ASSIGNS")) - 1;
                    }

                    for (int i = Inicio; i < Fin; i++)
                    {
                        Vector_Texto = Lineas_E2k[i].Split();
                        if (Vector_Texto[5].Contains("PANEL") == true)
                        {

                            List<Area_Assigns> Aux = Lista_Areas_Aux.FindAll(x => x.Label == Texto_sub(Vector_Texto, 3, 34));

                            for (int j = 0; j < Aux.Count; j++)
                            {
                                Shell_i = new Shells_Prop
                                {
                                    Label = Texto_sub(Vector_Texto, 3, 34),
                                    Puntos = new List<string> { Texto_sub(Vector_Texto, 9, 34), Texto_sub(Vector_Texto, 11, 34), Texto_sub(Vector_Texto, 13, 34), Texto_sub(Vector_Texto, 15, 34) },
                                    Coord = Extraccion_Coord(new List<string> { Texto_sub(Vector_Texto, 9, 34), Texto_sub(Vector_Texto, 11, 34), Texto_sub(Vector_Texto, 13, 34), Texto_sub(Vector_Texto, 15, 34) }),
                                    Pier = Aux[j].Pier,
                                    section = Aux[j].Section,
                                    Story = Aux[j].Story
                                };
                                Listas_Programa.Lista_shells.Add(Shell_i);
                            }

                        }
                    }

                }
                catch (FileNotFoundException e)
                {
                    e.Message.ToString();
                }


            }


        }

        private static string Texto_sub(string[] vector_texto, int indice, int Caracter)
        {
            int Pos;
            string Texto;
            Pos = vector_texto[indice].LastIndexOf((char)Caracter);
            Texto = vector_texto[indice].Substring(1, Pos - 1);

            return Texto;
        }

        private static List<List<double>> Extraccion_Coord(List<string> Puntos_Shell)
        {
            int Indice;
            List<List<double>> Lista_coord = new List<List<double>>();

            foreach (string Punto_i in Puntos_Shell)
            {
                Indice = Listas_Programa.Lista_Puntos.FindIndex(x => x.Label == Punto_i);
                Lista_coord.Add(new List<double> { Listas_Programa.Lista_Puntos[Indice].Xc, Listas_Programa.Lista_Puntos[Indice].Yc, Listas_Programa.Lista_Puntos[Indice].Zc });
            }
            return Lista_coord;
        }
    }


}
