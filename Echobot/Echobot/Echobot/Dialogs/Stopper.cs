using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Echobot.Dialogs
{
    public class Stopper { 

        public void stoppingfunction()
        {
            TextWriter txterrr = new StreamWriter(@"C:\Users\vishnu\Desktop\pyfolder\Stopper.txt", true);
            txterrr.WriteLine("Stopper");
            txterrr.Close();
        }
    }
}