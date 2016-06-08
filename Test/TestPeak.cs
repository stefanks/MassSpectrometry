// Copyright 2012, 2013, 2014 Derek J. Bailey
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
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class PeakTest
    {
        [Test]
        public void PeakEquality()
        {
            MzPeak ok = new MzPeak(1, 1);
            Assert.IsTrue(ok.Equals(new MzPeak(1 + 1e-11, 1 + 1e-11)));
        }

        [Test]
        public void PeakEquality2()
        {
            Peak ok = new MzPeak(1, 1);
            Peak ok2 = new MzPeak(1 + 1e-11, 1 + 1e-11);
            Assert.IsTrue(ok.Equals(ok2));
        }

        [Test]
        public void PeakStuff()
        {
            MzPeak ok = new MzPeak(1, 1);
            ok.AddIntensity(1);
            Assert.AreEqual(2,ok.Intensity);
            Assert.AreEqual("(1, 2)", ok);
        }
    }
}