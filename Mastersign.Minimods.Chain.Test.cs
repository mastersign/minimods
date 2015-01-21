using System;
using System.Linq;
using NUnit.Framework;

namespace Mastersign.Minimods.Chain.Test
{
    internal class ChainTest
    {
        [Test, Category("Base")]
        public void IsEmptyTest()
        {
            var o = new Chain<int>();
            Assert.IsTrue(o.IsEmpty);
        }

        [Test, Category("Base")]
        public void EmptyTest()
        {
            var o = Chain<int>.Empty;
            Assert.NotNull(o);
            Assert.IsTrue(o.IsEmpty);
        }

        [Test, Category("Base")]
        public void EmptyEqualTest()
        {
            var o1 = new Chain<int>();
            var o2 = new Chain<int>();
            Assert.AreEqual(Chain<int>.Empty, o1);
            Assert.AreEqual(Chain<int>.Empty, o2);
            Assert.AreEqual(o1, o2);
        }

        [Test, Category("Base")]
        public void HeadEqualTest()
        {
            const int h = 42;
            var o1 = new Chain<int>(h);
            var o2 = new Chain<int>(h);
            Assert.AreEqual(o1, o2);
        }

        [Test, Category("Base")]
        public void ImplicitCastTest()
        {
            const int h = 42;
            Chain<int> o1;
            o1 = h;
            Assert.AreEqual(h, o1.Head);
        }

        [Test, Category("Base")]
        public void HeadTest()
        {
            const int h = 42;
            Chain<int> o;

            o = new Chain<int>();
            Assert.Throws<InvalidOperationException>(() => { var x = o.Head; });

            o = new Chain<int>(h);
            Assert.AreEqual(h, o.Head);
        }

        [Test, Category("Base")]
        public void EmptyTailTest()
        {
            const int h = 42;
            Chain<int> o;

            o = new Chain<int>();
            Assert.AreEqual(Chain<int>.Empty, o.Tail);

            o = new Chain<int>(h);
            Assert.AreEqual(Chain<int>.Empty, o.Tail);
        }

        [Test, Category("Base")]
        public void TailTest()
        {
            const int h = 42;
            const int t = 43;
            var ot = new Chain<int>(t);
            var oh = new Chain<int>(h, ot);

            Assert.AreSame(oh.Tail, ot);
        }

        [Test, Category("Base")]
        public void EnumerableTest()
        {
            const int v1 = 42;
            const int v2 = 43;
            const int v3 = 44;
            var v = new[] { v3, v2, v1 };
            var o1 = new Chain<int>(v1);
            var o2 = new Chain<int>(v2, o1);
            var o3 = new Chain<int>(v3, o2);

            var i = 0;
            foreach (var h in o3)
            {
                Assert.AreEqual(v[i++], h);
            }
            Assert.AreEqual(v.Length, i);
            Assert.AreEqual(v.Length, o3.Count());

            var o = new Chain<int>();
            Assert.IsTrue(o.IsEmpty);
            Assert.AreEqual(0, o.Count());
        }

        [Test, Category("Algorithm")]
        public void PrependTest()
        {
            const int v1 = 42;
            const int v2 = 43;
            const int v3 = 44;
            var v = new[] { v1, v2, v3 };
            var o = new Chain<int>(v3);
            o = o.Prepend(v2);
            o = o.Prepend(v1);

            Assert.AreEqual(v.Length, o.Count());

            Assert.AreEqual(o.Head, v1);
            Assert.AreEqual(o.Tail.Head, v2);
            Assert.AreEqual(o.Tail.Tail.Head, v3);

            Assert.IsTrue(v.Zip(o, (a, b) => a == b).All(p => p));
        }

        [Test, Category("Algorithm")]
        public void AppendEmptyTest()
        {
            const int v1 = 42;
            const int v2 = 43;
            var o = new Chain<int>(v1, v2);
            Assert.AreEqual(2, o.Count());

            var o2 = o.Append(Chain<int>.Empty);
            Assert.AreSame(o, o2);

            var o3 = o.Append(new Chain<int>());
            Assert.AreSame(o, o3);

            var o4 = Chain<int>.Empty.Append(o);
            Assert.AreSame(o, o4);

            var o5 = new Chain<int>().Append(o);
            Assert.AreSame(o, o5);
        }

