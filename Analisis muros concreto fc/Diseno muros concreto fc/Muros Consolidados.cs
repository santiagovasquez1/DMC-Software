using System;
using System.Collections.Generic;
using System.Linq;

namespace Diseno_muros_concreto_fc
{
    public enum Reduccion
    {
        Arriba,
        Abajo,
        Izquierda,
        Derecha,
        Centro,
        NoAplica
    }

    [Serializable]
    public class Muros_Consolidados_1 : Diseño_de_muros_concreto_V2.Muros_Consolidados
    {
        public List<List<Shells_Prop>> Shells_piso_Izq = new List<List<Shells_Prop>>();
        public List<List<Shells_Prop>> Shells_piso_der = new List<List<Shells_Prop>>();

        public void Calculo_Peso_Aprox()
        {
            double Traslapo, Peso_long_i, Peso_malla_i, Area_piso;
            double P_LD, P_LI;
            double P_ZD, P_ZI;
            double P_Transversal;
            double suma_transv;
            double num_mallas;

            for (int i = 0; i < Stories.Count; i++)
            {
                Traslapo = 1 + Factores_Traslapo(Bw[i] / 100, Rho_l[i]);
                Peso_long_i = Traslapo * As_Long[i] * Hw[i] * 7850 / Math.Pow(100, 3);

                Area_piso = Area_malla(lw[i], Hw[i]);
                Peso_malla_i = Peso_unit_Malla(Malla[i]) * (Area_piso / Math.Pow(100, 2))*1.25;

                Peso_Long.Add(Peso_long_i);
                Peso_malla.Add(Peso_malla_i);

                P_LI = Lebe_Izq[i] > 0 ? Peso_ebe(Bw[i], fc[i], Lebe_Izq[i], Hw[i], Listas_Programa.Capacidad) : 0;
                P_LD = Lebe_Der[i] > 0 ? Peso_ebe(Bw[i], fc[i], Lebe_Der[i], Hw[i], Listas_Programa.Capacidad) : 0;
                P_ZI = Zc_Izq[i] > 0 ? Peso_zc(Bw[i], Listas_Programa.Capacidad) * (Zc_Izq[i] / 100) : 0;
                P_ZD = Zc_Der[i] > 0 ? Peso_zc(Bw[i], Listas_Programa.Capacidad) * (Zc_Der[i] / 100) : 0;

                P_Transversal = As_htal[i] > 0 ? As_htal[i] * (Hw[i] / 100) * lw[i] * 7850 / Math.Pow(100, 3) : 0;
                suma_transv = P_LI + P_LD + P_ZI + P_ZD + P_Transversal;
                Peso_Transv.Add(suma_transv);
            }
            Calculo_volumen();
        }

        private void Calculo_volumen()
        {
            double Volumen_i;
            for (int i = 0; i < Stories.Count; i++)
            {
                Volumen_i = Bw[i] * lw[i] * Hw[i] / Math.Pow(100, 3);
                Volumen.Add(Volumen_i);
            }
        }

        public double Factores_Traslapo(double bw, double pl)
        {
            double a, d, e;
            double factor = 1;

            if (bw == 0.12 & bw < 0.135) factor = 23.32 / 100;
            if (bw >= 0.135 & bw < 0.175) factor = 20.79 / 100;
            if (bw >= 0.175 & bw < 0.23) factor = 23.68 / 100;
            if (bw >= 0.23 & bw < 0.28) factor = 30.62 / 100;
            if (bw >= 0.28)
            {
                a = 3493.5;
                d = -26.287;
                e = 0.2349;
                factor = (a * Math.Pow(pl, 4)) + (d * pl) + e;
            }

            return factor;
        }

