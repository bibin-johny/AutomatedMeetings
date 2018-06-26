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
        
        


        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            Bing bing = new Bing();
            Summary summary = new Summary();
            SpeakerRecoginiser speakerrecoginiser = new SpeakerRecoginiser();
            TimeRepalcer timeRepalcer = new TimeRepalcer();
                        
          
            Stopper stopper = new Stopper();
            Thread speechtotext = new Thread(bing.ConvertSpeechToText);
            //Thread luiscontroller = new Thread(luis.MakeRequest);
            //Thread Summarycontroller = new Thread(summary.Summariser);
            Thread speakercontroller = new Thread(speakerrecoginiser.Recoginiser);
            Thread thread = new Thread(stopper.stoppingfunction);
            Thread timer = new Thread(timeRepalcer.replacer);
            var activity = await result as Activity;
            if (activity.Text == "start")
            {
                Glob.actioncount = 0;
                Glob.speechthreadcontrol = true;
                
                if (Glob.i == 1)
                {
                    speechtotext.Start();
                    speakercontroller.Start();
                    
                    
                 

                Glob.i =0;
                Glob.stopper = 1;
                }            
            }
            else if(activity.Text == "stop")
            {
                if (Glob.stopper == 1)
                {

                    Glob.speechthreadcontrol=false;
                    thread.Start();                    

                }
        
            }
            // Calculate something for us to return

            // Return our reply to the user
            await context.PostAsync($"Your process {activity.Text} is being processed");

            context.Wait(MessageReceivedAsync);
        }
    }

  
}