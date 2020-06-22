using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FileParser
{
    public class Parser
    {
        public static List<string> WhiteSpaceParser(string adress, CancellationToken cancelToken)
        {
            try
            {
                using (StreamReader fileToRead = new StreamReader(adress))
                {
                    string rowFromFile = String.Empty;
                    List<string> newWords = new List<string>();

                    while ((rowFromFile = fileToRead.ReadLine()) != null)
                    {
                        string[] separatedWords = rowFromFile.Split(new Char[] { ' ' });

                        foreach (var word in separatedWords)
                        {
                            if (cancelToken.IsCancellationRequested)
                                return new List<string>();

                            if (!string.IsNullOrEmpty(word))
                                newWords.Add(word);
                        }
                    };

                    return newWords;
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

