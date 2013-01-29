using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using de.mastersign.minimods.chain;

namespace de.mastersign.minimods.test.chain
{
    class ChainTest
    {
        [Test]
        public void IsEmptyTest()
        {
            var o = new Chain<int>();
            Assert.IsTrue(o.IsEmpty);
        }

        [Test]
        public void EmptyTest()
        {
            var o = Chain<int>.Empty;
            Assert.NotNull(o);
            Assert.IsTrue(o.IsEmpty);
        }
    }
}
