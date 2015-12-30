using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PAOService
{
    public static class MySqlAccess
    {
        //public static string ConnectingString = "Server=72.52.226.52;Port=3306;Database=sgronwqb_pa;Uid=sgron142_swm;Pwd=penny;";
        public static string ConnectingString = "Server=sgronline.in;Port=3306;Database=sgronwqb_pa;Uid=sgronwqb_pa;Pwd=penny;";

        public static DataSet ExecuteQuery(string query)
        {
            var dataSet = new DataSet();
            using (var connection = new MySqlConnection(ConnectingString))
            {
                connection.Open();
                var command = new MySqlCommand(query, connection);
                command.CommandType = CommandType.Text;
                var adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataSet);
            }
            return dataSet;
        }
        public static DataSet ExecuteStoredProcedure(string procedureName)
        {
            var dataSet = new DataSet();
            using (var connection = new MySqlConnection(ConnectingString))
            {
                connection.Open();
                var command = new MySqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                var adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataSet);
            }
            return dataSet;
        }

        private static ParkingStructure GetParkingStructure(int structureId)
        {
            try
            {
                var dataSet = new DataSet();
                using (var connection = new MySqlConnection(ConnectingString))
                {
                    connection.Open();
                    var command = new MySqlCommand(string.Format("SELECT * FROM `StructureParking` where `Id` = {0}", structureId), connection);
                    command.CommandType = CommandType.Text;
                    var adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataSet);
                }
                if (dataSet.Tables[0].Rows.Count > 0)
                    return new ParkingStructure(dataSet.Tables[0].Rows[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DataSet GetAllParkingStructures()
        {
            try
            {
                var dataSet = new DataSet();
                using (var connection = new MySqlConnection(ConnectingString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM `StructureParking`", connection);
                    command.CommandType = CommandType.Text;
                    var adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataSet);
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ParkingStructure> GetNearestParkingStructures(double destinationlatitude, double destinationLongitude, int count)
        {
            try
            { var structureList = new List<ParkingStructure>();
                var data = GetAllParkingStructures();
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var temp = new ParkingStructure(row);
                    temp.SetDistanceWithOrigin(destinationlatitude, destinationLongitude);
                    structureList.Add(new ParkingStructure(row));
                }
                var filteredStructureList = structureList.OrderBy(p => p.DistanceWithOrigin).Take(count).ToList();

                if(filteredStructureList.Where(s=> s.Id == 1).Count() <= 0)
                {
                    filteredStructureList.Insert(0, (structureList.Where(s => s.Id == 1).Take(1).ToList())[0]);
                }

                return filteredStructureList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCurrentAvailability(int structureId)
        {
            try
            {
                var parkingStructure = GetParkingStructure(structureId);
                return string.Format("{0}|{1}", parkingStructure.SpotCount - parkingStructure.OccupiedSpots, parkingStructure.OccupiedString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateParkingAvailability(int structureId, string occupiedString)
        {
            try
            {
                var characterArray = occupiedString.ToCharArray();
                var occupiedCount = characterArray.Where(a => a == '1').Count();

                var available = GetCurrentAvailability(structureId);
                using (var connection = new MySqlConnection(ConnectingString))
                {
                    connection.Open();
                    var command = new MySqlCommand(string.Format("UPDATE `StructureParking` SET `OccupiedSpots` = {1}, `OccupiedString` = '{2}' where `Id` = {0}", structureId, occupiedCount, occupiedString), connection);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}