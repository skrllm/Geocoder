using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Net;
using System.Globalization;

namespace Map.Model
{
    class MapModel
    {
        private string BingMapsKey = "9BiaMn4AogzSEbv7f0sQ~GjMX91KqpDE-SysAGA7X7Q~AqrVoErtWL9dcgxtYUe7SE_o5xxdKkyAlGT8dgpRJK0XPYUoCQL2WF7e59HjoVl3"; //developer individual key to BING map
        public ObservableCollection<String> AllItems { get; set; }  //Search history collection
        public string CurrentItem { get; set; } //Current Text in ComboBox
        public Pushpin Pushpin {get;set;}  //Only one pushpin on map
        public string Error { get; set; }

        public MapModel()
        {
            this.Pushpin = new Pushpin();
        }
        public Location GetLocation(string SearchText)
        {
            XmlDocument searchResponse = Geocode(SearchText);

            return FindLocation(searchResponse);
        }
        private XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapsKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }


        // Submit a REST Services or Spatial Data Services request and return the response
        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

        private Location FindLocation(XmlDocument xmlDoc)
        {

            //Create namespace manager
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get geocode location in the response 
            XmlNodeList locationElements = xmlDoc.SelectNodes("//rest:Location", nsmgr);
            if (locationElements.Count == 0)
            {
                return null; //If server not found Location
            }
            else
            {
                XmlNodeList displayGeocodePoints = locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);

                double latitude = double.Parse(displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText, CultureInfo.InvariantCulture); //Pushpin Coordinates
                double longitude = double.Parse(displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText, CultureInfo.InvariantCulture);

                Location Location = new Location(latitude, longitude);

                return Location;

            }
        }

    }
}
