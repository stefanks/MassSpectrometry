using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectra
{
    public class MzSpectrum : Spectrum<MzPeak, MzSpectrum>
    {
        public MzSpectrum() : base()
        {
        }

        public MzSpectrum(MzSpectrum MZSpectrum) : base(MZSpectrum)
        {
        }

        public MzSpectrum(double[] mz, double[] intensities, bool shouldCopy = true) : base(mz, intensities, shouldCopy)
        {
        }

        public override MzSpectrum Clone()
        {
            return new MzSpectrum(this);
        }

        public override MzSpectrum CorrectMasses(Func<double, double> convertor)
        {
            MzSpectrum newSpectrum = Clone();
            for (int i = 0; i < newSpectrum.Count; i++)
                newSpectrum.Masses[i] = convertor(newSpectrum.Masses[i]);
            return newSpectrum;
        }

        public override MzSpectrum Extract(double minMZ, double maxMZ)
        {
            double[] mz;
            double[] intensity;
            ExtractProtected(minMZ, maxMZ, out mz, out intensity);
            return new MzSpectrum(mz, intensity, false);
        }

        public override MzSpectrum FilterByIntensity(double minIntensity = 0, double maxIntensity = double.MaxValue)
        {
            double[] mz;
            double[] intensity;
            FilterByIntensityProtected(minIntensity, maxIntensity, out mz, out intensity);
            return new MzSpectrum(mz, intensity, false);
        }

        public override MzSpectrum FilterByMZ(IEnumerable<IRange<double>> mzRanges)
        {
            double[] mz;
            double[] intensity;
            FilterByMZProtected(mzRanges, out mz, out intensity);
            return new MzSpectrum(mz, intensity, false);
        }

        public override MzSpectrum FilterByMZ(double minMZ, double maxMZ)
        {
            double[] mz;
            double[] intensities;
            FilterByMZProtected(minMZ, maxMZ, out mz, out intensities);
            return new MzSpectrum(mz, intensities, false);
        }

        public override MzSpectrum FilterByNumberOfMostIntense(int topNPeaks)
        {
            double[] mz;
            double[] intensities;
            FilterByNumberOfMostIntenseProtected(topNPeaks, out mz, out intensities);
            return new MzSpectrum(mz, intensities, false);
        }

        public override MzPeak GetPeak(int index)
        {
            return new MzPeak(Masses[index], Intensities[index]);
        }
    }
}
