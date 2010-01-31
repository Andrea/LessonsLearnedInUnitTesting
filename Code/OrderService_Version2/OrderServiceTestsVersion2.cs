using Moq;
using Xunit;

// README
// This code sample is part of the Lessons Learned in Unit Testing talk
// In this version:
//  - We refactor the IRepository<Order> into a creation method,
//  - We are also showing Setup ( with xUnit you do setup in the constructor)
//  - simple Mock usage
//  - State vs Behaviour tests 
//  - AAA : Arrange Action Assert 
//
// I have kept classes in the same file for the purposes of the talk
// however this is not a recomended practise.
// Please have a look at OrderService_version2 and OrderService_version3 projects
// 

namespace OrderService_Version2
{
    public class OrderServiceTestsVersion2
    {

        public class Given_an_OrderService_version2
        {

            private Mock<IRepository<Order>> _orderRepositoryMock;
            private OrderService _orderService;


            public Given_an_OrderService_version2()
            {
                _orderRepositoryMock = new Mock<IRepository<Order>>();
            }

            // Save with creation - >uttthrowRefactor_v2
            [Fact]
            public void When_saving_Then_it_doesnt_throw()
            {
                var orderService = GetOrderService();
                Assert.DoesNotThrow(() => orderService.Save(null));
            }

            // Testing Behaviour 
            // AAA 
            // Then resfactor to setup and creation method  
            [Fact]
            public void When_saving_order_Then_calls_repo_to_persist_the_order()
            {
                _orderRepositoryMock.Setup(x => x.Save(It.IsAny<Order>()));
                _orderService = GetOrderService();

                _orderService.Save(new Order());

                _orderRepositoryMock.VerifyAll();
            }

            private OrderService GetOrderService()
            {
                return new OrderService(_orderRepositoryMock.Object);
            }

            public interface IRepository<T>
            {
                void Save(T entity);
            }

            public class OrderService
            {
                // Ctor with Repository
                private IRepository<Order> _orderRepository;

                public OrderService(IRepository<Order> orderRepository)
                {
                    _orderRepository = orderRepository;
                }

                public void Save(Order order)
                {
                    _orderRepository.Save(order);
                    return;
                }
            }

            public class Order
            {

            }
        }
    }
}