// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
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

using Spectra;
using System.Collections.Generic;

namespace MassSpectrometry
{

    public interface IMsDataFile<out TSpectrum> : IEnumerable<IMsDataScan<TSpectrum>>
        where TSpectrum : IMzSpectrum<MzPeak, MzRange, TSpectrum>
    {
        TSpectrum GetSpectrum(int spectrumNumber);
        IMsDataScan<TSpectrum> GetScan(int scanNumber);
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
        string FilePath { get; }

    }
}