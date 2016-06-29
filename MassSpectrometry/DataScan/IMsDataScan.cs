﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
// 
// This file (IMsDataScan.cs) is part of MassSpectrometry.
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

namespace MassSpectrometry
{

    public interface IMsDataScan<out TSpectrum>
        where TSpectrum : IMzSpectrum<MzPeak>
    {
        TSpectrum MassSpectrum { get; }
        int ScanNumber { get; }
        int MsnOrder { get; }
        double RetentionTime { get; }
        MzRange MzRange { get; }
        string ScanFilter { get; }
        string id { get; }
        bool isCentroid { get; }
        double InjectionTime { get; }
        Polarity Polarity { get; }
        MZAnalyzerType MzAnalyzer { get; }
        bool TryGetPrecursorScanNumber(out int precursorScanNumber);
        bool TryGetPrecursorID(out string PrecursorID);
        bool TryGetSelectedIonChargeState(out int SelectedIonChargeState);
        bool TryGetSelectedIonIsolationIntensity(out double SelectedIonIsolationIntensity);
        bool TryGetSelectedIonMZ(out double SelectedIonMZ);
        bool TryGetDissociationType(out DissociationType DissociationType);
        bool TryGetIsolationWidth(out double IsolationWidth);
        bool TryGetIsolationMZ(out double IsolationMZ);
        bool TryGetIsolationRange(out MzRange IsolationRange);
    }
}