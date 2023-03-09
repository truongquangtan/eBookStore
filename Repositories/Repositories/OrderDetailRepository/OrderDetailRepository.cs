using BookStoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderDetailRepository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly eBookStore5Context context;

        public OrderDetailRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public OrderDetail Add(OrderDetail orderDetail)
        {
            var entityEntry = context.OrderDetails.Add(orderDetail);
            context.SaveChanges();
            return entityEntry.Entity;
        }

        public void AddRange(IEnumerable<OrderDetail> orders)
        {
            context.OrderDetails.AddRange(orders);
            context.SaveChanges();
        }

        public int CountProductQuantity(int productId)
        {
            var orderDetails = context.OrderDetails.Where(o => o.ProductId == productId).ToList();
            int count = 0;
            foreach (var orderDetail in orderDetails)
            {
                count += orderDetail.Quantity.Value;
            }
            return count;
        }

        public void Delete(OrderDetail orderDetail)
        {
            context.OrderDetails.Remove(orderDetail);
            context.SaveChanges();
        }

        public IEnumerable<OrderDetail> GetByOrderIdIncludeProduct(int id)
        {
            return context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).Include(o => o.Product.ProductImages).ToList();
        }

        public void Update(OrderDetail updateOrderDetailInfo)
        {
            context.OrderDetails.Update(updateOrderDetailInfo);
            context.SaveChanges();
        }
    }
}
