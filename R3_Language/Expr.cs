using System;
using System.Diagnostics;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace R3_Language
{
    /// <summary>
    /// An R Expression
    /// 
    /// EXPR -> Value | Paren | BinOp | Parity | IfZeroExpr |    |    |     |
    /// 
    /// 
    /// </summary>
    abstract class Expr
    {
        public abstract override string ToString();

        public abstract Value Eval();

        public abstract Expr Substitute(Expr agent, Expr agentVal);


        public static Expr Parse(Scanner s)
        {
            if (s.IsNumber())
            {
                return new Num(s.Next<double>());
            }
            else if (s.IsParen())
            { 
                Expr temp =  Parse(s);
                s.Next<string>();
                return new Paren(temp);
            }
            else if (s.IsBinOp())
            {
                Expr left = Parse(s);
                Expr right = Parse(s);
                string op = s.Next<string>();
                s.Next<string>(); // throw away the |
                return new BinOp(left, right, op);
            }
            else if (s.IsParity())
            {
                Expr cond = Parse(s);
                Expr even = Parse(s);
                Expr odd = Parse(s);
                s.Next<string>(); // throw away the \
                return new Parity(cond, even, odd);
            }
            else if (s.IsIfZeroExpr())
            {
                Expr zero = Parse(s);
                Expr onTrue = Parse(s);
                Expr onFalse = Parse(s);
                s.Next<string>();
                return new IfZeroExpr(zero, onTrue, onFalse);
            }
            else if (s.IsLetExpr())
            {
                Id agent = (Id)Parse(s);
                Expr agentVal = Parse(s);
                s.Next<string>();
                Expr localContext = Parse(s);
                Debug.WriteLine(agentVal.GetType().ToString());
                Debug.WriteLine(localContext.GetType().ToString());
                return new LetExpr(agent, agentVal, localContext);
            }
            else if (s.IsId())
            {
                return new Id(s.Next<string>());
            }

            else
            {
                Value x  = new Num(4);
                return x;
            }
        }
    }
}
