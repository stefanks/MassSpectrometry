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


using System;
using System.Collections.Generic;

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

        public MzSpectrum(ISpectrum<Peak, DoubleRange> mZSpectrum) : base(mZSpectrum)
        {
        }

        public MzSpectrum(double[] mz, double[] intensities, bool shouldCopy) : base(mz, intensities, shouldCopy)
        {
        }

        #region implementing IMzSpectrum<TPeak, TRange>
        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumApplyFunctionToX(Func<double, double> convertor)
        {
            return newSpectrumApplyFunctionToX(convertor);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumExtract(IRange<double> xRange)
        {
            return newSpectrumExtract(xRange);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumExtract(double minX, double maxX)
        {
            return newSpectrumExtract(minX, maxX);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumFilterByNumberOfMostIntense(int topNPeaks)
        {
            return newSpectrumFilterByNumberOfMostIntense(topNPeaks);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumFilterByY(IRange<double> yRange)
        {
            return newSpectrumFilterByY(yRange);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumFilterByY(double minY, double maxY)
        {
            return newSpectrumFilterByY(minY, maxY);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumWithRangeRemoved(IRange<double> xRange)
        {
            return newSpectrumWithRangeRemoved(xRange);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumWithRangeRemoved(double minX, double maxX)
        {
            return newSpectrumWithRangeRemoved(minX, maxX);
        }

        IMzSpectrum<TPeak, TRange> IMzSpectrum<TPeak, TRange>.newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges)
        {
            return newSpectrumWithRangesRemoved(xRanges);
        }
        #endregion
    }
}