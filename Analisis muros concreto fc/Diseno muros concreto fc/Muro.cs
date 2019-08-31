using System;
using System.Collections.Generic;
using System.Linq;
using Diseño_de_muros_concreto_V2;

namespace Diseno_muros_concreto_fc
{
    [Serializable]
    public class Muro:Diseño_de_muros_concreto_V2.Muro
    {
        //public string Pier, Story;
        //public float bw, lw, hw, Fc, dw, h_acumulado;
        //public double Rho_l_Inicial, Spacing, Rho_l_Def;
        //public List<string> Loc = new List<string>();
        //public List<string> Load = new List<string>();
        //public List<double> P = new List<double>();
        //public List<double> V2 = new List<double>();
        //public List<double> V3 = new List<double>();
        //public List<double> M2 = new List<double>();
        //public List<double> M3 = new List<double>();
        public List<Shells_Prop> Shells_Muro = new List<Shells_Prop>();

        ////Datos para el diseño a cortante de los muros en concreto
        //public List<double> Phi_Vc;

        //public List<double> Phi_Vs;
        //public List<double> Pt_min;
        //public List<double> pl_min;
        //public List<double> pt_requerido1;                      //Según C.11.9.91
        //public List<double> ptt;                                //Cuantia transversal requerida por C.21.9.4.1
        //public List<double> pt_definitivo;
        //public double Phi_Vn_Max1;                              //Capacidad maxima de la sección segun C11.9.3
        //public List<double> Phi_Vn_Max2 = new List<double>();   //Capacidad maxima de la sección segun C21.9.4.1
        //public double Phi_Vs_Max;                               //Capacidad maxima del acero según C.11.4.7.9
        //public double Pt_max;                                   //Cuantia maxima de acero según C.11.4.7.9
        //public List<int> Cortinas;
        //public List<string> Error_Cortante;

        ////Datos para el analisis a flexo compresion
        //public List<double> C_def;
        //public List<double> L_Conf;
        //public List<double> Fa;
        //public List<double> Fv;
        //public List<double> Sigma_Max;
        //public List<double> Sigma_Min;
        //public List<double> Relacion;
        //public List<string> Error_Flexion;
        //public double C_balanceado, P_balanceado;

        public void Diseno_Cortante()
        {
            //Todas las unidades seran trabajadas en kgf/cm2

            Phi_Vc = new List<double>();
            Phi_Vs = new List<double>();
            Pt_min = new List<double>();
            pl_min = new List<double>();
            pt_requerido1 = new List<double>(); //Según C.11.9.91
            ptt = new List<double>(); //Cuantia transversal requerida por C.21.9.4.1
            pt_definitivo = new List<double>();
            Cortinas = new List<int>();
            Error_Cortante = new List<string>();

            double Fy = 4220; //[kgf/cm2]
            float Phi = 0.75F; //Definir si sera variable de entrada o un valor fijo
            double pt_auxiliar;
            List<double> Rango_Auxiliar;

            Phi_Vn_Max1 = Phi * 2.65 * Math.Sqrt(Fc) * (bw * dw) / 1000; //Tonf
            Phi_Vs_Max = Phi * 2.1 * Math.Sqrt(Fc) * (bw * dw) / 1000; //Tonf
            Pt_max = Phi * 2.1 * Math.Sqrt(Fc) / Fy;

            for (int i = 0; i < V2.Count; i++)

            {
                Phi_Vc.Add(Phi * Calc_Vc(Math.Abs(V2[i]), Math.Abs(M3[i]), -P[i]));
                if (V2[i] - Phi_Vc[i] < 0)
                {
                    Phi_Vs.Add(0);
                    pt_requerido1.Add(0);  //Según C.11.9.91
                }
                else
                {
                    Phi_Vs.Add((V2[i] - Phi_Vc[i]) / Phi);
                    pt_auxiliar = (V2[i] - Phi_Vc[i]) * Math.Pow(10, 3) / (Phi * Fy * dw * bw);
                    pt_requerido1.Add(pt_auxiliar); //Según C.11.9.91
                }

                //Calculo de pt minimo y numero de cortinas

                if (V2[i] * 1000 >= 0.5 * 0.53 * Math.Sqrt(Fc) * bw * dw)
                {
                    Pt_min.Add(0.0025);
                    pl_min.Add(0.0025);
                }
                else if (lw < 500)
                {
                    Pt_min.Add(0.0012);
                    pl_min.Add(0.0012);
                }
                else
                {
                    Pt_min.Add(0.0020);
                    pl_min.Add(0.0020);
                }

                if (V2[i] * 1000 >= 0.53 * Math.Sqrt(Fc) * bw * lw) Cortinas.Add(2); else Cortinas.Add(1);

                ptt.Add(Calc_ptt(Calc_alpah(h_acumulado), Math.Abs(V2[i] * 1000), Phi, Fy));
                Rango_Auxiliar = new List<double> { pt_requerido1[i], Pt_min[i], ptt[i] };
                pt_definitivo.Add(Rango_Auxiliar.Max());

                if (h_acumulado / lw <= 2) pl_min[i] = pt_definitivo[i];
                Phi_Vn_Max2.Add((bw * lw * (Calc_alpah(h_acumulado) * Math.Sqrt(Fc) + pt_definitivo[i] * Fy)) / 1000);

                //Condiciones

                Error_Cortante.Add("Ok");
                if (V2[i] > Phi_Vn_Max1 ^ V2[i] > Phi_Vn_Max2[i]) Error_Cortante[i] = "V2 mayor que Phi Vn max";
                if (Phi_Vs[i] > Phi_Vs_Max) Error_Cortante[i] = "Phi Vs mayor a Phi Vs max";
            }
            Rho_l_Def = Math.Max(Rho_l_Inicial, pl_min.Max());
        }

