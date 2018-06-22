using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace Echobot.Dialogs
{
    public class Summary
    {
        static Summary()
        {
        }
        public void Summariser()
        {
            string python = @"C:\Users\vishnu\AppData\Local\Programs\Python\Python36-32\python.exe";

            // python app to call
            string myPythonApp = @"C:\Users\vishnu\Desktop\summary.py";

            // Create new process start info
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.CreateNoWindow = false;
            //myProcessStartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            // start python app with 3 arguments 
            // 1st arguments is pointer to itself, 2nd and 3rd are actual arguments we want to send
            myProcessStartInfo.Arguments = myPythonApp;

            Process myProcess = new Process();
            // assign start information to the process
            myProcess.StartInfo = myProcessStartInfo;
            Console.WriteLine("Value received from script: " + myProcessStartInfo);
            myProcess.Start();
            // Read the standard output of the app we called. 
            // in order to avoid deadlock we will read output first and then wait for process terminate:
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadToEnd();
            /*if you need to read multiple lines, you might use:
                string myString = myStreamReader.ReadToEnd() */
            // wait exit signal from the app we called and then close it.

            myProcess.WaitForExit();
            try
            {
                myProcess.Close();
                myProcess.Dispose();
            }
            catch (Exception e)
            {
                myProcess.Close();
                myProcess.Dispose();
            }
        }
    }
}