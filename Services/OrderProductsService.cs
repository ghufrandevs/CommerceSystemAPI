using CommerceSystemAPI.Models;
using CommerceSystemAPI.Repositories;

namespace CommerceSystemAPI.Services
{
    public class OrderProductsService
    {
        private readonly OrderProductsRepository _orderProductsRepository;

        public OrderProductsService( OrderProductsRepository orderProductsRepository)
        {
            _orderProductsRepository = orderProductsRepository;
        }
        public string AddOrderProduct( OrderProducts orderProduct)
        {
            _orderProductsRepository.Add(orderProduct);

            return "Order Product Added Successfully";
        }
        public List<OrderProducts> GetAllOrderProducts()
        {
            return _orderProductsRepository.GetAll();
        }
        public OrderProducts? GetOrderProduct(  int orderId, int productId)
        {
            return _orderProductsRepository
                .GetByIds(orderId, productId);
        }

        public string UpdateOrderProduct(int orderId,int productId, OrderProducts orderProduct)
        {
            var op = _orderProductsRepository
                .GetByIds(orderId, productId);

            if (op == null)
            {
                return "Order Product Not Found";
            }

            op.Quantity = orderProduct.Quantity;

            _orderProductsRepository.Update(op);

            return "Order Product Updated Successfully";
        }

    }
}
    

