using System;
using System.Diagnostics;
using NUnit.Framework;

namespace R3_Language
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner x = new Scanner("876       2.2 <3> <6> 2 <3> <9>");
            Scanner y = new Scanner(@"| 1 [:0 1 2 3 0:] ;) |");
            Scanner z = new Scanner(":o y 3 :U :o x y :U | x y ;) |");
            Scanner n = new Scanner(":o x 3 :U :o x 5 :U |x 3 ;)|");
            Debug.WriteLine(Expr.Parse(z).GetType().ToString());
            //Debug.WriteLine(Expr.Parse(z).ToString());
            Scanner omg = new Scanner(":o y :o z 4 :U <:o y 99 :U z> :U <:o z 5 :U |<:o z 10 :U y> |y z ;)| ;)|>");
            foreach (string str in omg.Tokens)
            {
                Debug.WriteLine(str);
            }
            //Scanner why = new Scanner(":o x 5 :U <:o x |x 1 ;)| :U |x 2 ;)|>");
            //Debug.WriteLine(Expr.Parse(why).Eval());
            //Debug.WriteLine(Expr.Parse(y).ToString());

            String Input = "";
            while (Input != "QUIT")
            {

                Input = Console.ReadLine();
                try
                {
                    Console.WriteLine(Expr.Parse(new Scanner(Input)).Eval());
                }
                catch (AssertionException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            //Debug.WriteLine(Expr.Parse(x));
            ///Debug.WriteLine(Expr.Parse(x));
            // Debug.WriteLine(Expr.Parse(x));
            //Debug.WriteLine(Expr.Parse(x));
            //Debug.WriteLine("d");



        }
    }
}
