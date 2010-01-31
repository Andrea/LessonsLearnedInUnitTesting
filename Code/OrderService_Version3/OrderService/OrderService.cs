using System;

namespace CodeSamplesUnitTestingTalk.OrderService.Version3
{
    public class OrderService
    {
        private IRepository<Order> _orderRepository;
        private IEmailService _emailService;
        public ILogger Logger;
        public OrderService(IRepository<Order> orderRepository, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public bool Save(Order order)
        {
            if (!order.IsValid()) return false;

            try
            {
                _orderRepository.Save(order);
                _emailService.SendConfirmationEmail(order);
            }
            catch (Exception exception)
            {
                Logger.Log(exception.Message, exception);
            }
            return true;
        }
    }

    public interface ILogger
    {
        void Log(string error, Exception ex);
    }

    public interface IEmailService
    {
        void SendConfirmationEmail(Order order);
    }
}