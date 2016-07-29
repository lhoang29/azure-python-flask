using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.ApplicationInsights.TraceListener;

namespace setup
{
    class Program
    {
        /// <summary>
        /// Base address to where the Python zip package lives, e.g. http://myhost.com/
        /// </summary>
        const string PackageAddress = "";
        
        /// <summary>
        /// Name of the Python zip package, e.g. MyPythonFile.zip.
        /// </summary>
        const string PackageName = "";
        
        /// <summary>
        /// Optional; the key used to log diagnostic messages to Application Insights.
        /// This is useful if you want to see live messages while the setup is running.
        /// </summary>
        const string ApplicationInsightsInstrumentationKey = "";

        // https://azure.microsoft.com/en-us/documentation/articles/cloud-services-startup-tasks/
        static string TempPath = RoleEnvironment.GetLocalResource("StartupLocalStorage").RootPath;
        static string TempFile = Path.Combine(TempPath, "setup-logs.txt");
        static string BinPath = Path.Combine(TempPath, "bin");

        static void Main(string[] args)
        {
            if (!string.IsNullOrWhiteSpace(ApplicationInsightsInstrumentationKey))
            {
                Trace.Listeners.Add(new ApplicationInsightsTraceListener(ApplicationInsightsInstrumentationKey));
            }
            Trace.Listeners.Add(new ConsoleTraceListener());

            Directory.CreateDirectory(TempPath);
            Directory.CreateDirectory(BinPath);

            TraceMessage("Temp file path is: " + TempFile);
            TraceMessage("Temp path is: " + TempPath);
            TraceMessage("Bin path is: " + BinPath);

            try
            {
                string fileName = Path.Combine(BinPath, PackageName + ".zip");
                string dirName = Path.Combine(BinPath, PackageName);

                TraceMessage("Checking if needed to download...");
                if (!File.Exists(fileName))
                {
                    using (var wc = new WebClient())
                    {
                        wc.DownloadFile(PackageAddress + PackageName + ".zip", fileName);
                    }
                    TraceMessage("Downloaded file to: " + fileName);
                }

                TraceMessage("Checking if needed to extract " + fileName + "...");
                if (!Directory.Exists(dirName))
                {
                    ZipFile.ExtractToDirectory(fileName, BinPath);
                    TraceMessage("Extracted " + fileName + " into " + dirName);
                }

                TraceMessage("Checking if needed to set PATH variable ...");
                var name = "PATH";
                string pathvar = System.Environment.GetEnvironmentVariable(name);
                if (!pathvar.Contains(dirName))
                {
                    var value = pathvar + ";" + dirName + ";" + Path.Combine(dirName, "Scripts");
                    var target = EnvironmentVariableTarget.Machine;
                    System.Environment.SetEnvironmentVariable(name, value, target);
                    TraceMessage("Environment variable set to: " + value);
                }
            }
            catch (Exception ex)
            {
                TraceMessage(ex.ToString());
                throw;
            }
        }
        static void TraceMessage(string message)
        {
            Trace.WriteLine(message);
            File.AppendAllLines(TempFile, new string[] { message });
        }
    }
}
