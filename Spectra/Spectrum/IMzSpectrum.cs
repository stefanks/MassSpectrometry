// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
//
// This file (IMzSpectrum.cs) is part of MassSpectrometry.
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
    public interface IMzSpectrum<out TPeak> : ISpectrum<TPeak>
        where TPeak : MzPeak
    {
        new MzRange GetRange();
        new IMzSpectrum<TPeak> newSpectrumFilterByNumberOfMostIntense(int topNPeaks);
        new IMzSpectrum<TPeak> newSpectrumExtract(DoubleRange xRange);
        new IMzSpectrum<TPeak> newSpectrumExtract(double minX, double maxX);
        new IMzSpectrum<TPeak> newSpectrumWithRangesRemoved(IEnumerable<DoubleRange> xRanges);
        new IMzSpectrum<TPeak> newSpectrumWithRangeRemoved(DoubleRange xRange);
        new IMzSpectrum<TPeak> newSpectrumWithRangeRemoved(double minX, double maxX);
        new IMzSpectrum<TPeak> newSpectrumFilterByY(double minY, double maxY);
        new IMzSpectrum<TPeak> newSpectrumFilterByY(DoubleRange yRange);
        new IMzSpectrum<TPeak> newSpectrumApplyFunctionToX(Func<double, double> convertor);
    }

    public interface IMzSpectrum<out TPeak, out TSpectrum> : IMzSpectrum<TPeak>, ISpectrum<TPeak, TSpectrum>
        where TPeak : MzPeak
        where TSpectrum : IMzSpectrum<TPeak, TSpectrum>
    {
        new TSpectrum newSpectrumFilterByNumberOfMostIntense(int topNPeaks);
        new TSpectrum newSpectrumExtract(DoubleRange xRange);
        new TSpectrum newSpectrumExtract(double minX, double maxX);
        new TSpectrum newSpectrumWithRangesRemoved(IEnumerable<DoubleRange> xRanges);
        new TSpectrum newSpectrumWithRangeRemoved(DoubleRange xRange);
        new TSpectrum newSpectrumWithRangeRemoved(double minX, double maxX);
        new TSpectrum newSpectrumFilterByY(double minY, double maxY);
        new TSpectrum newSpectrumFilterByY(DoubleRange yRange);
        new TSpectrum newSpectrumApplyFunctionToX(Func<double, double> convertor);
    }
}