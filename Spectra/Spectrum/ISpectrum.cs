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
    public interface ISpectrum<out TPeak> : IEnumerable<TPeak>
        where TPeak : Peak
    {

        double[] xArray { get; }

        double[] yArray { get; }

        double FirstX { get; }

        double LastX { get; }

        double GetX(int index);

        double GetY(int index);

        int Count { get; }

        double GetYofPeakWithHighestY();

        double GetSumOfAllY();

        double[,] CopyTo2DArray();

        bool ContainsAnyPeaksWithinRange(double minX, double maxX);

        DoubleRange GetRange();

        TPeak this[int index] { get; }

        TPeak GetClosestPeak(double x);

        double GetClosestPeakXvalue(double x);

        TPeak GetPeakWithHighestY();

        TPeak GetClosestPeak(IRange<double> rangeX);

        ISpectrum<TPeak> newSpectrumFilterByNumberOfMostIntense(int topNPeaks);
        ISpectrum<TPeak> newSpectrumExtract(IRange<double> xRange);
        ISpectrum<TPeak> newSpectrumExtract(double minX, double maxX);
        ISpectrum<TPeak> newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges);
        ISpectrum<TPeak> newSpectrumWithRangeRemoved(IRange<double> xRange);
        ISpectrum<TPeak> newSpectrumWithRangeRemoved(double minX, double maxX);
        ISpectrum<TPeak> newSpectrumFilterByY(double minY, double maxY);
        ISpectrum<TPeak> newSpectrumFilterByY(IRange<double> yRange);
        ISpectrum<TPeak> newSpectrumApplyFunctionToX(Func<double, double> convertor);
    }

    public interface ISpectrum<out TPeak, out TSpectrum> : ISpectrum<TPeak>
        where TPeak : Peak
        where TSpectrum : ISpectrum<TPeak, TSpectrum>
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