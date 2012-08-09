using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WorldMap
{
    // Point.X and Point.Y are int types, but we need double precision for projections
    public class DoublePoint
    {
        public DoublePoint(double X0, double Y0)
        {
            X = X0;
            Y = Y0;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }
    }

    public interface IProjection
    {
        DoublePoint ProjectionOf(DoublePoint p);
    }

    class MercatorProjection : IProjection
    {
        public DoublePoint ProjectionOf(DoublePoint p)
        {
            double lat = p.Y;
            double lon = p.X;

            lat *= Math.PI / 180.0;
            lat = Math.Log((1.0 + Math.Sin(lat)) / Math.Cos(lat), Math.E);
            lat *= 180.0 / Math.PI;

            // clamp down the the latitude since it gets really elongated near the poles. 
            // just picks values that look good
            if (lat < -200) lat = -200;
            if (lat > 180) lat = 180;
    
            return new DoublePoint(p.X, lat);
        }
    }

    class Equirectangular : IProjection
    {
        public DoublePoint ProjectionOf(DoublePoint p)
        {
            return p;
        }
    }
}
