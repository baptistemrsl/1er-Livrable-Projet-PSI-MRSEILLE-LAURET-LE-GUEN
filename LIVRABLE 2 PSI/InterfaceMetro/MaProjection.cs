using System;

namespace LIVRABLE_2_PSI
{
    public class MaProjection
    {
        private readonly double minLat;
        private readonly double maxLat;
        private readonly double minLon;
        private readonly double maxLon;
        private readonly int width;
        private readonly int height;

        public MaProjection(double minLat, double maxLat, double minLon, double maxLon, int width, int height)
        {
            this.minLat = minLat;
            this.maxLat = maxLat;
            this.minLon = minLon;
            this.maxLon = maxLon;
            this.width = width;
            this.height = height;
        }

        public (int x, int y) Projeter(double latitude, double longitude)
        {
            double xRatio = (longitude - minLon) / (maxLon - minLon);
            double yRatio = (latitude - minLat) / (maxLat - minLat);

            int x = (int)(xRatio * width);
            int y = height - (int)(yRatio * height); 

            return (x, y);
        }

        public (int x, int y) Projeter(Station station)
        {
            return Projeter(station.Latitude, station.Longitude);
        }
    }
}
