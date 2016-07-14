// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
//
// This file (MsDataScan.cs) is part of MassSpectrometry.
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
using System;

namespace MassSpectrometry
{
    public sealed class MsDataScan<TSpectrum> : IMsDataScan<TSpectrum>, IEquatable<MsDataScan<TSpectrum>>
        where TSpectrum : IMzSpectrum<MzPeak>
    {
        private double isolationMZ;
        private string precursorID;
        private int selectedIonGuessChargeStateGuess;
        private double selectedIonGuessIsolationIntensity;
        private double selectedIonGuessMZ;
        private DissociationType dissociationType;
        private double isolationWidth;
        private int precursorScanNumber;

        /// <summary>
        /// The mass spectrum associated with the scan
        /// </summary>
        public TSpectrum MassSpectrum { get; private set; }

        public int ScanNumber { get; private set; }

        public int MsnOrder { get; private set; }

        public double RetentionTime { get; private set; }

        public Polarity Polarity { get; private set; }

        public MZAnalyzerType MzAnalyzer { get; private set; }

        public MzRange MzRange { get; private set; }

        public string ScanFilter { get; private set; }

        public bool isCentroid { get; private set; }

        public string id { get; private set; }

        public double InjectionTime { get; private set; }

        public MsDataScan(int ScanNumber, TSpectrum MassSpectrum, string id, int MsnOrder, bool isCentroid, Polarity Polarity, double RetentionTime, MzRange MzRange, string ScanFilter, MZAnalyzerType MzAnalyzer, double InjectionTime)
        {
            this.ScanNumber = ScanNumber;
            this.MassSpectrum = MassSpectrum;
            this.id = id;
            this.MsnOrder = MsnOrder;
            this.isCentroid = isCentroid;
            this.Polarity = Polarity;
            this.RetentionTime = RetentionTime;
            this.MzRange = MzRange;
            this.ScanFilter = ScanFilter;
            this.MzAnalyzer = MzAnalyzer;
            this.InjectionTime = InjectionTime;
        }
        public MsDataScan(int ScanNumber, TSpectrum MassSpectrum, string id, int MsnOrder, bool isCentroid, Polarity Polarity, double RetentionTime, MzRange MzRange, string ScanFilter, MZAnalyzerType MzAnalyzer, double InjectionTime, string precursorID, double selectedIonGuessMZ, int selectedIonGuessChargeStateGuess, double selectedIonGuessIsolationIntensity, double isolationMZ, double isolationWidth, DissociationType dissociationType, int precursorScanNumber)
            : this(ScanNumber, MassSpectrum, id, MsnOrder, isCentroid, Polarity, RetentionTime, MzRange, ScanFilter, MzAnalyzer, InjectionTime)
        {
            this.isolationMZ = isolationMZ;
            this.precursorID = precursorID;
            this.selectedIonGuessChargeStateGuess = selectedIonGuessChargeStateGuess;
            this.selectedIonGuessIsolationIntensity = selectedIonGuessIsolationIntensity;
            this.selectedIonGuessMZ = selectedIonGuessMZ;
            this.dissociationType = dissociationType;
            this.isolationWidth = isolationWidth;
            this.precursorScanNumber = precursorScanNumber;
        }

        public override string ToString()
        {
            return string.Format("Scan #{0}", ScanNumber);
        }

        public bool Equals(MsDataScan<TSpectrum> other)
        {
            if (ReferenceEquals(this, other)) return true;
            return false;
        }

        public bool TryGetPrecursorID(out string PrecursorID)
        {
            if (MsnOrder == 1)
            {
                PrecursorID = null;
                return false;
            }
            PrecursorID = precursorID;
            return true;
        }

        public bool TryGetSelectedIonGuessChargeStateGuess(out int SelectedIonGuessChargeStateGuess)
        {
            if (MsnOrder == 1)
            {
                SelectedIonGuessChargeStateGuess = 0;
                return false;
            }
            SelectedIonGuessChargeStateGuess = selectedIonGuessChargeStateGuess;
            return true;
        }

        public bool TryGetSelectedIonGuessIsolationIntensity(out double SelectedIonGuessIsolationIntensity)
        {
            if (MsnOrder == 1)
            {
                SelectedIonGuessIsolationIntensity = double.NaN;
                return false;
            }
            SelectedIonGuessIsolationIntensity = selectedIonGuessIsolationIntensity;
            return true;
        }

        public bool TryGetSelectedIonGuessMZ(out double SelectedIonGuessMZ)
        {
            if (MsnOrder == 1)
            {
                SelectedIonGuessMZ = double.NaN;
                return false;
            }
            SelectedIonGuessMZ = selectedIonGuessMZ;
            return true;
        }

        public bool TryGetDissociationType(out DissociationType DissociationType)
        {
            if (MsnOrder == 1)
            {
                DissociationType = DissociationType.None;
                return false;
            }
            DissociationType = dissociationType;
            return true;
        }

        public bool TryGetIsolationWidth(out double IsolationWidth)
        {
            if (MsnOrder == 1)
            {
                IsolationWidth = double.NaN;
                return false;
            }
            IsolationWidth = isolationWidth;
            return true;
        }
        public bool TryGetIsolationMZ(out double IsolationMZ)
        {
            if (MsnOrder == 1)
            {
                IsolationMZ = double.NaN;
                return false;
            }
            IsolationMZ = isolationMZ;
            return true;
        }

        public bool TryGetIsolationRange(out MzRange IsolationRange)
        {
            IsolationRange = null;
            if (MsnOrder == 1)
                return false;

            double isolationMz;
            TryGetIsolationMZ(out isolationMz);
            double isolationWidth;
            TryGetIsolationWidth(out isolationWidth);
            IsolationRange = new MzRange(isolationMz - isolationWidth / 2, isolationMz + isolationWidth / 2);

            return true;

        }

        public bool TryGetPrecursorScanNumber(out int PrecursorScanNumber)
        {
            if (MsnOrder == 1)
            {
                PrecursorScanNumber = -1;
                return false;
            }
            PrecursorScanNumber = precursorScanNumber;
            return true;
        }

        public void tranformByApplyingFunctionToX(Func<double, double> convertorForSpectrum, Func<double, double> convertorForPrecursor)
        {
            MassSpectrum.tranformByApplyingFunctionToX(convertorForSpectrum);
            selectedIonGuessMZ = convertorForPrecursor(selectedIonGuessMZ);
        }
    }
}
