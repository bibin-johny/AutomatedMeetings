using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Speech;
using System.Speech.Synthesis;

namespace Echobot.Dialogs
{
    public class Luis
    {
        public static int a = 0;
        public static string data;
             static Luis()
        {

        }

            
            public async void MakeRequest()
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // This app ID is for a public sample app that recognizes requests to turn on and turn off lights
                var luisAppId = "28312010-8d4c-49bd-b0bd-63c5a4d67311";
                var subscriptionKey = "2299b95aa00b41688558f2ae41d7a3b0";

                // The request header contains your subscription key
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            try
            {
                StreamReader sr = new StreamReader(@"C: \Users\vishnu\Desktop\pyfolder\Output.txt");
                String text = sr.ReadToEnd();
                sr.Close();
            
                                                                                                    
                String[] sentences = text.Split(new char[] { '.','?','!' });
                foreach (string sentence in sentences)
                {

                    queryString["q"] = sentence;
                    queryString["timezoneOffset"] = "0";
                    queryString["verbose"] = "false";
                    queryString["spellCheck"] = "false";
                    queryString["staging"] = "false";

                    var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
                    var response = await client.GetAsync(uri);

                    var strResponseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(strResponseContent.ToString());
                    JObject jObject = JObject.Parse(strResponseContent);
                    try
                    {
                        try
                        {
                            String intentonly = jObject.SelectToken("topScoringIntent.intent").ToString();

                            if (intentonly == "action")
                            {

                                TextWriter writer = new StreamWriter(@"C: \Users\vishnu\Desktop\pyfolder\Result.txt", true);
                                ++Glob.actioncount;
                                String data = $"{ Glob.actioncount }" + " " + queryString["q"] + ".";
                                writer.Write(data);
                                writer.Close();

                            }
                        }
                        catch (System.NullReferenceException erre) { }
                    }
                    catch (IOException e)
                    {

                    }


                }  

            /*    String appender = $"We have identified {Glob.actioncount}   action plans, they are: ";

                    try
                    {
                        StreamReader reader = new StreamReader(@"D:\Output\Result.txt");
                        String actions = reader.ReadToEnd();
                        String actionsappender = appender + actions;
                        reader.Close();
                        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                        synthesizer.Volume = 100;  // 0...100
                        synthesizer.Rate = -3;     // -10...10             
                        synthesizer.Speak(actionsappender);
                        
                 
                       
                    }
                    catch (IOException data) { }
                
            */

            }
            catch (IOException IOEerror) { }
            return;
            }



            public class Rootobject
            {
                public string query { get; set; }
                public Topscoringintent topScoringIntent { get; set; }
                public Intent[] intents { get; set; }
                public object[] entities { get; set; }
            }

            public class Topscoringintent
            {
                public string intent { get; set; }
                public float score { get; set; }
            }

            public class Intent
            {
                public string intent { get; set; }
                public float score { get; set; }
            }

        }
    }



