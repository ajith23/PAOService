using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PAOService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string GetNearestParkingStructures(double destinationlatitude, double destinationLongitude, int count)
        {
            if (count == 0) count = 1;
            var list = MySqlAccess.GetNearestParkingStructures(destinationlatitude, destinationLongitude, count);
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return javaScriptSerializer.Serialize(list);
        }

        public string GetAvailability(int structureId)
        {
            var availableSpotCount = MySqlAccess.GetCurrentAvailability(structureId);
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return javaScriptSerializer.Serialize(availableSpotCount);
        }

        public bool UpdateAvailability(int structureId, string occupiedStatus)
        {
            MySqlAccess.UpdateParkingAvailability(structureId, occupiedStatus);
            return true;
        }
    }
}
