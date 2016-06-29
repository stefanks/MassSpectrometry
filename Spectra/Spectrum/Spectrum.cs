﻿// Copyright 2016 Stefan Solnts// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (Spectrum.cs) is part of MassSpectrometry.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectra
{
    public abstract class Spectrum<TPeak, TSpectrum> : ISpectrum<TPeak, TSpectrum>
        where TPeak : Peak
        where TSpectrum : Spectrum<TPeak, TSpectrum>
    {

        #region fields

        // Populated on demand
        protected TPeak[] peakList;

        #endregion

        #region properties
        public virtual TPeak this[int index]
        {
            get
            {
                if (peakList[index] == null)
                    peakList[index] = (TPeak)Activator.CreateInstance(typeof(TPeak), new object[] { xArray[index], yArray[index] });
                return peakList[index];
            }
        }

        public double FirstX { get { return xArray[0]; } }
        public double LastX { get { return xArray[Count - 1]; } }

        public int Count { get { return xArray.Length; } }

        public double[] xArray { get; private set; }
        public double[] yArray { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new spectrum
        /// </summary>
        /// <param name="x">The m/z's</param>
        /// <param name="xy">The intensities</param>
        /// <param name="shouldCopy">Indicates whether the input arrays should be copied to new ones</param>
        public Spectrum(double[] x, double[] y, bool shouldCopy)
        {
            if (shouldCopy)
            {
                xArray = new double[x.Length];
                yArray = new double[y.Length];
                Array.Copy(x, xArray, x.Length);
                Array.Copy(y, yArray, y.Length);
            }
            else
            {
                xArray = x;
                yArray = y;
            }
            peakList = new TPeak[Count];
        }

        /// <summary>
        /// Initializes a new spectrum from another spectrum
        /// </summary>
        /// <param name="spectrumToClone">The spectrum to clone</param>
        public Spectrum(ISpectrum<Peak> spectrumToClone)
            : this(spectrumToClone.xArray, spectrumToClone.yArray, true)
        {
        }


        /// <summary>
        /// Initializes a new spectrum
        /// </summary>
        /// <param name="xy"></param>
        public Spectrum(double[,] xy)
            : this(xy, xy.GetLength(1))
        {
        }

        public Spectrum(double[,] mzintensities, int count)
        {
            int length = mzintensities.GetLength(1);

            xArray = new double[count];
            yArray = new double[count];
            Buffer.BlockCopy(mzintensities, 0, xArray, 0, sizeof(double) * count);
            Buffer.BlockCopy(mzintensities, sizeof(double) * length, yArray, 0, sizeof(double) * count);
            peakList = new TPeak[Count];
        }

        #endregion


        #region public methods

        public override string ToString()
        {
            return string.Format("{0} (Peaks {1})", GetRange(), Count);
        }

        #endregion

        #region implementing ISpectrum

        public virtual TSpectrum newSpectrumFilterByNumberOfMostIntense(int topNPeaks)
        {

            double[] newXarray = new double[xArray.Length];
            double[] newYarray = new double[yArray.Length];
            Array.Copy(xArray, newXarray, xArray.Length);
            Array.Copy(yArray, newYarray, yArray.Length);

            Array.Sort(newYarray, newXarray, Comparer<double>.Create((i1, i2) => i2.CompareTo(i1)));

            double[] newXarray2 = new double[topNPeaks];
            double[] newYarray2 = new double[topNPeaks];
            Array.Copy(newXarray, newXarray2, topNPeaks);
            Array.Copy(newYarray, newYarray2, topNPeaks);

            Array.Sort(newXarray2, newYarray2);

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { newXarray2, newYarray2, false });

        }

        public virtual TSpectrum newSpectrumWithRangeRemoved(double minX, double maxX)
        {

            Console.WriteLine("minX " + minX + " maxX " + maxX);
            int count = Count;

            // Peaks to remove
            HashSet<int> indiciesToRemove = new HashSet<int>();

            int index = Array.BinarySearch(xArray, minX);
            if (index < 0)
                index = ~index;

            while (index < count && xArray[index] <= maxX)
            {
                indiciesToRemove.Add(index);
                index++;
            }

            Console.WriteLine("index " + index);

            // The size of the cleaned spectrum
            int cleanCount = count - indiciesToRemove.Count;
            Console.WriteLine("The size of the cleaned spectrum " + cleanCount);

            // Create the storage for the cleaned spectrum
            double[] newXarray = new double[cleanCount];
            double[] newYarray = new double[cleanCount];

            // Transfer peaks from the old spectrum to the new one
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (indiciesToRemove.Contains(i))
                    continue;
                newXarray[j] = xArray[i];
                newYarray[j] = yArray[i];
                j++;
            }

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { newXarray, newYarray, false });

        }

        public virtual TSpectrum newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges)
        {

            int count = Count;

            // Peaks to remove
            HashSet<int> indiciesToRemove = new HashSet<int>();

            // Loop over each range to remove
            foreach (IRange<double> range in xRanges)
            {
                double min = range.Minimum;
                double max = range.Maximum;

                int index = Array.BinarySearch(xArray, min);
                if (index < 0)
                    index = ~index;

                while (index < count && xArray[index] <= max)
                {
                    indiciesToRemove.Add(index);
                    index++;
                }
            }

            // The size of the cleaned spectrum
            int cleanCount = count - indiciesToRemove.Count;
            Console.WriteLine("The size of the cleaned spectrum " + cleanCount);


            // Create the storage for the cleaned spectrum
            double[] newXarray = new double[cleanCount];
            double[] newYarray = new double[cleanCount];

            // Transfer peaks from the old spectrum to the new one
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (indiciesToRemove.Contains(i))
                    continue;
                newXarray[j] = xArray[i];
                newYarray[j] = yArray[i];
                j++;
            }

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { newXarray, newYarray, false });

        }

        public virtual TSpectrum newSpectrumExtract(double minX, double maxX)
        {

            int index = GetClosestPeakIndex(minX);

            int count = Count;
            double[] newXarray = new double[count];
            double[] newYarray = new double[count];
            int j = 0;

            while (index < Count && xArray[index] <= maxX)
            {
                newXarray[j] = xArray[index];
                newYarray[j] = yArray[index];
                index++;
                j++;
            }

            Array.Resize(ref newXarray, j);
            Array.Resize(ref newYarray, j);

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { newXarray, newYarray, false });

        }

        public virtual TSpectrum newSpectrumFilterByY(double minY, double maxY)
        {
            int count = Count;
            double[] newXarray = new double[count];
            double[] newYarray = new double[count];
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                double intensity = yArray[i];
                if (intensity >= minY && intensity < maxY)
                {
                    newXarray[j] = xArray[i];
                    newYarray[j] = intensity;
                    j++;
                }
            }

            if (j != count)
            {
                Array.Resize(ref newXarray, j);
                Array.Resize(ref newYarray, j);
            }
            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { newXarray, newYarray, false });

        }

        public virtual TSpectrum newSpectrumApplyFunctionToX(Func<double, double> convertor)
        {
            double[] modifiedXarray = new double[Count];
            for (int i = 0; i < Count; i++)
                modifiedXarray[i] = convertor(xArray[i]);
            double[] newYarray = new double[yArray.Length];
            Array.Copy(yArray, newYarray, yArray.Length);
            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { modifiedXarray, newYarray, false });
        }

        public virtual double[,] CopyTo2DArray()
        {
            double[,] data = new double[2, Count];
            const int size = sizeof(double);
            Buffer.BlockCopy(xArray, 0, data, 0, size * Count);
            Buffer.BlockCopy(yArray, 0, data, size * Count, size * Count);
            return data;
        }

        public virtual DoubleRange GetRange()
        {
            return new DoubleRange(FirstX, LastX);
        }

        public double GetX(int index)
        {
            return xArray[index];
        }

        public double GetY(int index)
        {
            return yArray[index];
        }

        public double GetSumOfAllY()
        {
            return yArray.Sum();
        }

        public double GetYofPeakWithHighestY()
        {
            return yArray.Max();
        }

        public TSpectrum newSpectrumFilterByY(IRange<double> yRange)
        {
            return newSpectrumFilterByY(yRange.Minimum, yRange.Maximum);
        }

        public TSpectrum newSpectrumWithRangeRemoved(IRange<double> xRange)
        {
            return newSpectrumWithRangeRemoved(xRange.Minimum, xRange.Maximum);
        }

        public TSpectrum newSpectrumExtract(IRange<double> xRange)
        {
            return newSpectrumExtract(xRange.Minimum, xRange.Maximum);
        }

        public TPeak GetClosestPeak(IRange<double> rangeX)
        {
            double mean = (rangeX.Maximum + rangeX.Minimum) / 2.0;
            return GetClosestPeak(mean);
        }

        public TPeak GetClosestPeak(double x)
        {
            return this[GetClosestPeakIndex(x)];
        }


        public double GetClosestPeakXvalue(double x)
        {
            return xArray[GetClosestPeakIndex(x)];
        }

        private TPeak _peakWithHighestY;

        public TPeak GetPeakWithHighestY()
        {
            if (_peakWithHighestY == null)
                _peakWithHighestY = this[Array.IndexOf(yArray, yArray.Max())];
            return _peakWithHighestY;
        }

        #endregion

        #region protected methods

        protected int GetClosestPeakIndex(double targetX)
        {
            if (Count == 0)
                throw new IndexOutOfRangeException("No peaks in spectrum!");

            int index = Array.BinarySearch(xArray, targetX);
            if (index >= 0)
                return index;
            index = ~index;

            int indexm1 = index - 1;

            if (index >= Count)
            {
                // only the indexm1 peak can be closer

                if (indexm1 >= 0)
                {
                    return indexm1;
                }
                return -1;
            }
            if (index == 0)
            {
                // only the index can be closer
                return index;
            }

            double p1 = xArray[indexm1];
            double p2 = xArray[index];

            if (targetX - p1 > p2 - targetX)
                return index;
            return indexm1;
        }

        #endregion

        #region enumeration
        public IEnumerator<TPeak> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsAnyPeaksWithinRange(double minX, double maxX)
        {
            if (Count == 0)
                return false;

            int index = Array.BinarySearch(xArray, minX);
            if (index >= 0)
                return true;

            index = ~index;

            return index < Count && xArray[index] <= maxX;
        }

        #endregion

        #region ISpectrum<TPeak> methods
        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumFilterByNumberOfMostIntense(int topNPeaks)
        {
            return newSpectrumFilterByNumberOfMostIntense(topNPeaks);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumExtract(IRange<double> xRange)
        {
            return newSpectrumExtract(xRange);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumExtract(double minX, double maxX)
        {
            return newSpectrumExtract(minX, maxX);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumWithRangesRemoved(IEnumerable<IRange<double>> xRanges)
        {
            return newSpectrumWithRangesRemoved(xRanges);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumWithRangeRemoved(IRange<double> xRange)
        {
            return newSpectrumWithRangeRemoved(xRange);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumWithRangeRemoved(double minX, double maxX)
        {
            return newSpectrumWithRangeRemoved(minX, maxX);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumFilterByY(double minY, double maxY)
        {
            return newSpectrumFilterByY(minY, maxY);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumFilterByY(IRange<double> yRange)
        {
            return newSpectrumFilterByY(yRange);
        }

        ISpectrum<TPeak> ISpectrum<TPeak>.newSpectrumApplyFunctionToX(Func<double, double> convertor)
        {
            return newSpectrumApplyFunctionToX(convertor);
        }

        #endregion

    }
}