        public double Peso_unit_Malla(string Malla_i)
        {
            double Peso_malla = 0;
            switch (Malla_i)
            {
                case "Sin Malla":
                    Peso_malla = 0;
                    break;

                case "D84":
                    Peso_malla = 1.32;
                    break;

                case "D106":
                    Peso_malla = 1.67;
                    break;

                case "D131":
                    Peso_malla = 2.06;
                    break;

                case "D158":
                    Peso_malla = 2.487;
                    break;

                case "D188":
                    Peso_malla = 2.96;
                    break;

                case "D221":
                    Peso_malla = 3.48;
                    break;

                case "D257":
                    Peso_malla = 4.03;
                    break;

                case "DD84":
                    Peso_malla = 1.32 * 2;
                    break;

                case "DD106":
                    Peso_malla = 1.67 * 2;
                    break;

                case "DD131":
                    Peso_malla = 2.06 * 2;
                    break;

                case "DD158":
                    Peso_malla = 2.487 * 2;
                    break;

                case "DD188":
                    Peso_malla = 2.96 * 2;
                    break;

                case "DD221":
                    Peso_malla = 3.48 * 2;
                    break;

                case "DD257":
                    Peso_malla = 4.03 * 2;
                    break;
            }
            return Peso_malla;
        }

        public double Peso_ebe(double bw, double fc, double lebe, double Hw, string Capacidad)
        {
            double Ast1, Ast2, G_As1, G_As2, LG_As1, LG_As2;
            double S_inicial; //Cm
            float delta;
            int pasos;
            double S_min;
            double P_ebe;

            var Num_ramas_1 = new List<int>();    //Numero de ramas a lo largo del muro para Ast1
            var Num_ramas_2 = new List<int>();    //Numero de ramas a lo largo del muro para Ast2
            var Separacion_L_1 = new List<double>();    //Espaciamiento entre cada una de las capas de refuerzo para Ast1
            var Separacion_L_2 = new List<double>();    //Espaciamiento entre cada una de las capas de refuerzo para Ast2

            var Num_ramas_T1 = new List<int>();    //Numero de ramas a lo ancho del muro para Ast1
            var Num_ramas_T2 = new List<int>();    //Numero de ramas a lo ancho del muro para Ast2

            var Num_Ramas_V = new List<int>();    //Numero de ramas en altura del muro para ambos casos de ast
            var GT_As1 = new List<double>();   //Longitud total de los gancho para As1, bajo cada una de las variaciones de la separacion
            var GT_As2 = new List<double>();   //Longitud total de los gancho para As2, bajo cada una de las variaciones de la separacion

            var Factor_Ast1 = new List<double>();
            var Factor_Ast2 = new List<double>();

            var P_As1 = new List<double>();     //'Peso total As1
            var P_As2 = new List<double>();     //'Peso total As1

            double Sep_max = 0;

            Ast1 = 0.71; //'Estribo #3
            Ast2 = 1.29; //'Estribo #4
            delta = 0.5f;

            S_min = Capacidad == "DES" ? 5 : 7.5;

            G_As1 = bw - 2 * 2 + 2 * 17; //'Longitud del gancho transversal
            G_As2 = bw - 2 * 2 + 2 * 20.5; //'Longitud del gancho transversal

            LG_As1 = lebe - 2 * 2 + 2 * 17; //'Longitud del gancho longitudinal
            LG_As2 = lebe - 2 * 2 + 2 * 20.5; //'Longitud del gancho longitudinal

            //'Calculo de la separacion inicial de los estribos
            if (Capacidad == "DES")
            {
                S_inicial = bw / 3;
                if (S_inicial < S_min)
                {
                    S_inicial = S_min;
                }
                pasos = Convert.ToInt32((S_inicial - S_min) / delta);
            }
            else
            {
                S_inicial = bw / 2;
                if (S_inicial <= S_min)
                {
                    S_inicial = S_min;
                }
                pasos = Convert.ToInt32((S_inicial - S_min) / delta);
            }

            if (pasos == 0) pasos = 1;
            S_inicial = S_min;

            for (int j = 0; j < pasos; j++)
            {
                GT_As1.Add(0);
                P_As1.Add(0);

                GT_As2.Add(0);
                P_As2.Add(0);

                Num_ramas_1.Add(0);
                Num_ramas_T1.Add(0);

                Num_ramas_2.Add(0);
                Num_ramas_T2.Add(0);

                Separacion_L_1.Add(0);
                Separacion_L_2.Add(0);

                Num_Ramas_V.Add(Convert.ToInt32(Math.Round((Hw - 10) / S_inicial, 0) + 1));

                //Caso 1 estribos #3
                Num_ramas_1[j] = Ramas_cuantia_volumetrica(lebe, fc, Ast1, Capacidad, S_inicial);
                Num_ramas_T1[j] = Ramas_cuantia_volumetrica(bw, fc, Ast1, Capacidad, S_inicial) - 2;

                if (Num_ramas_1.Last() != 0) Separacion_L_1[j] = (lebe - 2 * 3.8) / Num_ramas_1.Last();
                if (Num_ramas_T1.Last() < 0) Num_ramas_T1[j] = 0;

                GT_As1[j] = Num_Ramas_V[j] * ((G_As1 * Num_ramas_1[j]) + (LG_As1 * (Num_ramas_T1[j] + 2)));
                P_As1[j] = GT_As1[j] * Ast1 * 7850 / Math.Pow(100, 3);

                //Caso 2 estribos #4
                Num_ramas_2[j] = Ramas_cuantia_volumetrica(lebe, fc, Ast1, Capacidad, S_inicial);
                Num_ramas_T2[j] = Ramas_cuantia_volumetrica(bw, fc, Ast1, Capacidad, S_inicial) - 2;

                if (Num_ramas_2.Last() != 0) Separacion_L_2[j] = (lebe - 2 * 3.8) / Num_ramas_2.Last();
                if (Num_ramas_T2.Last() < 0) Num_ramas_T2[j] = 0;

                GT_As2[j] = Num_Ramas_V[j] * ((G_As2 * Num_ramas_2[j]) + (LG_As2 * (Num_ramas_T2[j] + 2)));
                P_As2[j] = GT_As2[j] * Ast2 * 7850 / Math.Pow(100, 3);

                if (Separacion_L_1[j] >= Sep_max) Sep_max = Separacion_L_1[j];
                if (Separacion_L_2[j] >= Sep_max) Sep_max = Separacion_L_2[j];

                S_inicial += delta;
            }

            P_ebe = P_As2.Min() < P_As1.Min() ? P_As2.Min() : P_As1.Min();
            return P_ebe;
        }

