using System;
using Moq;
using Xunit;


// README
// This code sample is part of the Lessons Learned in Unit Testing talk
// In this version:
//  - Difference between Stub and Mocks, example of a Hand rolled and auto-stub
//  - More advanced usage of the Mocking Framework
//  - Discussed Test duplication 
//
// I have kept classes in the same project for the purposes of the talk
// however this is not a recomended practise.
// 
// these tests can still be improved
namespace CodeSamplesUnitTestingTalk.OrderService.Version3
{
    public class OrderServiceTests
    {
        private readonly OrderStub _orderStubValid;

        public OrderServiceTests()
        {
            _orderStubValid = new OrderStub(true);
        }

        [Fact]
        public void When_saving_Then_it_doesnt_throw()
        {
            OrderService orderService = GetOrderService();
            Assert.DoesNotThrow(() => orderService.Save(_orderStubValid));
        }

        [Fact]
        public void When_saving_order_Then_calls_repo_to_persist_the_order()
        {
            var orderRepositoryMock = new Mock<IRepository<Order>>();
            orderRepositoryMock.Setup(x => x.Save(It.IsAny<Order>()));
            var orderService = GetOrderService(orderRepositoryMock.Object);

            orderService.Save(_orderStubValid);

            orderRepositoryMock.VerifyAll();
        }


        #region Test_Dupplication ?
        //However it did help me do write the validation section
        // refactor to a data | leave as is

        [Fact]
        public void Given_valid_order_When_saving_Then_return_true()
        {
            var orderService = GetOrderService();

            var result = orderService.Save(_orderStubValid);

            Assert.Equal(true, result);
        }

        [Fact]
        public void Given_invalid_order_When_saving_Then_return_false()
        {
            var invalidOrder = new OrderStub(false);
            var orderService = GetOrderService();

            var result = orderService.Save(invalidOrder);

            Assert.False(result);
        }
        #endregion

        //test that if the repo throws it doesnt send the email
        [Fact]
        public void When_repository_throws_Then_doesnt_send_email()
        {
            var orderRepositoryStub = new Mock<IRepository<Order>>();
            var order = new OrderStub(true);
            orderRepositoryStub.Setup(x => x.Save(It.IsAny<Order>())).
                Throws(new Exception("Oh no! the repository is falling appart "));

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(x => x.SendConfirmationEmail(It.IsAny<Order>()));
            var orderService = GetOrderService(orderRepositoryStub.Object, emailServiceMock.Object);
            orderService.Logger = new Mock<ILogger>().Object;

            orderService.Save(order);

            emailServiceMock.Verify(x=>x.SendConfirmationEmail(order), Times.Never());

        }


        #region Helpers
        private OrderService GetOrderService(IRepository<Order> orderRepository, IEmailService emailService)
        {
            return new OrderService(orderRepository, emailService);
        }

        private OrderService GetOrderService()
        {
            var orderRepositoryStub = new Mock<IRepository<Order>>();

            return GetOrderService(orderRepositoryStub.Object);
        }

        private OrderService GetOrderService(IRepository<Order> repository)
        {
            return GetOrderService(repository, new Mock<IEmailService>().Object);
        }
        #endregion
    }

    public class OrderStub : Order
    {
        private bool _isValid;

        public OrderStub(bool isValid)
        {
            _isValid = isValid;
        }
        public override bool IsValid()
        {
            return _isValid;
        }
    }
}