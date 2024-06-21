using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface ICartBL
    {
        CartEntity AddToCart(int UserId,int bookId);
        List<CartEntity> GetAllCarts(int UserId);
        CartEntity EditCart(UpdateCartQuantity quantity);
        bool RemoveCart(int cartId);
        List<CartEntity> ViewAllCarts();
        int CartCount(int UserId);
    }
}
