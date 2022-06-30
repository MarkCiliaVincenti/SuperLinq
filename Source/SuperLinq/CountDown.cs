﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Provides a countdown counter for a given count of elements at the
	/// tail of the sequence where zero always represents the last element,
	/// one represents the second-last element, two represents the
	/// third-last element and so on.
	/// </summary>
	/// <typeparam name="T">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <typeparam name="TResult">
	/// The type of elements of the resulting sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Count of tail elements of
	/// <paramref name="source"/> to count down.</param>
	/// <param name="resultSelector">
	/// A function that receives the element and the current countdown
	/// value for the element and which returns those mapped to a
	/// result returned in the resulting sequence. For elements before
	/// the last <paramref name="count"/>, the coundown value is
	/// <c>null</c>.</param>
	/// <returns>
	/// A sequence of results returned by
	/// <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. At most, <paramref name="count"/> elements of the source
	/// sequence may be buffered at any one time unless
	/// <paramref name="source"/> is a collection or a list.
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
	public static IEnumerable<TResult> CountDown<T, TResult>(
		this IEnumerable<T> source,
		int count, Func<T, int?, TResult> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return source.TryGetCollectionCount(out var i)
				 ? IterateCollection(source, count, i, resultSelector)
				 : IterateSequence(source, count, resultSelector);

		static IEnumerable<TResult> IterateCollection(IEnumerable<T> source, int count, int i, Func<T, int?, TResult> resultSelector)
		{
			foreach (var item in source)
				yield return resultSelector(item, i-- <= count ? i : null);
		}

		static IEnumerable<TResult> IterateSequence(IEnumerable<T> source, int count, Func<T, int?, TResult> resultSelector)
		{
			var queue = new Queue<T>(Math.Max(1, count + 1));

			foreach (var item in source)
			{
				queue.Enqueue(item);
				if (queue.Count > count)
					yield return resultSelector(queue.Dequeue(), null);
			}

			while (queue.Count > 0)
				yield return resultSelector(queue.Dequeue(), queue.Count);
		}
	}
}
