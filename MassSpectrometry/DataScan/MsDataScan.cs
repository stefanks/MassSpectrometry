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
    public class MsDataScan<TSpectrum> : IMsDataScan<TSpectrum>, IEquatable<MsDataScan<TSpectrum>>
        where TSpectrum : IMzSpectrum<MzPeak>
    {
        /// <summary>
        /// The mass spectrum associated with the scan
        /// </summary>
        public TSpectrum MassSpectrum { get; internal set; }

        public int SpectrumNumber { get; protected set; }
        
        public double Resolution { get; internal set; }

        public int MsnOrder { get; protected set; }
        
        public virtual double InjectionTime { get; internal set; }
        
        public double RetentionTime { get; internal set; }
        
        public Polarity Polarity { get; internal set; }
        
        public MZAnalyzerType MzAnalyzer { get; internal set; }
        
        public DoubleRange MzRange { get; internal set; }
        
        public int ParentScanNumber { get; internal set; }
        
        public string ScanFilter { get; internal set; }
        
        public bool isCentroid { get; internal set; }
        
        public string id { get; internal set; }
        
        public string PrecursorID { get; internal set; }

        public double SelectedIonMonoisotopicMZ { get; internal set; }

        public int SelectedIonChargeState { get; internal set; }
        
        public double SelectedIonIsolationIntensity { get; internal set; }

        public MsDataScan(int SpectrumNumber, TSpectrum MassSpectrum, string id, int MsnOrder, bool isCentroid, Polarity Polarity, double RetentionTime, string PrecursorID = null, double SelectedIonMonoisotopicMZ = double.NaN, int SelectedIonChargeState = 0, double SelectedIonIsolationIntensity = double.NaN)
        {
            this.SpectrumNumber = SpectrumNumber;
            this.MassSpectrum = MassSpectrum;
            this.id = id;
            this.MsnOrder = MsnOrder;
            this.isCentroid = isCentroid;
            this.Polarity = Polarity;
            this.RetentionTime = RetentionTime;
            this.PrecursorID = PrecursorID;
            this.SelectedIonMonoisotopicMZ = SelectedIonMonoisotopicMZ;
            this.SelectedIonChargeState = SelectedIonChargeState;
            this.SelectedIonIsolationIntensity = SelectedIonIsolationIntensity;
            MzRange = new DoubleRange(MassSpectrum.GetMzRange());
        }

        public override string ToString()
        {
            return string.Format("Scan #{0}", SpectrumNumber);
        }

        public bool Equals(MsDataScan<TSpectrum> other)
        {
            if (ReferenceEquals(this, other)) return true;
            return false;
        }
    }
}
