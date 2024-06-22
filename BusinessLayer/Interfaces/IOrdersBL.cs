using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IOrdersBL
    {
        OrderEntity AddOrder(OrderModel orderModel);
        List<OrderEntity> GetAllOrdersByUserId(int UserId);
        bool CancellingOrder(int orderId);
        List<OrderEntity> ViewAllOrders();
    }
}
