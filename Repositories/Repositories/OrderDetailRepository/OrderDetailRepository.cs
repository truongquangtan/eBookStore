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

        public void Delete(OrderDetail orderDetail)
        {
            context.OrderDetails.Remove(orderDetail);
            context.SaveChanges();
        }

        public void Update(OrderDetail updateOrderDetailInfo)
        {
            context.OrderDetails.Update(updateOrderDetailInfo);
            context.SaveChanges();
        }
    }
}
