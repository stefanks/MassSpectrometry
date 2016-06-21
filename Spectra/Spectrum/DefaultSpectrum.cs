using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectra.Spectrum
{
    class DefaultSpectrum : ISpectrum<DefaultPeak>
    {
        List<DefaultPeak> peaks;

        public DefaultSpectrum(double[] xArray, double[] yArray)
        {
            peaks = new List<DefaultPeak>();
            for (int i = 0; i < xArray.Count(); i++)
                peaks.Add(new DefaultPeak(xArray[i], yArray[i]));
        }

        public int Count
        {
            get
            {
                return peaks.Count;
            }
        }

        public bool ContainsAnyPeaks()
        {
            return peaks.Count > 0;
        }

        public bool ContainsAnyPeaksWithinRange(IRange<double> range)
        {
            throw new NotImplementedException();
        }

        public bool ContainsAnyPeaksWithinRange(double minX, double maxX)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<DefaultPeak> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return peaks[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
