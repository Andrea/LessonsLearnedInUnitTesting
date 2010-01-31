using System;
using Xunit;
// README
//This code sample is to demostrate how you evolve the test names
// the last one is the most correct, however a more BDD naming style follows in the following 
// examples
// I have kept classes in the same file for the purposes of the talk
// however this is not a recomended practise.
// Please have a look at OrderService_version2 and OrderService_version3 projects
// The recomended file organization in  production is either a *Test per assembly
// so for example in this case you will have your solution with a project called
// OrderService and another one called OrderServiceTests.

namespace OrderService_Version1
{
    public class OrderServiceVersion1
    {

        [Fact]
        public void Test1()
        {
            //What am I testing?
        }

        [Fact]
        public void Service_works()
        {
            //no, really?
        }

        [Fact]
        public void SendsEmailOnSave()
        {
            //ok better, butit could be more explicit
        }

        [Fact]
        //this is failing because we are throwing a NotImplementedException
        public void When_Save_Should_not_throw()
        {
            var orderService = new OrderService();
            Assert.DoesNotThrow( delegate { orderService.Save(); });
        }


        public class OrderService
        {
            public void Save()
            {
                throw new NotImplementedException();
            }
        }
    }
}