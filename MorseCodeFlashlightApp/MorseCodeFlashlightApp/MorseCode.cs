using System;
using System.Collections.Generic;
using System.Text;

namespace MorseCodeFlashlightApp
{
    public class MorseCode
    {
        public static Dictionary<char, string> _morseAlphabetDictionary;
        public static Dictionary<char, string> _morseRusDictionary;

        public static void InitializeDictionary()
        {
            _morseAlphabetDictionary = new Dictionary<char, string>()
                                   {
                                       {'a', ".-"},
                                       {'b', "-..."},
                                       {'c', "-.-."},
                                       {'d', "-.."},
                                       {'e', "."},
                                       {'f', "..-."},
                                       {'g', "--."},
                                       {'h', "...."},
                                       {'i', ".."},
                                       {'j', ".---"},
                                       {'k', "-.-"},
                                       {'l', ".-.."},
                                       {'m', "--"},
                                       {'n', "-."},
                                       {'o', "---"},
                                       {'p', ".--."},
                                       {'q', "--.-"},
                                       {'r', ".-."},
                                       {'s', "..."},
                                       {'t', "-"},
                                       {'u', "..-"},
                                       {'v', "...-"},
                                       {'w', ".--"},
                                       {'x', "-..-"},
                                       {'y', "-.--"},
                                       {'z', "--.."},
                                       {'0', "-----"},
                                       {'1', ".----"},
                                       {'2', "..---"},
                                       {'3', "...--"},
                                       {'4', "....-"},
                                       {'5', "....."},
                                       {'6', "-...."},
                                       {'7', "--..."},
                                       {'8', "---.."},
                                       {'9', "----."}
                                   };
            _morseRusDictionary = new Dictionary<char, string>()
            {
                                       {'а', ".-" },
                                       {'б', "-..." },
                                       {'в', ".--" },
                                       {'г', "--." },
                                       {'д', "-.." },
                                       {'е', "." },
                                       {'ж', "...-" },
                                       {'з', "--.." },
                                       {'и', ".." },
                                       {'й', ".--" },
                                       {'к', "-.-" },
                                       {'л', ".-.." },
                                       {'м', "--" },
                                       {'н', "-." },
                                       {'о', "---" },
                                       {'п', ".--." },
                                       {'р', ".-." },
                                       {'с', "..." },
                                       {'т', "-" },
                                       {'у', "..-" },
                                       {'ф', "..-." },
                                       {'х', "..." },
                                       {'ц', "-.-." },
                                       {'ч', "---." },
                                       {'ш', "----" },
                                       {'щ', "--.-" },
                                       {'ъ', ".--.-." },
                                       {'ы', "-.--" },
                                       {'ь', "-..-" },
                                       {'э', "...-..." },
                                       {'ю', ".-" },
                                       {'я', ".-.-" },
            };
        }

        public static string[] Translate(string input)
        {
            List<string> stringBuilder = new List<string>();

            foreach (char character in input)
            {
                if (_morseAlphabetDictionary.ContainsKey(character))
                {
                    stringBuilder.Add(_morseAlphabetDictionary[character]);
                }else if (_morseRusDictionary.ContainsKey(character))
                {
                    stringBuilder.Add(_morseRusDictionary[character]);
                }
            }
            Console.WriteLine(stringBuilder);
            string[] arr = stringBuilder.ToArray();
            return arr;
        }
    }
}
