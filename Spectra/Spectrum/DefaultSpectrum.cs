namespace Spectra.Spectrum
{
    class DefaultSpectrum : Spectrum<DefaultPeak, DoubleRange, DefaultSpectrum>
    {
        public DefaultSpectrum(double[] xArray, double[] yArray) : base(xArray, yArray)
        {
        }
    }
}
