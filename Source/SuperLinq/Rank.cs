﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Ranks each item in the sequence in ascending order using a default comparer,
	/// with no gaps in the ranking values. The rank of a specific item is one plus 
	/// the number of distinct rank values that come before that specific item.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IEnumerable<TSource> source)
	{
		return source.DenseRankBy(Identity<TSource>());
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order using a caller-supplied comparer,
	/// with no gaps in the ranking values. The rank of a specific item is one plus 
	/// the number of distinct rank values that come before that specific item.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IEnumerable<TSource> source, IComparer<TSource> comparer)
	{
		return source.DenseRankBy(Identity<TSource>(), comparer);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order by a specified key
	/// using a default comparer, with no gaps in the ranking values. The rank 
	/// of a specific item is one plus the number of distinct rank values that 
	/// come before that specific item.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> DenseRankBy<TSource, TKey>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();

		return RankByImpl(source, keySelector, comparer: null, isDense: true);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order by a specified key
	/// using a caller-supplied comparer, with no gaps in the ranking values. 
	/// The rank  of a specific item is one plus the number of distinct rank 
	/// values that come before that specific item.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
	/// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> DenseRankBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey> comparer)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();

		return RankByImpl(source, keySelector, comparer, isDense: true);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order using a default comparer.
	/// The rank of a specific item is one plus the number of items that come before 
	/// the first item in the current equivalence set.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source)
	{
		return source.RankBy(Identity<TSource>());
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order using a caller-supplied comparer.
	/// The rank of a specific item is one plus the number of items that come before 
	/// the first item in the current equivalence set.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source, IComparer<TSource> comparer)
	{
		return source.RankBy(Identity<TSource>(), comparer);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order by a specified key
	/// using a default comparer. The rank of a specific item is one plus the 
	/// number of items that come before the first item in the current equivalence
	/// set.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> RankBy<TSource, TKey>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();

		return RankByImpl(source, keySelector, comparer: null, isDense: false);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order by a specified key
	/// using a caller-supplied comparer. The rank of a specific item is one plus the 
	/// number of items that come before the first item in the current equivalence
	/// set.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
	/// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, int rank)> RankBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey> comparer)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();

		return RankByImpl(source, keySelector, comparer, isDense: false);
	}

	private static IEnumerable<(TSource, int)> RankByImpl<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		bool isDense)
	{
		comparer ??= Comparer<TKey>.Default;

		var rank = 0;
		var count = 1;
		foreach (var (cur, lag) in source
			.OrderBy(keySelector, comparer)
			.Lag(1))
		{
			if (rank == 0
				|| comparer.Compare(keySelector(cur), keySelector(lag!)) != 0)
			{
				rank += isDense ? 1 : count;
				count = 0;
			}

			count++;

			yield return (cur, rank);
		}
	}
}
