 using System;
using System.Collections.Generic;
 using System.Diagnostics;
//using System.Diagnostics.Eventing.Reader;
 using System.Dynamic;
 using System.Linq;
 using System.Runtime.CompilerServices;
 using System.Security.Cryptography;
 using System.Text;
 using System.Text.RegularExpressions;
 using System.Threading.Tasks;
 using NUnit.Framework;

namespace R3_Language
{
    class Value : Expr
    {
        public override int GetHashCode()
        {
            return 2934334;
        }

        public override Value Eval()
        {
            return this;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            throw new NotImplementedException();
        }
    }

    class Num : Value
    {
        private double Val;
        public Num (double v)
        {
            this.Val = v;
        }

        public double DoubleValue()
        {
            return this.Val;
        }

        public override string ToString()
        {
            return Convert.ToString(Val);
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            return agentVal;
        }

        public override bool Equals(Object that)
        {
            if(this == that) {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                Num thatt = (Num)that;
                return ScanningUtilities.EqualsApprox(this.Val, thatt.Val);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 3049494; // fingerprint
                hashSoFar += this.Val.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
            
        }


    }

    class Id : Expr
    {
        public Regex id_Pattern = new Regex("^[a-zA-z]\\w*$");
        private string Agent { get; set; }

        public Id(string agent)
        {
            Assert.True(id_Pattern.IsMatch(agent), "Identifier cannot start with number" );
            Agent = agent;
        }

        public override string ToString()
        {
            return Agent;
        }

        public override Value Eval()
        {
            Assert.Fail("Identifier cannot be evaluated in local context : " + Agent);
            return new Value();
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            return agentVal;
            
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                Id thatt = (Id)that;
                return this.Agent.Equals(thatt.Agent);
            }
        }
    }

    class Paren : Expr
    {
        private Expr Exp { get; set; }

        public Paren(Expr exp)
        {
            Exp = exp;
        }
        public override string ToString()
        {
            return "<" + this.Exp.ToString()
                   + ">";
        }

        public override Value Eval()
        {
            return this.Exp.Eval();
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                Paren thatt = (Paren) that;
                return this.Exp.Equals(thatt.Exp);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 3049494; // fingerprint
                hashSoFar += this.Exp.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            if (this.Exp.Equals(agent))
            {
                this.Exp = agentVal;
            }
            else
                this.Exp.Substitute(agent, agentVal);

            return this;
        }
    }

    class BinOp : Expr
    {
        public Expr Left, Right;
        public string Op;

        public BinOp(Expr left, Expr right, string op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public override string ToString()
        {
            return ("|"
                 + " "
                 + this.Left.ToString()
                 + " "
                 + this.Right.ToString()
                 + " "
                 + Op
                 + " "
                 + "|");
        }

        public override Value Eval()
        {
            double leftValue = ((Num) this.Left.Eval()).DoubleValue();
            double rightValue = ((Num) this.Right.Eval()).DoubleValue();
            if (this.Op.Equals("xD"))
            {
                return new Num(leftValue * rightValue);
            }
            else if (this.Op.Equals(";)"))
            {
                return new Num(leftValue + rightValue);
            }
            else if (this.Op.Equals(";("))
            {   
                return new Num(leftValue - rightValue);
            }
            else if (this.Op.Equals(";%"))
            {
                return new Num(rightValue * ((leftValue/rightValue)
                    - ScanningUtilities.Floor(leftValue/rightValue)));
            }

            else
            {
                throw new AssertionException("Not a valid Operation: " + Op);
            }
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                BinOp thatt = (BinOp)that;
                return (this.Right.Equals(thatt.Right) && 
                    this.Left.Equals(thatt.Left) &&
                    this.Op.Equals(thatt.Op));
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 3049494; // fingerprint
                hashSoFar += this.Right.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.Left.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.Op.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            if (this.Left.Equals(agent))
            {
                this.Left = agentVal;
            }
            else
                this.Left.Substitute(agent, agentVal);

            if (this.Right.Equals(agent))
            {
                this.Right = agentVal;
            }
            else
                this.Right.Substitute(agent, agentVal);
            return this;
        }
    }

    class Parity : Expr
    {
        public Expr Cond, EvenPart, OddPart;

        public Parity(Expr cond, Expr even, Expr odd)
        {
            Cond = cond;
            EvenPart = even;
            OddPart = odd;
        }
        public override string ToString()
        {
            return @"/"
                   + " "
                   + this.Cond.ToString()
                   + " "
                   + this.EvenPart.ToString()
                   + " "
                   + this.OddPart.ToString()
                   + " "
                   + @"\";
        }

