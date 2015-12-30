using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PAOService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetData?value={value}", ResponseFormat = WebMessageFormat.Json)]
        string GetData(int value);

        [OperationContract]
        [WebGet(UriTemplate = @"GetNearestParkingStructures?destinationlatitude={destinationlatitude}&destinationLongitude={destinationLongitude}&count={count}", ResponseFormat = WebMessageFormat.Json)]
        string GetNearestParkingStructures(double destinationlatitude, double destinationLongitude, int count);

        [OperationContract]
        [WebGet(UriTemplate = @"GetAvailability?structureId={structureId}", ResponseFormat = WebMessageFormat.Json)]
        string GetAvailability(int structureId);

        [OperationContract]
        [WebGet(UriTemplate = @"UpdateAvailability?structureId={structureId}&occupiedStatus={occupiedStatus}", ResponseFormat = WebMessageFormat.Json)]
        bool UpdateAvailability(int structureId, string occupiedStatus);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
