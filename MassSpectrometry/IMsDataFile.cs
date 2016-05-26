// Copyright 2012, 2013, 2014 Derek J. Bailey
// 
// This file (IMsDataFile.cs) is part of MassSpectrometry.
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
using Spectra;
using MassSpectrometry.Enums;

namespace MassSpectrometry
{

    public interface IMsDataFile<out TSpectrum> : IEnumerable<IMsDataScan<IMzSpectrum<MzPeak>>>, IDisposable
        where TSpectrum : IMzSpectrum<MzPeak>
    {
        TSpectrum GetSpectrum(int spectrumNumber);
        IMsDataScan<TSpectrum> this[int spectrumNumber] { get; }
        void Open();
        string Name { get; }
        bool IsOpen { get; }
        int FirstSpectrumNumber { get; }
        int LastSpectrumNumber { get; }
        int GetMsnOrder(int spectrumNumber);
        double GetInjectionTime(int spectrumNumber);
        double GetPrecursorMonoisotopicMz(int spectrumNumber);
        double GetRetentionTime(int spectrumNumber);
        DissociationType GetDissociationType(int spectrumNumber, int msnOrder = 2);
        Polarity GetPolarity(int spectrumNumber);
    }
}