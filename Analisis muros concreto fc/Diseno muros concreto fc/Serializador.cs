using Diseño_de_muros_concreto_V2;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    internal class Serializador
    {
        public static void Serializar(ref string ruta, Listas_Serializadas_i Lista_i)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (ruta == "")
            {
                SaveFileDialog Myfile = new SaveFileDialog
                {
                    Title = "Información Muros",
                    Filter = "Guardar Archivo |*.dmc"
                };

                Myfile.ShowDialog();
                ruta = Myfile.FileName;
            }

            Stream Escritor = new FileStream(ruta, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(Escritor, Lista_i);
            Escritor.Close();
        }

        public static void Deserializar(ref string Ruta_archivo, ref Listas_Serializadas_i Lista_i)
        {
         
            BinaryFormatter Formatter = new BinaryFormatter();
            OpenFileDialog Myfile = new OpenFileDialog
            {
                Filter = "Archivo de muros|*.dmc",
                Title = "Abrir archivo"
            };

            Myfile.ShowDialog();
            Ruta_archivo = Myfile.FileName;

            if (Ruta_archivo != "")
            {
                Stream Lector = new FileStream(Ruta_archivo, FileMode.Open, FileAccess.Read, FileShare.None);
            var aux = Formatter.Deserialize(Lector);
            Lector.Close();

            if (aux.GetType().Namespace == "Diseño_de_muros_concreto_V2")
            {
                var aux2 = (Listas_serializadas)aux;
                Lista_i = new Listas_Serializadas_i
                {
                    Lista_Muros = aux2.Lista_Muros,
                    Lista_Alzados = aux2.Lista_Alzados,
                    lista_refuerzo = aux2.lista_refuerzo,
                    Muros_generales = aux2.Muros_generales,
                    Capacidad_proyecto = aux2.Capacidad_proyecto
                };
            }
            else
            {
                Lista_i = (Listas_Serializadas_i)aux;
            }

            Convertir_Listas(Lista_i);
            }
        }

        private static void Convertir_Listas(Listas_Serializadas_i Lista_i)
        {
            foreach (Muros_Consolidados_1 prueba in (IEnumerable)Lista_i.Lista_Muros)
            {
                Listas_Programa.Muros_Consolidados_Listos.Add(prueba);
            }

            foreach (Muro prueba in (IEnumerable)Lista_i.Muros_generales)
            {
                Listas_Programa.Lista_Muros.Add(prueba);

                foreach (Shells_Prop prueba2 in prueba.Shells_Muro)
                {
                    Listas_Programa.Lista_shells.Add(prueba2);
                }
            }
        }
    }
}