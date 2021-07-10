using AutoFixture;
using System.Drawing;
using System.Linq;
using Xunit;

namespace RocketSience.Tests
{
    public class PointExtensionsTests
    {
        private readonly Fixture fixture;
        public PointExtensionsTests() {
            fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Empty_Point_returns_empty_Rectangle()
        {
            var point = Point.Empty;
            var safetyPerimeter = point.SafetyPerimeter();

            Assert.Equal(Rectangle.Empty, safetyPerimeter);
        }

        [Theory]
#pragma warning disable xUnit1012 // Null should not be used for value type parameters
        [InlineData(null)]
#pragma warning restore xUnit1012 // Null should not be used for value type parameters
        public void Null_Point_returns_empty_Rectangle(Point point)
        {
            var safetyPerimeter = point.SafetyPerimeter();

            Assert.Equal(Rectangle.Empty, safetyPerimeter);
        }

        [Fact]
        public void SafetyPerimeter_for_default_radius_OK()
        {
            var point = fixture.Create<Point>();
            var safetyPerimeter = point.SafetyPerimeter();

            Assert.Equal(point.X, safetyPerimeter.X);
            Assert.Equal(point.Y, safetyPerimeter.Y);
            Assert.Equal(1, safetyPerimeter.Width);
            Assert.Equal(1, safetyPerimeter.Height);
        }

        [Fact]
        public void SafetyPerimeter_for_random_radius_OK()
        {
            var safetyRadius = fixture.Create<int>();
            var point = fixture.Create<Point>();

            var safetyPerimeter = point.SafetyPerimeter(safetyRadius);

            var expectedLength = (safetyRadius * 2) + 1;

            Assert.Equal(point.X - safetyRadius, safetyPerimeter.X);
            Assert.Equal(point.Y - safetyRadius, safetyPerimeter.Y);
            Assert.Equal(expectedLength, safetyPerimeter.Width);
            Assert.Equal(expectedLength, safetyPerimeter.Height);
        }
    }
}
