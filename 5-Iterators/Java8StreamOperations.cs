using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Iterators
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The static class declares extension methods which use the same naming used by Java 8 with Stream API.
    /// </summary>
    public static class Java8StreamOperations
    {
        /// <summary>
        /// Performs an action for each element of this stream.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="consumer">a non-interfering action to perform on the elements.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        public static void ForEach<TAny>(this IEnumerable<TAny> sequence, Action<TAny> consumer)
        {
            foreach (var v in sequence)
            {
                consumer(v);
            }
        }

        /// <summary>
        /// Returns an enumerable consisting of the elements of the specified <paramref name="sequence"/>,
        /// additionally performing the provided action on each element as elements are consumed from the sequence.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="consumer">a non-interfering action to perform on the elements as they are consumed.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> Peek<TAny>(this IEnumerable<TAny> sequence, Action<TAny> consumer)
        {
            foreach (var c in sequence)
            {
                consumer(c);
                yield return c;    
            }
            
        }

        /// <summary>
        /// Returns a sequence consisting of the results of applying the given function
        /// to the elements of the <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="mapper">a non-interfering, stateless function to apply to each element.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <typeparam name="TOther">The element type of the new sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TOther> Map<TAny, TOther>(this IEnumerable<TAny> sequence, Func<TAny, TOther> mapper)
        {
            foreach (var c in sequence)
            {
                var v = mapper(c);
                yield return v;    
            }
        }

        /// <summary>
        /// Returns a stream consisting of the elements of this stream that match the given predicate.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="predicate">
        /// a non-interfering, stateless predicate to apply to each element to determine if it should be included.
        /// </param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> Filter<TAny>(this IEnumerable<TAny> sequence, Predicate<TAny> predicate)
        {
            foreach (var c in sequence)
            {
                if (predicate(c))
                {
                    yield return c;
                }
            }
        }

        /// <summary>
        /// Returns a new sequence containing a tuple with each element and its index in the original sequence.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<Tuple<int, TAny>> Indexed<TAny>(this IEnumerable<TAny> sequence)
        {
            int i = 0;
            foreach (var c in sequence)
            {
                yield return new Tuple<int, TAny>(i,c);
                i++;
            }
        }

        /// <summary>
        /// Performs a reduction on the elements of the <paramref name="sequence"/>, using a <paramref name="reducer"/>
        /// function and returns the reduced value, if any, or <see langword="default"/> if not.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="seed">the base accumulator value.</param>
        /// <param name="reducer">the reducer function.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <typeparam name="TOther">the type of the accomulation result.</typeparam>
        /// <returns>the new sequence.</returns>
        public static TOther Reduce<TAny, TOther>(this IEnumerable<TAny> sequence, TOther seed, Func<TOther, TAny, TOther> reducer)
        {
            TOther res = default(TOther);
            bool first = true;
            foreach (var element in sequence)
            {
                if (first)
                {
                    first = false;
                    res = reducer(seed, element);
                }
                else
                {
                    res = reducer(res, element);
                }
            }
            return res;
            
        }

        /// <summary>
        /// Returns a sequence containing all elements except the first elements that satisfy the given predicate.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="predicate">
        /// a non-interfering, stateless predicate to apply to the first elements.
        /// </param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> SkipWhile<TAny>(this IEnumerable<TAny> sequence, Predicate<TAny> predicate)
        {
            bool skip = true;
            foreach (var element in sequence)
            {
                if (skip && !predicate(element))
                {
                    skip = false;
                }
                else
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Returns a sequence consisting of the remaining elements of the <paramref name="sequence"/> after discarding
        /// the first <paramref name="count"/> elements of the sequence.
        /// If this sequence contains fewer elements, then an empty sequence will be returned.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="count">the number of leading elements to skip.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> SkipSome<TAny>(this IEnumerable<TAny> sequence, long count)
        {
            long skippedElements = 0;
            foreach (var element in sequence)
            {
                if (skippedElements >=count)
                {
                    yield return element;
                }
                else
                {
                    skippedElements++;
                }
            }
        }

        /// <summary>
        /// Returns a sequence consisting of a subset of elements taken from the <paramref name="sequence"/>
        /// that match the given predicate.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="predicate">
        /// a non-interfering, stateless predicate to apply to elements to determine the elements to take.
        /// </param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> TakeWhile<TAny>(this IEnumerable<TAny> sequence, Predicate<TAny> predicate)
        {
            foreach (var element in sequence)
            {
                if (predicate(element))
                {
                    yield return element;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns a sequence consisting of the first <paramref name="count"/> elements
        /// of the <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">the sequence.</param>
        /// <param name="count">the number of leading elements to take.</param>
        /// <typeparam name="TAny">the type of the items in the sequence.</typeparam>
        /// <returns>the new sequence.</returns>
        public static IEnumerable<TAny> TakeSome<TAny>(this IEnumerable<TAny> sequence, long count)
        {
            long taken = 0;
            foreach (var element in sequence)
            {
                if (taken < count)
                {
                    yield return element;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns an infinite sequence of integers.
        /// </summary>
        /// <param name="start">the starting element.</param>
        /// <returns>an infinite sequence of integers.</returns>
        public static IEnumerable<int> Integers(int start)
        {
            int i = start;
            while (true)
            {
                yield return i;
                i++;
            }
        }

        /// <summary>
        /// Returns an infinite sequence of integers starting from <c>0</c>.
        /// </summary>
        /// <returns>an infinite sequence of integers.</returns>
        public static IEnumerable<int> Integers() => Integers(0);

        /// <summary>
        /// Returns a sequence of <paramref name="count"/> integers starting <paramref name="start"/>.
        /// </summary>
        /// <param name="start">the starting element.</param>
        /// <param name="count">the number of items of the sequence.</param>
        /// <returns>the sequence of integers.</returns>
        public static IEnumerable<int> Range(int start, int count) => Integers(start).TakeSome(count);
    }
}
