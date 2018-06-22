using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Echobot.Dialogs
{
    public class SpeakerRecoginiser
    {
        static SpeakerRecoginiser()
        {

        }

        public static string python = @"C:\Users\vishnu\AppData\Local\Programs\Python\Python36-32\python.exe";

        // python app to call
        string myPythonApp = @"C:\Users\vishnu\Desktop\Speaker.py";

        // Create new process start info
         ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

        // make sure we can read the output from stdout
        

            Process myProcess = new Process();

        public void Recoginiser()
        {
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.CreateNoWindow = false;
            //myProcessStartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            // start python app with 3 arguments 
            // 1st arguments is pointer to itself, 2nd and 3rd are actual arguments we want to send
            myProcessStartInfo.Arguments = myPythonApp;
            // assign start information to the process
            myProcess.StartInfo = myProcessStartInfo;
            Console.WriteLine("Value received from script: " + myProcessStartInfo);
            myProcess.Start();
            // Read the standard output of the app we called. 
            // in order to avoid deadlock we will read output first and then wait for process terminate:
            // StreamReader myStreamReader = myProcess.StandardOutput;
            //string myString = myStreamReader.ReadToEnd();
            /*if you need to read multiple lines, you might use:
                string myString = myStreamReader.ReadToEnd() */
            // wait exit signal from the app we called and then close it.
            myProcess.WaitForExit();
        }
        public void speakerCloser()
        {
            myProcess.Close();
            myProcess.Dispose();

        }
    }
}