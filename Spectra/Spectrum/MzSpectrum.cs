﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
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
    public abstract class MzSpectrum<TPeak, TSpectrum> : Spectrum<TPeak, TSpectrum>, IMzSpectrum<TPeak, TSpectrum>
        where TPeak : MzPeak
        where TSpectrum : MzSpectrum<TPeak, TSpectrum>
    {

        protected MzSpectrum(double[,] mzintensities) : base(mzintensities)
        {
        }

        protected MzSpectrum(ISpectrum<Peak> mZSpectrum) : base(mZSpectrum)
        {
        }

        protected MzSpectrum(double[] mz, double[] intensities, bool shouldCopy) : base(mz, intensities, shouldCopy)
        {
        }

        public override DoubleRange GetRange()
        {
            return new MzRange(FirstX, LastX);
        }


        #region implementing IMzSpectrum<TPeak>

        MzRange IMzSpectrum<TPeak>.GetRange()
        {
            return new MzRange(FirstX, LastX);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumApplyFunctionToX(Func<double, double> convertor)
        {
            return newSpectrumApplyFunctionToX(convertor);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumExtract(DoubleRange xRange)
        {
            return newSpectrumExtract(xRange);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumExtract(double minX, double maxX)
        {
            return newSpectrumExtract(minX, maxX);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumFilterByNumberOfMostIntense(int topNPeaks)
        {
            return newSpectrumFilterByNumberOfMostIntense(topNPeaks);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumFilterByY(DoubleRange yRange)
        {
            return newSpectrumFilterByY(yRange);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumFilterByY(double minY, double maxY)
        {
            return newSpectrumFilterByY(minY, maxY);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumWithRangeRemoved(DoubleRange xRange)
        {
            return newSpectrumWithRangeRemoved(xRange);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumWithRangeRemoved(double minX, double maxX)
        {
            return newSpectrumWithRangeRemoved(minX, maxX);
        }

        IMzSpectrum<TPeak> IMzSpectrum<TPeak>.newSpectrumWithRangesRemoved(IEnumerable<DoubleRange> xRanges)
        {
            return newSpectrumWithRangesRemoved(xRanges);
        }
        #endregion
    }
}