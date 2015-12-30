using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PAOService
{
    public class ParkingStructure
    {
        public ParkingStructure(DataRow row)
        {
            Id = Convert.ToInt32(row["Id"].ToString());
            Name = row["Name"].ToString();
            Address = row["Address"].ToString();
            FloorCount = Convert.ToInt32(row["FloorCount"].ToString());
            Latitude = Convert.ToDouble(row["Latitude"].ToString());
            Longitude = Convert.ToDouble(row["Longitude"].ToString());
            Cost = Convert.ToDouble(row["Cost"].ToString());
            SpotCount = Convert.ToInt32(row["SpotCount"].ToString());
            OccupiedSpots = Convert.ToInt32(row["OccupiedSpots"].ToString());
            OccupiedString = row["OccupiedString"].ToString();
            if(row["Map"] != null && row["Map"].ToString() != string.Empty)
                Map = (byte[])row["Map"];
            //Map = new byte[row["Map"].ToString().Length * sizeof(char)];
            //Buffer.BlockCopy(row["Map"].ToString().ToCharArray(), 0, Map, 0, Map.Length);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int FloorCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Cost { get; set; }
        public int SpotCount { get; set; }
        public int OccupiedSpots { get; set; }
        public string OccupiedString { get; set; }
        public double DistanceWithOrigin { get; set; }

        public byte[] Map { get; set; }
        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }
        public const double EarthRadiusInMiles = 3956.0;
        internal void SetDistanceWithOrigin(double destinationlatitude, double destinationLongitude)
        {
            var radius = EarthRadiusInMiles;
            DistanceWithOrigin = radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(Latitude, destinationlatitude)) / 2.0), 2.0) + Math.Cos(ToRadian(Latitude)) * Math.Cos(ToRadian(destinationlatitude)) * Math.Pow(Math.Sin((DiffRadian(Longitude, destinationLongitude)) / 2.0), 2.0)))));
            //DistanceWithOrigin = Math.Sqrt(((destinationlatitude - Latitude) * (destinationlatitude - Latitude) + (destinationLongitude - Longitude) * (destinationLongitude - Longitude)));
        }
    }
}