        public double Peso_zc(double bw, string Capacidad)
        {
            double P_zc = 0;
            //El factor debe multiplicarse por la longitud del elemento confinado en [m]

            //Peso en estructuras DES
            if (bw == 12 & bw < 13.5 & Capacidad == "DES") P_zc = 49.6 * 0.95;
            if (bw >= 13.5 & bw < 17.5 & Capacidad == "DES") P_zc = 46.14;
            if (bw >= 17.5 & bw < 23 & Capacidad == "DES") P_zc = 35.49;
            if (bw >= 23 & bw < 28.5 & Capacidad == "DES") P_zc = 52.13;
            if (bw >= 28.5 & Capacidad == "DES") P_zc = 52.13;

            //Peso en estructuras DMO
            if (bw == 12 & bw < 13.5 & Capacidad == "DMO") P_zc = 43.59;
            if (bw >= 13.5 & bw < 17.5 & Capacidad == "DMO") P_zc = 43.46;
            if (bw >= 17.5 & bw < 23 & Capacidad == "DMO") P_zc = 48.46;
            if (bw >= 23 & bw < 28.5 & Capacidad == "DMO") P_zc = 42.74;
            if (bw >= 28.5 & Capacidad == "DMO") P_zc = 42.74;

            return P_zc;
        }

        public int Ramas_cuantia_volumetrica(double Lc, double fc, double As_t, string capacidad, double sep)
        {
            double Ash;
            int Ramas;

            if (capacidad == "DES")
            {
                Ash = 0.09 * sep * Lc * fc / 4220;
            }
            else
            {
                Ash = 0.06 * sep * Lc * fc / 4220;
            }

            Ramas = Convert.ToInt32(Math.Round(Ash / As_t, 0));
            return Ramas;
        }

