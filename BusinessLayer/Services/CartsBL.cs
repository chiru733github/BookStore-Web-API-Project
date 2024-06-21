using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CartsBL : ICartBL
    {
        private readonly ICartRepo _cartrepo;
        public CartsBL(ICartRepo cart)
        { 
            this._cartrepo = cart;
        }
        public CartEntity AddToCart(int UserId, int bookId)
        {
            return _cartrepo.AddToCart(UserId, bookId);
        }

        public List<CartEntity> GetAllCarts(int UserId)
        {
            return _cartrepo.GetAllCarts(UserId);
        }

        public CartEntity EditCart(UpdateCartQuantity quantity)
        {
            return _cartrepo.EditCart(quantity);
        }

        public bool RemoveCart(int cartId)
        {
            return _cartrepo.RemoveCart(cartId);
        }

        public List<CartEntity> ViewAllCarts()
        {
            return _cartrepo.ViewAllCarts();
        }

        public int CartCount(int UserId)
        {
            return _cartrepo.CartCount(UserId);
        }
    }
}
