// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (MzSpectrum.cs) is part of MassSpectrometry.
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


namespace Spectra
{
    public abstract class MzSpectrum<TPeak, TRange, TSpectrum> : Spectrum<TPeak, TRange, TSpectrum>, IMzSpectrum<TPeak, TRange, TSpectrum>
        where TPeak : MzPeak
        where TRange : MzRange
        where TSpectrum : MzSpectrum<TPeak, TRange, TSpectrum>
    {

        public MzSpectrum(double[,] mzintensities) : base(mzintensities)
        {
        }

        public MzSpectrum(TSpectrum mZSpectrum) : base(mZSpectrum)
        {
        }

        public MzSpectrum(double[] mz, double[] intensities, bool shouldCopy) : base(mz, intensities, shouldCopy)
        {
        }
    }
}