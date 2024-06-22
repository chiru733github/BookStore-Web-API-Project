using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IOrdersRepo
    {
        OrderEntity AddOrder(OrderModel orderModel);
        List<OrderEntity> GetAllOrdersByUserId(int UserId);
        bool CancellingOrder(int orderId);
        List<OrderEntity> ViewAllOrders();
    }
}
