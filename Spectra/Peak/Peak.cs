using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectra
{
    public abstract class Peak :  IEquatable<Peak>
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }

        public bool Equals(Peak other)
        {
            if (ReferenceEquals(this, other)) return true;
            return X.FuzzyEquals(other.X) && Y.FuzzyEquals(other.Y);
        }
    }
}
