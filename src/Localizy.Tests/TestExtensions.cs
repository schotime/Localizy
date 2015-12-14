using Xunit;

namespace Localizy.Tests
{
    public static class TestExtensions
    {
        public static void ShouldEqual<T>(this T value, T otherValue)
        {
            Assert.Equal(otherValue, value);
        }

        public static void ShouldNotEqual<T>(this T value, T otherValue)
        {
            Assert.NotEqual(otherValue, value);
        }

        public static void ShouldBeTrue(this bool value)
        {
            Assert.True(value);
        }
    }
}