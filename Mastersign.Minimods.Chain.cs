﻿#region MiniMod
// <MiniMod>
//   <Name>Chain</Name>
//   <Author>Tobias Kiertscher &lt;dev@mastersign.de&gt;</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/48db629fd2375a41ab5d/raw/Mastersign.Minimods.Chain.cs</Url>
//   <Description>
//     Chain is a simple functional and immutable data type
//     for chained lists. It is enumerable and generic, 
//     and supports typical functional programming patterns.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mastersign.Minimods.Chain
{
    /// <summary>
    /// A simple functional immutable data structure for a chained list.
    /// </summary>
    /// <typeparam name="T">The element type of the list.</typeparam>
    public class Chain<T> : IEnumerable<T>
    {
        /// <summary>
        /// The empty list.
        /// </summary>
        public static readonly Chain<T> Empty = new Chain<T>();

        private readonly T head;

        private readonly Chain<T> tail;

        private readonly bool isEmpty;

        /// <summary>
        /// Creastes an empty list.
        /// </summary>
        /// <remarks>Complexity of O(1).</remarks>
        public Chain()
        {
            this.isEmpty = true;
        }

        /// <summary>
        /// Creates a list with one element, the head.
        /// </summary>
        /// <param name="head">The only element of the list.</param>
        /// <remarks>Complexity of O(1).</remarks>
        public Chain(T head)
        {
            this.head = head;
            this.isEmpty = false;
            this.tail = null;
        }

        /// <summary>
        /// Creates a list with a head element and a tail chain.
        /// </summary>
        /// <param name="head">The head element.</param>
        /// <param name="tail">The tail chain.</param>
        /// <remarks>Complexity of O(1).</remarks>
        public Chain(T head, Chain<T> tail)
        {
            this.head = head;
            this.isEmpty = false;
            this.tail = tail;
        }

        /// <summary>
        /// Returns the head element of the list.
        /// </summary>
        /// <remarks>Complexity of O(1).</remarks>
        public T Head
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException("Chain is empty.");
                return head;
            }
        }

        /// <summary>
        /// Checks if the chained list is empty.
        /// </summary>
        /// <remarks>Complexity of O(1).</remarks>
        public bool IsEmpty { get { return isEmpty; } }

        /// <summary>
        /// Returns the tail of the list.
        /// </summary>
        /// <remarks>Complexity of O(1).</remarks>
        public Chain<T> Tail { get { return tail ?? Empty; } }

        /// <summary>
        /// Creates a new chain by prepending the given value as new head at the start of the chain.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>A chain, using the new value as head.</returns>
        /// <remarks>Complexity of O(1).</remarks>
        public Chain<T> Prepend(T value)
        {
            return isEmpty ? new Chain<T>(value) : new Chain<T>(value, this);
        }

        /// <summary>
        /// Creates a new chain by appending the given tail to the end of the chain.
        /// </summary>
        /// <param name="extension">The extension to the end of the chain.</param>
        /// <returns>A new chain, representing this chain extended by the given extension.</returns>
        /// <remarks>Complexity of O(n).</remarks>
        public Chain<T> Append(Chain<T> extension)
        {
            return IsEmpty
                ? extension
                : extension.IsEmpty
                    ? this
                    : new Chain<T>(
                        Head,
                        Tail.IsEmpty
                            ? extension
                            : Tail.Append(extension));
        }

        /// <summary>
        /// Returns a <see cref="IEnumerator"/> to iterate through the values of the chain.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="IEnumerator{T}"/> to iterate through the values of the chain.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = this;
            while (!current.IsEmpty)
            {
                yield return current.Head;
                current = current.Tail;
            }
        }

        /// <summary>
        /// Reverses the order of the chain.
        /// </summary>
        /// <returns>A new chain with all values from this chain in reversed order.</returns>
        /// <remarks>Complexity of O(n).</remarks>
        public Chain<T> Reverse()
        {
            return IsEmpty || Tail.IsEmpty
                ? this
                : this.Aggregate(new Chain<T>(), (chain, item) => chain.Prepend(item));
        }

        /// <summary>
        /// Implicit cast from a value into a chain with one value.
        /// </summary>
        /// <param name="head">The only value for the chain.</param>
        /// <returns>A new chain.</returns>
        /// <remarks>Complexity of O(1).</remarks>
        public static implicit operator Chain<T>(T head)
        {
            return new Chain<T>(head);
        }
    }

    /// <summary>
    /// A static helper class with extension methods for <see cref="Chain{T}"/>.
    /// </summary>
    public static class ChainExtension
    {
        /// <summary>
        /// Creates a new chain with all elements of <paramref name="enumerable"/>
        /// in reversed order.
        /// </summary>
        /// <typeparam name="T">The value type for the new chain.</typeparam>
        /// <param name="enumerable">The values for the new chain.</param>
        /// <returns>The created chain.</returns>
        /// <remarks>Complexity of O(n).</remarks>
        public static Chain<T> ToChainReverse<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Aggregate(new Chain<T>(), (chain, item) => chain.Prepend(item));
        }

        /// <summary>
        /// Creates a new chain with all elements of <paramref name="enumerable"/>
        /// in the original order.
        /// </summary>
        /// <typeparam name="T">The value type for the new chain.</typeparam>
        /// <param name="enumerable">The values for the new chain.</param>
        /// <returns>The created chain.</returns>
        /// <remarks>Complexity of O(2n).</remarks>
        public static Chain<T> ToChain<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToChainReverse().Reverse();
        }

        /// <summary>
        /// Creates a new chain, by prepending the given head value to the given tail chain.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="head">The head value.</param>
        /// <param name="tail">The tail chain.</param>
        /// <returns>A new chain.</returns>
        public static Chain<T> Cons<T>(T head, Chain<T> tail)
        {
            return new Chain<T>(head, tail);
        }
    }
}