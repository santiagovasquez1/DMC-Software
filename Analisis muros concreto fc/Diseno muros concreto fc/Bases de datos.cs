using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Diseno_muros_concreto_fc
{
    public class Bases_de_datos
    {
        public static DataSet Ds_Shear;

        public static void Datos_Fuerzas()

        {
            List<string> Encabezados = new List<string> { "Story", "Pier", "Load", "Loc", "P" + Environment.NewLine + "Tonf", "V2" + Environment.NewLine + "Tonf", "V3" + Environment.NewLine + "Tonf", "M2" + Environment.NewLine + "Tonf-m", "M3" + Environment.NewLine + "Tonf-m" };
            List<Muro> Muro_i = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Listas_Programa.Texto_combo).ToList();
            DataTable T_Fuerzas = new DataTable("Fuerzas");

            int Pos, X;
            Pos = -1;
            X = 0;

            foreach (DataTable Tabla in Ds_Shear.Tables)
            {
                if (Tabla.TableName == "Fuerzas") { Pos = X; break; }
                X++;
            }

            if (Pos > -1)
            {
                Ds_Shear.Tables.Clear();
                Ds_Shear.Tables.Add(T_Fuerzas);
            }
            else
            {
                Ds_Shear.Tables.Add(T_Fuerzas);
            }

            for (int i = 0; i < Encabezados.Count; i++)
            {
                DataColumn Columna_i = new DataColumn(Encabezados[i]);
                T_Fuerzas.Columns.Add(Columna_i);
            }
            for (int i = 0; i < Muro_i.Count; i++)
            {
                for (int j = 0; j < Muro_i[i].P.Count; j++)
                {
                    DataRow dr = T_Fuerzas.NewRow();
                    dr[0] = Muro_i[i].Story;
                    dr[1] = Muro_i[i].Pier;
                    dr[2] = Muro_i[i].Load[j];
                    dr[3] = Muro_i[i].Loc[j];
                    dr[4] = Math.Round(Muro_i[i].P[j], 2);
                    dr[5] = Math.Round(Muro_i[i].V2[j], 2);
                    dr[6] = Math.Round(Muro_i[i].V3[j], 2);
                    dr[7] = Math.Round(Muro_i[i].M2[j], 2);
                    dr[8] = Math.Round(Muro_i[i].M3[j], 2);
                    T_Fuerzas.Rows.Add(dr);
                }
            }

        }

        public static void Datos_Geometria()
        {
            List<string> Encabezados = new List<string> { "Story", "Pier", "Lw" + Environment.NewLine + "m", "Bw" + Environment.NewLine + "m", "Fc" + Environment.NewLine + "kgf/m²", "Hpiso" + Environment.NewLine + "m", "H acumulada" + Environment.NewLine + "m", "rho l" };
            List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Listas_Programa.Texto_combo).ToList();
            DataTable T_Geometria = new DataTable("Geometria");
            int Pos, X;
            Pos = -1;
            X = 0;

            foreach (DataTable Tabla in Ds_Shear.Tables)
            {
                if (Tabla.TableName == "Geometria") { Pos = X; }
                X++;
            }

            if (Pos > -1)
            {

                Ds_Shear.Tables.Clear();
                Ds_Shear.Tables.Add(T_Geometria);
            }
            else
            {
                Ds_Shear.Tables.Add(T_Geometria);
            }

            for (int i = 0; i < Encabezados.Count; i++)
            {
                DataColumn Columna_i = new DataColumn(Encabezados[i]);
                T_Geometria.Columns.Add(Columna_i);
            }

            for (int i = 0; i < Lista_ordenada.Count; i++)
            {
                DataRow dr = T_Geometria.NewRow();
                dr[0] = Lista_ordenada[i].Story;
                dr[1] = Lista_ordenada[i].Pier;
                dr[2] = Math.Round(Lista_ordenada[i].lw / 100, 2);
                dr[3] = Math.Round(Lista_ordenada[i].bw / 100, 2);
                dr[4] = Lista_ordenada[i].Fc;
                dr[5] = Math.Round(Lista_ordenada[i].hw / 100, 2);
                dr[6] = Math.Round(Lista_ordenada[i].h_acumulado / 100, 2);
                dr[7] = Math.Round(Lista_ordenada[i].Rho_l_Inicial, 4);
                T_Geometria.Rows.Add(dr);
            }
        }

        public static void Datos_Cortante()
        {
            List<string> Encabezados = new List<string> { "Story", "Pier", "Lw(m)", "Bw(m)", "Fc (kgf/cm²)", "Ht(m)","Load", "P (Tonf)", "V2 (Tonf)", "M3 (Tonf-m)", "Phi Vc (Tonf)" + Environment.NewLine + "C.11.9.5", "Phi Vn (Tonf)" + Environment.NewLine + "C.11.9.3", "Phi Vn (Tonf)" + Environment.NewLine + "C.21.9.4.1","Phi Vs (Tonf)"+Environment.NewLine+"Requerido","Phi Vs max (Tonf)"+Environment.NewLine+"C.11.4.7.9",
            "Rho T max"+Environment.NewLine+"cm²/m","Rho T col"+Environment.NewLine+"cm²/m","Rho L col" + Environment.NewLine+"cm²/m","# Cortinas"+Environment.NewLine+"C.21.9.2.3","Mensaje"};
            List<Muro> Lista_ordenada;

            if (Listas_Programa.Texto_combo == "Errores")
            {
                Lista_ordenada = Listas_Programa.Muros_insuficientes;
            }
            else
            {
                Lista_ordenada = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Listas_Programa.Texto_combo).ToList();
            }

            DataTable T_Shear = new DataTable("Cortante");
            int Pos, X;
            Pos = -1;
            X = 0;
            foreach (DataTable Tabla in Ds_Shear.Tables)
            {
                if (Tabla.TableName == "Cortante") { Pos = X; }
                X++;
            }

            if (Pos > -1)
            {

                Ds_Shear.Tables.Clear();
                Ds_Shear.Tables.Add(T_Shear);
            }
            else
            {
                Ds_Shear.Tables.Add(T_Shear);
            }

            for (int i = 0; i < Encabezados.Count; i++)
            {
                DataColumn Columna_i = new DataColumn(Encabezados[i]);
                T_Shear.Columns.Add(Columna_i);
            }

            for (int i = 0; i < Lista_ordenada.Count; i++)
            {
                for (int j = 0; j < Lista_ordenada[i].P.Count; j++)
                {
                    DataRow dr = T_Shear.NewRow();
                    dr[0] = Lista_ordenada[i].Story;
                    dr[1] = Lista_ordenada[i].Pier;
                    dr[2] = Math.Round(Lista_ordenada[i].lw / 100, 2);
                    dr[3] = Math.Round(Lista_ordenada[i].bw / 100, 2);
                    dr[4] = Lista_ordenada[i].Fc;
                    dr[5] = Math.Round(Lista_ordenada[i].h_acumulado / 100, 2);
                    dr[6] = Lista_ordenada[i].Load[j];
                    dr[7] = Math.Round(Lista_ordenada[i].P[j], 2);
                    dr[8] = Math.Round(Lista_ordenada[i].V2[j], 2);
                    dr[9] = Math.Round(Lista_ordenada[i].M3[j], 2);

                    if (Lista_ordenada[i].Phi_Vc == null)
                    {
                        dr[10] = 0;
                        dr[11] = 0;
                        dr[12] = 0;
                        dr[13] = 0;
                        dr[14] = 0;
                        dr[15] = 0;
                        dr[16] = 0;
                        dr[17] = 0;
                        dr[18] = 0;
                        dr[19] = "N.A";
                    }
                    else
                    {
                        dr[10] = Math.Round(Lista_ordenada[i].Phi_Vc[j], 2);
                        dr[11] = Math.Round(Lista_ordenada[i].Phi_Vn_Max1, 2);
                        dr[12] = Math.Round(Lista_ordenada[i].Phi_Vn_Max2[j], 2);
                        dr[13] = Math.Round(Lista_ordenada[i].Phi_Vs[j], 2);
                        dr[14] = Math.Round(Lista_ordenada[i].Phi_Vs_Max, 2);
                        dr[15] = Math.Round(Lista_ordenada[i].Pt_max, 5);
                        dr[16] = Math.Round(Lista_ordenada[i].pt_definitivo[j], 5);
                        dr[17] = Math.Round(Lista_ordenada[i].Rho_l_Def, 5);
                        dr[18] = Lista_ordenada[i].Cortinas[j];
                        dr[19] = Lista_ordenada[i].Error_Cortante[j];
                    }
                    T_Shear.Rows.Add(dr);
                }
            }

        }

        public static void Datos_Flexion()
        {
            List<string> Encabezados = new List<string> { "Story", "Pier", "Lw(m)", "Bw(m)", "Fc (kgf/cm²)", "Ht(m)", "Load", "P (Tonf)", "M3 (Tonf-m)","Fa"+Environment.NewLine+"(kgf/cm²)",
                "Fv"+Environment.NewLine+"(kgf/cm²)","σ max"+Environment.NewLine+"(kgf/cm²)","σ min"+Environment.NewLine+"(kgf/cm²)","σ max/f'c"+Environment.NewLine+"%","C (cm)","L_Conf" + Environment.NewLine+"(cm)","Error"};
            List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Listas_Programa.Texto_combo).ToList();
            DataTable T_Flexion = new DataTable("Flexion");
            int Pos, X;
            Pos = -1;
            X = 0;

            foreach (DataTable Tabla in Ds_Shear.Tables)
            {
                if (Tabla.TableName == "Flexion") { Pos = X; }
                X++;
            }

            if (Pos > -1)
            {
                Ds_Shear.Tables.Clear();
                Ds_Shear.Tables.Add(T_Flexion);
            }
            else
            {
                Ds_Shear.Tables.Add(T_Flexion);
            }

            for (int i = 0; i < Encabezados.Count; i++)
            {
                DataColumn Columna_i = new DataColumn(Encabezados[i]);
                T_Flexion.Columns.Add(Columna_i);
            }

            for (int i = 0; i < Lista_ordenada.Count; i++)
            {
                for (int j = 0; j < Lista_ordenada[i].P.Count; j++)
                {
                    DataRow dr = T_Flexion.NewRow();
                    dr[0] = Lista_ordenada[i].Story;
                    dr[1] = Lista_ordenada[i].Pier;
                    dr[2] = Math.Round(Lista_ordenada[i].lw / 100, 2);
                    dr[3] = Math.Round(Lista_ordenada[i].bw / 100, 2);
                    dr[4] = Lista_ordenada[i].Fc;
                    dr[5] = Math.Round(Lista_ordenada[i].h_acumulado / 100, 2);
                    dr[6] = Lista_ordenada[i].Load[j];
                    dr[7] = Math.Round(Lista_ordenada[i].P[j], 2);
                    dr[8] = Math.Round(Lista_ordenada[i].M3[j], 2);

                    if (Lista_ordenada[i].Fa == null)
                    {
                        dr[9] = 0;
                        dr[10] = 0;
                        dr[11] = 0;
                        dr[12] = 0;
                        dr[13] = 0;
                        dr[14] = 0;
                        dr[15] = 0;
                        dr[16] = "N.A";
                    }
                    else
                    {
                        dr[9] = Math.Round(Lista_ordenada[i].Fa[j], 2);
                        dr[10] = Math.Round(Lista_ordenada[i].Fv[j], 2);
                        dr[11] = Math.Round(Lista_ordenada[i].Sigma_Max[j], 2);
                        dr[12] = Math.Round(Lista_ordenada[i].Sigma_Min[j], 2);
                        dr[13] = Math.Round(Lista_ordenada[i].Relacion[j] * 100, 2);
                        dr[14] = Math.Round(Lista_ordenada[i].C_def[j], 2);
                        dr[15] = Math.Round(Lista_ordenada[i].L_Conf[j], 2);
                        dr[16] = Lista_ordenada[i].Error_Flexion[j];
                    }
                    T_Flexion.Rows.Add(dr);
                }
            }

        }

        public static void Datos_resumen()
        {
            List<string> Encabezados = new List<string> { "Story", "Pier", "Lw(m)", "Bw(m)", "Fc (kgf/cm²)", "rho t", "rho l","Malla", "C" + Environment.NewLine+"(cm)","Lebe_Izq"+Environment.NewLine+"(cm)",
                "Lebe_Der"+Environment.NewLine+"(cm)","Zc_Izq"+Environment.NewLine+"(cm)" ,"Zc_der"+Environment.NewLine+"(cm)"};
            List<Muros_Consolidados> Lista_ordenada = Listas_Programa.Muros_Consolidados_Listos.FindAll(x => x.Pier_name == Listas_Programa.Texto_combo).ToList();

            DataTable T_Resumen = new DataTable("Resumen");
            int Pos, X;
            Pos = -1;
            X = 0;

            foreach (DataTable Tabla in Ds_Shear.Tables)
            {
                if (Tabla.TableName == "Resumen") { Pos = X; }
                X++;
            }

            if (Pos > -1)
            {
                Ds_Shear.Tables.Clear();
                Ds_Shear.Tables.Add(T_Resumen);
            }
            else
            {
                Ds_Shear.Tables.Add(T_Resumen);
            }

            for (int i = 0; i < Encabezados.Count; i++)
            {
                DataColumn Columna_i = new DataColumn(Encabezados[i]);
                T_Resumen.Columns.Add(Columna_i);
            }

            for (int i = 0; i < Lista_ordenada.Count; i++)
            {
                for (int j = 0; j < Lista_ordenada[i].Stories.Count; j++)
                {
                    DataRow dr = T_Resumen.NewRow();

                    dr[0] = Lista_ordenada[i].Stories[j];
                    dr[1] = Lista_ordenada[i].Pier_name;
                    dr[2] = Math.Round(Lista_ordenada[i].lw[j] / 100, 2);
                    dr[3] = Math.Round(Lista_ordenada[i].Bw[j] / 100, 2);
                    dr[4] = Lista_ordenada[i].fc[j];
                    dr[5] = Math.Round(Lista_ordenada[i].Rho_T[j], 4);
                    dr[6] = Math.Round(Lista_ordenada[i].Rho_l[j], 4);
                    dr[7] = Lista_ordenada[i].Malla[j];
                    dr[8] = Math.Round(Lista_ordenada[i].C_Def[j], 0);
                    dr[9] = Math.Round(Lista_ordenada[i].Lebe_Izq[j], 0);
                    dr[10] = Math.Round(Lista_ordenada[i].Lebe_Der[j], 0);
                    dr[11] = Math.Round(Lista_ordenada[i].Zc_Izq[j], 0);
                    dr[12] = Math.Round(Lista_ordenada[i].Zc_Der[j], 0);
                    T_Resumen.Rows.Add(dr);
                }
            }

        }


    }
}
