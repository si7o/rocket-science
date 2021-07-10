using System.Drawing;

namespace RocketSience
{
    public static class PointExtensions
    {
        /// <summary>
        /// Creates a safety perimeter Rectangle from a Point
        /// </summary>
        /// <param name="point">Point to use as center of the safetyPerimeter</param>
        /// <param name="safetyRadius">points separation for safetyPerimeter</param>
        /// <returns></returns>
        public static Rectangle SafetyPerimeter(this Point point, int safetyRadius = 0)
        {
            if (point.IsEmpty)
            {
                return Rectangle.Empty;
            }

            var diameter = (safetyRadius * 2) + 1;
            var offset = safetyRadius;

            return new Rectangle(
                point.X - offset,
                point.Y - offset,
                diameter,
                diameter);
        }
    }
}
