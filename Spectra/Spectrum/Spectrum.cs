using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectra
{
    public abstract class Spectrum<TPeak, TRange, TSpectrum> : ISpectrum<TPeak, TRange, TSpectrum>
        where TPeak : Peak
        where TRange : DoubleRange
        where TSpectrum : Spectrum<TPeak, TRange, TSpectrum>
    {

        #region protected fields

        // Populated at creation and stays constant
        protected double[] xArray;
        protected double[] yArray;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new spectrum
        /// </summary>
        /// <param name="mz">The m/z's</param>
        /// <param name="intensities">The intensities</param>
        /// <param name="shouldCopy">Indicates whether the input arrays should be copied to new ones</param>
        public Spectrum(double[] mz, double[] intensities, bool shouldCopy = true)
        {
            xArray = ClassExtensionsAndHelpers.CopyData(mz, shouldCopy);
            yArray = ClassExtensionsAndHelpers.CopyData(intensities, shouldCopy);
            peakList = new TPeak[Count];
        }

        /// <summary>
        /// Initializes a new spectrum from another spectrum
        /// </summary>
        /// <param name="MZSpectrum">The spectrum to clone</param>
        public Spectrum(TSpectrum MZSpectrum)
            : this(MZSpectrum.xArray, MZSpectrum.yArray)
        {
        }


        /// <summary>
        /// Initializes a new spectrum
        /// </summary>
        /// <param name="mzintensities"></param>
        public Spectrum(double[,] mzintensities)
            : this(mzintensities, mzintensities.GetLength(1))
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

        public Spectrum(byte[] mzintensities)
        {
            int size = sizeof(double) * mzintensities.Length / (sizeof(double) * 2);
            xArray = new double[mzintensities.Length / (sizeof(double) * 2)];
            yArray = new double[mzintensities.Length / (sizeof(double) * 2)];
            Buffer.BlockCopy(mzintensities, 0, xArray, 0, size);
            Buffer.BlockCopy(mzintensities, size, yArray, 0, size);
            peakList = new TPeak[Count];
        }

        #endregion


        public virtual TSpectrum newSpectrumFilterByNumberOfMostIntense(int topNPeaks)
        {

            double[] mz = ClassExtensionsAndHelpers.CopyData(xArray);
            double[] intensities = ClassExtensionsAndHelpers.CopyData(yArray);

            IComparer<double> mycomparer = new ReverseComparer();

            Array.Sort(intensities, mz, mycomparer);

            mz = mz.SubArray(0, topNPeaks);
            intensities = intensities.SubArray(0, topNPeaks);

            Array.Sort(mz, intensities);

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { mz, intensities, false });

        }


        #region public methods

        public override string ToString()
        {
            return string.Format("{0} (Peaks {1})", GetRange(), Count);
        }

        #endregion

        #region implementing ISpectrum

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
            double[] mz = new double[cleanCount];
            double[] intensities = new double[cleanCount];

            // Transfer peaks from the old spectrum to the new one
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (indiciesToRemove.Contains(i))
                    continue;
                mz[j] = xArray[i];
                intensities[j] = yArray[i];
                j++;
            }

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { mz, intensities, false });

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
            double[] mz = new double[cleanCount];
            double[] intensities = new double[cleanCount];

            // Transfer peaks from the old spectrum to the new one
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (indiciesToRemove.Contains(i))
                    continue;
                mz[j] = xArray[i];
                intensities[j] = yArray[i];
                j++;
            }

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { mz, intensities, false });

        }

        public virtual TSpectrum newSpectrumExtract(double minX, double maxX)
        {

            int index = GetClosestPeakIndex(minX);

            int count = Count;
            double[] mz = new double[count];
            double[] intensity = new double[count];
            int j = 0;

            while (index < Count && xArray[index] <= maxX)
            {
                mz[j] = xArray[index];
                intensity[j] = yArray[index];
                index++;
                j++;
            }

            Array.Resize(ref mz, j);
            Array.Resize(ref intensity, j);

            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { mz, intensity, false });

        }

        public virtual TSpectrum newSpectrumFilterByY(double minY, double maxY)
        {
            int count = Count;
            double[] mz = new double[count];
            double[] intensities = new double[count];
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                double intensity = yArray[i];
                if (intensity >= minY && intensity < maxY)
                {
                    mz[j] = xArray[i];
                    intensities[j] = intensity;
                    j++;
                }
            }


            if (j != count)
            {
                Array.Resize(ref mz, j);
                Array.Resize(ref intensities, j);
            }
            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { mz, intensities, false });

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

        public virtual TSpectrum newSpectrumApplyFunctionToX(Func<double, double> convertor)
        {
            double[] modifiedXarray = new double[Count];
            for (int i = 0; i < Count; i++)
                modifiedXarray[i] = convertor(xArray[i]);
            return (TSpectrum)Activator.CreateInstance(typeof(TSpectrum), new object[] { modifiedXarray, ClassExtensionsAndHelpers.CopyData(yArray) });
        }

        public double[,] CopyTo2DArray()
        {
            double[,] data = new double[2, Count];
            const int size = sizeof(double);
            Buffer.BlockCopy(xArray, 0, data, 0, size * Count);
            Buffer.BlockCopy(yArray, 0, data, size * Count, size * Count);
            return data;
        }

        public byte[] ToBytes(bool zlibCompressed = false)
        {
            return ClassExtensionsAndHelpers.ToBytes(zlibCompressed, Count, xArray, yArray);
        }

        public double GetYofPeakWithHighestY()
        {
            return yArray.Max();
        }

        public TPeak GetClosestPeak(IRange<double> rangeX)
        {
            double mean = (rangeX.Maximum + rangeX.Minimum) / 2.0;
            return GetClosestPeak(mean);
        }
        public TPeak GetClosestPeak(double mean)
        {
            return this[GetClosestPeakIndex(mean)];
        }

        public TPeak GetPeakWithHighestY()
        {
            return this[yArray.MaxIndex()];
        }

        public virtual TRange GetRange()
        {
            return (TRange)Activator.CreateInstance(typeof(TRange), new object[] { FirstX, LastX });
        }


        public double[] GetCopyofXarray()
        {
            return ClassExtensionsAndHelpers.CopyData(xArray);
        }
        public double[] GetCopyofYarray()
        {
            return ClassExtensionsAndHelpers.CopyData(yArray);
        }

        #endregion

        #region private methods

        private int GetClosestPeakIndex(double targetX)
        {
            if (Count == 0)
                return -1;

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

    }

    internal class ReverseComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x > y)
                return -1;
            else if (x < y)
                return 1;
            else
                return 0;
        }
    }
}