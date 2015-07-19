using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CountyRoutePlanner
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLoading ldng = new frmLoading();
            ldng.Show();
            Application.DoEvents();
            List<County> counties = GetCounties();
            ldng.Close();
            Application.Run(new frmMain(counties));
            
        }
        
        private static List<County> GetCounties()
        {
            int line = 0;
            List<County> lCounties = new List<County>();
            String[] strCounties = File.ReadAllLines("us.csv");
            Regex rgxRemoveXMLFromBounds = new Regex("([-\\.\\d]+,[-\\.\\d]+)");
            foreach (String strCounty in strCounties)
            {
                if (line > 0)
                {
                    String[] fields = ParseCSVLine(strCounty);
                    if (fields.Length == 12)
                    {
                        // 0 = County Name
                        // 3 = State Abbreviation
                        // 4 = Boundary Coordinates

                        County curCounty = new County();
                        curCounty.Name = fields[0];
                        curCounty.StateAbbr = fields[3];

                        String[] boundaries = fields[4].Split(' ');
                        Coordinate curCoord = null;
                        foreach (String bound in boundaries)
                        {
                            String cleanBound = rgxRemoveXMLFromBounds.Match(bound).Groups[1].Value;
                            String[] coords = cleanBound.Split(',');
                            curCoord = new Coordinate();
                            curCoord.Longitude = Convert.ToDouble(coords[0]);
                            curCoord.Latitude = Convert.ToDouble(coords[1]);
                            curCounty.OuterBoundary.Add(curCoord);
                        }

                        lCounties.Add(curCounty);
                    }
                }
                line++;
            }
            return lCounties;
        }
        private static String[] ParseCSVLine(String line)
        {
            List<String> fields = new List<String>();
            String field = "";
            int intQuoteCount = 0; // Counter for the number of double quotation marks left of the cursor.
            Char prev = ' ';
            for (int i = 0; i < line.Length; i++)
            {
                if (i == 0) { field = line[i].ToString(); }
                if (i > 0)
                {
                    Char c = line[i];
                    if (c == '"' && prev != '\\')
                    {
                        intQuoteCount++;
                    }
                    else if (c == ',' && (intQuoteCount % 2 == 0))
                    {
                        fields.Add(field);
                        field = "";
                    }
                    else
                    {
                        field += c.ToString();
                    }
                }

                prev = line[i];
            }
            return fields.ToArray();
        }

    }
}
