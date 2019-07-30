using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
delegate void Strmod(ref string str);

namespace Diseno_muros_concreto_fc
{
    public class Muros_Consolidados
    {
        public string Pier_name;
        public List<string> Stories = new List<string>();
        public List<float> Bw = new List<float>();
        public List<float> lw = new List<float>();
        public List<float> fc = new List<float>();
        public List<float> Hw = new List<float>();
        public List<double> Rho_T = new List<double>();
        public List<double> Rho_l = new List<double>();
        public List<string> Malla = new List<string>();
        public List<double> Sigma_piso = new List<double>();
        public List<string> Confinamiento = new List<string>();
        public List<double> C_max = new List<double>();
        public List<double> C_min = new List<double>();
        public List<double> C_esfuerzo = new List<double>();
        public List<double> C_Def = new List<double>();
        public List<double> L_esfuerzo = new List<double>();
        public List<double> L_Conf_Max = new List<double>();
        public List<double> L_Conf_Min = new List<double>();
        public List<double> Lebe_Izq = new List<double>();
        public List<double> Lebe_Der = new List<double>();
        public List<double> Lebe_Centro = new List<double>();
        public List<double> Zc_Izq = new List<double>();
        public List<double> Zc_Der = new List<double>();

        //Nuevas variables
        public List<int> Est_ebe = new List<int>();
        public List<double> Sep_ebe = new List<double>();
        public List<int> Est_Zc = new List<int>();
        public List<double> Sep_Zc = new List<double>();
        public List<double> As_Long = new List<double>();
        //
        public List<int> ramas_der = new List<int>();
        public List<int> ramas_izq = new List<int>();
        public List<int> ramas_centro = new List<int>();
        public List<double> As_htal = new List<double>();
        public List<string> Ref_htal = new List<string>();
        public List<int> Capas_htal = new List<int>();
        public List<double> sep_htal = new List<double>();
        public List<double> As_Htal_Total = new List<double>();
        //
        public List<List<Shells_Prop>> Shells_piso_Izq = new List<List<Shells_Prop>>();
        public List<List<Shells_Prop>> Shells_piso_der = new List<List<Shells_Prop>>();

       
    }
   public class Strops
    {
        static void Reemplazar_espacios(ref string a)
        {
            Console.WriteLine("Reemplaza espacios con guiones");
            a=a.Replace(' ', '-');
        }

        static void Eliminar_espacios(ref string a)
        {
            string temp = "";
            int i;

            Console.WriteLine("Elimina espacios");

            for (i = 0; i < a.Length; i++)
            {
                if (a[i] != ' ') temp += a[i];
            }

            a= temp;
        }


        static void Invierte(ref string a)
        {
            string temp = "";
            int i,j;
    
        Console.WriteLine("Invierte una cadena.");
            for (j = 0, i = a.Length-1; i >= 0;i--, j++)
                temp += a[i];
            a = temp;
        }

        
        public static void main() //Ejemplo de distribucion multiple de delegados
        {

            //Constructor de delegados
            Strmod StrOp; //Construye una instancia del delegado
            Strmod ReemplazaSp = Reemplazar_espacios;
            Strmod Eliminarsp = Eliminar_espacios;
            Strmod InvierteStr = Invierte;

            string Str= "Esta es una prueba";

            //Establecer distribucion multiple

            StrOp = ReemplazaSp;
            StrOp += InvierteStr;

            //invocacion distribucion multiple
            StrOp(ref Str);
            Console.WriteLine("Cadena resultante" + Str);
            Console.WriteLine("");

            //Elimina reemplazo y agrega remover
            StrOp -= ReemplazaSp;
            StrOp += Eliminarsp;

            Str = "Esta es una prueba"; //Reestablece la cadena

             //invocacion distribucion multiple
            StrOp(ref Str);
            Console.WriteLine("Cadena resultante" + Str);
            Console.WriteLine("");

        }

    }

}


namespace Diseno_muros_concreto_fc
{
    public class Procesar_info
    {
        public static void Compilar_Datos()
        {
            
            List<double> prueba;
            int indice;
            double Factor1, Factor2;
            double Xmax, Xmin, Ymax, Ymin;
            Muros_Consolidados Muro_i;
            List<string> Muros_distintos = Listas_Programa.Lista_Muros.Select(x => x.Pier).Distinct().ToList();
            Listas_Programa.Muros_Consolidados_Listos = new List<Muros_Consolidados>();

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

            for (int i= 0; i < Muros_distintos.Count; i++)
            {
                List<Muro> Auxiliar = Listas_Programa.Lista_Muros.FindAll(x => x.Pier == Muros_distintos[i]).ToList();

                Muro_i = new Muros_Consolidados();
                Muro_i.Pier_name = Muros_distintos[i];
                Muro_i.Stories.AddRange(Auxiliar.Select(x => x.Story).Distinct().ToList());
                Muro_i.fc.AddRange(Auxiliar.Select(x => x.Fc));
                Muro_i.Bw.AddRange(Auxiliar.Select(x => x.bw));
                Muro_i.lw.AddRange(Auxiliar.Select(x => x.lw));
                Muro_i.Hw.AddRange(Auxiliar.Select(x => x.hw));
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
                        prueba = Auxiliar[j].Sigma_Max.FindAll(x=> x>=Factor2*Muro_i.fc[j]);
                    }
                    catch
                    {
                        prueba = null;
                    }

                    if (prueba.Count>0)
                    {
                        List<double> C_Aux=new List<double>();
                        foreach(double Valor in prueba)
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

                    Xmax = Auxiliar[j].Shells_Muro.Select(x => x.Coord.Max(x1=>x1[0])).Max();
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
                Determinacion_EBE(Muro_i,Factor1,Factor2);
                Determinacion_Lado(Muro_i,Factor2);
                Det_As_Long(Muro_i);
                Det_At(Muro_i);
                Listas_Programa.Muros_Consolidados_Listos.Add(Muro_i);
               
            }
            
        }