        public double Area_malla(double Lw,double Hw)
        {
            int Num_traslapos;
            float Long_Malla = 245f;
            float traslapo = 30f;
            double Area_i=0;

            Num_traslapos = Lw > 245 ? Convert.ToInt32(Math.Ceiling((Lw - Long_Malla) / (Long_Malla-traslapo))) : 0;
            Area_i = (Lw - 10 + (Num_traslapos * traslapo)) * (Hw+30);

            return Area_i;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static implicit operator List<object>(Muros_Consolidados_1 v)
        {
            throw new NotImplementedException();
        }
    }
}

namespace Diseno_muros_concreto_fc
{
    public class Procesar_info
    {
        internal static List<Muros_Error> Muros_errores { get; set; } = new List<Muros_Error>();

        public static void Compilar_Datos()
        {
            var prueba = new List<double>();
            int indice;
            double Factor1, Factor2;
            double Xmax, Xmin, Ymax, Ymin;

            double Long_mayor, H_prom,Mu_Vu_max;
            int Relacion1;

            Long_mayor = Listas_Programa.Lista_Muros.Select(x => x.lw).Max();
            H_prom = Listas_Programa.Lista_Muros.Select(x => x.hw).Sum() / Listas_Programa.Lista_Muros.Count;
            Mu_Vu_max = Relacion_Mu_Vu()*100;

            Relacion1 = Long_mayor / H_prom >= Mu_Vu_max / H_prom ? Convert.ToInt32(Long_mayor / H_prom) : Convert.ToInt32(Mu_Vu_max / H_prom);
                       
            Muros_Consolidados_1 Muro_i;

            List<string> Muros_distintos = Listas_Programa.Lista_Muros.Select(x => x.Pier).Distinct().ToList();
            Listas_Programa.Muros_Consolidados_Listos = new List<Muros_Consolidados_1>();

            if (Listas_Programa.Capacidad == "DMO")
            {
                Factor1 = 0.30;
                Factor2 = 0.22;
            }
            else
            {
                Factor1 = 0.20;
                Factor2 = 0.15;
            }

            Lectura_Esfuerzos.Cargar_Esfuerzos();

            for (int i = 0; i < Muros_distintos.Count; i++)
            {
                List<Muro> Auxiliar = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Muros_distintos[i]).ToList();

                Muro_i = new Muros_Consolidados_1
                {
                    Pier_name = Muros_distintos[i],
                };
                Muro_i.Stories.AddRange(Auxiliar.Select(x => x.Story).Distinct().ToList());
                Muro_i.fc.AddRange(Auxiliar.Select(x => x.Fc));
                Muro_i.Bw.AddRange(Auxiliar.Select(x => x.bw));
                Muro_i.lw.AddRange(Auxiliar.Select(x => x.lw));
                Muro_i.Hw.AddRange(Auxiliar.Select(x => x.hw));
                Muro_i.Calculo_H_acumulado();
                Muro_i.Reduccion = Diseño_de_muros_concreto_V2.Reduccion.NoAplica;
                Muro_i.Rho_l.AddRange(Auxiliar.Select(x => x.Rho_l_Def));

                for (int j = 0; j < Auxiliar.Count; j++)
                {
                    Muro_i.C_esfuerzo.Add(0);
                    Muro_i.L_esfuerzo.Add(0);

                    Muro_i.Rho_T.Add(Auxiliar[j].pt_definitivo.Max());
                    Muro_i.Sigma_piso.Add(Auxiliar[j].Sigma_Max.Max());

                    Muro_i.C_max.Add(Auxiliar[j].C_def.Max());
                    Muro_i.C_min.Add(Auxiliar[j].C_def.FindAll(x => x > 0).ToList().Min());

                    Muro_i.Malla.Add(Det_malla(Muro_i.Rho_T[j], Muro_i.Rho_l[j], Muro_i.Bw[j]));

                    //Determinacion de C maximo cuando los esfuezos superan los limites
                    try
                    {
                        prueba = Auxiliar[j].Sigma_Max.FindAll(x => x >= Factor2 * Muro_i.fc[j]);
                    }
                    catch
                    {
                        prueba = new List<double>();
                    }

                    if (prueba.Count > 0)
                    {
                        List<double> C_Aux = new List<double>();
                        foreach (double Valor in prueba)
                        {
                            indice = Auxiliar[j].Sigma_Max.FindIndex(x => x == Valor);
                            C_Aux.Add(Auxiliar[j].C_def[indice]);
                        }
                        Muro_i.C_esfuerzo[j] = C_Aux.Max();
                        indice = Auxiliar[j].C_def.FindIndex(x => x == C_Aux.Max());
                        Muro_i.L_esfuerzo[j] = Auxiliar[j].L_Conf[indice];
                    }
                    //

                    Muro_i.L_Conf_Max.Add(Auxiliar[j].L_Conf.Max());
                    Muro_i.L_Conf_Min.Add(Auxiliar[j].L_Conf.Min());

                    //Buscar shells a la derecha y a la izquierda del muro

                    Xmax = Auxiliar[j].Shells_Muro.Select(x => x.Coord.Max(x1 => x1[0])).Max();
                    Xmin = Auxiliar[j].Shells_Muro.Select(x => x.Coord.Min(x1 => x1[0])).Min();
                    Ymax = Auxiliar[j].Shells_Muro.Select(x => x.Coord.Max(x1 => x1[1])).Max();
                    Ymin = Auxiliar[j].Shells_Muro.Select(x => x.Coord.Min(x1 => x1[1])).Min();

                    if (Xmax - Xmin == 0)
                    {
                        Muro_i.Shells_piso_Izq.Add(Seleccion_Muros(Auxiliar[j].Shells_Muro, Ymin, 1));
                        Muro_i.Shells_piso_der.Add(Seleccion_Muros(Auxiliar[j].Shells_Muro, Ymax, 1));
                    }
                    else
                    {
                        Muro_i.Shells_piso_Izq.Add(Seleccion_Muros(Auxiliar[j].Shells_Muro, Xmin, 0));
                        Muro_i.Shells_piso_der.Add(Seleccion_Muros(Auxiliar[j].Shells_Muro, Xmax, 0));
                    }
                    //
                    Muro_i.Confinamiento.Add("No");
                    Muro_i.C_Def.Add(0);
                    Muro_i.Lebe_Izq.Add(0);
                    Muro_i.Lebe_Der.Add(0);
                    Muro_i.Lebe_Centro.Add(0);
                    Muro_i.Zc_Izq.Add(0);
                    Muro_i.Zc_Der.Add(0);
                    Muro_i.Est_ebe.Add(0);
                    Muro_i.Sep_ebe.Add(0);
                    Muro_i.Est_Zc.Add(0);
                    Muro_i.Sep_Zc.Add(0);
                    Muro_i.As_Long.Add(0);
                    Muro_i.ramas_der.Add(0);
                    Muro_i.ramas_izq.Add(0);
                    Muro_i.ramas_centro.Add(0);
                    Muro_i.As_htal.Add(0);
                    Muro_i.Ref_htal.Add("");
                    Muro_i.Capas_htal.Add(0);
                    Muro_i.sep_htal.Add(0);
                    Muro_i.As_Htal_Total.Add(0);
                    //
                }

                Determinacion_EBE(Muro_i, Factor1, Factor2);
                Determinacion_Lado(Muro_i, Factor2);
                Det_As_Long(Muro_i, Relacion1);
                Det_At(Muro_i, Relacion1);
                Errores_muro(Muro_i);
                Listas_Programa.Muros_Consolidados_Listos.Add(Muro_i);
            }
        }

