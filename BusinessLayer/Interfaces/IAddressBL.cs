using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IAddressBL
    {
        AddressEntity AddToAddress(int UserId, AddressModel address);
        List<AddressEntity> GetAllAddressByUserId(int UserId);
        AddressEntity EditAddress(int AddressId, int UserId, AddressModel address);
        bool RemoveAddress(int userId,int AddressId);
        AddressEntity GetAddressById(int UserId, int AddressId);
    }
}
