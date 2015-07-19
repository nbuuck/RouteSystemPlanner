using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CountyRoutePlanner
{
    public partial class frmMain : Form
    {

        // https://maps.google.com/maps?saddr=fort+wayne,+in&daddr=Indianapolis,+IN&hl=en&sll=39.766555,-86.441277&sspn=5.986119,11.634521&geocode=FenRcgIdaeDs-ilLgSL_3eQViDGT90J2FHsHgA%3BFcTRXgIdBlXd-ikDanmn_1BriDF86rlA9p2O1g&oq=indiana&mra=ls&t=m&z=9
        private String DirectionsAPIURL = @"http://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&sensor=false";
        private String DirectionsCustomAPIURL = @"http://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&waypoints={2}&sensor=false";
        private List<County> Counties;

        public frmMain()
        {
            InitializeComponent();
        }
        public frmMain(List<County> lCounties):this()
        {
            Counties = lCounties;
        }

        #region Event Handlers
        private void txtMapsURL_TextChanged(object sender, EventArgs e)
        {
            txtOrigin.Text = "";
            txtDestination.Text = "";
        }
        private void txtOrigin_TextChanged(object sender, EventArgs e)
        {
            txtMapsURL.Text = "";
        }
        private void txtDestination_TextChanged(object sender, EventArgs e)
        {
            txtMapsURL.Text = "";
        }
        private void btnPlanIt_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) { return; }
            txtOutput.Text = "";
            
            /*
             * Retrieve Maps Route
             */
            String stream = "";
            try
            {
                WebRequest req = ((txtMapsURL.Text.Trim() != "") ? GetRequestGMapsURL() : GetRequestByOriginDestination());
                WebResponse rep = req.GetResponse();
                StreamReader sr = new StreamReader(rep.GetResponseStream());
                stream = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                OutputLine("There was an error while retrieving the route from Google Maps:");
                Output(ex.ToString());
                return;
            }

            /*
             * Parse Maps Response & Output
             */
            try
            {
                RenderRouteCounties(stream); 
            }
            catch (Exception ex)
            {
                OutputLine("There was an error while parsing the Google Maps response:");
                Output(ex.ToString());
                return;
            }           

        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //Coordinate cTest = new Coordinate();
            //cTest.Latitude = 40.944865;
            //cTest.Longitude = -84.981743;

            //foreach (County curCounty in Counties)
            //{
            //    if (IsCoordinateInPoly(cTest, curCounty.OuterBoundary))
            //    {
            //        txtOutput.Text += "cTest is in " + curCounty.Name + ".\n";
            //    }
            //}
        }
        #endregion

        #region Helper Methods
        
        private bool ValidateInputs()
        {

            if (txtOrigin.Text.Trim() == ""
                && txtDestination.Text.Trim() == ""
                && txtMapsURL.Text.Trim() == "")
            {
                ErrorMessage("You must provide a origin and destination or the Google Maps URL of your route.");
                return false;
            }

            if((txtOrigin.Text.Trim() != "" && txtDestination.Text.Trim() == "")
                || (txtOrigin.Text.Trim() == "" && txtDestination.Text.Trim() != ""))
            {
                ErrorMessage("If specifying a origin or destination, the other must also be specified.");
                return false;
            }

            return true;
        }
        private void Output(String strText)
        {
            txtOutput.Text += strText.Replace("\n", Environment.NewLine);
        }
        private void OutputLine(String strLine)
        {
            Output(strLine + Environment.NewLine);
        }
        private void ErrorMessage(String strMessage, String strCaption = "Error")
        {
            MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private WebRequest GetRequestGMapsURL()
        {

            WebRequest req = null;
            System.Collections.Specialized.NameValueCollection args = HttpUtility.ParseQueryString(txtMapsURL.Text.Trim().Replace("?","&"));
            String src = args["saddr"];
            String dst = args["daddr"];

            // Parse any custom waypoints from the destination before continuing.
            String[] dstparts = dst.Split(':');
            if (dstparts.Length > 1)
            {
                req = HttpWebRequest.Create(String.Format(DirectionsCustomAPIURL, src, dstparts[1], dstparts[0].Replace(" to", "")));
            }
            else
            {
                req = HttpWebRequest.Create(String.Format(DirectionsAPIURL, src, dst));
            }

            return req;
            
        }
        private WebRequest GetRequestByOriginDestination()
        {
            String url = String.Format(DirectionsAPIURL, txtOrigin.Text.Trim(), txtDestination.Text.Trim());
            WebRequest req = HttpWebRequest.Create(url);
            req.ContentType = "application/json; charset=utf-8";
            return req;
        }
        private String GetRouteJSON()
        {
            WebRequest req = ((txtMapsURL.Text.Trim() != "") ? GetRequestGMapsURL() : GetRequestByOriginDestination());
            WebResponse rep = req.GetResponse();
            StreamReader sr = new StreamReader(rep.GetResponseStream());
            return sr.ReadToEnd();
        }
        
        private Boolean IsCoordinateInPoly(Coordinate c, List<Coordinate> bounds)
        {
            Coordinate c1, c2;
            bool inside = false;
            
            if (bounds.Count < 3)
                return inside;
            
            var oldPoint = bounds.Last<Coordinate>();

            foreach(Coordinate newPoint in bounds)
            {
                if (newPoint.Latitude > oldPoint.Latitude)
                {
                    c1 = oldPoint;
                    c2 = newPoint;
                }
                else
                {
                    c1 = newPoint;
                    c2 = oldPoint;
                }
                
                if ((newPoint.Latitude < c.Latitude) == (c.Latitude <= oldPoint.Latitude)
                    && (c.Longitude - c1.Longitude) * (c2.Latitude - c1.Latitude)
                    < (c2.Longitude - c1.Longitude) * (c.Latitude - c1.Latitude))
                {
                    inside = !inside;
                }
                
                oldPoint = newPoint;
            }
            
            return inside;
        }
        private County GetCoordinateCounty(Coordinate c)
        {
            County cnty = null;

            foreach (County cntyTest in Counties)
            {
                if(IsCoordinateInPoly(c, cntyTest.OuterBoundary)) cnty = cntyTest;
            }

            return cnty;
        }
        List<Coordinate> DecodePolyline(String polyline)
        {
            if (polyline == null || polyline == "") return null;

            char[] polylinechars = polyline.ToCharArray();
            int index = 0;
            List<Coordinate> points = new List<Coordinate>();
            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                points.Add(new Coordinate((Convert.ToDouble(currentLat) / 100000.0), (Convert.ToDouble(currentLng) / 100000.0)));
            }

            return points;
        }
        private void RenderRouteCounties(String strRouteJSON)
        {
            JObject o = JObject.Parse(strRouteJSON);
            List<County> matches = new List<County>();

            if (o["routes"] == null)
            {
                OutputLine("The Google Maps response had no viable routes.");
            }

            foreach (JObject joRoute in o["routes"])
            {
                String poly = joRoute["overview_polyline"]["points"].ToString();
                List<Coordinate> points = DecodePolyline(poly);

                foreach (Coordinate c in points)
                {
                    County thisCounty = GetCoordinateCounty(c);
                    if (!matches.Contains(thisCounty)) matches.Add(thisCounty);
                }
            }

            foreach (County match in matches)
            {
                OutputLine(match.Name + " County, " + match.StateAbbr);                
            }
        }
        
        #endregion

    }

    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Coordinate() { }
        public Coordinate(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
    }
    public class County
    {
        public String Name { get; set; }
        public String StateAbbr { get; set; }
        public List<Coordinate> OuterBoundary { get; set; }

        public County()
        {
            OuterBoundary = new List<Coordinate>();
        }
    }

}