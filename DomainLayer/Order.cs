using DomainLayer.ProductModel;
using DomainLayer.Users;

namespace DomainLayer
{
    public class Order
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public ProductEntity product { get; set; }
        public UserRegistration users { get; set; }
        public int status { get; set; }
        public int paymentId { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
