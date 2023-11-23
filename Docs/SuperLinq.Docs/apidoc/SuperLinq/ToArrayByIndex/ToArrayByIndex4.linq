<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "alp", "car", };

// Transform a sequence by index
var result = sequence
	.ToArrayByIndex(c => c[0] - 'a', (c, i) => $"{i}:{c}");

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [0:alp, 1:bar, 2:car, , , 5:foo]
