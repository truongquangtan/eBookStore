using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        public OrderSum? GetById(int id);
        public OrderSum Add(OrderSum order);
        public void Delete(OrderSum order);
        public void Update(OrderSum updateOrderInfo);
    }
}
