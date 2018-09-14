using Limilabs.FTP.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FtpDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ftpHost = Environment.GetEnvironmentVariable("WEB_HOST");
            var userName = Environment.GetEnvironmentVariable("WEB_DEPLOYER_USERNAME");
            var password = Environment.GetEnvironmentVariable("WEB_DEPLOYER_PASSWORD");
            var path =  Environment.GetEnvironmentVariable("WEBSITE_PATH");
            var releasePath = args[0];
            var deployed = false;
            var maxAttempts = 3;
            while (!deployed && maxAttempts > 0)
            {
                using (Ftp client = new Ftp())
                {
                    Console.WriteLine("Connection to FTP HOST...");
                    client.Connect($"{ftpHost}");
                    Console.WriteLine("Connected to FTP HOST, trying to login now...");
                    client.Login(userName, password);
                    Console.WriteLine("Successfuly logged in!");
                    client.ChangeFolder(path);
                    try
                    {
                        deployed = ProceedDeploy(client, releasePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}!");
                    }
                    client.Close();
                }
                maxAttempts--;
            }
            if (!deployed)
            {
                throw new Exception("Failed to deploy!");
            }
            Console.WriteLine("SUCCESS!");
        }


        private static bool ProceedDeploy(Ftp client, string releasePath)
        {
            List<FtpItem> items = client.GetList();
            Console.WriteLine("Trying to clear remote folder");
            foreach (FtpItem item in items)
            {
                if (item.IsFolder)
                {
                    client.DeleteFolderRecursively(item.Name);
                }
                if (item.IsFile)
                {
                    client.DeleteFile(item.Name);
                }
            }
            Console.WriteLine("Trying to deploy web site...");
            client.UploadFiles("./", releasePath);
            return true;
        }
    }
}
