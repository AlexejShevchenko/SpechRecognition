using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OneInc.Services
{
    public class RecognizeService : IRecognizeService
    {
        private List<string> _options;
        public RecognizeService()
        {
            _options = new List<string>()
            {
                "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"
            };
        }

        public int Recognize(Stream stream)
        {
            try
            {
                using (SpeechRecognitionEngine recognizer =
        new SpeechRecognitionEngine(new CultureInfo("en-US")))
                {

                    // Create a grammar, construct a Grammar object, and load it to the recognizer.
                    GrammarBuilder gb = new GrammarBuilder();
                    gb.Culture = recognizer.RecognizerInfo.Culture;

                    Choices digChoises = new Choices(_options.ToArray());
                    gb.Append(digChoises);

                    Grammar grammar = new Grammar(gb);
                    grammar.Name = "digits";

                    recognizer.LoadGrammar(grammar);

                    recognizer.SetInputToWaveStream(stream);

                    var result = recognizer.Recognize();

                    return _GetOptionNum(result?.Text);
                }
            }
            catch(Exception e)
            {
                throw e;
                //return 0;
            }
        }

        private int _GetOptionNum(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return _options.IndexOf(text)+1;
        }

    }
}