using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SWOF.Api.Utils;
using Xunit;

namespace SWOF.Api.Tests.Utils
{
	public class ShuffleExtensionTests
	{
		[Fact]
		public void ShuffleShouldHaveRandomness()
		{
			var input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			var previous = input;
			var unmovedOccurence = 0;

			for (var i = 0; i < 200; i++)
			{
				var shuffled = input.Shuffle();

				shuffled.Should().OnlyHaveUniqueItems();
				shuffled.First().Should().NotBe(input.Last());

				var unmoved = shuffled.Where((t, x) => t == previous[x]).Count();

				if (unmoved >= 3) unmovedOccurence++; // margin for unmoved elements in the shuffle

				previous = shuffled;
			}

			unmovedOccurence.Should().BeLessOrEqualTo(30); // marging for shuffles with unmoved occurrence
		}
	}
}
