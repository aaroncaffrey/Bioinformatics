using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BioinformaticsHelperLibrary.ProproteinInterface
{
    public static class ProproteinInterfaceServiceClient
    {
        
        public static string CacheFilename(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            var invalid = Path.GetInvalidFileNameChars();

            const string folder = @"c:\k\";
            const string extension = @".xml";

            var queryString = string.Join("", scanProproteinInterfaceParameters.QueryString(false).Select(a => !invalid.Contains(a) ? a : '_').ToArray());

            var filename = folder + queryString + extension;

            if (filename.Length > 259)
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    var data = Encoding.UTF8.GetBytes(queryString);
                    var hash = shaM.ComputeHash(data);
                    filename = folder + BitConverter.ToString(hash) + extension;
                }
            }

            var path = Path.GetDirectoryName(filename);

            if (path != null) Directory.CreateDirectory(path);

            return filename;
        }

        public static void SaveProproteinInterfaceResponse(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters, string xml)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            if (xml == null) return;

            var filename = CacheFilename(scanProproteinInterfaceParameters);

            File.WriteAllText(filename, xml);
        }

        public static bool IsProproteinInterfaceResponseCached(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            var filename = CacheFilename(scanProproteinInterfaceParameters);

            return File.Exists(filename);
        }

        public static ProproteinInterfaceMatchSet LoadProproteinInterfaceResponse(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters, bool requestNonCahcedResponse = false)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            var filename = CacheFilename(scanProproteinInterfaceParameters);
            string xmlString = null;

            if (File.Exists(filename))// && new System.IO.FileInfo(filename).Length > 0)
            {
                //return null;
                xmlString = File.ReadAllText(filename);
            }
            
            if (xmlString == null && requestNonCahcedResponse)
            {
                xmlString = GetProproteinInterfaceXml(scanProproteinInterfaceParameters);

                SaveProproteinInterfaceResponse(scanProproteinInterfaceParameters, xmlString);
            }

            if (!string.IsNullOrEmpty(xmlString))
            {
                return ProproteinInterfaceXmlToObject(xmlString);
            }

            return null;
        }

        public static ProproteinInterfaceMatchSet ProproteinInterfaceXmlToObject(string xmlString)
        {
            if (xmlString == null) throw new ArgumentNullException(nameof(xmlString));

            ProproteinInterfaceMatchSet result = null;

            using (StringReader stringReader = new StringReader(xmlString))
            {
                //XmlSerializer xmlSerializer = new XmlSerializer(typeof (ProproteinInterfaceMatchSet));
                XmlSerializer xmlSerializer = XmlSerializer.FromTypes(new [] { typeof(ProproteinInterfaceMatchSet) })[0];

                var xmlDeserialized = xmlSerializer.Deserialize(stringReader);

                ProproteinInterfaceMatchSet proproteinInterfaceMatchSet = xmlDeserialized as ProproteinInterfaceMatchSet;
                if (proproteinInterfaceMatchSet != null)
                {
                    result = proproteinInterfaceMatchSet;
                }
            }

            return result;
        }

        public static string GetProproteinInterfaceXml(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            var timeout = new TimeSpan(0, 1, 30, 0);
            const int retries = 5;
            var retryDelay = new TimeSpan(0, 0, 0, 5);

            return GetProproteinInterfaceXml(scanProproteinInterfaceParameters, timeout, retries, retryDelay);
        }

        public static string GetProproteinInterfaceXml(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters, TimeSpan timeout)
        {
            if (scanProproteinInterfaceParameters == null) throw new ArgumentNullException(nameof(scanProproteinInterfaceParameters));

            const int retries = 5;
            var retryDelay = new TimeSpan(0, 0, 0, 5);

            return GetProproteinInterfaceXml(scanProproteinInterfaceParameters, timeout, retries, retryDelay);
        }

        public static string GetProproteinInterfaceXml(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters, int retries, TimeSpan retryDelay)
        {
            if (retries < 0) throw new ArgumentOutOfRangeException(nameof(retries));

            var timeout = new TimeSpan(0, 1, 30, 0);

            return GetProproteinInterfaceXml(scanProproteinInterfaceParameters, timeout, retries, retryDelay);
        }

        public static string GetProproteinInterfaceXml(ScanProproteinInterfaceParameters scanProproteinInterfaceParameters, TimeSpan timeout, int retries, TimeSpan retryDelay)
        {
            while (retries > 0)
            {
                retries--;

                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(1, 30, 0);
                    client.BaseAddress = new Uri("http://www.expasy.org/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    //client.Timeout = ?

                    var queryString = "cgi-bin/proproteinInterface/PSScan.cgi?" + scanProproteinInterfaceParameters.QueryString();
                    // HTTP GET
                    var response = client.GetAsync(queryString);

                    if (response.IsCanceled || response.IsFaulted)
                    {
                        if (response.Exception != null) throw response.Exception;
                    }

                    //response.Result.EnsureSuccessStatusCode();

                    try
                    {
                        response.Wait();

                        if (response.Result.IsSuccessStatusCode)
                        {
                            var readAsStringAsync = response.Result.Content.ReadAsStringAsync();

                            readAsStringAsync.Wait();

                            var resultString = readAsStringAsync.Result;

                            if (resultString == null) { resultString = ""; }

                            return resultString;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    catch (AggregateException)
                    {
                        if (retries == 0)
                        {
                            return null;
                        }
                        else
                        {
                            var t = Task.Delay(1000);
                            t.Wait();
                        }
                    }
                }
            }

            return null;
        }


    }
}
