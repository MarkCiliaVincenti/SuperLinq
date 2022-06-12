﻿using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class MaxByTest
{
	[Test]
	public void MaxByIsLazy()
	{
		new BreakingSequence<int>().MaxBy(BreakingFunc.Of<int, int>());
	}

	[Test]
	public void MaxByReturnsMaxima()
	{
		Assert.AreEqual(new[] { "hello", "world" },
						SampleData.Strings.MaxBy(x => x.Length));
	}

	[Test]
	public void MaxByNullComparer()
	{
		Assert.AreEqual(SampleData.Strings.MaxBy(x => x.Length),
						SampleData.Strings.MaxBy(x => x.Length, null));
	}

	[Test]
	public void MaxByEmptySequence()
	{
		Assert.That(Array.Empty<string>().MaxBy(x => x.Length), Is.Empty);
	}

	[Test]
	public void MaxByWithNaturalComparer()
	{
		Assert.AreEqual(new[] { "az" }, SampleData.Strings.MaxBy(x => x[1]));
	}

	[Test]
	public void MaxByWithComparer()
	{
		Assert.AreEqual(new[] { "aa" }, SampleData.Strings.MaxBy(x => x[1], Comparable<char>.DescendingOrderComparer));
	}

	public class First
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.First(maxima), Is.EqualTo("hello"));
		}

		[Test]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.First(maxima), Is.EqualTo("ax"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.First(strings.MaxBy(s => s.Length)));
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.First(strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer)));
		}
	}

	public class FirstOrDefault
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.FirstOrDefault(maxima), Is.EqualTo("hello"));
		}

		[Test]
		public void WithComparerReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.FirstOrDefault(maxima), Is.EqualTo("ax"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.FirstOrDefault(maxima), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.FirstOrDefault(maxima), Is.Null);
		}
	}

	public class Last
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.Last(maxima), Is.EqualTo("world"));
		}

		[Test]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.Last(maxima), Is.EqualTo("az"));
		}

		[Test]
		public void WithEmptySourceThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.Last(strings.MaxBy(s => s.Length)));
		}

		[Test]
		public void WithEmptySourceWithComparerThrows()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.Last(strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer)));
		}
	}

	public class LastOrDefault
	{
		[Test]
		public void ReturnsMaximum()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.LastOrDefault(maxima), Is.EqualTo("world"));
		}

		[Test]
		public void WithComparerReturnsMaximumPerComparer()
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.LastOrDefault(maxima), Is.EqualTo("az"));
		}

		[Test]
		public void WithEmptySourceReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length);
			Assert.That(SuperEnumerable.LastOrDefault(maxima), Is.Null);
		}

		[Test]
		public void WithEmptySourceWithComparerReturnsDefault()
		{
			using var strings = Enumerable.Empty<string>().AsTestingSequence();
			var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
			Assert.That(SuperEnumerable.LastOrDefault(maxima), Is.Null);
		}
	}

	public class Take
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "hello" })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] ReturnsMaxima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxBy(s => s.Length).Take(count).ToArray();
		}

		[TestCase(0, 0, ExpectedResult = new string[0])]
		[TestCase(3, 1, ExpectedResult = new[] { "aa" })]
		[TestCase(1, 0, ExpectedResult = new[] { "ax" })]
		[TestCase(2, 0, ExpectedResult = new[] { "ax", "aa" })]
		[TestCase(3, 0, ExpectedResult = new[] { "ax", "aa", "ab" })]
		[TestCase(4, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay" })]
		[TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .Take(count)
						  .ToArray();
		}
	}

	public class TakeLast
	{
		[TestCase(0, ExpectedResult = new string[0])]
		[TestCase(1, ExpectedResult = new[] { "world" })]
		[TestCase(2, ExpectedResult = new[] { "hello", "world" })]
		[TestCase(3, ExpectedResult = new[] { "hello", "world" })]
		public string[] TakeLastReturnsMaxima(int count)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxBy(s => s.Length).TakeLast(count).ToArray();
		}

		[TestCase(0, 0, ExpectedResult = new string[0])]
		[TestCase(3, 1, ExpectedResult = new[] { "aa" })]
		[TestCase(1, 0, ExpectedResult = new[] { "az" })]
		[TestCase(2, 0, ExpectedResult = new[] { "ay", "az" })]
		[TestCase(3, 0, ExpectedResult = new[] { "ab", "ay", "az" })]
		[TestCase(4, 0, ExpectedResult = new[] { "aa", "ab", "ay", "az" })]
		[TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		[TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
		public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
		{
			using var strings = SampleData.Strings.AsTestingSequence();
			return strings.MaxBy(s => s[index], Comparable<char>.DescendingOrderComparer)
						  .TakeLast(count)
						  .ToArray();
		}
	}
}
