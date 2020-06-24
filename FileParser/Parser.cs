using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace FileParser
{
    public class Parser
    {

        public static EventHandler<(long, long)> LineReadHandler;

        public static List<string> WhiteSpaceParser(string adress, CancellationToken cancelToken)
        {
            try
            {
                using (StreamReader fileToRead = new StreamReader(adress))
                {
                    long length = fileToRead.BaseStream.Length;
                    string rowFromFile = String.Empty;
                    List<string> newWords = new List<string>();

                    while ((rowFromFile = fileToRead.ReadLine()) != null)
                    {
                        string[] separatedWords = rowFromFile.Split(new Char[] { ' ' });
                        LineReadHandler.Invoke(null, (length, rowFromFile.Length));
                        foreach (var word in separatedWords.ToList())
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


