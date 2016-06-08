// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
//
// This file (TestChromatogram.cs) is part of MassSpectrometry.Tests.
//
// MassSpectrometry.Tests is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MassSpectrometry.Tests is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with MassSpectrometry.Tests. If not, see <http://www.gnu.org/licenses/>.

using MassSpectrometry;
using NUnit.Framework;
using Spectra;
using System;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class ChromatogramTestFixture
    {
        private DefaultMzSpectrum _mzSpectrumA;

        [SetUp]
        public void Setup()
        {
            double[] mz = { 328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345 };
            double[] intensities = { 81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0 };

            _mzSpectrumA = new DefaultMzSpectrum(mz, intensities);
        }

        [Test]
        public void ChromatogramTest()
        {
            Chromatogram a = new Chromatogram(new double[5] { 1, 2, 3, 4, 5 }, new double[5] { 1, 2, 6, 4, 2 });
            var b = a.CreateSmoothChromatogram(SmoothingType.BoxCar, 4);
            Assert.IsTrue(b.GetTimes().SequenceEqual(new double[3] { 2, 3, 4 }));
            Assert.IsTrue(b.GetIntensities().SequenceEqual(new double[3] { 3, 4, 4 }));
            var c = new Chromatogram(a);

            Chromatogram d = new Chromatogram(new double[9] { 1, 2, 3, 4, 5, 6, 7, 8,9 }, new double[9] {10, 0, 2, 6, 2, 0, 1, 10, 1 });
            // Finds the APEX! Not nearest peak!
            Assert.AreEqual(6, d.FindNearestApex(5.9).Intensity);
            Assert.AreEqual(10, d.FindNearestApex(6.1).Intensity);
            // Finds the width of a large peak! Includes the zeros!
            Assert.AreEqual(new DoubleRange(1, 2), d.GetPeakWidth(1));
            Assert.AreEqual(new DoubleRange(2, 6), d.GetPeakWidth(3));
            Assert.AreEqual(new DoubleRange(6, 9), d.GetPeakWidth(9));

            var thePeak = new ChromatographicPeak(1, 10);
            Assert.AreEqual(1, thePeak.Time);
            Assert.AreEqual(10, thePeak.Intensity);
            Assert.AreEqual("(1, 10)", thePeak.ToString());
        }
    }
}