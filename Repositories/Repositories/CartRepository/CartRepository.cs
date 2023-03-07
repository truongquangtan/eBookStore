using BookStoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly eBookStore5Context context;

        public CartRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public Cart Add(Cart cart)
        {
            var entityEntry = context.Carts.Add(new Cart()
            {
                ProductId = cart.ProductId,
                UserId = cart.UserId,
                Quantity = 1,
            });
            context.SaveChanges();
            return entityEntry.Entity;
        }

        public Cart GetById(string id)
        {
            var cartItem = context.Carts.Where(c => c.Id == id).FirstOrDefault();
            return cartItem;
        }

        public Cart GetCartByProductIdAndUserId(int productId, string userId)
        {
            var cartItem = context.Carts.Where(c => c.ProductId == productId && c.UserId == userId).FirstOrDefault();
            return cartItem;
        }

        public IEnumerable<Cart> GetCartByUserId(string userId)
        {
            var cartItem = context.Carts.Where(c => c.UserId == userId).Include(c => c.Product).Include(c => c.Product.ProductImages).ToList();
            return cartItem;
        }

        public int GetCartCountByUserId(string userId)
        {
            var total = context.Carts.Where(c => c.UserId == userId).Count();
            return total;
        }

        public IEnumerable<Cart> GetCartProductByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Cart cart)
        {
            context.Carts.Remove(cart);
            context.SaveChanges();
        }

        public void RemoveRangeCart(Cart cart)
        {
            throw new NotImplementedException();
        }

        public void Upadte(Cart updatedCartInfo)
        {
            context.Carts.Update(updatedCartInfo);
            context.SaveChanges();
        }
    }
}
