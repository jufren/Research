using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitCoinPriceTrackerLog
{
    class Program
    {
        static bool stop = false;
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(InvokeMethod));
            t.Start();

        }
        static void InvokeMethod()
        {
            while (!stop)
            {
                string URI = ConfigurationSettings.AppSettings["HostUrl"];
                int interval = int.Parse(ConfigurationSettings.AppSettings["Interval"].ToString());
                
                string rate_filename = "rates_" + DateTime.Today.ToString("ddMMyyyy") + ".txt";
                
                GetJsonFromURL(URI, rate_filename);
                Thread.Sleep(1000 * 60 * interval); // 5 Minutes
                                             //Have a break condition
            }
        }
        static int StartThresholdCounter = 0;
        static void CheckThreshold(float rate)
        {
            
            float MinThreshold = float.Parse(ConfigurationSettings.AppSettings["MinThresholdPrice"].ToString());
            int MaxThresholdCount = int.Parse(ConfigurationSettings.AppSettings["MaxThresholdCount"].ToString());
            string logFile="log_" + DateTime.Today.ToString("ddMMyyyy") + ".txt";
            if (rate <= MinThreshold) { 
                WriteFile(logFile,"Rate is lower than the Min Threshold:"+ rate + ", Counter:"+ StartThresholdCounter);
                StartThresholdCounter++;
            }
            if (StartThresholdCounter > MaxThresholdCount)
            {
                StartThresholdCounter = 0;
                stop = true;
            }
            
        }
        static void GetJsonFromURL(string url,string filename)
        {
            using (WebClient wc = new WebClient())
            {
                string getpage = wc.DownloadString(url);
                //wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                //string HtmlResult = wc.UploadString(URI, myParameters);
                JObject o = JObject.Parse(getpage);
                //o.USD.rate;
                string rate = o["SGD"].ToString();
                //string updated = o["time"]["updated"].ToString();
                //updated = updated.Replace(" UTC", "");
                DateTime t = DateTime.Now.AddSeconds(-10);
                WriteFile(filename, t.ToString() + "," + rate);
                CheckThreshold(float.Parse(rate));
            }
        }
        static void WriteFile(string filename, string value)
        {
            
            string filename1 = ConfigurationSettings.AppSettings["DataFileFolder"] + filename;
            StreamWriter sw = new StreamWriter(filename1,true);
            sw.WriteLine(value);
            sw.Close();
        }
    }
}

