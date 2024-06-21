using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepo
    {
        CartEntity AddToCart(int UserId, int bookId);
        List<CartEntity> GetAllCarts(int UserId);
        CartEntity EditCart(UpdateCartQuantity quantity);
        bool RemoveCart(int cartId);
        List<CartEntity> ViewAllCarts();
        int CartCount(int UserId);
    }
}
