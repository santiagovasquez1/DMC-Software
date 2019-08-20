using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    class Guardar_archivo
    {
        private static List<string> Lista_texto;

        public static void Generar_texto()
        {

            try
            {
                StreamWriter escritor = new StreamWriter(Listas_Programa.Ruta_archivo);

                //Almacenar la capacidad de disipacion de energia
                escritor.WriteLine("0.Capacidad");
                escritor.WriteLine("");
                escritor.WriteLine(Listas_Programa.Capacidad);
                escritor.WriteLine("");

                //Escritor de pier forces
                escritor.WriteLine("1.Wall Forces");
                escritor.WriteLine("");
                Texto_Pier_Forces();

                for (int i = 0; i < Lista_texto.Count; i++)
                {
                    escritor.WriteLine(Lista_texto[i]);
                }

                //Escritor Geometria
                escritor.WriteLine("");
                escritor.WriteLine("2.Wall Geometry");
                escritor.WriteLine("");
                Texto_Geometria();
                for (int i = 0; i < Lista_texto.Count; i++)
                {
                    escritor.WriteLine(Lista_texto[i]);
                }

                //Escritor diseño a cortante
                escritor.WriteLine("");
                escritor.WriteLine("3.Shear Design");
                escritor.WriteLine("");
                Texto_shear_design();

                for (int i = 0; i < Lista_texto.Count; i++)
                {
                    escritor.WriteLine(Lista_texto[i]);
                }

                //Escritor flexural Stress
                escritor.WriteLine("");
                escritor.WriteLine("4.Flexural Stress");
                escritor.WriteLine("");
                Texto_Flexural_Stress();

                for (int i = 0; i < Lista_texto.Count; i++)
                {
                    escritor.WriteLine(Lista_texto[i]);
                }

                //Escritor reporte de diseno
                escritor.WriteLine("");
                escritor.WriteLine("5.Reporte");
                escritor.WriteLine("");
                Texto_reporte();

                for (int i = 0; i < Lista_texto.Count; i++)
                {
                    escritor.WriteLine(Lista_texto[i]);
                }
                escritor.WriteLine("");
                escritor.WriteLine("Fin");
                escritor.Close();
            }
            catch
            {

            }
        }

        public static void Crear_Archivo_Texto()
        {
            string Ruta_archivo;
            SaveFileDialog Guardar_archivo = new SaveFileDialog
            {
                Title = "Guardar archivo de diseño de muros de concreto",
                Filter = "Guardar Archivo |*.dmc"
            };
            Guardar_archivo.ShowDialog();

            Ruta_archivo = Guardar_archivo.FileName;

            Listas_Programa.Ruta_archivo = Ruta_archivo;
            try
            {

                int FinPunto = 0;
                int FinSla = 0;
                string Ruta_Carpeta = "";

                for (int n = 0; n < (Guardar_archivo.FileName.Length); n++) { if (Guardar_archivo.FileName.Substring(n, 1) == ".") { FinPunto = n; } }
                for (int n = FinPunto; n >= 0; n--) { if (Guardar_archivo.FileName.Substring(n, 1) == @"\") { FinSla = n; break; } }
                for (int n = 0; n < FinSla; n++) { Ruta_Carpeta = Ruta_Carpeta + (Guardar_archivo.FileName.Substring(n, 1)); }

                Listas_Programa.Ruta_Carpeta = Ruta_Carpeta;
            }
            catch
            {


            }
        }

        private static void Texto_Pier_Forces()
        {
            Lista_texto = new List<string>();
            string Texto;

            if (Listas_Programa.Lista_Muros is null == false)
            {
                foreach (Muro Muro_i in Listas_Programa.Lista_Muros)
                {
                    for (int j = 0; j < Muro_i.P.Count; j++)
                    {
                        Texto = Muro_i.Story + "\t" + Muro_i.Pier + "\t" + Muro_i.Load[j] + "\t" + Muro_i.Loc[j]
                        + "\t" + Muro_i.P[j] + "\t" + Muro_i.V2[j] + "\t" + Muro_i.V3[j] + "\t" + Muro_i.M2[j] + "\t" + Muro_i.M3[j];
                        Lista_texto.Add(Texto);
                    }
                }

            }


        }

        private static void Texto_Geometria()
        {
            Lista_texto = new List<string>();
            string Texto;

            if (Listas_Programa.Lista_Muros is null == false)
            {
                foreach (Muro Muro_i in Listas_Programa.Lista_Muros)
                {
                    Texto = Muro_i.Story + "\t" + Muro_i.Pier + "\t" + Muro_i.lw + "\t" + Muro_i.bw + "\t" + Muro_i.Fc
                        + "\t" + Muro_i.hw + "\t" + Muro_i.h_acumulado + "\t" + Muro_i.Rho_l_Def;
                    Lista_texto.Add(Texto);
                }
            }
        }

        private static void Texto_shear_design()
        {
            Lista_texto = new List<string>();
            string Texto;

            if (Listas_Programa.Lista_Muros is null == false)
            {
                foreach (Muro Muro_i in Listas_Programa.Lista_Muros)
                {
                    for (int j = 0; j < Muro_i.P.Count; j++)
                    {
                        Texto = Muro_i.Story + "\t" + Muro_i.Pier + "\t" + Muro_i.lw + "\t" + Muro_i.bw + "\t" + Muro_i.Fc + "\t" + Muro_i.h_acumulado + "\t"
                            + Muro_i.Load[j] + "\t" + Muro_i.P[j] + "\t" + Muro_i.V2[j] + "\t" + Muro_i.M3[j] + "\t" + Muro_i.Phi_Vc[j] + "\t"
                            + Muro_i.Phi_Vn_Max1 + "\t" + Muro_i.Phi_Vn_Max2[j] + "\t" + Muro_i.Phi_Vs[j] + "\t" + Muro_i.Phi_Vs_Max + "\t" + Muro_i.Pt_max + "\t"
                            + Muro_i.pt_definitivo[j] + "\t" + Muro_i.Rho_l_Def + "\t" + Muro_i.Cortinas[j] + "\t" + Muro_i.Error_Cortante[j];
                        Lista_texto.Add(Texto);
                    }
                }

            }


        }

        private static void Texto_Flexural_Stress()
        {
            Lista_texto = new List<string>();
            string Texto;

            if (Listas_Programa.Lista_Muros is null == false)
            {
                foreach (Muro Muro_i in Listas_Programa.Lista_Muros)
                {
                    for (int j = 0; j < Muro_i.P.Count; j++)
                    {
                        Texto = Muro_i.Story + "\t" + Muro_i.Pier + "\t" + Muro_i.lw + "\t" + Muro_i.bw + "\t" + Muro_i.Fc + "\t" + Muro_i.h_acumulado + "\t"
                            + Muro_i.Load[j] + "\t" + Muro_i.P[j] + "\t" + Muro_i.M3[j] + "\t" + Muro_i.Fa[j] + "\t" + Muro_i.Fv[j] + "\t" + Muro_i.Sigma_Max[j] + "\t" + Muro_i.Sigma_Min[j]
                            + "\t" + Muro_i.Relacion[j] + "\t" + Muro_i.C_def[j] + "\t" + Muro_i.L_Conf[j] + "\t" + Muro_i.Error_Flexion[j];
                        Lista_texto.Add(Texto);
                    }
                }

            }
        }

        private static void Texto_reporte()
        {
            Lista_texto = new List<string>();
            string Texto;

            if (Listas_Programa.Muros_Consolidados_Listos is null == false)
            {
                foreach (Muros_Consolidados Muro_i in Listas_Programa.Muros_Consolidados_Listos)
                {
                    for (int i = 0; i < Muro_i.Stories.Count; i++)
                    {
                        Texto = Muro_i.Stories[i] + "\t" + Muro_i.Pier_name + "\t" + Muro_i.lw[i] + "\t" + Muro_i.Bw[i] + "\t" + Muro_i.fc[i] + "\t" + Muro_i.Rho_T[i]
                            + "\t" + Muro_i.Rho_l[i] + "\t" + Muro_i.Malla[i] + "\t" + Muro_i.C_Def[i] + "\t" + Muro_i.Lebe_Izq[i] + "\t" + Muro_i.Lebe_Der[i] + "\t" + Muro_i.Lebe_Centro[i]
                            + "\t" + Muro_i.Zc_Izq[i] + "\t" + Muro_i.Zc_Der[i] + "\t" + Muro_i.Hw[i] + "\t" + Muro_i.Est_ebe[i] + "\t" + Muro_i.Sep_ebe[i]
                            + "\t" + Muro_i.Est_Zc[i] + "\t" + Muro_i.Sep_Zc[i] + "\t" + Muro_i.As_Long[i] + "\t" + Muro_i.ramas_izq[i] + "\t" + Muro_i.ramas_der[i]
                            + "\t" + Muro_i.ramas_centro[i] + "\t" + Muro_i.As_htal[i] + "\t" + Muro_i.Ref_htal[i] + "\t" + Muro_i.Capas_htal[i] + "\t" + Muro_i.sep_htal[i]
                            + "\t" + Muro_i.As_Htal_Total[i];
                        Lista_texto.Add(Texto);
                    }
                }

            }

        }
    }
}