        public static double Relacion_Mu_Vu()
        {
            var Hmin = Listas_Programa.Lista_Muros.Select(x => x.h_acumulado).Min();
            var Prueba = Listas_Programa.Lista_Muros.FindAll(x => x.h_acumulado == Hmin);
            double Relacion_i;
            double max=0;
            int pos = 0;
            int Caso = 0;

            List<double> Relacion = new List<double>();

            foreach (var muro_i in Prueba)
            {
                for (int i = 0; i < muro_i.Load.Count; i++) 
                {
                    if (muro_i.Load[i].Contains("-") == true)
                    {
                        pos = muro_i.Load[i].IndexOf('-') + 1;
                        Caso = Convert.ToInt32(muro_i.Load[i].Substring(pos));
                    }

                    if (muro_i.Load[i].Contains("Max")==true| muro_i.Load[i].Contains("Min") == true | Caso>1 )
                    {
                        Relacion_i = Math.Abs(muro_i.M3[i] / (4 * muro_i.V2[i]));
                        Relacion.Add(Relacion_i);
                    }
                }

            }
            max = Relacion.Max();
            return max;
        }

        private static List<Shells_Prop> Seleccion_Muros(List<Shells_Prop> Shells_i, double Parametro, int indice)
        {
            List<Shells_Prop> Auxiliar = new List<Shells_Prop>();

            for (int i = 0; i < Shells_i.Count; i++)
            {
                for (int j = 0; j < Shells_i[i].Coord[j].Count; j++)
                {
                    if (Shells_i[i].Coord[j][indice] == Parametro)
                    {
                        Auxiliar.Add(Shells_i[i]);
                        break;
                    }
                }
            }

            return Auxiliar;
        }

