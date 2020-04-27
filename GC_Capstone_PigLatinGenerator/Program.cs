using System;
using System.Globalization;

namespace GC_Capstone_PigLatinGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //WELCOME
            string programTitle = "Pig Latin Translator";
            string inputQuery = "text";
            WelcomeUser(programTitle, inputQuery);

            //LOOP
            do
            {
                //INPUT
                string invalidMessage = "This entry has no letters.";
                string input = ValidateInput(inputQuery, invalidMessage);

                //TRANSLATION
                Console.Write("Your entry translates to '");
                string[] inputWords = input.Split();
                string translation;
                for (int i = 0; i < (inputWords.Length); i++)
                {
                    if (DetectSymbols(inputWords[i]))
                    {
                        translation = inputWords[i];
                    } else
                    {
                        translation = TranslateString(inputWords[i]);
                    }

                    if (i == inputWords.Length) {
                        Console.Write($"{translation}");
                    } else
                    {
                        Console.Write($"{translation} ");
                    }
                }
                //Console.Write(TranslateString(inputWords[inputWords.Length -1]));
                Console.WriteLine("' in Pig Latin.");
            } while (RunAgain(programTitle, inputQuery));
        }

        //WELCOME
        public static void WelcomeUser(string programTitle, string inputQuery)
        {
            Console.WriteLine($"Welcome to {programTitle}.");
        }

        //INPUT VALIDATION
        public static string ValidateInput(string inputQuery, string invalidMessage)
        {
            string input = "";
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine($"Please enter {inputQuery}.");
                input = Console.ReadLine().Trim();
                char[] inputArray = input.ToCharArray();
                for (int i = 0; i < inputArray.Length; i++)
                {
                    if (Char.IsLetter(inputArray[i]))
                    {
                        valid = true;
                        break;
                    }
                    else
                    {
                        valid = false;
                    }
                    Console.WriteLine("This entry has no letters.");
                    break;
                }
            }
            return input;
        }

        //METHOD TO DETECT NUMERALS AND SYMBOLS
        public static bool DetectSymbols(string word)
        {
            char[] characterArray = word.ToCharArray();
            for (int i = 0; i < (characterArray.Length - 1); i++)
            {
                if (Char.IsNumber(characterArray[i]) || Char.IsSymbol(characterArray[i]) || characterArray[i] == '@' || characterArray[i] == '#')
                {
                    return true;
                } else if (Char.IsNumber(characterArray[characterArray.Length - 1]))
                {
                    return true;
                } else
                {
                    continue;
                }
            }
            return false;
        }

        //METHOD TO DETECT A VOWEL
        public static bool IsVowel(char c)
        {
            char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
            foreach(char vowel in vowels)
            {
                if (vowel == c)
                {
                    return true;
                }
            }

            return false;
        }

        //METHOD TO FIND THE INDEX OF THE FIRST VOWEL IN A STRING
        public static int FindFirstVowel(string word)
        {
            string lowerCaseWord = word.ToLower();
            char[] characterArray = lowerCaseWord.ToCharArray();
            for(int i = 0; i < lowerCaseWord.Length; i++)
            {
                char letter = lowerCaseWord[i];
                if (IsVowel(letter))
                {
                    return i;
                }
            }
                return -1;
        }

        //METHOD TO IDENTIFY THE CASING OF A WORD (LOWER, UPPER OR TITLE CASE)
        public static string IDCasing(string word)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            if (word.ToLower().Equals(word))
            {
                return "lowerCase";
            } else if (word.ToUpper().Equals(word))
            {
                if (word.Length > 1)
                {
                    return "upperCase";
                } else
                {
                    return "titleCase";
                }
            } else if (myTI.ToTitleCase(word).Equals(word))
            {
                return "titleCase";
            } else {
                return "mixedCase";
            }
        }

        //METHOD TO REAPPLY ORIGINAL CASING TO PIG LATIN TRANSLATION
        public static string ReapplyCasing(string casedWord, string translation)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            if ( IDCasing(casedWord) == "lowerCase" )
            {
                return myTI.ToLower(translation);
            } else if ( IDCasing(casedWord) == "upperCase")
            {
                return myTI.ToUpper(translation);
            } else if ( IDCasing(casedWord) == "titleCase" )
            {
                return myTI.ToTitleCase(translation);
            } else
            {
                return translation;
            }
        }

        //METHOD TO DETECT PUNCTUATION
        public static bool HasPunctuation(char c)
        {
            char[] punctuations = { '.', ',', ':', ';', '?', '¿', '!', '¡' };
            foreach (char punctuation in punctuations)
            {
                if (punctuation == c)
                {
                    return true;
                }
            }

            return false;
        }

        //METHOD TO INDEX PUNCTUATION
        public static int FindPunctuationIndex(string word)
        {
            char[] characterArray = word.ToCharArray();
            for (int i = 0; i < word.Length; i++)
            {
                char letter = characterArray[i];
                if (HasPunctuation(letter))
                {
                    return i;
                }
            }
            return -1;
        }

        //METHOD TO REMOVE PUNCTUATION
        public static string RemovePunctuation(string word)
        {
            string punctuationRemoved;
            if (FindPunctuationIndex(word) == -1)
            {
                return word;
            }
            else
            {
                int punctuationIndex = FindPunctuationIndex(word);
                punctuationRemoved = word.Substring(0, punctuationIndex);
                return punctuationRemoved;
            }

        }

        //METHOD TO TRANSPOSE PUNCTUATION TO THE END OF A WORD
        public static string TransposePunctuation(string word)
        {
            string transposition;
            if (FindPunctuationIndex(word) == -1)
            {
                return word;
            }
            else
            {
                int punctuationIndex = FindPunctuationIndex(word);
                string prefix = word.Substring(0, punctuationIndex);
                string suffix = word.Substring(punctuationIndex + 1);
                string punctuationMark = word.Substring(punctuationIndex, 1);
                transposition = prefix + suffix + punctuationMark;
                return transposition;
            }
        }

        //METHOD TO TRANSLATE A WORD TO PIG LATIN
        public static string TranslateString(string word)
        {
            string removedPunctuation = RemovePunctuation(word);
            int firstVowelIndex = FindFirstVowel(removedPunctuation);
            string translation;
            if (firstVowelIndex == 0)
            {
                translation = word + "way";
            }
            else if (firstVowelIndex == -1)
            {
                translation = word;
            }
            else
            {
                string prefix = word.Substring(firstVowelIndex);
                string suffix = word.Substring(0, firstVowelIndex) + "ay";
                translation = prefix + suffix;
            }
            string transposedTranslation = TransposePunctuation(translation);
            string casedTranslation = ReapplyCasing(word, transposedTranslation);
            return casedTranslation;

        }

        // REPEAT DIALOG
        public static bool RunAgain(string programTitle, string inputQuery)
        {
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine($"Would you like to run {programTitle} again? (y/n)");
                string userResponse = Console.ReadLine().Trim().ToLower();

                if (userResponse == "yes" || userResponse == "y" || userResponse == "esyay")
                {
                    valid = true;
                    return true;
                }
                else if (userResponse == "no" || userResponse == "n" || userResponse == "onay")
                {
                    Console.WriteLine($"Thank you for using {programTitle}.");
                    valid = true;
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid entry.");
                    valid = false;
                }
            }
            return true;
        }
    }
}
