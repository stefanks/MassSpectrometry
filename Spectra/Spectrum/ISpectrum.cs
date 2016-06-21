// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
//
// This file (ISpectrum.cs) is part of MassSpectrometry.
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
    public interface ISpectrum
    {
        double FirstX { get; }

        double LastX { get; }

        double GetX(int index);

        double GetY(int index);

        int Count { get; }

        double[] GetCopyofXarray();

        double[] GetCopyofYarray();

        double GetYofPeakWithHighestY();

        double GetSumOfAllY();

        byte[] ToBytes(bool zlibCompressed);

        double[,] CopyTo2DArray();

        bool ContainsAnyPeaksWithinRange(double minX, double maxX);
    }

    public interface ISpectrum<out TPeak> : ISpectrum, IEnumerable<TPeak>
        where TPeak : Peak
    {

        TPeak this[int index] { get; }

        TPeak GetClosestPeak(double mean);

        TPeak GetPeakWithHighestY();

        TPeak GetClosestPeak(IRange<double> rangeX);
    }

    public interface ISpectrum<out TPeak, out TRange> : ISpectrum<TPeak>
        where TPeak : Peak
        where TRange : DoubleRange
    {
        TRange GetRange();
    }

    public interface ISpectrum<out TPeak, out TRange, out TSpectrum> : ISpectrum<TPeak, TRange>
        where TPeak : Peak
        where TRange : DoubleRange
        where TSpectrum : ISpectrum<TPeak, TRange, TSpectrum>
    {

        TSpectrum newSpectrumFilterByNumberOfMostIntense(int topNPeaks);

        TSpectrum newSpectrumExtract(IRange<double> xRange);

        TSpectrum newSpectrumExtract(double minX, double maxX);

        TSpectrum newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges);

        TSpectrum newSpectrumWithRangeRemoved(IRange<double> xRange);

        TSpectrum newSpectrumWithRangeRemoved(double minX, double maxX);

        TSpectrum newSpectrumFilterByY(double minY, double maxY);

        TSpectrum newSpectrumFilterByY(IRange<double> yRange);

        TSpectrum newSpectrumApplyFunctionToX(Func<double, double> convertor);

    }
}