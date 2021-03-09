using QuartierLatin.Backend.Utils;
using Xunit;

namespace QuartierLatin.Backend.Tests.Infrastructure
{
    public static class Extensions
    {
        public static TResult AssertSuccess<TResult>(this Result<TResult> res)
        {
            Assert.True(res.Success, res.Error?.Code);
            return res.Value;
        }

        public static void AssertSuccess(this Result res) => Assert.True(res.Success, res.Error?.Code);
    }
}