using BookHub.Models;

namespace BusinessLayer.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDetail>> GetOrdersAsync(int? userId, string? username,
        DateTime? startDate, DateTime? endDate, double? totalPrice, int? bookId, string? bookName);
    Task<OrderDetail> GetOrderByIdAsync(int id);
    Task<OrderDetail> PostOrderAsync(OrderCreate orderCreate);
    Task<OrderUpdate> UpdateOrderAsync(int id, OrderUpdate orderUpdate);
    Task DeleteOrderAsync(int id);
}