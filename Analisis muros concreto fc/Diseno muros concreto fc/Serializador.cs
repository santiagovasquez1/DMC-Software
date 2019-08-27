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

        public static void Deserializar(string Ruta_archivo, ref Listas_Serializadas_i Lista_i)
        {
            BinaryFormatter Formatter = new BinaryFormatter();

            OpenFileDialog Myfile = new OpenFileDialog
            {
                Filter = "Archivo de muros|*.dmc",
                Title = "Abrir archivo"
            };

            Myfile.ShowDialog();
            Ruta_archivo = Myfile.FileName;

            Stream Lector = new FileStream(Ruta_archivo, FileMode.Open, FileAccess.Read, FileShare.None);
            Lista_i = (Listas_Serializadas_i)Formatter.Deserialize(Lector);
            Lector.Close();
        }
    }
}