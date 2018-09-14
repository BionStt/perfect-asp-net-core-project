using Limilabs.FTP.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FtpDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("WEB_DEPLOYER");
            var token = Environment.GetEnvironmentVariable("AUTH_TOKEN");
            var accountName = Environment.GetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME");
            var projectName = Environment.GetEnvironmentVariable("APPVEYOR_PROJECT_NAME");
            var webSite = Environment.GetEnvironmentVariable("WEB_SITE");
            var stage = Environment.GetEnvironmentVariable("RELEASE_STAGE");
            var artifactPath = args[0];
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["AccountName"] = accountName;
                values["ProjectName"] = projectName;
                values["WebSite"] = webSite;
                values["Stage"] = stage;
                values["Token"] = token;
                values["Package"] = artifactPath;

                var response = client.UploadValues(host, "POST", values);

                var responseString = Encoding.Default.GetString(response);
                Console.WriteLine(responseString);
            }

        }
    }
}
