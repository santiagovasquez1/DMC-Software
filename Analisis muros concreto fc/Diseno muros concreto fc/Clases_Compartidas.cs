﻿using Diseño_de_muros_concreto_V2;
using System;

namespace Diseno_muros_concreto_fc
{
    [Serializable]
    public class Refuerzo_muros : Diseño_de_muros_concreto_V2.Refuerzo_muros
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
    public class Listas_Serializadas_i : Listas_serializadas
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
}