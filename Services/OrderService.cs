using CommerceSystemAPI.Models;
using CommerceSystemAPI.Repositories;

namespace CommerceSystemAPI.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(
            OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order? GetOrderById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public List<Order> ViewMyOrders(int userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public object GetOrderDetails(int orderId, int userId)
    
        {
            var order = _orderRepository.GetById(orderId);

            if (order == null)
            {
                return "Order Not Found";
            }

            if (order.UserId != userId)
            {
                return "Forbidden";
            }

            var details = _orderRepository.GetOrderDetails(orderId);

            if (!details.Any())
            {
                return "No Order Details Found";
            }

            return details;
        }
    }
}

