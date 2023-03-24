namespace Vector
{
    public class VectorM
    {
        #region Constructors

        public VectorM(int length, double number = 0)
        {
            content = new double[length];
            for (int i = 0; i < length; i++)
            {
                content[i] = number;
            }
            Size = length;
        }

        public VectorM(double[] massiv)
        {
            content = new double[massiv.Length];
            massiv.CopyTo(content, 0);

            Size = massiv.Length;

            Norm = VectorLength();
        }

        public VectorM(VectorM vector)
        {
            content = new double[vector.Size];
            vector.content.CopyTo(content, 0);

            Size = vector.Size;
        }

        #endregion

        #region Methods and properties

        public int Size { get; }

        public double Norm { get; private set; }

        public int Count { get => content.Length; }

        public void CopyTo(VectorM vector)
        {
            if (Size == vector.Size)
            {
                content.CopyTo(vector.content, 0);

                vector.Norm = Norm;
            }
            else
                throw new RankException("Different length of vectors");
        }

        private double[] content;

        private double VectorLength()
        {
            double sum = 0;
            for (int i = 0; i < Size; i++)
            {
                sum += Math.Pow(content[i], 2);
            }
            return Math.Sqrt(sum);
        }

        #endregion

        #region Overloading and overriding

        public override int GetHashCode()
        {
            return Size;
        }

        public override bool Equals(object? obj)
        {
            if (obj is VectorM vector)
            {
                for (int i = 0; i < vector.Size; i++)
                {
                    if (vector[i] != this[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        public double this[int index]
        {
            get => content[index];
            set => content[index] = value;
        }

        public static VectorM operator +(VectorM vector1, VectorM vector2)
        {
            if (vector1.Size == vector2.Size)
            {
                VectorM vector = new VectorM(vector1);
                for (int i = 0; i < vector1.Size; i++)
                {
                    vector[i] = vector1[i] + vector2[i];
                }
                return vector;
            }
            else
                throw new RankException("Different length of vectors");
        }

        public static VectorM operator -(VectorM vector1, VectorM vector2)
        {
            if (vector1.Size == vector2.Size)
            {
                VectorM vector = new VectorM(vector1);
                for (int i = 0; i < vector1.Size; i++)
                {
                    vector[i] = vector1[i] - vector2[i];
                }
                return vector;
            }
            else
                throw new RankException("Different length of vectors");
        }

        public static VectorM operator +(VectorM vector1, double number)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector1.Size; i++)
            {
                vector[i] = vector1[i] + number;
            }
            return vector;
        }

        public static VectorM operator +(double number, VectorM vector1)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector1.Size; i++)
            {
                vector[i] = vector1[i] + number;
            }
            return vector;
        }

        public static VectorM operator -(double number, VectorM vector1)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector1.Size; i++)
            {
                vector[i] = vector1[i] - number;
            }
            return vector;
        }

        public static double operator *(VectorM vector1, VectorM vector2)
        {
            if (vector1.Size == vector2.Size)
            {
                double product = 0;
                for (int i = 0; i < vector1.Size; i++)
                {
                    product += vector1[i] * vector2[i];
                }
                return product;
            }
            else
                throw new RankException("Different length of vectors");
        }

        public static VectorM operator *(VectorM vector1, double multiplier)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector.Size; i++)
            {
                vector[i] *= multiplier;
            }
            return vector;
        }

        public static VectorM operator *(double multiplier, VectorM vector1)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector.Size; i++)
            {
                vector[i] *= multiplier;
            }
            return vector;
        }

        public static VectorM operator /(VectorM vector1, double multiplier)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector.Size; i++)
            {
                vector[i] /= multiplier;
            }
            return vector;
        }

        public static VectorM operator /(double multiplier, VectorM vector1)
        {
            VectorM vector = new VectorM(vector1);
            for (int i = 0; i < vector.Size; i++)
            {
                vector[i] /= multiplier;
            }
            return vector;
        }

        public static bool operator ==(VectorM vector1, VectorM vector2)
        {
            return vector1.Equals(vector2);
        }

        public static bool operator !=(VectorM vector1, VectorM vector2)
        {
            return !vector1.Equals(vector2);
        }

        public override string ToString()
        {
            string str = "(";
            for (int i = 0; i < Size - 1; i++)
            {
                str += $"{content[i]}; ";
            }
            str += $"{content[Size - 1]})";
            return str;
        }

        #endregion
    }
}
