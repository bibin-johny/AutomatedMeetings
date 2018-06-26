using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using Microsoft.CognitiveServices.SpeechRecognition;
using System.IO;

namespace Echobot.Dialogs
{
    public class Bing
    {

        static Bing()
        { }
        MicrophoneRecognitionClient microphoneRecognitionClient;
        string result;
        
        
        public void ConvertSpeechToText()
        {            
                var speechRecognitionMode = SpeechRecognitionMode.LongDictation;
                string language = "en-us";
                string ApiKey = "efa2f45a7d3e4e7a8ef841ed967ee5c0";
                microphoneRecognitionClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(speechRecognitionMode, language, ApiKey);
                microphoneRecognitionClient.OnResponseReceived += ResponseReceived;
                microphoneRecognitionClient.StartMicAndRecognition();
        }

        private void ResponseReceived(object sender, SpeechResponseEventArgs e)

        {
            if (Glob.speechthreadcontrol)
            {              
                    {
                        for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
                        {
                            result = e.PhraseResponse.Results[i].DisplayText;
                        }
                    try
                    {
                        TextWriter txt = new StreamWriter(@"C: \Users\vishnu\Desktop\pyfolder\Output.txt", true);
                        txt.Write(" "+ DateTime.Now.ToString("HH:mm:ss")+" " + result);
                        txt.Close();
                    }
                    catch (IOException error) { }
                }
            }
            else
            {
                result = e.PhraseResponse.Results.ToString();
            }
        }
      
        public void Stop_Click()
        {
            microphoneRecognitionClient.EndMicAndRecognition();
            microphoneRecognitionClient.Dispose();
            microphoneRecognitionClient = null;
        }
    }
}