        public void Flexural_Analisis()
        {
            double Fy = 4220;           //[kgf/cm2]
            double es = 2000000;        //[kgf/cm2]
            double recubrimiento = 2.5; //[cm]
            int ramas, x;
            double Beta, Acero_Long, P_error, Factor1, Factor2;
            double Pc_Sup, Pc_inf, D_Pc, Ps_inf, Ps_Sup, D_Ps;
            double Numerador, Denominador;

            List<double> As_i = new List<double>();
            List<double> d_i = new List<double>();
            List<double> C_inf;
            List<double> C_Sup;
            List<double> Pn_inf;
            List<double> Pn_sup;
            List<double> D_Pn;

            C_def = new List<double>();
            L_Conf = new List<double>();
            Fa = new List<double>();
            Fv = new List<double>();
            Sigma_Max = new List<double>();
            Sigma_Min = new List<double>();
            Relacion = new List<double>();
            Error_Flexion = new List<string>();

            //Calculation of number of rebar layers within the wall
            ramas = Convert.ToInt32(((lw - 2 * (recubrimiento + 0.3175)) / Spacing) + 1);
            if (Rho_l_Def > 0) Acero_Long = Rho_l_Def * bw * lw; else Acero_Long = Rho_l_Inicial * bw * lw;  //[cm²]
            if (Fc <= 280) Beta = 0.85; else Beta = 0.75;
            if (Fc == 350) Beta = 0.80;

            for (int i = 0; i < ramas; i++)
            {
                As_i.Add(Acero_Long / ramas);
                if (i == 0) d_i.Add(recubrimiento + 0.375);
                if (i > 0 & i < ramas - 1) d_i.Add(d_i.Last() + Spacing);
                if (i == ramas - 1) d_i.Add(lw - d_i.First());
            }

            P_balanceado = Calculo_Pn_Balanceado(Acero_Long, Beta, As_i, d_i, Fy, es);

            for (int i = 0; i < P.Count; i++)
            {
                if (-P[i] > 0)
                {
                    //Proceso iterativo para hallar la profundidad del eje neutro

                    P_error = 100;
                    C_inf = new List<double>();
                    C_Sup = new List<double>();
                    Pn_inf = new List<double>();
                    Pn_sup = new List<double>();
                    D_Pn = new List<double>();
                    x = 0;
                    Factor1 = 0;
                    Factor2 = 0;

                    do
                    {
                        if (x == 0)
                        {
                            if (-P[i] >= P_balanceado)
                            {
                                C_inf.Add(C_balanceado);
                                C_Sup.Add(lw / Beta);
                            }
                            else
                            {
                                C_inf.Add(0);
                                C_Sup.Add(C_balanceado);
                            }
                        }
                        else
                        {
                            Factor1 = ((Pn_inf[x - 1] - Math.Abs(P[i])) / (Pn_sup[x - 1] - Pn_inf[x - 1])) * (C_Sup[x - 1] - C_inf[x - 1]);
                            Factor2 = (Pn_sup[x - 1] - Math.Abs(P[i])) / (D_Pn[x - 1]);

                            C_inf.Add(C_inf[x - 1] - Factor1);
                            C_Sup.Add(C_Sup[x - 1] - Factor2);
                        }

                        Pc_inf = 0.85 * Fc * Beta * C_inf.Last() * bw;
                        Pc_Sup = 0.85 * Fc * Beta * C_Sup.Last() * bw;
                        D_Pc = 0.85 * Fc * Beta * bw; //Constante para secciones rectangulares

                        Ps_inf = Fuerza_As(0.003, es, As_i, C_inf.Last(), d_i);
                        Ps_Sup = Fuerza_As(0.003, es, As_i, C_Sup.Last(), d_i);
                        D_Ps = Fs_prima(0.003, es, As_i, C_Sup.Last(), d_i);

                        Pn_inf.Add((Pc_inf + Ps_inf) / 1000);
                        Pn_sup.Add((Pc_Sup + Ps_Sup) / 1000);
                        D_Pn.Add((D_Pc + D_Ps) / 1000);

                        if (Pn_inf.Last() >= 0)
                        {
                            P_error = Math.Abs((Pn_inf.Last() - Math.Abs(P[i])) / Math.Abs(P[i]));
                        }
                        else
                        {
                            P_error = 100;
                        }

                        x++;
                    }
                    while (P_error > 0.025);

                    C_def.Add(C_inf.Last());

                    if (C_def.Last() - 0.1 * lw >= C_def.Last() / 2)
                    {
                        L_Conf.Add(C_def.Last() - 0.1 * lw);
                    }
                    else
                    {
                        L_Conf.Add(C_def.Last() / 2);
                    }
                }
                else
                {
                    C_def.Add(0);
                    L_Conf.Add(0);
                }

                //Calculo de esfuerzos en el muro

                Numerador = 6 * M3[i] * Math.Pow(10, 5);
                Denominador = bw * Math.Pow(lw, 2);

                Fa.Add(Math.Abs(P[i] * 1000) / (bw * lw));  //[kgf/cm²]
                Fv.Add(Numerador / Denominador);            //[kgf/cm²]
                Sigma_Max.Add(Fa.Last() + Fv.Last());       //[kgf/cm²]
                Sigma_Min.Add(Fa.Last() - Fv.Last());       //[kgf/cm²]
                Relacion.Add(Math.Max(Sigma_Max.Last(), Math.Abs(Sigma_Min.Last())) / Fc);
                Error_Flexion.Add("Ok");
                if (Relacion.Last() >= 0.40) Error_Flexion[i] = "Cambiar espesor";
            }
        }

