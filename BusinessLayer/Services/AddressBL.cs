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
    public class AddressBL : IAddressBL
    {
        private readonly IAddressRepo _addressRepo;
        public AddressBL(IAddressRepo address)
        {
            this._addressRepo = address;
        }
        public AddressEntity AddToAddress(int UserId, AddressModel address)
        {
            return _addressRepo.AddToAddress(UserId, address);
        }

        public AddressEntity EditAddress(int AddressId, int UserId, AddressModel address)
        {
            return _addressRepo.EditAddress(AddressId, UserId, address);
        }

        public AddressEntity GetAddressById(int UserId, int AddressId)
        {
            return _addressRepo.GetAddressById(UserId, AddressId);
        }

        public List<AddressEntity> GetAllAddressByUserId(int UserId)
        {
            return _addressRepo.GetAllAddressByUserId(UserId);
        }

        public bool RemoveAddress(int userId,int AddressId)
        {
            return _addressRepo.RemoveAddress(userId,AddressId);
        }
    }
}
