using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSail
{
    public class Vector
    {
        public double[] vector;
        public static int dim;

        public Vector(params double[] list)
        {
            dim = list.Length;
            vector = new double[dim];
            for (int i = 0; i < dim; ++i)
                vector[i] = list[i];
        }
        public Vector(int dim)
        {
            vector = new double[dim];
        }

        public static Vector operator*(Vector vector, double value)
        {
            Vector tmp = new Vector();
            for (int i = 0; i < dim; i++)
                tmp[i] = vector[i] * value;
            return tmp;
        }
        public static Vector operator *(double value, Vector vector)
        {
            return vector * value;
        }

        public static Vector operator *(Vector vector1, Vector vector2)
        {
            Vector tmp = new Vector();
            for (int i = 0; i < dim; i++)
                tmp[i] = vector1[i] * vector2[i];
            return tmp;
        }

        public static Vector Abs(Vector vector)
        {
            Vector tmp = new Vector();
            for (int i = 0; i < dim; i++)
                tmp[i] = Math.Abs(vector[i]);
            return tmp;
        }

        public static Vector operator -(Vector vec1, Vector vec2)
        {
            return vec1 + (-1 * vec2);
        }

        public static Vector operator +(Vector vec1, Vector vec2)
        {
            Vector tmp = new Vector();
            for (int i = 0; i < dim; i++)
                tmp[i] = vec1[i] + vec2[i];
            return tmp;
        }

        public static Vector operator +(Vector vec, double val)
        {
            Vector tmp = new Vector();
            for (int i = 0; i < dim; i++)
                tmp[i] = vec[i] + val;
            return tmp;
        }

        public static Vector operator +(double val, Vector vec)
        {
            return vec + val;
        }
        public static Vector operator /(Vector vec, double val)
        {
            return vec * (1.0 / val);
        }

        public double this[int index]
        {
            get { return vector[index]; }
            set { vector[index] = value; }
        }
    }
}