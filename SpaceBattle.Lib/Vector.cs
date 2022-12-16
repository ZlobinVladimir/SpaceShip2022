using System;
    public class Vector
    {
        public int[] Vector_;
        public int Size;
        public Vector(params int[] coord)
        {
            Vector_ = coord;
            Size = coord.Length;
        }
        public int this[int i]
        {
            get
            {
                return Vector_[i];
            }

        }
        public override string ToString()
        {
            string vivod = "Vector(";
            for (int i = 0; i < Size; i++)
            {
                if (i != Size - 1)
                    vivod += $"{Vector_[i]}, ";
                else
                    vivod += $"{Vector_[i]}";
            }
            vivod += ")";
            return vivod;
        }

        public static Vector operator +(Vector A, Vector B)
        {
            if (A.Size != B.Size)
                throw new System.ArgumentException();
            else
            {
                int[] vectSum = new int[A.Size];
                for (int i = 0; i < A.Size; i++)
                    vectSum[i] = A[i] + B[i];
                return new Vector(vectSum);
            }
        }
        public static Vector operator -(Vector A, Vector B)
        {
            if (A.Size != B.Size)
                throw new System.ArgumentException();
            else
            {
                int[] vectRazn = new int[A.Size];
                for (int i = 0; i < A.Size; i++)
                    vectRazn[i] = A[i] - B[i];
                return new Vector(vectRazn);
            }
        }
        public static Vector operator *(int a, Vector A)
        {
            int[] vectMnozh = new int[A.Size];
            for (int i = 0; i < A.Size; i++)
                vectMnozh[i] = A[i] * a;
            return new Vector(vectMnozh);
        }
        public static bool operator ==(Vector A, Vector B)
        {
            if (A.Size != B.Size)
            {
                return false;
            }
            for (int i = 0; i < A.Size; i++)
            {
                if (A[i] != B[i]) return false;
            }
            return true;
        }
        public static bool operator !=(Vector A, Vector B)
        {
            return !(A == B);
        }

        public static bool operator <(Vector A, Vector B)
        {
            if (A == B) return false;
            if (A.Size > B.Size) return false;
            for (int i = 0; i < Math.Min(A.Size, B.Size); i++)
                if (A[i] > B[i]) return false;
            return true;
        }
        public static bool operator >(Vector A, Vector B)
        {
            return !(A < B);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            throw new NotImplementedException();
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Vector_);
        }
    }