using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NUnit.Framework;

//using System.IO;


namespace R3_Language
{
    /// <summary>
    /// Scanner class for the R Language. 
    /// 
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// Once Scanner is initialized this will be filled with strings that are processable
        /// </summary>
        public string[] Tokens;
        
        /// <summary>
        /// Active index of Tokens String array
        /// </summary>
        private int Head { get; set; } = 0;
        
        /// <summary>
        /// Constructor for Scanner
        /// </summary>
        /// <param name="s">string to scan</param>
        public Scanner(string s)
        {
            this.Explode(s, "");
            if (Tokens.Length == 0)
            {
                Assert.Fail("There must be input");
            }
            else if (Tokens[Head] == "") Head++;
        }
        
        /// <summary>
        /// Checks if there is more processable input. Ignores whitespace.
        /// </summary>
        /// <returns>bool for if there is processable char in scanner</returns>
        public bool HasNext()
        {
            bool ise = false;
            for (int i = Head; i < Tokens.Length; i++)
            {
                ise = Tokens[i] != null;
                Head = i;
            }
            //return Tokens[Head] != null && Tokens[Head] !="";\
            return ise;
        }

        
        /// <summary>
        /// Checks to see if the next valid input matches the word 
        /// If success, it is consumed and head is incremented.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool HasNext(string word)
        {
            
            if (Head <= Tokens.Length | Tokens.Length == 0)
            {
                if (Regex.IsMatch(Tokens[Head], "^" + word + "$"))
                {
                    Head++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Returns the next string
        /// </summary>
        /// <returns>next string</returns>
        public T Next<T>()
        {
            string temp = this.Tokens[Head];
            Head++;
            return (T) Convert.ChangeType(temp, typeof(T));
        }
            
        /// <summary>
        /// Tokenize
        /// </summary>
        /// <param name="s">String to be tokenized</param>
        /// <param name="by">Tokenize on this string</param>
        private void Explode(string s, string by)
        {
			//Tokens = s.Split(new String[] {" ", "<"}, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				Tokens = Regex.Split(s, @" +|(<)|(>)|(\|)").Where(tempLine => tempLine != String.Empty).ToArray<string>();
			}
			catch (Exception se)
			{
			}

        }

        /// <summary>
        /// Prints all tokens of scanner to console
        /// </summary>
        public void PrintAll()
        {
            foreach (string token in Tokens)
            {
                Console.WriteLine(token);
            }
        }

        /// <summary>
        /// Prints rest of tokens of scanner to console
        /// </summary>
        public void PrintRest()
        {
            for (int i = Head; i < Tokens.Length; i++)
            {
                Console.WriteLine(Tokens[i]);
            }
        }

        /// <summary>
        /// IsNumber function
        /// </summary>
        /// <returns>Number next</returns>
        public bool IsNumber()
        {
            double success;
            if (Head < Tokens.Length)
                return double.TryParse(Tokens[Head], out success);
            else return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsParen()
        {
            bool success = this.HasNext("<");
            return success;
        }

		/// <summary>
		/// Ises the bin op.
		/// </summary>
		/// <returns><c>true</c>, if bin op was ised, <c>false</c> otherwise.</returns>
        public bool IsBinOp()
        {
            bool success = this.HasNext(@"\|");
            return success;
        }

		/// <summary>
		/// Ises the parity.
		/// </summary>
		/// <returns><c>true</c>, if parity was ised, <c>false</c> otherwise.</returns>
        public bool IsParity()
        {
            bool success = this.HasNext(@"/");
            return success;
        }

		/// <summary>
		/// Ises if zero expr.
		/// </summary>
		/// <returns><c>true</c>, if if zero expr was ised, <c>false</c> otherwise.</returns>
        public bool IsIfZeroExpr()
        {
            bool success = this.HasNext(@"\[:0");
            return success;
        }

        public bool IsLetExpr()
        {
            bool success = this.HasNext(@"\:o");
            return success;
        }

        public bool IsId()
        {
            return Head < Tokens.Length;
        }







    }
    
}

