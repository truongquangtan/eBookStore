using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderDetailRepository
{
    public interface IOrderDetailRepository
    {
        public OrderDetail Add(OrderDetail orderDetail);

        public void Delete(OrderDetail orderDetail);

        public void Update(OrderDetail updateOrderDetail);

        public void AddRange(IEnumerable<OrderDetail> orderDetails);
        public IEnumerable<OrderDetail> GetByOrderIdIncludeProduct(int id);
        public int CountProductQuantity(int productId);
    }
}
