using System;

namespace ComplexAlgebra
{
    /// <summary>
    /// A type for representing Complex numbers.
    /// </summary>
    public class Complex
    {
        public readonly double Real;
        public readonly double Imaginary;
        public readonly double Modulus;
        public readonly double Phase;

        public Complex(double real, double imaginary)
        {
            Imaginary = imaginary;
            Real = real;
            Modulus = Math.Sqrt(Math.Pow(Real, 2) + Math.Pow(Imaginary, 2));
            Phase = Math.Atan2(Imaginary, Real);
        }

        public Complex Complement() => new Complex(Real, -Imaginary);
        
        public Complex Plus(Complex num) => new Complex(Real + num.Real, Imaginary + num.Imaginary);
        
        public Complex Minus(Complex num) => new Complex(Real - num.Real, Imaginary - num.Imaginary);

        private bool Equals(Complex other)
        {
            return Real.Equals(other.Real) && Imaginary.Equals(other.Imaginary);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Complex) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Real, Imaginary);
        }

        private string PrintMinusOrPlus() => Imaginary >= 0 ? "+" : "-";

        public override string ToString()
        {
            if (Imaginary == 0d)
            {
                return $"{Real}";
            }

            if (Real == 0d)
            {
                if (Imaginary.CompareTo(1) == 0)
                {
                    return "i";
                }
                if (Imaginary.CompareTo(-1) == 0)
                {
                    return "-i";
                }
                return $"i{Imaginary}";
            }

            string ImString = Math.Abs(Imaginary).CompareTo(1) == 0 ? "" : Math.Abs(Imaginary).ToString();
            return $"{Real}{this.PrintMinusOrPlus()}i{ImString}";
        }
    }
}