        private double Calculo_Pn_Balanceado(double As_long, double Beta, List<double> As_i, List<double> d_i, double Fy, double es)
        {
            double ey = Fy / es;
            double ab, Pc_b, Pn_b, Ps_b;

            C_balanceado = (0.003 / (0.003 + ey)) * d_i.Last();
            ab = Beta * C_balanceado;
            Pc_b = 0.85 * Fc * ab * bw;
            Ps_b = Fuerza_As(0.003, es, As_i, C_balanceado, d_i);

            Pn_b = (Pc_b + Ps_b) / 1000;
            return Pn_b;
        }

        private double Fuerza_As(double ecu, double es, List<double> As_i, double Ci, List<double> d_i)
        {
            double Ps, fsl;
            List<double> Ps_i = new List<double>();

            for (int i = 0; i < d_i.Count; i++)
            {
                fsl = ecu * es * (Ci - d_i[i]) / Ci;
                if (Math.Abs(fsl) > 4220)
                {
                    if (fsl < 0) fsl = -4220; else fsl = 4220;
                }
                Ps_i.Add(fsl * As_i[i]);
            }
            Ps = Ps_i.Sum();
            return Ps;
        }

        private double Fs_prima(double ecu, double es, List<double> As_i, double Ci, List<double> d_i)
        {
            double fs_prima, Ps_prima;
            List<double> Psi_prima = new List<double>();

            for (int i = 0; i < d_i.Count; i++)
            {
                fs_prima = (ecu * es * d_i[i]) / (Math.Pow(Ci, 2));
                Psi_prima.Add(fs_prima * As_i[i]);
            }
            Ps_prima = Psi_prima.Sum();
            return Ps_prima;
        }

        private double Calc_Vc(double Vu, double Mu, double Pu)
        {
            double Vc1, Vc2, Vc3;
            double Numerador, Denominador, Ag;

            Ag = bw * lw;
            Vc1 = 0.53 * Math.Sqrt(Fc) * (bw * dw) / 1000;   //Tonf
            Vc2 = (0.88 * Math.Sqrt(Fc) * (bw * dw) + (Pu * 1000 * dw / (4 * lw))) / 1000;    //Tonf

            Numerador = (0.33 * Math.Sqrt(Fc) + (Pu * 1000 / (5 * lw * bw))) * lw; //Unidades en centimetros
            Denominador = ((Mu * Math.Pow(10, 5)) / (Vu * 1000)) - (lw / 2);   //Unidad en centimetros

            if (Denominador <= 0)
            {
                Vc3 = Math.Pow(10, 10);
            }
            else
            {
                Vc3 = bw * dw * (0.16 * Math.Sqrt(Fc) + (Numerador / Denominador)) / 1000;
            }

            if (Pu > 0)
            {
                return Math.Max(Vc1, Math.Min(Vc2, Vc3));
            }
            else
            {
                double Vtraccion;
                Vtraccion = (0.53 * Math.Sqrt(Fc) + 0.53 * Math.Sqrt(Fc) * Pu * 1000 / (35 * Ag)) / 1000;
                if (Vtraccion < 0) Vtraccion = 0;
                return Vtraccion;
            }
        }

        private double Calc_alpah(float Htotal)
        {
            double alpha, Relacion, m;

            alpha = 0;
            Relacion = Htotal / lw;
            m = (0.53 - 0.80) / (2 - 1.50);

            if (Relacion <= 1.50) alpha = 0.80;
            if (Relacion >= 2) alpha = 0.53;
            if (Relacion > 1.50 & Relacion < 2) alpha = m * (Relacion - 1.50) + 0.80;

            return alpha;
        }

        private double Calc_ptt(double alpha, double Vu, double Phi, double Fy)
        {
            double Rho_tt;
            Rho_tt = (Vu / (Phi * bw * lw) - alpha * Math.Sqrt(Fc)) / Fy;
            if (Rho_tt < 0) return 0; else return Rho_tt;
        }
    }
}