        private static void Determinacion_EBE(Muros_Consolidados_1 Muro_i, double Limite1, double Limite2)
        {
            for (int j = Muro_i.Stories.Count - 1; j >= 0; j--)
            {
                if (j == Muro_i.Stories.Count - 1)
                {
                    if (Muro_i.Sigma_piso[j] >= Limite1 * Muro_i.fc[j]) Muro_i.Confinamiento[j] = "Si";
                    if (Muro_i.Rho_l[j] >= 0.01) Muro_i.Confinamiento[j] = "Si";
                }
                else
                {
                    if (Muro_i.Sigma_piso[j] >= Limite2 * Muro_i.fc[j] & Muro_i.Confinamiento[j + 1] == "Si") Muro_i.Confinamiento[j] = "Si";
                    if (Muro_i.Rho_l[j] >= 0.01) Muro_i.Confinamiento[j] = "Si";
                }
            }
        }

        private static void Determinacion_Lado(Muros_Consolidados_1 Muro_i, double Limite)

        {
            for (int i = 0; i < Muro_i.Confinamiento.Count; i++)
            {
                if (Muro_i.Confinamiento[i] == "Si")
                {
                    Lectura_Esfuerzos.Enviar_a_Shells(Muro_i.Shells_piso_der[i]);
                    Lectura_Esfuerzos.Enviar_a_Shells(Muro_i.Shells_piso_Izq[i]);
                    Muro_i.C_Def[i] = Muro_i.C_esfuerzo[i];

                    if (Muro_i.Rho_l[i] >= 0.01)
                    {
                        Muro_i.Lebe_Izq[i] = Muro_i.lw[i];
                    }
                    else
                    {
                        if (Muro_i.Shells_piso_Izq[i].Select(x => x.S22).Min() <= -Limite * Muro_i.fc[i] * 10)
                        {
                            Muro_i.Lebe_Izq[i] = Muro_i.L_esfuerzo[i];
                        }
                        if (Muro_i.Shells_piso_der[i].Select(x => x.S22).Min() <= -Limite * Muro_i.fc[i] * 10)
                        {
                            Muro_i.Lebe_Der[i] = Muro_i.L_esfuerzo[i];
                        }
                    }
                }

                //Casos esperados de confinamiento
                if (Muro_i.Rho_l[i] >= 0.0066 & Muro_i.Lebe_Izq[i] > 0 & Muro_i.Lebe_Der[i] == 0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Der[i] = Muro_i.L_Conf_Max[i];
                }
                else if (Muro_i.Lebe_Izq[i] > 0 & Muro_i.Lebe_Der[i] == 0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Der[i] = Muro_i.L_Conf_Min[i];
                }

                if (Muro_i.Rho_l[i] >= 0.0066 & Muro_i.Lebe_Izq[i] == 0 & Muro_i.Lebe_Der[i] > 0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Izq[i] = Muro_i.L_Conf_Max[i];
                }
                else if (Muro_i.Lebe_Izq[i] == 0 & Muro_i.Lebe_Der[i] >= 0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Izq[i] = Muro_i.L_Conf_Min[i];
                }

                if (Muro_i.Rho_l[i] >= 0.0066 & Muro_i.Lebe_Izq[i] == 0 & Muro_i.Lebe_Der[i] == 0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Der[i] = Muro_i.L_Conf_Max[i];
                    Muro_i.Zc_Izq[i] = Muro_i.L_Conf_Max[i];
                    Muro_i.C_Def[i] = Muro_i.C_max[i];
                }
                else if (Muro_i.Lebe_Izq[i] == 0 & Muro_i.Lebe_Der[i] == 0)
                {
                    Muro_i.Zc_Der[i] = Muro_i.L_Conf_Min[i];
                    Muro_i.Zc_Izq[i] = Muro_i.L_Conf_Min[i];
                    Muro_i.C_Def[i] = Muro_i.C_min[i];
                }

                if (Muro_i.Zc_Der[i] < 30) Muro_i.Zc_Der[i] = 0;
                if (Muro_i.Zc_Izq[i] < 30) Muro_i.Zc_Izq[i] = 0;
            }
        }

