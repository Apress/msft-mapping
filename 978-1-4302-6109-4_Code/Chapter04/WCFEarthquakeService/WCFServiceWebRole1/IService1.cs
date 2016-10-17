using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    
    public interface IService1
    {

        [OperationContract]
        List<Earthquake> GetEarthquakeData();

        [OperationContract]
        List<Earthquake> GetEarthquakeDataBBox(double TLLong, double TLLat, double BRLong, double BRLat);
        //List<String> GetEarthquakeDataBBox();
    }
}
