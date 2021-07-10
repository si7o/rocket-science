using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace RocketSience.Tests
{
    public class LandingServiceTests
    {
        private readonly LandingService sut;

        public LandingServiceTests()
        {
            sut = new LandingService();
        }

        public static IEnumerable<object[]> LandingPlatformsOK()
        {
            yield return new object[] { new Rectangle(LandingService.LandingAreaX, LandingService.LandingAreaY, 1, 1) };
            yield return new object[] { new Rectangle(LandingService.LandingAreaWidth, LandingService.LandingAreaHeight, 1, 1) };
            yield return new object[] { new Rectangle(LandingService.LandingAreaX, LandingService.LandingAreaY, LandingService.LandingAreaWidth, LandingService.LandingAreaHeight) };
            yield return new object[] { new Rectangle(3,12, 45, 62) };
        }

        [Theory]
        [MemberData(nameof(LandingPlatformsOK))]
        public void LandingService_canConfigureLandingPlatform_OK(Rectangle landingPlatform)
        {
            sut.Configure(landingPlatform);

            Assert.Equal(landingPlatform, sut.LandingPlatform);
        }

        public static IEnumerable<object[]> LandingPlatformsOutOfRange()
        {
            yield return new object[] { new Rectangle(-3, 1, 20, 12)};
            yield return new object[] { new Rectangle(100, 100, 20, 12)};
            yield return new object[] { new Rectangle(100, 100, 20, 12)};
            yield return new object[] { new Rectangle(100, 100, 2, 2) };
            yield return new object[] { new Rectangle(5, 5, 72, 0) };
            yield return new object[] { new Rectangle(5, 5, 0, 0) };
        }

        [Theory]
        [MemberData(nameof(LandingPlatformsOutOfRange))]
        public void LandingService_LandingPlatform_outsideLandingArea_throws_OutOfRangeException(Rectangle landingPlatform)
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => sut.Configure(landingPlatform));
        }

        public static IEnumerable<object[]> OkForLanding()
        {
            yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(5, 5) };
            yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(5, 14) };
            yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(14, 14) };
            yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(14, 5) };
        }

        [Theory]
        [MemberData(nameof(OkForLanding))]
        public void LandingService_checkTrajectory_OkForLanding(Rectangle landingPlatform, Point point)
        {
            sut.Configure(landingPlatform);
            var message = sut.CheckTrajectory(point);

            Assert.Equal(LandingService.OkForLanding, message);
        }

        public static IEnumerable<object[]> OutOfPlatform()
        {
            yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(16, 15) };

            yield return new object[] { new Rectangle(10, 10, 7, 3), new Point(LandingService.LandingAreaX -1, LandingService.LandingAreaY -1 ) };
            yield return new object[] { new Rectangle(10, 10, 7, 3), new Point(LandingService.LandingAreaWidth + 1, LandingService.LandingAreaHeight +1) };
        }

        [Theory]
        [MemberData(nameof(OutOfPlatform))]
        public void LandingService_checkTrajectory_OutOfPlatform(Rectangle landingPlatform, Point point)
        {
            sut.Configure(landingPlatform);
            var message = sut.CheckTrajectory(point);

            Assert.Equal(LandingService.OutOfPlatform, message);
        }

        public static IEnumerable<object[]> Clash()
        {
            yield return new object[] {
                new Rectangle(5, 5, 10, 10),
                new Point(7, 7),
                new Point[] {
                    new Point(7,7),
                    new Point(7,8),
                    new Point(6,7),
                    new Point(6,6),
                }
            };
        }

        [Theory]
        [MemberData(nameof(Clash))]
        public void LandingService_checkTrajectory_Clash(Rectangle landingPlatform, Point point, Point[] clashingPoints)
        {
            sut.Configure(landingPlatform);

            Assert.Equal(LandingService.OkForLanding, sut.CheckTrajectory(point));

            Assert.All(clashingPoints, clashingPoint =>
                Assert.Equal(LandingService.Clash, sut.CheckTrajectory(clashingPoint))
            );
        }
    }
}
