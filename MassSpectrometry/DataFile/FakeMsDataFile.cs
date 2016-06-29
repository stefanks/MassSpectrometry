// Copyright 2016 Stefan Solntsev
// 
// This file (FakeMsDataFile.cs) is part of MassSpectrometry.
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
using System.Linq;

namespace MassSpectrometry
{
    public class FakeMsDataFile : MsDataFile<DefaultMzSpectrum>
    {
        public FakeMsDataFile(string filePath, MsDataFileType filetype = MsDataFileType.UnKnown) : base(filePath, true, filetype)
        {
        }

        public void Add(MsDataScan<DefaultMzSpectrum>[] Scans)
        {
            this.Scans = Scans;
        }

        public override DissociationType GetDissociationType(int spectrumNumber, int msnOrder = 2)
        {
            throw new NotImplementedException();
        }

        public override double GetInjectionTime(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override bool GetIsCentroid(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override MzRange GetIsolationRange(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override double GetIsolationWidth(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetMsnOrder(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override MZAnalyzerType GetMzAnalyzer(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override MzRange GetMzRange(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetParentSpectrumNumber(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override Polarity GetPolarity(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override string GetPrecursorID(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override double GetPrecursorIsolationIntensity(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override double GetPrecursorIsolationMz(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override double GetPrecursorMonoisotopicMz(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetPrecusorCharge(int spectrumNumber, int msnOrder = 2)
        {
            throw new NotImplementedException();
        }

        public override double GetResolution(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override double GetRetentionTime(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override string GetScanFilter(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override string GetSpectrumID(int spectrumNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetSpectrumNumber(double retentionTime)
        {
            int ok = Array.BinarySearch(Scans.Select(b => b.RetentionTime).ToArray(), retentionTime);
            if (ok < 0)
                ok = ~ok;
            Console.WriteLine("Returning spectrum number " + (ok + FirstSpectrumNumber));
            return ok + FirstSpectrumNumber;
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        protected override int GetFirstSpectrumNumber()
        {
            return 1;
        }

        protected override int GetLastSpectrumNumber()
        {
            return Scans.Count();
        }

        protected override MsDataScan<DefaultMzSpectrum> GetMsDataScanFromFile(int spectrumNumber)
        {
            throw new NotImplementedException();
        }
    }
}