        public override Value Eval()
        {
            if (((Num) (this.Cond.Eval())).DoubleValue()%2 == 0)
            {
                this.OddPart.Eval();
                return this.EvenPart.Eval();
            }
            else
            {
                this.EvenPart.Eval();
                return this.OddPart.Eval();
            }
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                Parity thatt = (Parity)that;
                return (this.Cond.Equals(thatt.Cond) &&
                    this.EvenPart.Equals(thatt.EvenPart) &&
                    this.OddPart.Equals(thatt.OddPart));
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 30439494; // fingerprint
                hashSoFar += this.Cond.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.EvenPart.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.OddPart.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            if (this.Cond.Equals(agent))
            {
                this.Cond = agentVal;
            }
            else
                this.Cond.Substitute(agent, agentVal);
            if (this.EvenPart.Equals(agent))
            {
                this.EvenPart = agentVal;
            }
            else
                this.EvenPart.Substitute(agent, agentVal);
            if (this.OddPart.Equals(agent))
            {
                this.OddPart = agentVal;
            }
            else
                this.OddPart.Substitute(agent, agentVal);
            return this;
        }
    }

    class IfZeroExpr : Expr
    {
        public Expr Zero, OnTrue, OnFalse;

        public IfZeroExpr(Expr zero, Expr onTrue, Expr onFalse)
        {
            Zero = zero;
            OnTrue = onTrue;
            OnFalse = onFalse;
        }
        public override string ToString()
        {
            return "[:0"
                   + " "
                   + this.Zero.ToString()
                   + " "
                   + this.OnTrue.ToString()
                   + " "
                   + this.OnFalse.ToString()
                   + " 0:]";
        }

        public override Value Eval()
        {

            if (((Num) (this.Zero.Eval())).DoubleValue() == 0)
            {
                this.OnFalse.Eval();
                return this.OnTrue.Eval();
            }
            else
            {
                this.OnTrue.Eval();
                return this.OnFalse.Eval();
            }
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                IfZeroExpr thatt = (IfZeroExpr)that;
                return (this.Zero.Equals(thatt.Zero) &&
                    this.OnTrue.Equals(thatt.OnTrue) &&
                    this.OnFalse.Equals(thatt.OnFalse));
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 30439494; // fingerprint
                hashSoFar += this.Zero.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.OnTrue.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.OnFalse.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            if (this.Zero.Equals(agent))
            {
                this.Zero = agentVal;
            }
            else
                this.Zero.Substitute(agent, agentVal);
            if (this.OnTrue.Equals(agent))
            {
                this.OnTrue = agentVal;
            }
            else
                this.OnTrue.Substitute(agent, agentVal);
            if (this.OnFalse.Equals(agent))
            {
                this.OnFalse = agentVal;
            }
            else
                this.OnFalse.Substitute(agent,agentVal);

            return this;
        }
    }

    class LetExpr : Expr
    {
        private Id Agent { get; set; }
        private Expr AgentVal { get; set; }
        private Expr LocalContext { get; set; }

        public LetExpr(Id agent, Expr agentVal, Expr localContext)
        {
            Agent = agent;
            AgentVal = agentVal;
            LocalContext = localContext;
        }


        public override string ToString()
        {
            return ":o"
                   + " "
                   + Agent.ToString()
                   + " "
                   + AgentVal.ToString()
                   + " :U "
                   + LocalContext.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Value Eval()
        {
            //TODO : WORK ON THIS
            LocalContext = LocalContext.Substitute(Agent, AgentVal);
            //Assert.Pass(LocalContext.ToString());
            //Debug.WriteLine(LocalContext.Eval());
            return LocalContext.Eval();
        }

        public override bool Equals(Object that)
        {
            if (this == that)
            {
                return true;
            }
            else if (that == null)
            {
                return false;
            }
            else if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                LetExpr thatt = (LetExpr)that;
                return (this.Agent.Equals(thatt.Agent) &&
                    this.AgentVal.Equals(thatt.AgentVal) &&
                    this.LocalContext.Equals(thatt.LocalContext));
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashSoFar = 30439494; // fingerprint
                hashSoFar += this.Agent.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.AgentVal.GetHashCode();
                hashSoFar *= 31;
                hashSoFar += this.LocalContext.GetHashCode();
                hashSoFar *= 31;
                return hashSoFar;
            }
        }

        public override Expr Substitute(Expr agent, Expr agentVal)
        {
            /*
            if (this.Agent.Equals(agent))
            {
                LocalContext = LocalContext.Substitute(Agent, AgentVal);
                LocalContext = LocalContext.Substitute(agentVal, agent);
                

            }
            else if (this.AgentVal.Equals(agent))
            {
                
                LocalContext = LocalContext.Substitute(Agent, agentVal);
                AgentVal = AgentVal.Substitute(AgentVal, agentVal);
                
            }

            LocalContext = LocalContext.Substitute(agent, agentVal);
            */

            if (this.Agent.Equals(agent))
            {
                this.LocalContext.Substitute(Agent, AgentVal);
            }
            else
                this.Agent.Substitute(agent, agentVal);

            if (this.AgentVal.Equals(agent))
            {
                this.AgentVal = agentVal;
            }
            else
                this.AgentVal.Substitute(agent, agentVal);

            if (this.LocalContext.Equals(agent))
            {
                this.AgentVal = agentVal;
            }
            this.LocalContext.Substitute(agent, agentVal);
            

            return this;
        }
    }


}
