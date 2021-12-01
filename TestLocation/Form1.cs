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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GMap;
using System.Web.UI.WebControls;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace TestLocation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        
        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            choosetime.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
        }


        List<string> latid = new List<string>() { };
        List<string> longi = new List<string>() { };
        private void button1_Click(object sender, EventArgs e)
        {
            string ticks = choosetime.Value.ToString();
            string dateticks = createDates(ticks);
            string dis = getPosition(dateticks);
            List<MyClass> asd = JsonConvert.DeserializeObject<List<MyClass>>(dis);
            double lat = 0;
            double lon = 0;

            for (int i = 0; i < asd.Count; i++)
            {
                //string name = asd[i].name;
                //int id = asd[i].id;
                lat = asd[i].latitude;
                lon = asd[i].longitude;
                //float altitude = asd[i].altitude;
                //float velocity = asd[i].velocity;
                //string visibility = asd[i].visibility;
                //float footprint = asd[i].footprint;
                //float timestamp = asd[i].timestamp;
                //float daynum = asd[i].daynum;
                //float solar_lat = asd[i].solar_lat;
                //float solar_lon = asd[i].solar_lon;
                //string units = asd[i].units;
                latid.Add(lat.ToString());
                longi.Add(lon.ToString());

            }

            gMapControl1_Load(sender, e);

            //asd.FirstOrDefault();
            //var value = getMapping(asd.FirstOrDefault().latitude, asd.FirstOrDefault().longitude);
        }
        private string createDates(string dTime)
        {
            DateTime curTime = DateTime.Parse(dTime);
            DateTime startTime = curTime.AddHours(-1);
            DateTime endTime = curTime.AddHours(1);
            string ticks = "";
            for (DateTime time = startTime; time <= endTime; time = time.AddMinutes(10))
            {
                if (time == startTime)
                {
                    ticks += (time.Ticks / 100000000);
                }
                else
                {
                    ticks += "," + (time.Ticks / 100000000);
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

        public void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            //gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = 0; i < latid.Count; i++)
            {
                    double slatid = Convert.ToDouble(latid[i]);
                    double slongi = Convert.ToDouble(longi[i]);
                    points.Add(new PointLatLng(slatid,slongi));
                
            }
            GMapPolygon polygon = new GMapPolygon(points, "Route of ISS");
            polyOverlay.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(polyOverlay);

        }
    }

}
