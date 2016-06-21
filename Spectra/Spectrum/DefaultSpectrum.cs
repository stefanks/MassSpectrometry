namespace Spectra
{
    public class DefaultSpectrum : Spectrum<DefaultPeak, DoubleRange, DefaultSpectrum>
    {
        public DefaultSpectrum(DefaultMzSpectrum MZSpectrum) : base(MZSpectrum)
        {
        }

        public DefaultSpectrum(double[] mz, double[] intensities, bool shouldCopy = true) : base(mz, intensities, shouldCopy)
        {
        }


        public DefaultSpectrum(double[,] mzintensities)
            : base(mzintensities)
        {
        }

    }
}
