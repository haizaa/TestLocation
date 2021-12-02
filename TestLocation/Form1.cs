using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using GMap;
using System.Web.UI.WebControls;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;

namespace TestLocation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(0, 0);
            gMapControl1.Zoom = 10;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
        }
        
        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            choosetime.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
        }


        List<string> latid = new List<string>();
        List<string> longi = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            string ticks = choosetime.Value.ToString();
            string dateticks = createDates(ticks);
            string dis = getPosition(dateticks);
            List<MyClass> asd = JsonConvert.DeserializeObject<List<MyClass>>(dis);
            double  lat = 0;
            double  lon =0;

            for (int i = 0; i < asd.Count; i++)
            {
                lat = asd[i].latitude;
                lon = asd[i].longitude;

                GMapOverlay markers = new GMapOverlay("markers");
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(lat, lon),
                GMarkerGoogleType.blue_pushpin);
                markers.Markers.Add(marker);
                gMapControl1.Overlays.Add(markers);
            }

            float timestamp = 0;
            for (int i = 0; i < asd.Count; i++)
            {
                string latid = asd[i].latitude.ToString();
                string longi = asd[i].longitude.ToString();
                timestamp = asd[i].timestamp;

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
                string dateee = dateTime.ToString();

                listView1.Items.Add(new ListViewItem(new string[] { dateee, latid, longi }));
            }

        }
        private string createDates(string dTime)
        {
            DateTime curTime = DateTime.Parse(dTime);
            DateTime startTime = curTime.AddHours(-1);
            DateTime endTime = curTime.AddHours(1);
            string ticks = "";
            for (DateTime time = startTime; time <= endTime; time = time.AddMinutes(10))
            {
                Int32 unixtimestamp = (Int32)(time.ToUniversalTime().Subtract(new DateTime(1970,1,1))).TotalSeconds;
                if (time == startTime)
                {
                    ticks += unixtimestamp;
                }
                else
                {
                    ticks += "," + unixtimestamp;
                }

            }
            //var displayTime = detime.Select(x => x.Ticks.ToString());
            return ticks;
        }
        private string getPosition(string ticks)
        {
            //string position = "https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps=1436029892,1436029902&units=km";
            string position = $"https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps={ticks}";
            var client = new RestClient(position);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }
        private string getPeople()
        {
            string position = $"http://api.open-notify.org/astros.json";
            var client = new RestClient(position);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }
        private string getMapping(float lat, float lon)
        {
            //string position = "https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps=1436029892,1436029902&units=km";
            string map = $"https://api.wheretheiss.at/v1/coordinates/37.795517,-122.393693";
            var client = new RestClient(map);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }
        class MyClass
        {


            public string name { get; set; }
            public int id { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
            public float altitude { get; set; }
            public float velocity { get; set; }
            public string visibility { get; set; }
            public float footprint { get; set; }
            public float timestamp { get; set; }
            public float daynum { get; set; }
            public float solar_lat { get; set; }
            public float solar_lon { get; set; }
            public string units { get; set; }

        }

        class peopleiniss
        {

        }
        public void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.DragButton = MouseButtons.Left;
            //GMapOverlay polyOverlay = new GMapOverlay("polygons");
            //List<PointLatLng> points = new List<PointLatLng>();
            //for (int i = 0; i < latid.Count; i++)
            //{
            //    double slatid = Convert.ToDouble(latid[i]);
            //    double slongi = Convert.ToDouble(longi[i]);
            //    points.Add(new PointLatLng(slatid, slongi));
            //}
            //GMapPolygon polygon = new GMapPolygon(points, "Route of ISS");
            //polyOverlay.Polygons.Add(polygon);
            //gMapControl1.Overlays.Add(polyOverlay);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ticks = choosetime.Value.ToString();
            string dateticks = createDates(ticks);
            string dis = getPosition(dateticks);
            List<MyClass> asd = JsonConvert.DeserializeObject<List<MyClass>>(dis);
            double lat = 0;
            double lon = 0;
            float timestamp = 0;

            for (int i = 0; i < asd.Count; i++)
            {
                lat = asd[i].latitude;
                lon = asd[i].longitude;
                timestamp = asd[i].timestamp;

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();

                listView1.Items.Add(datetime.ToString());
                listView1.Items.Add(lat.ToString());
                listView1.Items.Add(lon.ToString());
            }
            }
        }

}
