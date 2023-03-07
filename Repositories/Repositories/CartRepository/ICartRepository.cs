using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.CartRepository
{
    public interface ICartRepository
    {
        public IEnumerable<Cart> GetCartByUserId(string userId);
        public Cart GetCartByProductIdAndUserId(int productId,string userId);
        public int GetCartCountByUserId(string userId);
        public Cart? GetById(string id);
        public IEnumerable<Cart> GetCartProductByUserId(string userId);
        public Cart Add(Cart cart);
        public void Upadte(Cart updatedCartInfo);
        public void Remove(Cart cart);
        public void RemoveRangeCart(Cart cart);
    }
}
