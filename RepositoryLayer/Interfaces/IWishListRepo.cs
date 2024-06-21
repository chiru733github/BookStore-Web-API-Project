using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRepo
    {
        WishListEntity AddToWishList(int UserId, int bookId);
        List<WishListEntity> GetAllWishListByUserId(int UserId);
        bool RemoveWishList(int WishListId);
        List<WishListEntity> ViewAllWishLists();
    }
}
