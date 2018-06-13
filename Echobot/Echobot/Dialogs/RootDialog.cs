using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Speech.Synthesis;

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
              
            Luis luis = new Luis();
            Thread speechtotext = new Thread(bing.ConvertSpeechToText);
            Thread luiscontroller = new Thread(luis.MakeRequest);
            var activity = await result as Activity;
            if (activity.Text == "start")
            {
               

                Glob.speechthreadcontrol = true;             

                if (i == 1)
                {
                 speechtotext.Start();
                  i = 2;
                }            
            }
            else if(activity.Text == "stop")
            {

            
             Glob.speechthreadcontrol = false;
              luiscontroller.Start();
              //luis.MakeRequest();
            // String Formattedactionplans = "So ladies and gentle men Action plans for todays meeting are " + Glob.Actionpalns;
             //synthesizer.Volume = 100;  // 0...100
             //synthesizer.Rate = -2;
             //synthesizer.Speak(Formattedactionplans);
            }
            // Calculate something for us to return

            // Return our reply to the user
            await context.PostAsync($"Your process {activity.Text} is being processed");

            context.Wait(MessageReceivedAsync);
        }
    }
}