// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (DefaultMzSpectrum.cs) is part of MassSpectrometry.
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
using System.Collections.Generic;

namespace Spectra
{
    public class DefaultMzSpectrum : MzSpectrum<MzPeak, DefaultMzSpectrum>
    {

        #region constructors

        public DefaultMzSpectrum() : base()
        {
        }

        public DefaultMzSpectrum(DefaultMzSpectrum MZSpectrum) : base(MZSpectrum)
        {
        }

        public DefaultMzSpectrum(double[] mz, double[] intensities, bool shouldCopy = true) : base(mz, intensities, shouldCopy)
        {
        }

        #endregion

        #region overriding methods

        public override DefaultMzSpectrum Clone()
        {
            return new DefaultMzSpectrum(this);
        }

        public override DefaultMzSpectrum CorrectMasses(Func<double, double> convertor)
        {
            DefaultMzSpectrum newSpectrum = Clone();
            for (int i = 0; i < newSpectrum.Count; i++)
                newSpectrum.Masses[i] = convertor(newSpectrum.Masses[i]);
            return newSpectrum;
        }

        public override DefaultMzSpectrum Extract(double minMZ, double maxMZ)
        {
            double[] mz;
            double[] intensity;
            ExtractProtected(minMZ, maxMZ, out mz, out intensity);
            return new DefaultMzSpectrum(mz, intensity, false);
        }

        public override DefaultMzSpectrum FilterByIntensity(double minIntensity = 0, double maxIntensity = double.MaxValue)
        {
            double[] mz;
            double[] intensity;
            FilterByIntensityProtected(minIntensity, maxIntensity, out mz, out intensity);
            return new DefaultMzSpectrum(mz, intensity, false);
        }

        public override DefaultMzSpectrum FilterByMZ(IEnumerable<IRange<double>> mzRanges)
        {
            double[] mz;
            double[] intensity;
            FilterByMZProtected(mzRanges, out mz, out intensity);
            return new DefaultMzSpectrum(mz, intensity, false);
        }

        public override DefaultMzSpectrum FilterByMZ(double minMZ, double maxMZ)
        {
            double[] mz;
            double[] intensities;
            FilterByMZProtected(minMZ, maxMZ, out mz, out intensities);
            return new DefaultMzSpectrum(mz, intensities, false);
        }

        public override DefaultMzSpectrum FilterByNumberOfMostIntense(int topNPeaks)
        {
            double[] mz;
            double[] intensities;
            FilterByNumberOfMostIntenseProtected(topNPeaks, out mz, out intensities);
            return new DefaultMzSpectrum(mz, intensities, false);
        }

        public override MzPeak GetPeak(int index)
        {
            return new MzPeak(Masses[index], Intensities[index]);
        }

        #endregion

    }
}
