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
    public interface IMzSpectrum<out TPeak, out TRange> : ISpectrum<TPeak, TRange>
        where TPeak : MzPeak
        where TRange : MzRange
    {
        new IMzSpectrum<TPeak, TRange> newSpectrumFilterByNumberOfMostIntense(int topNPeaks);
        new IMzSpectrum<TPeak, TRange> newSpectrumExtract(IRange<double> xRange);
        new IMzSpectrum<TPeak, TRange> newSpectrumExtract(double minX, double maxX);
        new IMzSpectrum<TPeak, TRange> newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges);
        new IMzSpectrum<TPeak, TRange> newSpectrumWithRangeRemoved(IRange<double> xRange);
        new IMzSpectrum<TPeak, TRange> newSpectrumWithRangeRemoved(double minX, double maxX);
        new IMzSpectrum<TPeak, TRange> newSpectrumFilterByY(double minY, double maxY);
        new IMzSpectrum<TPeak, TRange> newSpectrumFilterByY(IRange<double> yRange);
        new IMzSpectrum<TPeak, TRange> newSpectrumApplyFunctionToX(Func<double, double> convertor);
    }

    public interface IMzSpectrum<out TPeak, out TRange, out TSpectrum> : IMzSpectrum<TPeak, TRange>, ISpectrum<TPeak, TRange, TSpectrum>
        where TPeak : MzPeak
        where TRange : MzRange
        where TSpectrum : IMzSpectrum<TPeak, TRange, TSpectrum>
    {
        new TSpectrum newSpectrumFilterByNumberOfMostIntense(int topNPeaks);
        new TSpectrum newSpectrumExtract(IRange<double> xRange);
        new TSpectrum newSpectrumExtract(double minX, double maxX);
        new TSpectrum newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges);
        new TSpectrum newSpectrumWithRangeRemoved(IRange<double> xRange);
        new TSpectrum newSpectrumWithRangeRemoved(double minX, double maxX);
        new TSpectrum newSpectrumFilterByY(double minY, double maxY);
        new TSpectrum newSpectrumFilterByY(IRange<double> yRange);
        new TSpectrum newSpectrumApplyFunctionToX(Func<double, double> convertor);
    }
}