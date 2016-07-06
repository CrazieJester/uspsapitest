using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;

namespace USPStest
{
    class Program
    {
        static void Main(string[] args)
        {
            // fields used in XML string
            string userID = "992DATIX7926";
            string packageID = "1ST";
            string service = "Priority";
            string originZip = "44106";
            string destinationZip = "20770";
            string pounds = "5";
            string ounces = "8";
            string container = "NONRECTANGULAR";
            string size = "LARGE";
            string width = "15";
            string length = "30";
            string height = "15";
            string girth = "55";

            // url of API
            string dest = "http://production.shippingapis.com/ShippingApi.dll?API=RateV4&";

            // xml request to be sent
            string xml = "XML=<RateV4Request " + 
                         "USERID=\"" + userID + "\"> " + 
                         "<Package ID=\"" + packageID + "\"> " + 
                         "<Service>" + service + "</Service> " + 
                         "<ZipOrigination>" + originZip + "</ZipOrigination> " + 
                         "<ZipDestination>" + destinationZip + "</ZipDestination> " + 
                         "<Pounds>" + pounds + "</Pounds> " + 
                         "<Ounces>" + ounces + "</Ounces> " + 
                         "<Container>" + container + "</Container> " + 
                         "<Size>" + size + "</Size> " + 
                         "<Width>" + width + "</Width> " + 
                         "<Length>" + length + "</Length> " + 
                         "<Height>" + height + "</Height> " + 
                         "<Girth>" + girth + "</Girth> " + 
                         "</Package> </RateV4Request>";

            // execute request, store response
            string response = postXMLData(dest, xml);

            Console.WriteLine("Start test");

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            doc.LoadXml(response);

            // show rate
            XmlNodeList elemList = doc.GetElementsByTagName("Rate");
            for (int i = 0; i < elemList.Count; i++)
            {
                Console.WriteLine(elemList[i].InnerXml);
            }

            // if there is an error, desplay it
            elemList = doc.GetElementsByTagName("Description");
            for (int i = 0; i < elemList.Count; i++)
            {
                Console.WriteLine(elemList[i].InnerXml);
            }

            Console.WriteLine("End test");
        }

        // function that takes API url and XML string to make request
        public static string postXMLData(string destinationUrl, string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }
    }

}
