using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace R3_Language
{
    [TestFixture]
    class ExprTest
    {
        [TestFixture]
        class LetExprTest
        {
            [Test]
            public void TestEquals1()
            {
                Scanner scan = new Scanner(@":o d 2 :U d");
                Assert.True(Expr.Parse(scan).Equals(new LetExpr(new Id("d"), new Num(2), new Id("d"))));
            }

            [Test]
            public void TestEval1()
            {
                Scanner scan = new Scanner(@":o d 2 :U d");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(2)));
            }

            [Test]
            public void TestEval2()
            {
                Scanner scan = new Scanner(@":o d | 1 2 ;) | :U d");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestNestedEval1()
            {
                Scanner scan = new Scanner(@"| 1 :o d 2 :U | d 3 ;) | ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(6)));
            }
        }


        [TestFixture]
        class IfZeroExprTest
        {
            [Test]
            public void TestEquals1()
            {
                Scanner scan = new Scanner(@"[:0 1 2 3 0:]");
                Assert.True(Expr.Parse(scan).Equals(new IfZeroExpr(new Num(1), new Num(2), new Num(3))));
            }

            [Test]
            public void TestEval1()
            {
                Scanner scan = new Scanner(@"[:0 1 2 3 0:]");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestInnerEval1()
            {
                Scanner scan = new Scanner(@"[:0 0 / 1 2 3 \ 9 0:]");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestInnerEval2()
            {
                Scanner scan = new Scanner(@"| 1 [:0 1 2 3 0:] ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(4)));
            }

            [Test]
            public void TestVarSub1()
            {
                Scanner scan = new Scanner(@":o lol [:0 0 1 2 0:] :U lol");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(1)));
            }

            [Test]
            public void TestVarSub2()
            {
                Scanner scan = new Scanner(@":o test 4 :U [:0 0 test 6 0:]");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(4)));
            }

            [Test]
            public void TestVarSub3()
            {
                Scanner scan = new Scanner(@":o test [:0 0 90 9 0:] :U [:0 1 2 test 0:]");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(90)));
            }
        }

        [TestFixture]
        class ParityTest
        {
            [Test]
            public void TestEquals1()
            {
                Scanner scan = new Scanner(@"/ 1 2 3 \");
                Assert.True(Expr.Parse(scan).Equals(new Parity(new Num(1), new Num(2), new Num(3))));
            }

            [Test]
            public void TestEval1()
            {
                Scanner scan = new Scanner(@"/ 1 2 3 \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num (3)));
            }

            [Test]
            public void TestEval2()
            {
                Scanner scan = new Scanner(@"/ 2 2 3 \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(2)));
            }

            [Test]
            public void TestInnerEval1()
            {
                Scanner scan = new Scanner(@"/ 2 / 2 2 3 \ 3 \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(2)));
            }

            [Test]
            public void TestInnerVar1()
            {
                Scanner scan = new Scanner(@":o fudge 6 :U / 0 fudge 3 \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(6)));   
            }

            [Test]
            public void TestInnerVar2()
            {
                Scanner scan = new Scanner(@"/ :o lol 6 :U lol 3 9 \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestInnerVar3()
            {
                Scanner scan = new Scanner(@":o fudge 9 :U / fudge fudge / fudge fudge fudge \ \");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(9)));
            }
        }

        [TestFixture]
        class BinOpTest
        {
            [Test]
            public void TestEquals1()
            {
                Scanner scan = new Scanner("| 1 2 ;) |");
                Assert.True(Expr.Parse(scan).Equals(new BinOp(new Num(1), new Num(2), ";)")));
            }

            [Test]
            public void TestEvalAddition()
            {
                Scanner scan = new Scanner("| 1 2 ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestEvalSubtraction()
            {
                Scanner scan = new Scanner("| 1 2 ;( |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(-1)));
            }

            [Test]
            public void TestInnerSubstraction()
            {
                Scanner scan = new Scanner("| 1 | 1 2 ;( | ;( |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(2)));
            }

            [Test]
            public void TestParenthesisIntegration()
            {
                Scanner scan = new Scanner("| <1> 2 ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void TestParenthesisNestedIntegration()
            {
                Scanner scan = new Scanner("| <1> | <2> <3> ;) | ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(6)));
            }

            [Test]
            public void TestInnerAddition()
            {
                Scanner scan = new Scanner("| 1 | 1 2 ;) | ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(4)));
            }

            [Test]
            public void TestInnerInnerAddition()
            {
                Scanner scan = new Scanner("| | 1 2 ;) | | 12 | 1 2 ;) | ;) | ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(18)));
            }

            [Test]
            public void TestModuloOp()
            {
                Scanner scan = new Scanner("| 2 4 % |");
            }
        }

        [TestFixture]
        class NumTest
        {
            private string TestString;

            [Test]
            public void NumberTestString() 
            {
                TestString = "  1  2 3 3  4 4 5  df  23  ";
                Scanner numberTest = new Scanner(TestString);
                Assert.True(Expr.Parse(numberTest).ToString() == "1");
                Assert.True(Expr.Parse(numberTest).ToString() == "2");
            }

            [Test]
            public void NumberTestEquals()
            {
                TestString = "  1  2 3 3  4 4 5  df  23  ";
                Scanner numberTest = new Scanner(TestString);
                Assert.True(Expr.Parse(numberTest).Equals(new Num(1)));
                Assert.False(Expr.Parse(numberTest).Equals(new Num(1)));
                
            }

            [Test]
            public void NumberTestEqualsObj()
            {
                Num num1 = new Num(1);
                Num num2 = new Num(1);
                Assert.True(num1.Equals(num2));
            }

            [Test]
            public void NumberTestNotEqualsObj()
            {
                Num num1 = new Num(2);
                Num num2 = new Num(1);
                Assert.False(num1.Equals(num2));
            }

            [Test]
            public void NumberTestNotEqualsObjFRINGE()
            {
                Num num1 = new Num(2);
                Num num2 = new Num(2.1);
                Assert.False(num1.Equals(num2));
            }

            [Test]
            public void ParseTest1()
            {
                Scanner scan = new Scanner("1");
                Assert.True(Expr.Parse(scan).Equals(new Num(1)));
            }

            [Test]
            public void ParseTest2()
            {
                Scanner scan = new Scanner("<1>");
                Assert.False(Expr.Parse(scan).Equals(new Num(1)));
            }

            [Test]
            public void ParseTest3()
            {
                Scanner scan = new Scanner("12");
                Assert.True(Expr.Parse(scan).Equals(new Paren(new Num(12)).Eval()));
            }

            

        }

        [TestFixture]
        class ParenTest
        {
            [Test]
            public void SubstituteParentTest1()
            {
                Scanner scan = new Scanner(":o d 3 :U <d>");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void SubstituteParentTest2()
            {
                Scanner scan = new Scanner("<:o d 3 :U d>");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

            [Test]
            public void ParentTest1()
            {
                Scanner scan = new Scanner("<3>");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(3)));
            }

        }

        [TestFixture]
        class R3LetTest
        {
            [Test]
            public void ShadowTest1()
            {
                Scanner scan = new Scanner(":o x 3 :U :o x 5 :U |x 3 ;)|");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(8)));
            }

            [Test]
            public void ShadowTest2()
            {
                Scanner scan = new Scanner(":o y 3 :U :o x y :U | x y ;) |");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(6)));
            }

            [Test]
            public void ShadowTest3()
            {
                Scanner scan = new Scanner(":o x 5 :U :o y 3 :U | :o x y :U |x y ;)| x ;)|");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(11)));
            }

            [Test]
            public void ShadowTest4()
            {
                Scanner scan = new Scanner(":o x 5 :U <:o x |x 1 ;)| :U | x 2 ;)|>");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(8)));
            }

            [Test]
            public void ShadowTest5()
            {
                Scanner scan = new Scanner(":o y :o z 4 :U <:o y 99 :U z> :U <:o z 5 :U |<:o z 10 :U y> |y z ;)| ;)|>");
                Assert.True(Expr.Parse(scan).Eval().Equals(new Num(13)));
            }

        }

    }
}
