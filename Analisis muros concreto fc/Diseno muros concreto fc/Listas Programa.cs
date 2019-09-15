using System;
using System.Collections.Generic;

namespace Diseno_muros_concreto_fc
{
    [Serializable]
    internal class Listas_Programa
    {
        public static List<Puntos_Modelo> Lista_Puntos;
        public static List<Shells_Prop> Lista_shells;
        public static List<Esfuerzos> Lista_Esfuerzos;
        public static List<Muro> Lista_Muros;
        public static List<Muro> Muros_insuficientes=new List<Muro>();
        public static List<Material> Lista_Materiales;
        public static string Texto_combo;
        public static string Capacidad;
        public static string Ruta_archivo;
        public static string Ruta_Carpeta;
        public static string Name_Proyecto;
        public static List<Muros_Consolidados_1> Muros_Consolidados_Listos;
        public static string Area_ParaTenorApprox = "";
        [NonSerialized] public static List<Procesar_info.Muros_Error> Muros_errores=new List<Procesar_info.Muros_Error>();
        [NonSerialized] public static string Error_Cortante = "";
        [NonSerialized] public static string Error_Flexion = "";
    }
}