using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IWishListBL
    {
        WishListEntity AddToWishList(int UserId, int bookId);
        List<WishListEntity> GetAllWishListByUserId(int UserId);
        bool RemoveWishList(int WishListId);
        List<WishListEntity> ViewAllWishLists();
    }
}