        private static void Det_As_Long(Muros_Consolidados_1 Muro_i, int Pisos_Sin_malla)
        {
            double Aux_As_Long, Acero_malla, Aux_Long;
            for (int i = 0; i < Muro_i.Stories.Count; i++)
            {
                if (i >= Muro_i.Stories.Count - Pisos_Sin_malla)
                {
                    if (Muro_i.Malla[i] != "Sin Malla")
                    {
                        int Num_barras = Convert.ToInt32(Math.Round((Muro_i.lw[i] - 2 * 3.8) / 30, 0) + 1);
                        double Rho_aux;

                        if (Muro_i.Malla[i].Contains("DD") == true)
                        {
                            Rho_aux = Num_barras * 1.42 / (Muro_i.lw[i] * Muro_i.Bw[i]);
                        }
                        else
                        {
                            Rho_aux = Num_barras * 0.71 / (Muro_i.lw[i] * Muro_i.Bw[i]);
                        }
                        Muro_i.Malla[i] = "Sin Malla";
                        if (Rho_aux > Muro_i.Rho_l[i]) Muro_i.Rho_l[i] = Rho_aux;
                    }
                }

                Aux_As_Long = Muro_i.Bw[i] * Muro_i.lw[i] * Muro_i.Rho_l[i];
                Aux_Long = Muro_i.lw[i] - Muro_i.Lebe_Izq[i] - Muro_i.Lebe_Der[i] - Muro_i.Lebe_Centro[i] - Muro_i.Zc_Izq[i] - Muro_i.Zc_Der[i];
                Acero_malla = (Aux_Long / 100) * Al_Malla(Muro_i.Malla[i]);

                if (Aux_As_Long - Acero_malla > 0)
                {
                    Muro_i.As_Long[i] = Aux_As_Long - Acero_malla;
                }
            }
        }

        private static void Det_At(Muros_Consolidados_1 Muro_i, int Pisos_Sin_malla)
        {
            double Aux_As_t, Aux_As_tm;
            for (int i = 0; i < Muro_i.Stories.Count; i++)
            {
                if (i >= Muro_i.Stories.Count - Pisos_Sin_malla)
                {
                    int Num_barras = Convert.ToInt32((100 / 30) + 1);
                    double Rho_aux;

                    if (Muro_i.Rho_T[i] >= 0.0020 | Muro_i.Bw[i] >= 20)
                    {
                        Rho_aux = (1.42 / 0.30) / (100 * Muro_i.Bw[i]);
                    }
                    else
                    {
                        Rho_aux = (.71 / 0.30) / (100 * Muro_i.Bw[i]);
                    }
                    if (Rho_aux > Muro_i.Rho_T[i]) Muro_i.Rho_T[i] = Rho_aux;
                }

                if (Muro_i.Rho_T[i] < 0.0020) Muro_i.Rho_T[i] = 0.0020;
                Aux_As_t = Muro_i.Bw[i] * 100 * Muro_i.Rho_T[i];
                Aux_As_tm = Al_Malla(Muro_i.Malla[i]);

                if (Aux_As_t - Aux_As_tm > 0)
                {
                    Muro_i.As_htal[i] = Aux_As_t - Aux_As_tm;
                }
            }
        }

