namespace Spectra
{
    public class DefaultSpectrum : Spectrum<DefaultPeak, DoubleRange, DefaultSpectrum>
    {
        public DefaultSpectrum(ISpectrum MZSpectrum) : base(MZSpectrum)
        {
        }

        public DefaultSpectrum(double[] x, double[] y, bool shouldCopy = true) : base(x, y, shouldCopy)
        {
        }


        public DefaultSpectrum(double[,] xy)
            : base(xy)
        {
        }

    }
}
