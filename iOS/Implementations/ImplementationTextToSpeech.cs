using System;
using AssistidCollector2.Interfaces;
using AssistidCollector2.iOS.Implementations;
using AVFoundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImplementationTextToSpeech))]
namespace AssistidCollector2.iOS.Implementations
{
    public class ImplementationTextToSpeech : InterfaceTextToSpeech
    {
        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();

            var speechUtterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 3,
                Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                Volume = 0.9f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
}
