using ServiceLayer.DatabaseServices.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestAspNetCore
{
  public   class TestCalculateReviewsToMatch
    {
        [Theory]
        [InlineData(5)]
        [InlineData(5, 1)]
        [InlineData(5, 1, 1)]
        [InlineData(5,1,1,1,1)]
        [InlineData(5,1,5)]
        [InlineData(3,4,3)]
        public void TestCalcReviewsOk(params int[] voteVals)
        {
            var avgRating = voteVals.Average();
            var numVotes = voteVals.Length;

            var reviews = BookJsonLoader.CalculateReviewsToMatch(avgRating, numVotes);

            reviews.Count.ShouldEqual(numVotes);
            reviews.Average(x => x.NumStars).ShouldEqual(avgRating);
        }
    }
}
