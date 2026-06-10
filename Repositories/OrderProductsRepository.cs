using CommerceSystemAPI.Models;
namespace CommerceSystemAPI.Repositories
{
    public class OrderProductsRepository
    {
        private readonly AppDbContext _context;

        public OrderProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(OrderProducts orderProduct)
        {
            _context.OrderProductss.Add(orderProduct);
            _context.SaveChanges();
        }

        public List<OrderProducts> GetAll()
        {
            return _context.OrderProductss.ToList();
        }

        public OrderProducts? GetByIds(
            int orderId,
            int productId)
        {
            return _context.OrderProductss
                .FirstOrDefault(op =>
                    op.OrderId == orderId &&
                    op.ProductId == productId);
        }

        public void Update(OrderProducts orderProduct)
        {
            _context.OrderProductss.Update(orderProduct);
            _context.SaveChanges();
        }
    }
}
    

