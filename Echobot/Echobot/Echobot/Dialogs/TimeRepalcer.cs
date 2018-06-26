using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Echobot.Dialogs
{
    public class TimeRepalcer
    {
       static TimeRepalcer()
        {
        }

        public void replacer()
        {
            var Names = new List<string>();
            var Times = new List<string>();
            StreamReader sr = new StreamReader(@"C: \Users\vishnu\Desktop\pyfolder\Output.txt");
            string[] words = sr.ReadToEnd().Split(' ');
            sr.Close();
            StreamReader speakers = new StreamReader(@"C: \Users\vishnu\Desktop\pyfolder\speaker.txt");
            string[] speakerdatas = speakers.ReadToEnd().Split(' ');
            
            for (int i = 0;i< speakerdatas.Length-2;i=i+2)
            {
                    Names.Add(speakerdatas[i]+":");
               
                   Times.Add(speakerdatas[i+1]);              
            }        
            string[] data = words.ToArray();
           
            for(int dat =0;dat<data.Length -1; dat++)
            {
               

                int index;
                int target = 0;  
                double least = 10000000;
                String righttime = "unknown";
                try
                {
                    
                    DateTime oDate = DateTime.ParseExact(data[dat], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);                                              
                    for (index = 0; index < Times.Count - 1; index++)
                    {
                        DateTime cDate = DateTime.ParseExact(Times[index], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        var diffInSeconds = Math.Abs((oDate - cDate).TotalSeconds);
                        if (diffInSeconds < least)
                        {
                            least = diffInSeconds;
                            target = index;
                            righttime = data[dat];
                       
                        }

                        for(int a = 0; a < data.Length; a++)
                        {
                            if(data[a]== righttime)
                            {
                                data[a] = Names[target];
                                break;
                            }
                        }

                    }
                }
                catch (Exception rre)
                {
  
                }
                }
                var result = String.Join(" ", data.ToArray());
            String[] tokens = result.Split('.','?','!');
            TextWriter txte = new StreamWriter(@"C:\Users\vishnu\Desktop\pyfolder\samples.txt", true);


            foreach(var token in tokens)
            {
                txte.WriteLine(token);
            }
                txte.Close();
        }
    }
}