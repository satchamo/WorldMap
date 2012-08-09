using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldMap
{
    public class Geometry
    {
        // ...list-ception
        public List<List<List<double>>> Coordinates { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public Geometry Geometry { get; set; }
        public int Color { get; set; } // the color index
        public Color FillColor { get; set; }
    }
}
