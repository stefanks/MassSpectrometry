namespace Spectra
{
    public class DefaultSpectrum : Spectrum<DefaultPeak, DoubleRange, DefaultSpectrum>
    {
        public DefaultSpectrum(ISpectrum<Peak, DoubleRange> spectrum) : base(spectrum)
        {
        }

        public DefaultSpectrum(double[] x, double[] y, bool shouldCopy) : base(x, y, shouldCopy)
        {
        }


        public DefaultSpectrum(double[,] xy)
            : base(xy)
        {
        }

    }
}