        static List<Shells_Prop>Seleccion_Muros(List<Shells_Prop>Shells_i,double Parametro,int indice)
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

        private static void Determinacion_EBE(Muros_Consolidados Muro_i,double Limite1,double Limite2)
        {
            for (int j = Muro_i.Stories.Count - 1; j >= 0; j--)
            {
                if (j == Muro_i.Stories.Count -1)
                {
                    if (Muro_i.Sigma_piso[j] >= Limite1 * Muro_i.fc[j]) Muro_i.Confinamiento[j] = "Si";  
                    if (Muro_i.Rho_l[j]>=0.01) Muro_i.Confinamiento[j] = "Si";
                }
                else
                {
                    if (Muro_i.Sigma_piso[j] >= Limite2 * Muro_i.fc[j] & Muro_i.Confinamiento[j + 1] == "Si") Muro_i.Confinamiento[j] = "Si";
                    if (Muro_i.Rho_l[j] >= 0.01) Muro_i.Confinamiento[j] = "Si";
                }

            }

        }

        private static void Determinacion_Lado(Muros_Consolidados Muro_i,double Limite)

        {
            for(int i = 0; i < Muro_i.Confinamiento.Count; i++)
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
                        if (Muro_i.Shells_piso_Izq[i].Select(x => x.S22).Min() <= -Limite * Muro_i.fc[i]*10)
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
                if (Muro_i.Rho_l[i]>=0.0066 & Muro_i.Lebe_Izq[i] > 0 & Muro_i.Lebe_Der[i]==0 & Muro_i.Rho_l[i] < 0.01)
                {
                    Muro_i.Zc_Der[i]= Muro_i.L_Conf_Max[i];
                    
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

        private static void Det_As_Long(Muros_Consolidados Muro_i)
        {
            double Aux_As_Long, Acero_malla,Aux_Long;            
            for (int i = 0; i < Muro_i.Stories.Count; i++)
            {
                Aux_As_Long = Muro_i.Bw[i] * Muro_i.lw[i] * Muro_i.Rho_l[i];
                Aux_Long = Muro_i.lw[i] - Muro_i.Lebe_Izq[i] - Muro_i.Lebe_Der[i]-Muro_i.Lebe_Centro[i]-Muro_i.Zc_Izq[i]-Muro_i.Zc_Der[i];
                Acero_malla = (Aux_Long / 100)*Al_Malla(Muro_i.Malla[i]);

                if (Aux_As_Long - Acero_malla > 0)
                {
                    Muro_i.As_Long[i] = Aux_As_Long - Acero_malla;
                }

            }
        }

        private static void Det_At(Muros_Consolidados Muro_i)
        {
            double Aux_As_t, Acero_malla, Aux_As_tm;
            for (int i = 0; i < Muro_i.Stories.Count; i++)
            {
                if (Muro_i.Rho_T[i] < 0.0020) Muro_i.Rho_T[i]=0.0020;
                Aux_As_t = Muro_i.Bw[i] * 100 * Muro_i.Rho_T[i];
                Aux_As_tm= Al_Malla(Muro_i.Malla[i]);

                if (Aux_As_t - Aux_As_tm > 0)
                {
                    Muro_i.As_htal[i] = Aux_As_t - Aux_As_tm;
                }
            }
        }

        static string Det_malla(double rt,double rl,float espesor)
        {
            string Malla;
            Malla = "Sin Malla";

            if(espesor>=8 & espesor < 10)
            {
                if (rt >= 0.0012 & rt < 0.0020 & rl<0.01) Malla = "D106";
                if (rt >= 0.0020 & rt < 0.0025 & rl < 0.01) Malla = "D188";
                if (rt >= 0.0025 & rl < 0.01) Malla = "DD106";
            }

            if (espesor >=10 & espesor < 12)
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

        static double Al_Malla(string Malla)
        {
           
            double Aux_acero=0;
            switch (Malla)

            {
                case "Sin Malla":
                    Aux_acero = 0;
                    break;
                case "D84":
                    Aux_acero =0.84;
                    break;
                case "D106":
                    Aux_acero =1.06;
                    break;
                case "D131":
                    Aux_acero = 1.31;
                    break;
                case "D158":
                    Aux_acero = 1.58;
                    break;
                case "D188":
                    Aux_acero =1.88;
                    break;
                case "D221":
                    Aux_acero = 2.21;
                    break;
                case "D257":
                    Aux_acero = 2.57;
                    break;

                case "DD84":
                    Aux_acero = 0.84*2;
                    break;
                case "DD106":
                    Aux_acero = 1.06*2;
                    break;
                case "DD131":
                    Aux_acero = 1.31*2;
                    break;
                case "DD158":
                    Aux_acero = 1.58*2;
                    break;
                case "DD188":
                    Aux_acero = 1.88*2;
                    break;
                case "DD221":
                    Aux_acero = 2.21*2;
                    break;
                case "DD257":
                    Aux_acero = 2.57*2;
                    break;
            }
            return Aux_acero;
        }
       
    }
}
