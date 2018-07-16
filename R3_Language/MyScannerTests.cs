using System;
using NUnit.Framework;

namespace R3_Language
{
    [TestFixture]
    class MyScannerTests
    {
        /**
         * MISSION : Ensure that the hasNext can consume char up to false; 
         * 
         * */
        private Scanner _hasTests;
        private Scanner _isNumberTests;
        private Scanner _hasEndTests;
        
        [Test]
        public void HasNext1()
        {
            _hasTests = new Scanner("this is a test for the hasNext function");
            Assert.True(_hasTests.HasNext("this"));
        }

        [Test]
        public void HasNext2()
        {
            _hasTests = new Scanner("this is a test for the hasNext function");
            Assert.False(_hasTests.HasNext("thiss"));
            Assert.False(_hasTests.HasNext("is"));
        }

        /**
         * Ultimate goal is for this to pass, meaning no consumption of chars unless success
         * */
        [Test]
        public void HasNext3()
        {
            _hasTests = new Scanner("this is a test for the hasNext function");
            Assert.False(_hasTests.HasNext("thi"));
            Assert.True(_hasTests.HasNext("this"));
        }

        [Test]
        public void HasNext4()
        {
            _hasTests = new Scanner("this is a test for the hasNext function");
            Assert.True(_hasTests.HasNext("this"));
            Assert.True(_hasTests.HasNext("is"));
        }

        [Test]
        public void HasNext5()
        {
            _hasTests = new Scanner("this is a test for the hasNext function");
            Assert.False(_hasTests.HasNext("this is"));
            Assert.True(_hasTests.HasNext("this"));
        }

        [Test]
        public void HasNext6()
        {
            _hasTests = new Scanner("      this is a");
            Assert.True(_hasTests.HasNext("this"));
        }

        [Test]
        public void HasNextFRINGE1()
        {
            _hasTests = new Scanner("           this is a");
            Assert.False(_hasTests.HasNext("is"));
        }

        [Test]
        public void HasNextFRINGE2()
        {
            _hasTests = new Scanner("           this is a");
            Assert.False(_hasTests.HasNext("is"));
        }

        

        [Test]
        public void HasNextFRINGE4()
        {
            _hasTests = new Scanner("     this is a");
            Assert.False(_hasTests.HasNext("is"));
        }

        [Test]
        public void HasNextFRINGE5()
        {
            _hasTests = new Scanner("           this is a");
            Assert.False(_hasTests.HasNext("a"));
        }
        [Test]
        public void IsNumber1()
        {
            _isNumberTests = new Scanner("1 2 h 1 4 dj 6");
            Assert.True(_isNumberTests.IsNumber() == true);
        }

        

        [Test]
        public void IsNumber3()
        {
            _isNumberTests = new Scanner("1.232323 laskjdhajkdw");
            Assert.True(_isNumberTests.IsNumber());
               
        }

        [Test]
        public void IsNumber4()
        {
            _isNumberTests = new Scanner(" 1.232323 laskjdhajkdw");
            Assert.True(_isNumberTests.IsNumber());

        }

        [Test]
        public void IsNumber5()
        {
            _isNumberTests = new Scanner("    1.232323 laskjdhajkdw");
            Assert.True(_isNumberTests.IsNumber());

        }

        [Test]
        public void IsNumber6()
        {
            _isNumberTests = new Scanner("   d    1.232323 laskjdhajkdw");
            Assert.False(_isNumberTests.IsNumber());

        }


    }
}
