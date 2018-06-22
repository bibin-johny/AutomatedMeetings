using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Speech.Synthesis;
using System.Diagnostics;

namespace Echobot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
        int i = 1;
        


        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            Bing bing = new Bing();
            Summary summary = new Summary();
            SpeakerRecoginiser speakerrecoginiser = new SpeakerRecoginiser();
            TimeRepalcer timeRepalcer = new TimeRepalcer();
            Luis luis = new Luis();
            Thread speechtotext = new Thread(bing.ConvertSpeechToText);
            Thread luiscontroller = new Thread(luis.MakeRequest);
            Thread Summarycontroller = new Thread(summary.Summariser);
            Thread speakercontroller = new Thread(speakerrecoginiser.Recoginiser);
            Thread replacerfunc = new Thread(timeRepalcer.replacer); 
            var activity = await result as Activity;
            if (activity.Text == "start")
            {
               // Glob.outfile = @"D:\Output\Output" + DateTime.Now.ToString("h:mm:ss tt") + ".txt";
                
              //  String namehalf = DateTime.Now.ToString("h:mm:ss tt");
               // Glob.outfile = $"Output{namehalf}.txt";
             //   Glob.resfile = @"D:\Output\Result" + DateTime.Now.ToString("h:mm:ss tt") + ".txt";
                Glob.actioncount = 0;
                Glob.speechthreadcontrol = true;             

                if (i == 1)
                {
                 speechtotext.Start();
                 speakercontroller.Start();

                  i = 2;
                }            
            }
            else if(activity.Text == "stop")
            {
             
             Glob.speechthreadcontrol = false;
             speakerrecoginiser.speakerCloser();
             luiscontroller.Start();
             Summarycontroller.Start();
             replacerfunc.Start();
             

             
            }
            // Calculate something for us to return

            // Return our reply to the user
            await context.PostAsync($"Your process {activity.Text} is being processed");

            context.Wait(MessageReceivedAsync);
        }
    }
}