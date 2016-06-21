﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
// 
// This file (MsDataFile.cs) is part of MassSpectrometry.
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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MassSpectrometry
{
    /// <summary>
    /// A data file for storing data collected from a Mass Spectrometer
    /// </summary>
    public abstract class MsDataFile<TSpectrum> : IMsDataFile<TSpectrum>
        where TSpectrum : IMzSpectrum<MzPeak, MzRange, TSpectrum>
    {
        /// <summary>
        /// Defines if MS scans should be cached for quicker retrieval. Cached scans are held in an internal
        /// array and don't get cleared until the file is disposed or the ClearCacheScans() method is called.
        /// Of course, if you store the scans somewhere else, they will persist. The default value is True.
        /// </summary>
        public static bool CacheScans;

        internal MsDataScan<TSpectrum>[] Scans = null;

        private string _filePath;

        private bool _isOpen;

        private string _name;

        static MsDataFile()
        {
            CacheScans = true;
        }

        protected MsDataFile(string filePath, MsDataFileType filetype = MsDataFileType.UnKnown)
        {
            FilePath = filePath;
            FileType = filetype;
            _isOpen = false;
        }

        public string FilePath
        {
            get { return _filePath; }
            private set
            {
                _filePath = value;
                _name = Path.GetFileNameWithoutExtension(value);
            }
        }

        public MsDataFileType FileType { get; private set; }

        public abstract string GetSpectrumID(int spectrumNumber);

        bool _firstSpectrumNumberSet = false;
        int _firstSpectrumNumber;
        public virtual int FirstSpectrumNumber
        {
            get
            {
                if (_firstSpectrumNumberSet)
                    return _firstSpectrumNumber;
                _firstSpectrumNumberSet = true;
                _firstSpectrumNumber = GetFirstSpectrumNumber();
                return _firstSpectrumNumber;
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            protected set { _isOpen = value; }
        }

        bool _lastSpectrumNumberSet = false;
        int _lastSpectrumNumber;
        public virtual int LastSpectrumNumber
        {
            get
            {
                if (_lastSpectrumNumberSet)
                    return _lastSpectrumNumber;
                _lastSpectrumNumberSet = true;
                _lastSpectrumNumber = GetLastSpectrumNumber();
                return _lastSpectrumNumber;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(MsDataFile<TSpectrum> other)
        {
            if (ReferenceEquals(this, other)) return true;
            return FilePath.Equals(other.FilePath);
        }

        public IEnumerator<IMsDataScan<TSpectrum>> GetEnumerator()
        {
            return GetMsScans().GetEnumerator();
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }

        public abstract DissociationType GetDissociationType(int spectrumNumber, int msnOrder = 2);

        public abstract int GetMsnOrder(int spectrumNumber);

        /// <summary>
        /// Get the spectrum number of the parent scan that caused this scan to be executed.
        /// Typically MS1s will return 0 and MS2s will return the preceding MS1 scan (if in DDA mode)
        /// </summary>
        /// <param name="spectrumNumber">The spectrum number to get the parent scan number of</param>
        /// <returns>The spectrum number of the parent scan. 0 if no parent</returns>
        public virtual int GetParentSpectrumNumber(int spectrumNumber)
        {
            return 0;
        }

        public abstract string GetScanFilter(int spectrumNumber);

        /// <summary>
        /// Get the MS Scan at the specific spectrum number.
        /// </summary>
        /// <param name="scanNumber">The spectrum number to get the MS Scan at</param>
        /// <returns></returns>
        public virtual IMsDataScan<TSpectrum> GetScan(int scanNumber)
        {
            if (!CacheScans)
                return GetMsDataScanFromFile(scanNumber);
            if (Scans == null)
                Scans = new MsDataScan<TSpectrum>[LastSpectrumNumber - FirstSpectrumNumber + 1];
            if (Scans[scanNumber - FirstSpectrumNumber] == null)
            {
                Scans[scanNumber - FirstSpectrumNumber] = GetMsDataScanFromFile(scanNumber);
            }

            return Scans[scanNumber - FirstSpectrumNumber];
        }

        public TSpectrum GetSpectrum(int spectrumNumber)
        {
            return GetScan(spectrumNumber).MassSpectrum;
        }

        public abstract bool GetIsCentroid(int spectrumNumber);

        public virtual void LoadAllScansInMemory()
        {
            if (Scans == null)
            {
                Scans = new MsDataScan<TSpectrum>[LastSpectrumNumber - FirstSpectrumNumber + 1];
            }

            for (int scanNumber = FirstSpectrumNumber; scanNumber <= LastSpectrumNumber; scanNumber++)
            {
                if (Scans[scanNumber - FirstSpectrumNumber] == null)
                {
                    Scans[scanNumber - FirstSpectrumNumber] = GetMsDataScanFromFile(scanNumber);
                }
            }
        }

        public abstract string GetPrecursorID(int spectrumNumber);

        public virtual void ClearCachedScans()
        {
            if (Scans == null)
                return;
            Array.Clear(Scans, 0, Scans.Length);
        }

        protected abstract MsDataScan<TSpectrum> GetMsDataScanFromFile(int spectrumNumber);
        //{
        //    // Roundabout way of doing things. The last parameter is key, it's not null so can read the full scan from file eventually. 
        //    return new MsDataScan<TSpectrum>(spectrumNumber, GetMsnOrder(spectrumNumber), this);
        //}

        public abstract int GetPrecusorCharge(int spectrumNumber, int msnOrder = 2);

        public abstract MzRange GetMzRange(int spectrumNumber);

        public abstract double GetPrecursorMonoisotopicMz(int spectrumNumber);

        public abstract double GetPrecursorIsolationMz(int spectrumNumber);

        public abstract double GetIsolationWidth(int spectrumNumber);

        public abstract MzRange GetIsolationRange(int spectrumNumber);
        //{
        //    double precursormz = GetPrecursorIsolationMz(spectrumNumber);
        //    double halfWidth = GetIsolationWidth(spectrumNumber)/2;
        //    return new MzRange(precursormz - halfWidth, precursormz + halfWidth);
        //}

        public abstract double GetPrecursorIsolationIntensity(int spectrumNumber);

        public virtual IEnumerable<IMsDataScan<TSpectrum>> GetMsScans()
        {
            return GetMsScansInIndexRange(FirstSpectrumNumber, LastSpectrumNumber);
        }

        public virtual IEnumerable<IMsDataScan<TSpectrum>> GetMsScansInIndexRange(int FirstSpectrumNumber, int LastSpectrumNumber)
        {
            for (int spectrumNumber = FirstSpectrumNumber; spectrumNumber <= LastSpectrumNumber; spectrumNumber++)
            {
                yield return GetScan(spectrumNumber);
            }
        }

        public virtual IEnumerable<IMsDataScan<TSpectrum>> GetMsScansInTimeRange(double firstRT, double lastRT)
        {
            int spectrumNumber = GetSpectrumNumber(firstRT - 0.0000001);
            while (spectrumNumber <= LastSpectrumNumber)
            {
                IMsDataScan<TSpectrum> scan = GetScan(spectrumNumber++);
                double rt = scan.RetentionTime;
                if (rt < firstRT)
                    continue;
                if (rt > lastRT)
                    yield break;
                yield return scan;
            }
        }

        public virtual IEnumerable<IMsDataScan<TSpectrum>> GetMsScans(IRange<int> range)
        {
            return GetMsScansInIndexRange(range.Minimum, range.Maximum);
        }

        public abstract MZAnalyzerType GetMzAnalyzer(int spectrumNumber);

        public abstract Polarity GetPolarity(int spectrumNumber);

        public abstract double GetRetentionTime(int spectrumNumber);

        public abstract double GetInjectionTime(int spectrumNumber);

        public abstract double GetResolution(int spectrumNumber);

        public abstract int GetSpectrumNumber(double retentionTime);

        /// <summary>
        /// Open up a connection to the underlying MS data stream
        /// </summary>
        public abstract void Open();

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Enum.GetName(typeof(MsDataFileType), FileType));
        }

        protected abstract int GetFirstSpectrumNumber();

        protected abstract int GetLastSpectrumNumber();

    }
}