using BookHub.Models;
using DataAccessLayer;
using BusinessLayer.Mapper;
using BusinessLayer.Exceptions;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookHubDbContext _context;

        public OrderService(BookHubDbContext context)
        {
            _context = context;
        }
        
        //TODO pridat kontroly

        public async Task<IEnumerable<OrderDetail>> GetOrdersAsync(int? userId, string? username,
            DateTime? startDate, DateTime? endDate, double? totalPrice, int? bookId, string? bookName)
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Books)
                .ThenInclude(b => b.Authors)
                .AsQueryable();
            
            if (userId.HasValue)
            {
                orders = orders.Where(o => o.User.Id == userId.Value);
            }
            
            if (!string.IsNullOrEmpty(username))
            {
                orders = orders.Where(o => o.User.Name == username);
            }

            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.Date <= endDate.Value);
            }

            if (totalPrice.HasValue)
            {
                orders = orders.Where(o => Math.Abs(o.TotalPrice - totalPrice.Value) < 0.0001);
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Name == bookName || b.Id == bookId);
            if (book != null)
            {
                orders = orders.Where(o => o.Books.Contains(book));
            }

            var orderList = await orders.Select(o => ControllerHelpers.MapOrderToOrderDetail(o)).ToListAsync();
            return orderList;
        }
        
        public async Task<OrderDetail> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Books)
                .ThenInclude(b => b.Authors)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                throw new OrderNotFoundException($"Order 'ID={id}' could not be found");
            }
            return ControllerHelpers.MapOrderToOrderDetail(order);
        }

        public async Task<OrderDetail> PostOrderAsync(OrderCreate orderCreate)
        {
            if (orderCreate.Books.IsNullOrEmpty())
            {
                throw new BooksEmptyException("Collection Books is empty");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Name == orderCreate.User.Name || u.Id == orderCreate.User.Id);
            if (user == null)
            {
                throw new UserNotFoundException($"User 'Name={orderCreate.User.Name}' <OR> 'ID={orderCreate.User.Id}' could not be found");
            }
            var order = new Order
            {
                User = user,
                TotalPrice = orderCreate.TotalPrice
            };
            
            foreach (var bookRelatedModel in orderCreate.Books)
            {
                var book = await _context.Books.FirstOrDefaultAsync(b =>
                    b.Name == bookRelatedModel.Name || b.Id == bookRelatedModel.Id);
                if (book == null)
                {
                    throw new BookNotFoundException($"Book 'Name={bookRelatedModel.Name}' <OR> 'ID={bookRelatedModel.Id}' could not be found");
                }
                order.Books.Add(book);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return ControllerHelpers.MapOrderToOrderDetail(order);
        }

        public async Task<OrderDetail> UpdateOrderAsync(int id, OrderUpdate orderUpdate)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new OrderNotFoundException($"Order 'ID={id}' could not be found");
            }
            
            order.TotalPrice = orderUpdate.TotalPrice;

            if (orderUpdate.Books.Count != 0)
            {
                order.Books.Clear();
                foreach (var bookRelatedModel in orderUpdate.Books)
                {
                    var book = await _context.Books.FirstOrDefaultAsync(b =>
                        b.Name == bookRelatedModel.Name || b.Id == bookRelatedModel.Id);
                    if (book == null)
                    {
                        throw new BookNotFoundException($"Book 'ID={id}' could not be found");
                    }
                    order.Books.Add(book);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return ControllerHelpers.MapOrderToOrderDetail(order);
            }
            catch (Exception ex)
            {
                throw new EntityUpdateException($"Error updating order: {ex.Message}");
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new BookNotFoundException($"Order 'ID={id}' could not be found");
            }
            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new EntityDeleteException($"Error deleting order: {ex.Message}");
            }
        }
    } 
}