        private static string Det_malla(double rt, double rl, float espesor)
        {
            string Malla;
            Malla = "Sin Malla";

            if (espesor >= 8 & espesor < 10)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl < 0.01) Malla = "D106";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "D188";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD106";
            }

            if (espesor >= 10 & espesor < 12)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl < 0.01) Malla = "D131";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "D106";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD131";
            }

            if (espesor >= 12 & espesor < 15)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl < 0.01) Malla = "D158";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "DD131";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD158";
            }

            if (espesor >= 15 & espesor < 20)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl < 0.01) Malla = "D188";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "DD158";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD188";
            }

            if (espesor >= 20 & espesor < 25)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl < 0.01) Malla = "DD131";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "DD221";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD257";
            }

            return Malla;
        }

        private static double Al_Malla(string Malla)
        {
            double Aux_acero = 0;
            switch (Malla)

            {
                case "Sin Malla":
                    Aux_acero = 0;
                    break;

                case "D84":
                    Aux_acero = 0.84;
                    break;

                case "D106":
                    Aux_acero = 1.06;
                    break;

                case "D131":
                    Aux_acero = 1.31;
                    break;

                case "D158":
                    Aux_acero = 1.58;
                    break;

                case "D188":
                    Aux_acero = 1.88;
                    break;

                case "D221":
                    Aux_acero = 2.21;
                    break;

                case "D257":
                    Aux_acero = 2.57;
                    break;

                case "DD84":
                    Aux_acero = 0.84 * 2;
                    break;

                case "DD106":
                    Aux_acero = 1.06 * 2;
                    break;

                case "DD131":
                    Aux_acero = 1.31 * 2;
                    break;

                case "DD158":
                    Aux_acero = 1.58 * 2;
                    break;

                case "DD188":
                    Aux_acero = 1.88 * 2;
                    break;

                case "DD221":
                    Aux_acero = 2.21 * 2;
                    break;

                case "DD257":
                    Aux_acero = 2.57 * 2;
                    break;
            }
            return Aux_acero;
        }

        public static void Errores_muro(Muros_Consolidados_1 Muro_i)
        {
            string Mensaje;
            Muros_Error Muro_1;

            for (int i = 0; i < Muro_i.Stories.Count; i++)
            {
                if (Muro_i.Rho_l[i] >= 0.0066 & Muro_i.Bw[i] < 15 || Muro_i.Lebe_Izq[i] > 0 & Muro_i.Bw[i] < 15 || Muro_i.Lebe_Der[i] > 0 & Muro_i.Bw[i] < 15)
                {
                    Muro_1 = new Muros_Error
                    {
                        Piername = Muro_i.Pier_name,
                        Story = Muro_i.Stories[i]
                    };

                    if (Muro_i.Rho_l[i] >= 0.0066 & Muro_i.Bw[i] < 15)
                    {
                        Mensaje = "Bw < 0.15m y Rho_l>=0.0066";
                        Muro_1.Mensaje.Add(Mensaje);
                    }
                    if (Muro_i.Lebe_Izq[i] > 0 & Muro_i.Bw[i] < 15 || Muro_i.Lebe_Der[i] > 0 & Muro_i.Bw[i] < 15)
                    {
                        Mensaje = "Bw<0.15m y Ebe>0";
                        Muro_1.Mensaje.Add(Mensaje);
                    }
                    Muros_errores.Add(Muro_1);
                }
            }
        }

        internal class Muros_Error
        {
            public string Piername { get; set; }
            public string Story { get; set; }
            public List<string> Mensaje { get; set; } = new List<string>();
        }
    }
}
