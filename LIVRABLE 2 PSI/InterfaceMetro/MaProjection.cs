
using System;
using System.Drawing;
using LIVRABLE_2_PSI;

public static class MapProjection
{
    // Ces bornes doivent correspondre aux extrémités géographiques des stations de métro
    private const double MinLat = 48.80;
    private const double MaxLat = 48.90;
    private const double MinLon = 2.25;
    private const double MaxLon = 2.42;

    // Taille de l'image de fond utilisée (fondMetro.png)
    private const int ImageWidth = 1000;  // largeur en pixels
    private const int ImageHeight = 1000; // hauteur en pixels

    public static Point Projection(Station station)
    {
        double xRatio = (station.Longitude - MinLon) / (MaxLon - MinLon);
        double yRatio = (MaxLat - station.Latitude) / (MaxLat - MinLat); // inversé pour avoir le nord en haut

        int x = (int)(xRatio * ImageWidth);
        int y = (int)(yRatio * ImageHeight);

        return new Point(x, y);
    }
}

