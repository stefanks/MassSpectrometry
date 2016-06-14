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
        /// <summary>
        /// The first m/z of the spectrum
        /// </summary>
        double FirstMZ { get; }

        /// <summary>
        /// The last m/z of the spectrum
        /// </summary>
        double LastMZ { get; }

        /// <summary>
        /// Gets the m/z at a particular index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        double GetMass(int index);

        /// <summary>
        /// Gets the intensity at a particular index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        double GetIntensity(int index);

        /// <summary>
        /// Gets an array of m/z values for this spectrum
        /// </summary>
        /// <returns></returns>
        double[] GetMasses();

        /// <summary>
        /// Gets an array of intensity values for this spectrum, ordered by m/z value
        /// </summary>
        /// <returns></returns>
        double[] GetIntensities();

        /// <summary>
        /// Get the intensity of the most intense peak in this spectrum
        /// </summary>
        /// <returns></returns>
        double GetBasePeakIntensity();

        /// <summary>
        /// Get the sum of the intensities for this spectrum
        /// </summary>
        /// <returns></returns>
        double GetTotalIonCurrent();

        byte[] ToBytes(bool zlibCompressed);

        double[,] ToArray();

        TPeak GetPeak(int index);

        TPeak GetClosestPeak(double mean);

        TPeak GetClosestPeak(IRange<double> rangeMZ);

        TPeak this[int index] { get; }

        TPeak GetBasePeak();

        IMzSpectrum<TPeak> Extract(IRange<double> mzRange);

        IMzSpectrum<TPeak> Extract(double minMZ, double maxMZ);

        IMzSpectrum<TPeak> FilterByMZ(IEnumerable<IRange<double>> mzRanges);

        IMzSpectrum<TPeak> FilterByMZ(IRange<double> mzRange);

        IMzSpectrum<TPeak> FilterByMZ(double minMZ, double maxMZ);

        IMzSpectrum<TPeak> FilterByIntensity(double minIntensity, double maxIntensity);

        IMzSpectrum<TPeak> FilterByIntensity(IRange<double> intenistyRange);

        IMzSpectrum<TPeak> CorrectMasses(Func<double, double> convertor);

        MzRange GetMzRange();
    }
}