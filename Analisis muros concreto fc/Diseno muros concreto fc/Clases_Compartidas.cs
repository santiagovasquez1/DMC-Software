using Diseño_de_muros_concreto_V2;
using System;
using System.Collections.Generic;

namespace Diseno_muros_concreto_fc
{
    [Serializable]
    public class refuerzo_muros : Refuerzo_muros
    {
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
    }

    [Serializable]
    public class Alzado_muro : alzado_muro
    {
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
    }

    [Serializable]
    public class Listas_Serializadas_i :Listas_serializadas
    {
        public new List<Muros_Consolidados> Lista_Muros;        
        public string Capacidad_proyecto;
        public List<Muro> Muros_generales;

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
    }
}