        [Test, Category("Algorithm")]
        public void AppendTest()
        {
            const int v1 = 42;
            const int v2 = 43;
            const int v3 = 44;
            var v = new[] { v1, v2, v3 };
            var o = new Chain<int>(v1);
            Assert.AreEqual(1, o.Count());
            o = o.Append(v2);
            Assert.AreEqual(2, o.Count());
            o = o.Append(v3);
            Assert.AreEqual(3, o.Count());

            Assert.AreEqual(o.Head, v1);
            Assert.AreEqual(o.Tail.Head, v2);
            Assert.AreEqual(o.Tail.Tail.Head, v3);

            Assert.IsTrue(v.Zip(o, (a, b) => a == b).All(p => p));
        }

        [Test, Category("Algorithm")]
        public void ReverseEmpty()
        {
            Assert.AreSame(Chain<int>.Empty, Chain<int>.Empty.Reverse());

            var o = new Chain<int>();
            Assert.AreSame(o, o.Reverse());
        }

        [Test, Category("Algorithm")]
        public void ReverseSingle()
        {
            const int v = 42;
            var o = new Chain<int>(v);
            Assert.AreSame(o, o.Reverse());
        }

        [Test, Category("Algorithm")]
        public void ReverseTest()
        {
            const int v1 = 42;
            const int v2 = 43;
            const int v3 = 44;
            var v = new[] { v1, v2, v3 };

            var o = new Chain<int>(v1, new Chain<int>(v2, new Chain<int>(v3)));
            Assert.IsTrue(v.Zip(o, (a, b) => a == b).All(p => p));

            Array.Reverse(v);
            var o2 = o.Reverse();
            Assert.IsTrue(v.Zip(o2, (a, b) => a == b).All(p => p));
        }
    }

    public class ChainExtensionTest
    {
        [Test, Category("Algorithm")]
        public void ToChainReverseEmptyTest()
        {
            var v = new int[0];
            Assert.AreEqual(Chain<int>.Empty, v.ToChainReverse());
        }

        [Test, Category("Algorithm")]
        public void ToChainReverseTest()
        {
            var v1 = new[] { 42 };
            var v2 = new[] { 42, 43 };
            var v3 = new[] { 42, 43, 44 };

            var o1 = v1.ToChainReverse();
            Array.Reverse(v1);
            Assert.AreEqual(v1.Length, o1.Count());
            Assert.IsTrue(v1.Zip(o1, (a, b) => a == b).All(p => p));

            var o2 = v2.ToChainReverse();
            Array.Reverse(v2);
            Assert.AreEqual(v2.Length, o2.Count());
            Assert.IsTrue(v2.Zip(o2, (a, b) => a == b).All(p => p));

            var o3 = v3.ToChainReverse();
            Array.Reverse(v3);
            Assert.AreEqual(v3.Length, o3.Count());
            Assert.IsTrue(v3.Zip(o3, (a, b) => a == b).All(p => p));
        }

        [Test, Category("Algorithm")]
        public void ToChainEmptyTest()
        {
            var v = new int[0];
            Assert.AreEqual(Chain<int>.Empty, v.ToChain());
        }

        [Test, Category("Algorithm")]
        public void ToChainTest()
        {
            var v1 = new[] { 42 };
            var v2 = new[] { 42, 43 };
            var v3 = new[] { 42, 43, 44 };

            var o1 = v1.ToChain();
            Assert.AreEqual(v1.Length, o1.Count());
            Assert.IsTrue(v1.Zip(o1, (a, b) => a == b).All(p => p));

            var o2 = v2.ToChain();
            Assert.AreEqual(v2.Length, o2.Count());
            Assert.IsTrue(v2.Zip(o2, (a, b) => a == b).All(p => p));

            var o3 = v3.ToChain();
            Assert.AreEqual(v3.Length, o3.Count());
            Assert.IsTrue(v3.Zip(o3, (a, b) => a == b).All(p => p));
        }
    }
}