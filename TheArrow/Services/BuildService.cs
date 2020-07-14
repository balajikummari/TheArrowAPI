using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace TheArrow.Services
{
    public class BuildService
    {

        public void CloneRepo(string StoreID)
        {
            Directory.CreateDirectory(@"D:\TheArrowBuilds\" + StoreID);
            Repository.Clone("https://github.com/balajikummari/TheArrowTest", @"D:\TheArrowBuilds\" + StoreID);
        }

        public void AddStoreIdtoConfigFile(string StoreID)
        {
            XmlDocument config = new XmlDocument();
            config.Load(@"D:\TheArrowBuilds\" + StoreID + @"\assets\config\config.xml");
            config.SelectSingleNode("//config/storeid").InnerText =  StoreID  ;
            config.Save(@"D:\TheArrowBuilds\" + StoreID + @"\assets\config\config.xml");;
        }

        public void RenameApp(string StoreID)
        {
            string WorkingDirectory = @"D:\TheArrowBuilds\" + StoreID; 
            string RenameCommand = @"flutter pub global run rename --bundleId com.TheArrow." + StoreID;
            RunCommand(RenameCommand, WorkingDirectory);
            string buildCommnad = @"flutter build apk";
            RunCommand(buildCommnad, WorkingDirectory);
        }
    
        public void RunCommand(string Command, string workingDirectory)
        {

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;

            StringBuilder output = new StringBuilder();
            p.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    output.AppendLine(e.Data);
                }
            };

            p.Start();
            p.StandardInput.WriteLine(Command);
            p.BeginOutputReadLine();

            int timeoutParts = 10;
            int timeoutPart = (int)60000 * 5 / timeoutParts;

            do
            {
                Thread.Sleep(500);
                //sometimes halv scond is enough to empty output buff (therefore "exit" will be accepted without "timeoutPart" waiting)
                p.StandardInput.WriteLine("exit");
                timeoutParts--;
            }
            while (!p.WaitForExit(timeoutPart) && timeoutParts > 0);

            if (timeoutParts <= 0)
            {
                output.AppendLine("------ Shell TIMEOUT: " + 10000 + "ms ------");
            }
            Console.WriteLine(output.ToString());
        }
        
    }
}
