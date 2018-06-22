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
            StreamReader sr = new StreamReader(@"D:\Output\Outputwithtime.txt");
            string[] words = sr.ReadToEnd().Split(' ');
            sr.Close();
            StreamReader speakers = new StreamReader(@"C: \Users\vishnu\Desktop\Speaker.txt");
            string[] speakerdatas = speakers.ReadToEnd().Split(' ');
            
            for (int i = 0;i< speakerdatas.Length-2;i=i+3)
            {
                    Names.Add(speakerdatas[i]+":");
               
                   Times.Add(speakerdatas[i + 2]);              
            }        
            string[] data = words.ToArray();
            TextWriter txterr = new StreamWriter(@"C:\Users\vishnu\Desktop\Times.txt", true);
            for (int i = 0; i < Times.Count; i++)
            {
                txterr.WriteLine(Times[i]);
            }
            txterr.Close();
            foreach (String word in words)
            {
                int index;
                int target = 0;
                double least = 10000000;
                String righttime = "unknown";
                int pos;
                try
                {
                    DateTime oDate = DateTime.ParseExact(word, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    for (index = 0; index < Times.Count - 1; index++)
                    {
                        DateTime cDate = DateTime.ParseExact(Times[index], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        var diffInSeconds = Math.Abs((oDate - cDate).TotalSeconds);
                        if (diffInSeconds < least)
                        {
                            least = diffInSeconds;
                            target = index;
                            righttime = word;
                            TextWriter txterrr = new StreamWriter(@"C:\Users\vishnu\Desktop\data.txt", true);
                            txterrr.WriteLine(righttime);
                            txterrr.Close();
                        }
                        TextWriter txter = new StreamWriter(@"C:\Users\vishnu\Desktop\Lines.txt", true);
                        txter.WriteLine(diffInSeconds);
                        txter.Close();
                    }
                    TextWriter saver = new StreamWriter(@"C:\Users\vishnu\Desktop\FileDAta.txt", true);
                    saver.WriteLine(Names[target]);
                    saver.Close();

                }
                catch (Exception e) { }

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == righttime)
                    {
                        data[i] = Names[target];
                        break;
                    }
                }
            }
            var result = String.Join(" ", data.ToArray());
            String[] tokens = result.Split(',','?','!');
            TextWriter txte = new StreamWriter(@"C:\Users\vishnu\Desktop\samples.txt", true);

            foreach(var token in tokens) {
                txte.WriteLine(token);

            }
            txte.Close();
        }
    }
}