using OrderAPIV1.Models;

namespace OrderAPIV1.Services
{
    public interface IOrderService
    {
        Task<string> PublishOrder(Order order, IConfiguration configuration);
    }
}
