using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class WishListBL : IWishListBL
    {
        private readonly IWishListRepo _wishListrepo;
        public WishListBL(IWishListRepo wishList)
        {
            this._wishListrepo = wishList;
        }
        public WishListEntity AddToWishList(int UserId, int bookId)
        {
            return _wishListrepo.AddToWishList(UserId, bookId);
        }

        public List<WishListEntity> GetAllWishListByUserId(int UserId)
        {
            return _wishListrepo.GetAllWishListByUserId(UserId);
        }

        public bool RemoveWishList(int WishListId)
        {
            return _wishListrepo.RemoveWishList(WishListId);
        }

        public List<WishListEntity> ViewAllWishLists()
        {
            return _wishListrepo.ViewAllWishLists();
        }
    }
}
