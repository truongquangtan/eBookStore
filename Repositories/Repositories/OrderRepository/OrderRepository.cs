using BookStoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly eBookStore5Context context;

        public OrderRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public OrderSum Add(OrderSum order)
        {
            var entityEntry = context.Orders.Add(order);
            context.SaveChanges();
            return entityEntry.Entity;
        }

        public void Delete(OrderSum order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public IEnumerable<OrderSum> GetAllOrdersByCreatedTime()
        {
            return context.Orders.OrderByDescending(o => o.OrderAt).ToList();
        }

        public IEnumerable<OrderSum> GetOrdersByUserIdOrderByUpdateTimeDesc(string userId)
        {
            return context.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.UpdateAt).ToList();
        }

        public OrderSum GetOrderByOrderIdAndUserId(int orderId, string userId)
        {
            return context.Orders.Where(o => o.UserId == userId && o.Id == orderId).FirstOrDefault();
        }
        public OrderSum GetById(int id)
        {
            var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            return order;
        }

        public void Update(OrderSum updateOrderInfo)
        {
            context.Orders.Update(updateOrderInfo);
            context.SaveChanges();
        }
    }
}
