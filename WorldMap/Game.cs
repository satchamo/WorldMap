using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace WorldMap
{
    public partial class Game : Form
    {
        MapView mv;
        Dictionary<string, Country> countries;

        // state of the game
        int questionsAsked = 0;
        int questionsCorrect = 0;
        Country country;
        int countryIndex = 0;

        public Game()
        {
            InitializeComponent();

            // load country data and create map
            string json = System.IO.File.ReadAllText("map.json");
            countries = JsonConvert.DeserializeObject<Dictionary<string, Country>>(json);
            foreach (KeyValuePair<string, Country> pair in countries)
            {
                pair.Value.FillColor = Color.Gray;
            }
            mv = new MapView();
            mv.Countries = countries;
            // put the map right below the country textbox
            mv.Location = new Point(countryNameTextBox.Location.X, countryNameTextBox.Location.Y + countryNameTextBox.Size.Height + 5);
            mv.Width = countryNameTextBox.Width;
            mv.Projection = new MercatorProjection();
            mv.Click += new EventHandler(mv_Click);
            Controls.Add(mv);
            // center on the great state of Washington
            //mv.CenterOn(new DoublePoint(-120, 47));

            Random rand = new Random();
            countryIndex = rand.Next(0, countries.Count);
            AskQuestion();
        }

        public Country NextCountry()
        {
            return countries.ElementAt<KeyValuePair<string, Country>>(countryIndex++ % countries.Count).Value;
        }

        public void AskQuestion()
        {
            country = NextCountry();
            countryNameTextBox.Text = country.Name;
        }

        public void UpdateScore(bool correct)
        {
            questionsAsked++;
            if (correct)
            {
                questionsCorrect++;
            }
            
            scoreStatusLabel.Text = String.Format("{0}/{1}", questionsCorrect, questionsAsked);
        }

        void mv_Click(object sender, EventArgs e)
        {
            if (mv.ClickedCountry != null)
            {
                bool correct = mv.ClickedCountry == country;
                country.FillColor = correct ? Color.Green : Color.Red;
                mv.Refresh();
                UpdateScore(correct);
                AskQuestion();
            }
        }
    }
}
