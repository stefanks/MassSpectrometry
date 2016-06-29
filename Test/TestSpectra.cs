﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
//
// This file (TestSpectra.cs) is part of MassSpectrometry.Tests.
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class SpectrumTestFixture
    {
        private DefaultMzSpectrum _mzSpectrumA;

        [SetUp]
        public void Setup()
        {
            double[] mz = { 328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345 };
            double[] intensities = { 81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0 };

            _mzSpectrumA = new DefaultMzSpectrum(mz, intensities, false);
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
            Assert.AreEqual(328.73795, _mzSpectrumA.FirstX);
        }

        [Test]
        public void SpectrumLastMZ()
        {
            Assert.AreEqual(723.35345, _mzSpectrumA.LastX);
        }

        #endregion Properties

        [Test]
        public void SpectrumBasePeakIntensity()
        {
            double basePeakIntensity = _mzSpectrumA.GetYofPeakWithHighestY();

            Assert.AreEqual(122781408.0, basePeakIntensity);
        }

        [Test]
        public void SpectrumTIC()
        {
            double tic = _mzSpectrumA.GetSumOfAllY();

            Assert.AreEqual(843998894.0, tic);
        }

        [Test]
        public void SpectrumGetIntensityFirst()
        {
            double intensity = _mzSpectrumA.GetY(0);

            Assert.AreEqual(81007096.0, intensity);
        }

        [Test]
        public void SpectrumGetIntensityRandom()
        {
            double intensity = _mzSpectrumA.GetY(6);

            Assert.AreEqual(44238040.0, intensity);
        }

        [Test]
        public void SpectrumGetMassFirst()
        {
            double intensity = _mzSpectrumA.GetX(0);

            Assert.AreEqual(328.73795, intensity);
        }

        [Test]
        public void SpectrumGetMassRandom()
        {
            double intensity = _mzSpectrumA.GetX(6);

            Assert.AreEqual(482.90393, intensity);
        }

        #region Contains Peak

        [Test]
        public void SpectrumContainsPeak()
        {
            Assert.IsTrue(_mzSpectrumA.Count > 0);
        }

        [Test]
        public void SpectrumContainsPeakInRange()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsAnyPeaksWithinRange(448.23987 - 0.001, 448.23987 + 0.001));
        }

        [Test]
        public void SpectrumContainsPeakInRangeEnd()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsAnyPeaksWithinRange(448.23987 - 0.001, 448.23987));
        }

        [Test]
        public void SpectrumContainsPeakInRangeStart()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsAnyPeaksWithinRange(448.23987, 448.23987 + 0.001));
        }

        [Test]
        public void SpectrumContainsPeakInRangeStartEnd()
        {
            Assert.IsTrue(_mzSpectrumA.ContainsAnyPeaksWithinRange(448.23987, 448.23987));
        }

        [Test]
        public void SpectrumContainsPeakInRangeBackwards()
        {
            Assert.IsFalse(_mzSpectrumA.ContainsAnyPeaksWithinRange(448.23987 + 0.001, 448.23987 - 0.001));
        }

        [Test]
        public void SpectrumDoesntContainPeakInRange()
        {
            Assert.IsFalse(_mzSpectrumA.ContainsAnyPeaksWithinRange(603.4243 - 0.001, 603.4243 + 0.001));
        }

        #endregion Contains Peak

        [Test]
        public void SpectrumMassRange()
        {
            MzRange range = new MzRange(328.73795, 723.35345);

            Assert.AreEqual(range, _mzSpectrumA.GetRange());
        }


        [Test]
        public void SpectrumFilterCount()
        {
            var filteredMzSpectrum = _mzSpectrumA.newSpectrumFilterByY(28604417, 28604419);

            Assert.AreEqual(1, filteredMzSpectrum.Count);
        }

        [Test]
        public void SpectrumSelect()
        {
            MzSpectrum<MzPeak, DefaultMzSpectrum> v2 = _mzSpectrumA;
            ISpectrum<Peak> v3 = v2;

            v3.Take(4);

            var v5 = v3.Select(b => b.X);
            Assert.AreEqual(328.73795, v5.First());

            var bn = v2[0];

            var bsrg = _mzSpectrumA[0];
        }


        [Test]
        public void FilterByNumberOfMostIntenseTest()
        {
            Assert.AreEqual(5, _mzSpectrumA.newSpectrumFilterByNumberOfMostIntense(5).Count);
        }

        [Test]
        public void GetBasePeak()
        {
            Assert.AreEqual(122781408.0, _mzSpectrumA.GetPeakWithHighestY().Intensity);
        }

        [Test]
        public void GetClosestPeak()
        {
            Assert.AreEqual(448.23987, _mzSpectrumA.GetClosestPeak(448).MZ);
            Assert.AreEqual(447.73849, _mzSpectrumA.GetClosestPeak(447.9).MZ);
        }

        [Test]
        public void Extract()
        {
            Assert.AreEqual(3, _mzSpectrumA.newSpectrumExtract(500, 600).Count);
        }

        [Test]
        public void CorrectOrder()
        {
            _mzSpectrumA = new DefaultMzSpectrum(new double[3] { 5, 6, 7 }, new double[3] { 1, 2, 3 }, false);
            Assert.IsTrue(_mzSpectrumA.newSpectrumFilterByNumberOfMostIntense(2)[0].MZ < _mzSpectrumA.newSpectrumFilterByNumberOfMostIntense(2)[1].MZ);
        }


        [Test]
        public void FilterByMZ()
        {
            Console.WriteLine("In test!");
            List<IRange<double>> ok = new List<IRange<double>>();
            ok.Add(new DoubleRange(300, 400));
            ok.Add(new DoubleRange(700, 800));
            Console.WriteLine("In test before first FilterByMZ");
            Assert.AreEqual(11, _mzSpectrumA.newSpectrumWithRangesRemoved(ok).Count);

            Console.WriteLine("In test before second FilterByMZ");
            Assert.AreEqual(10, _mzSpectrumA.newSpectrumWithRangeRemoved(new DoubleRange(400, 500)).Count);
            Console.WriteLine("In test after second FilterByMZ");
            Assert.AreEqual(10, _mzSpectrumA.newSpectrumWithRangeRemoved(400, 500).Count);
        }

        [Test]
        public void Test2D()
        {
            double[,] array2D = new double[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 } };
            _mzSpectrumA = new DefaultMzSpectrum(array2D);
            Assert.AreEqual("1 - 4 m/z (Peaks 4)", _mzSpectrumA.ToString());
        }

        [Test]
        public void TestClone()
        {
            var ok = new DefaultSpectrum(_mzSpectrumA);
            Assert.AreNotEqual(ok[0], _mzSpectrumA[0]);
            Assert.AreEqual(ok[0].Y, _mzSpectrumA[0].Y);


            var ok2 = new DefaultSpectrum(_mzSpectrumA.xArray, _mzSpectrumA.yArray, true);

            ok2.xArray[0] = 0;
            Assert.AreNotEqual(ok2.xArray[0], _mzSpectrumA.xArray[0]);

            var ok3 = new DefaultSpectrum(_mzSpectrumA.xArray, _mzSpectrumA.yArray, false);

            ok3.xArray[0] = 0;
            Assert.AreEqual(ok3.xArray[0], _mzSpectrumA.xArray[0]);

        }

        [Test]
        public void TestFunctionToX()
        {
            var ok = _mzSpectrumA.newSpectrumApplyFunctionToX(b => -1);
            Assert.AreEqual(-1, ok[0].X);
        }

        [Test]
        public void Test2dArray()
        {
            var ok = _mzSpectrumA.CopyTo2DArray();
            var ok2 = new DefaultMzSpectrum(ok);
            Assert.AreEqual(ok2[0].X, _mzSpectrumA[0].X);
        }

        [Test]
        public void TestFilterAndExtract()
        {
            Assert.AreEqual(447.73849, _mzSpectrumA.newSpectrumFilterByY(new DoubleRange(78353510, 81007097)).newSpectrumExtract(new DoubleRange(400, 500)).GetClosestPeak(new DoubleRange(327, 328)).X);
        }

        [Test]
        public void TestGetClosestPeakXValue()
        {
            Assert.AreEqual(447.73849, _mzSpectrumA.GetClosestPeakXvalue(447.73849));
            Assert.AreEqual(447.73849, _mzSpectrumA.GetClosestPeakXvalue(447));
            Assert.Throws<IndexOutOfRangeException>(() => { new DefaultMzSpectrum(new double[0], new double[0], false).GetClosestPeakXvalue(1); }, "No peaks in spectrum!");
        }

    }
}