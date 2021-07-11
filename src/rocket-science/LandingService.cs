using System;
using System.Drawing;

namespace RocketSience
{
    public class LandingService
    {
        public const string OkForLanding = "ok for landing";
        public const string Clash = "clash";
        public const string OutOfPlatform = "out of platform";

        public const int LandingAreaWidth = 100;
        public const int LandingAreaHeight = 100;
        public const int LandingAreaX = 1;
        public const int LandingAreaY = 1;
        public const int SafetyRadius = 1;

        public Rectangle LandingArea { get; private set; }
        public Rectangle LandingPlatform { get; private set; }
        public Point LastRequestedLandingPoint { get; private set; }


        /// <summary>
        /// Creates a LandingService with a LandingArea of 100x100 located at coordinates(x= 1, y= 1)         
        /// </summary>
        public LandingService()
        {
            LandingArea = new Rectangle(LandingAreaX, LandingAreaY, LandingAreaWidth, LandingAreaHeight);
        }

        /// <summary>
        /// Configure LandingService by setting the LandingPlatform
        /// </summary>
        /// <param name="landingPlatform">Rectangle defining the coordinates and size of the LandingPlatform</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the landingPlatform is invalid</exception>
        public void Configure(Rectangle landingPlatform)
        {
            if (!LandingArea.Contains(landingPlatform)) {
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), "Landing Platform is outside the Landing Area");
            }

            if (landingPlatform.Height == 0 || landingPlatform.Width==0)
            {
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), "Landing Platform area is 0");
            }

            LastRequestedLandingPoint = Point.Empty;
            LandingPlatform = landingPlatform;
        }


        /// <summary>
        /// Check if a given Point is safe for landing.
        /// </summary>
        /// <param name="point">Point cordinates to check</param>
        /// <returns></returns>
        public string CheckTrajectory(Point point)
        {
            if (!LandingPlatform.Contains(point))
            {
                return OutOfPlatform;
            }

            if (LastRequestedLandingPoint.SafetyPerimeter(SafetyRadius).Contains(point))
            {
                LastRequestedLandingPoint = point;
                return Clash;
            }

            if (LandingArea.Contains(point))
            {
                LastRequestedLandingPoint = point;
                return OkForLanding;
            }

            return OutOfPlatform;
        }
    }
}
