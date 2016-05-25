// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (MZPeak.cs) is part of MassSpectrometry.
// 
// MassSpectrometry is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MassSpectrometry is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with MassSpectrometry. If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Spectra
{
    /// <summary>
    /// A peak in a mass spectrum that has a well defined m/z and intenisty value
    /// </summary>
    public class MzPeak : Peak
    {
        public double Intensity {
            get
            {
                return Y;
            }
            private set
            {
                Y = value;
            }
        }

        public double MZ
        {
            get
            {
                return X;
            }
            private set
            {
                X = value;
            }
        }

        public MzPeak(double mz = 0.0, double intensity = 0.0)
        {
            MZ = mz;
            Intensity = intensity;
        }

        public override string ToString()
        {
            return string.Format("({0:F4},{1:G5})", MZ, Intensity);
        }
        
        
        public override bool Equals(object obj)
        {
            return obj is MzPeak && Equals((MzPeak) obj);
        }

        public override int GetHashCode()
        {
            return MZ.GetHashCode() ^ Intensity.GetHashCode();
        }

        public bool Equals(MzPeak other)
        {
            // Odd to use mass equals on intensity, might have to make that more generic sometime
            return MZ.FuzzyEquals(other.MZ) && Intensity.FuzzyEquals(other.Intensity);
        }
    }
}