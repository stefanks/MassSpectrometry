﻿// Copyright 2016 Stefan Solntsev
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
        public FakeMsDataFile(string filePath, MsDataScan<DefaultMzSpectrum>[] Scans) : base(filePath, true, MsDataFileType.UnKnown)
        {
            this.Scans = Scans;
        }

        public override int GetParentSpectrumNumber(int spectrumNumber)
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
