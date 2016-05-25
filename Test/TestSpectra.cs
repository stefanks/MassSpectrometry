﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
//
// This file (SpectrumTestFixture.cs) is part of MassSpectrometry.Tests.
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

using NUnit.Framework;
using Spectra;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture, Category("Spectral")]
    public sealed class SpectrumTestFixture
    {
        private MZSpectrum<MzPeak> _mzSpectrumA;

        [SetUp]
        public void Setup()
        {
            double[] mz = { 328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345 };
            double[] intensities = { 81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0 };

            _mzSpectrumA = new MZSpectrum<MzPeak>(mz, intensities);
        }
        

        #region Properties

        [Test]
        public void SpectrumCount()
        {
            Assert.AreEqual(15, _mzSpectrumA.Count);
        }

        [Test]
        public void SpectrumFirstMZ()
        {
            Assert.AreEqual(328.73795, _mzSpectrumA.FirstMZ);
        }

        [Test]
        public void SpectrumLastMZ()
        {
            Assert.AreEqual(723.35345, _mzSpectrumA.LastMZ);
        }

        #endregion Properties

        [Test]
        public void SpectrumBasePeakIntensity()
        {
            double basePeakIntensity = _mzSpectrumA.GetBasePeakIntensity();

            Assert.AreEqual(122781408.0, basePeakIntensity);
        }

        [Test]
        public void SpectrumTIC()
        {
            double tic = _mzSpectrumA.GetTotalIonCurrent();

            Assert.AreEqual(843998894.0, tic);
        }

        [Test]
        public void SpectrumGetMasses()
        {
            double[] mz = { 328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345 };
            double[] masses = _mzSpectrumA.GetMasses();

            Assert.AreEqual(mz, masses);
        }

        [Test]
        public void SpectrumGetIntensities()
        {
            double[] intensities = { 81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0 };
            double[] intensities2 = _mzSpectrumA.GetIntensities();

            Assert.AreEqual(intensities, intensities2);
        }

        [Test]
        public void SpectrumToArray()
        {
            double[,] data = _mzSpectrumA.ToArray();
            double[,] realData =
            {
                {328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345}
                , {81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0}
            };

            Assert.AreEqual(data, realData);
        }

        [Test]
        public void SpectrumGetIntensityFirst()
        {
            double intensity = _mzSpectrumA.GetIntensity(0);

            Assert.AreEqual(81007096.0, intensity);
        }

        [Test]
        public void SpectrumGetIntensityRandom()
        {
            double intensity = _mzSpectrumA.GetIntensity(6);

            Assert.AreEqual(44238040.0, intensity);
        }

        [Test]
        public void SpectrumGetMassFirst()
        {
            double intensity = _mzSpectrumA.GetMass(0);

            Assert.AreEqual(328.73795, intensity);
        }

        [Test]
        public void SpectrumGetMassRandom()
        {
            double intensity = _mzSpectrumA.GetMass(6);

            Assert.AreEqual(482.90393, intensity);
        }

        #region Contains Peak

        [Test]
        public void SpectrumContainsPeak()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsAnyPeaks());
        }

        [Test]
        public void SpectrumContainsPeakInRange()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsPeak(448.23987 - 0.001, 448.23987 + 0.001));
        }

        [Test]
        public void SpectrumContainsPeakInRangeEnd()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsPeak(448.23987 - 0.001, 448.23987));
        }

        [Test]
        public void SpectrumContainsPeakInRangeStart()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsPeak(448.23987, 448.23987 + 0.001));
        }

        [Test]
        public void SpectrumContainsPeakInRangeStartEnd()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsPeak(448.23987, 448.23987));
        }

        [Test]
        public void SpectrumContainsPeakInRangeBackwards()
        {
            Assert.IsFalse(_mzSpectrumA.ContainsPeak(448.23987 + 0.001, 448.23987 - 0.001));
        }

        [Test]
        public void SpectrumDoesntContainPeakInRange()
        {
            Assert.IsFalse(_mzSpectrumA.ContainsPeak(603.4243 - 0.001, 603.4243 + 0.001));
        }

        #endregion Contains Peak

        [Test]
        public void SpectrumMassRange()
        {
            MzRange range = new MzRange(328.73795, 723.35345);

            Assert.AreEqual(range, _mzSpectrumA.GetMzRange());
        }
        

        [Test]
        public void SpectrumFilterCount()
        {
            var filteredMzSpectrum = _mzSpectrumA.FilterByIntensity(28604417, 28604419);

            Assert.AreEqual(1, filteredMzSpectrum.Count);
        }

        [Test]
        public void SpectrumSelect()
        {
            ISpectrum<IPeak> v2 = _mzSpectrumA;
            IEnumerable<IPeak> v3 = v2;
            v3.Take(4);
            var v4 = v3.Select(b => b.X);
            Assert.AreEqual(328.73795, v4.First());
        }
    }
}