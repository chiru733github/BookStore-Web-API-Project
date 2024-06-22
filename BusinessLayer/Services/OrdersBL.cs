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
    public class OrdersBL : IOrdersBL
    {
        private readonly IOrdersRepo _ordersRepo;
        public OrdersBL(IOrdersRepo repo)
        {
            this._ordersRepo=repo;
        }
        public OrderEntity AddOrder(OrderModel orderModel)
        {
            return _ordersRepo.AddOrder(orderModel);
        }

        public bool CancellingOrder(int orderId)
        {
            return _ordersRepo.CancellingOrder(orderId);
        }

        public List<OrderEntity> GetAllOrdersByUserId(int UserId)
        {
            return _ordersRepo.GetAllOrdersByUserId(UserId);
        }

        public List<OrderEntity> ViewAllOrders()
        {
            return _ordersRepo.ViewAllOrders();
        }
    }
}
