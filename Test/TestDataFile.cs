// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
//
// This file (TestDataFile.cs) is part of MassSpectrometry.Tests.
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

namespace Test
{
    [TestFixture]
    public sealed class TestDataFile
    {
        private DefaultMzSpectrum _mzSpectrumA;

        [SetUp]
        public void Setup()
        {
            double[] mz = { 328.73795, 329.23935, 447.73849, 448.23987, 482.23792, 482.57089, 482.90393, 500.95358, 501.28732, 501.62131, 611.99377, 612.32806, 612.66187, 722.85217, 723.35345 };
            double[] intensities = { 81007096.0, 28604418.0, 78353512.0, 39291696.0, 122781408.0, 94147520.0, 44238040.0, 71198680.0, 54184096.0, 21975364.0, 44514172.0, 43061628.0, 23599424.0, 56022696.0, 41019144.0 };

            _mzSpectrumA = new DefaultMzSpectrum(mz, intensities);
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
        public void DataFileTest()
        {
            FakeMsDataFile thefile = new FakeMsDataFile("Somepath");

            MsDataScan<DefaultMzSpectrum> theSpectrum = new MsDataScan<DefaultMzSpectrum>(1, _mzSpectrumA, "first spectrum", 1, true, Polarity.Positive, 1, new MzRange(300, 1000), "fake scan filter");

            MsDataScan<DefaultMzSpectrum>[] theList = new MsDataScan<DefaultMzSpectrum>[1];

            theList[0] = theSpectrum;

            thefile.Add(theList);

            Assert.AreEqual(15, thefile.GetSpectrum(thefile.FirstSpectrumNumber).Count);
            Assert.AreEqual(15, thefile.GetSpectrum(thefile.FirstSpectrumNumber).Count);

            Assert.AreEqual(1, thefile.LastSpectrumNumber);
            Assert.AreEqual(1, thefile.LastSpectrumNumber);


            Assert.IsTrue(thefile.GetScan(1).isCentroid);

            foreach (var ok in thefile.GetMsScans())
                Assert.AreEqual(new MzRange(300, 1000), ok.MzRange);


            IMsDataFile<IMzSpectrum<MzPeak, MzRange>> okyee = thefile;

            Assert.AreEqual("Somepath (UnKnown)", okyee.ToString());

            Assert.AreEqual("Somepath", okyee.FilePath);

            //int ok1 = 0;
            //foreach (var i in thefile.GetMsScansInTimeRange(0, 2))
            //    ok1 += 1;

            //Assert.AreEqual(1, ok1);


            //int ok2 = 0;
            //foreach (var i in thefile.GetMsScansInTimeRange(2, 4))
            //    ok2 += 1;

            //Assert.AreEqual(0, ok2);

        }
    }
}