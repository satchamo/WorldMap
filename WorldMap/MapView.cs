using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;


namespace WorldMap
{
    public class MapView : System.Windows.Forms.UserControl
    {
        public Dictionary<string, Country> Countries { get; set; }

        // keep a list of the rendered polygons for each country so we can detect a click in the country
        private Dictionary<string, List<Region>> countryRegions;

        private IProjection projection;
        private double[] lonBounds = { 0, 0 };
        private double[] latBounds = { 0, 0 };
        private double pixelsPerLon;
        private Point offset;

        private double zoomMin = 1;
        private double zoomMax = 5;
        private double zoomStep = 1;
        private double scale = 1;

        private enum MouseStates { Up = 1, Down};
        private MouseStates MouseState = MouseStates.Up;
        private Point mouseDelta;
        public Country ClickedCountry { get; set; }

        public float StrokeThickness { get; set; }
        public Color StrokeColor { get; set; }

        public MapView() : base()
        {
            InitializeComponent();

            Countries = new Dictionary<string, Country>();
            projection = new Equirectangular();
            countryRegions = new Dictionary<string, List<Region>>();
            offset = new Point(0, 0);
            StrokeThickness = 2;
            StrokeColor = Color.Black;

            DoubleBuffered = true; // suppress the flickers during redraws

            MouseDown += new MouseEventHandler(MouseDown_Handler);
            MouseUp += new MouseEventHandler(MouseUp_Handler);
            MouseMove += new MouseEventHandler(MouseMove_Handler);
            MouseWheel += new MouseEventHandler(MouseWheel_Handler);
        }

        public void CenterOn(DoublePoint coord)
        {
            coord = projection.ProjectionOf(coord);
            coord.X = (coord.X - lonBounds[0]) * pixelsPerLon;
            coord.Y = (lonBounds[1] - coord.Y) * pixelsPerLon;

            offset.X = (int)((double)Width / 2.0 - coord.X);
            offset.Y = (int)((double)Height / 2.0 - coord.Y);

            Refresh();
        }

        public IProjection Projection
        {
            get { return projection; }
            set
            {
                projection = value;
                // calculate bounds of projection
                DoublePoint bounds;
                // west-most longitude and south most latitude
                bounds = projection.ProjectionOf(new DoublePoint(-180, -90));
                lonBounds[0] = bounds.X;
                latBounds[0] = bounds.Y;
                // east-most longitude and north-most latitude
                bounds = projection.ProjectionOf(new DoublePoint(180, 90));
                lonBounds[1] = bounds.X;
                latBounds[1] = bounds.Y;
                
                // At a scale of 1, we want the map to stretch in the x direction from the left edge of the drawable area, to the right edge
                pixelsPerLon = Width / ((lonBounds[1] - lonBounds[0]) / scale);
            }
        }

        // zoom
        private void MouseWheel_Handler(object sender, MouseEventArgs e)
        {
            double oldScale = scale;
            scale += e.Delta > 0 ? zoomStep : -zoomStep;
            // keep the zoom level between min and max
            scale = Math.Min(Math.Max(zoomMin, scale), zoomMax);

            // move the point where the mouse is to the top left corner
            offset.X -= e.X;
            offset.Y -= e.Y;
            // apply the scaling
            pixelsPerLon = Width / ((lonBounds[1] - lonBounds[0]) / scale);
            offset.X = (int)(offset.X * (scale / oldScale));
            offset.Y = (int)(offset.Y * (scale / oldScale));
            // move the point back to where the mouse is
            offset.X += e.X;
            offset.Y += e.Y;

            Refresh();
        }

        private void MouseMove_Handler(object sender, MouseEventArgs e)
        {
            // drag the map around with the mouse
            if (MouseState == MouseStates.Down)
            {
                // because they are dragging, they didn't click a country
                ClickedCountry = null;
                
                offset.X = offset.X + e.X - mouseDelta.X;
                offset.Y = offset.Y + e.Y - mouseDelta.Y;
                mouseDelta = new Point(e.X, e.Y);

                Refresh();
            }
        }

        private void MouseDown_Handler(object sender, MouseEventArgs e)
        {
            MouseState = MouseStates.Down;
            mouseDelta = new Point(e.X, e.Y);
            ClickedCountry = null;

            // figure out the country that was clicked
            foreach (KeyValuePair<string, List<Region>> entry in countryRegions)
            {
                foreach (Region r in entry.Value)
                {
                    if (r.IsVisible(e.Location))
                    {
                        ClickedCountry = Countries[entry.Key];
                        return;
                    }
                }
            }
        }

        private void MouseUp_Handler(object sender, MouseEventArgs e)
        {
            MouseState = MouseStates.Up;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            // wrap the map
            double wrapPoint = (lonBounds[1] - lonBounds[0]) * pixelsPerLon;
            offset.X = offset.X % (int)wrapPoint;
            
            // plot the map 3 times longitudally, so it wraps around like a globe
            int[] xDeltas = { -(int)wrapPoint, 0, (int)wrapPoint };

            foreach (KeyValuePair<string, Country> entry in Countries)
            {
                var brush = new SolidBrush(entry.Value.FillColor);
                countryRegions[entry.Key] = new List<Region>();

                // for each polygon that defines the country
                foreach (List<List<double>> polygon in entry.Value.Geometry.Coordinates)
                {
                    List<Point> points = new List<Point>();
                    // use the projection to determine where the point should be drawn
                    foreach (List<double> p in polygon)
                    {
                        DoublePoint projected = Projection.ProjectionOf(new DoublePoint(p[0], p[1]));
                        projected.X = (projected.X - lonBounds[0]) * pixelsPerLon;
                        projected.Y = (lonBounds[1] - projected.Y) * pixelsPerLon;

                        // include the offset from the mouse dragging
                        projected.X += offset.X;
                        projected.Y += offset.Y;
                        points.Add(projected.ToPoint());
                    }

                    // draw all 3 copies of the polygon
                    foreach (int dx in xDeltas)
                    {
                        Point[] pointsArray = points.ToArray<Point>();
                        for (int i = 0; i < pointsArray.Length; i++)
                        {
                            pointsArray[i] = new Point(pointsArray[i].X, pointsArray[i].Y);
                            pointsArray[i].X += dx;
                        }
                        
                        graphics.DrawPolygon(new Pen(StrokeColor, StrokeThickness), pointsArray);
                        graphics.FillPolygon(brush, pointsArray);
                        // save the region so we can detect clicks inside the polygon
                        var gp = new GraphicsPath();
                        gp.AddPolygon(pointsArray);
                        countryRegions[entry.Key].Add(new Region(gp));
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MapView
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "MapView";
            this.Size = new System.Drawing.Size(635, 452);
            this.ResumeLayout(false);

        }
    }
}
