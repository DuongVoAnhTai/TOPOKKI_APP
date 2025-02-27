﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOKKI_APP.Models;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class OrderController
    {
        private static OrderController instance;

        public static OrderController Instance
        {
            get { if (instance == null) instance = new OrderController(); return instance; }
            set { instance = value; }
        }

        private OrderController() { }

        public List<Menu> GetMenuItem(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.TableID == id && o.Status == 0);
                if (order == null)
                {
                    return new List<Menu>();
                }
                var orderDetails = context.OrderDetails.Where(od => od.OrderID == order.ID).ToList();
                var menuItems = orderDetails.Select(od => new Menu
                {
                    Name = context.Products.FirstOrDefault(p => p.ID == od.ProductID).Name,
                    Quantity = od.Quantity,
                    Price = context.Products.FirstOrDefault(p => p.ID == od.ProductID).Price
                }).ToList();

                return menuItems;
            }
        }

        public int GetUncheckOrderByTableID(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var uncheckOrder = context.Orders.FirstOrDefault(o => o.TableID == id && o.Status == 0);
                if (uncheckOrder != null)
                {
                    return uncheckOrder.ID;
                }
                return -1;
            }
        }

        public Order GetUncheckOrderByTable(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var uncheckOrder = context.Orders.FirstOrDefault(o => o.TableID == id && o.Status == 0);
                if (uncheckOrder != null)
                {
                    return uncheckOrder;
                }
                return null;
            }
        }

        public void InsertOrder(int tableID)
        {
            using (var context = new TopokkiEntities())
            {
                var order = new Order
                {
                    TableID = tableID,
                    DateCheckIn = DateTime.Now,
                    Status = 0
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public void InsertOrderDetail(int orderID, int productID, int quantity)
        {
            using (var context = new TopokkiEntities())
            {
                var existOrderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderID == orderID && od.ProductID == productID);

                //Neu order da ton tai
                if (existOrderDetail != null)
                {
                    int newQuantity = existOrderDetail.Quantity + quantity;
                    if (newQuantity > 0)
                    {
                        existOrderDetail.Quantity = newQuantity;
                    }
                    else 
                    {
                        context.OrderDetails.Remove(existOrderDetail);
                    }
                    context.SaveChanges();
                }
                // Neu order chua ton tai
                else
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = orderID,
                        ProductID = productID,
                        Quantity = quantity
                    };
                    context.OrderDetails.Add(orderDetail);
                    context.SaveChanges();
                }


            }
        }

        public int GetMaxOrderID()
        {
            try
            {
                using (var context = new TopokkiEntities())
                {
                    return context.Orders.Max(o => o.ID);
                }
            }
            catch
            {
                return 1;
            }
        }

        public void CheckOut(int id, decimal totalPrice)
        {
            using (var context = new TopokkiEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.ID == id);
                if(order != null)
                {
                    order.Status = 1;
                    order.DateCheckOut = DateTime.Now;
                    order.TotalPrice = totalPrice;
                    context.SaveChanges();
                }
            }
        }

        public DataTable GetOrderListByDate(DateTime checkIn, DateTime checkOut, int currentPage, int pageSize)
        {
            using ( var context = new TopokkiEntities())
            {
                var orderList = context.Orders
                    .Where(o => o.DateCheckIn >= checkIn && o.DateCheckOut <= checkOut && o.Status == 1)
                    .Select(o => new
                    {
                        Name = o.TableFood.Name,
                        TotalPrice = o.TotalPrice,
                        DateCheckIn = o.DateCheckIn,
                        DateCheckOut = o.DateCheckOut
                    }).ToList();

                int totalRecords = orderList.Count();

                var pageData = orderList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Tạo DataTable và định nghĩa cột
                DataTable dt = new DataTable();
                dt.Columns.Add("Tên bàn", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(decimal));
                dt.Columns.Add("Ngày vào", typeof(DateTime));
                dt.Columns.Add("Ngày ra", typeof(DateTime));

                // Thêm dữ liệu vào DataTable
                foreach (var order in pageData)
                {
                    dt.Rows.Add(order.Name, order.TotalPrice, order.DateCheckIn, order.DateCheckOut);
                }

                return dt;
            }
        }

        public DataTable GetOrderByDate(DateTime checkIn, DateTime checkOut)
        {
            using (var context = new TopokkiEntities())
            {
                var orderList = context.Orders
                    .Where(o => o.DateCheckIn >= checkIn && o.DateCheckOut <= checkOut && o.Status == 1)
                    .Select(o => new
                    {
                        Name = o.TableFood.Name,
                        TotalPrice = o.TotalPrice,
                        DateCheckIn = o.DateCheckIn,
                        DateCheckOut = o.DateCheckOut
                    }).ToList();

                // Tạo DataTable và định nghĩa cột
                DataTable dt = new DataTable();
                dt.Columns.Add("Tên bàn", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(decimal));
                dt.Columns.Add("Ngày vào", typeof(DateTime));
                dt.Columns.Add("Ngày ra", typeof(DateTime));

                // Thêm dữ liệu vào DataTable
                foreach (var order in orderList)
                {
                    dt.Rows.Add(order.Name, order.TotalPrice, order.DateCheckIn, order.DateCheckOut);
                }

                return dt;
            }
        }

        public int CaculateTotalPages (DateTime checkIn, DateTime checkOut, int pageSize)
        {
            using (var context = new TopokkiEntities())
            {
                int totalRecords = context.Orders.Count(o => o.DateCheckIn >= checkIn && o.DateCheckOut <= checkOut && o.Status == 1);
                return (int)Math.Ceiling((double)totalRecords / pageSize);
            }
        }

        public void DeleteOrderDetailByProductID(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var orderDetail = context.OrderDetails.FirstOrDefault(od => od.ProductID == id);
                if(orderDetail != null)
                {
                    context.OrderDetails.Remove(orderDetail);
                    context.SaveChanges();
                }
            }
        }

        public DataTable GetOrdersByMonth(int month, int year)
        {
            using (var context = new TopokkiEntities())
            {
                var orders = context.Orders
                    .Where(o => o.DateCheckIn.Month == month && o.DateCheckIn.Year == year && o.Status == 1)
                    .Select(o => new
                    {
                        TableName = o.TableFood.Name,
                        TotalPrice = o.TotalPrice,
                        DateCheckIn = o.DateCheckIn,
                        DateCheckOut = o.DateCheckOut
                    }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Tên bàn", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(decimal));
                dt.Columns.Add("Ngày vào", typeof(DateTime));
                dt.Columns.Add("Ngày ra", typeof(DateTime));

                foreach (var order in orders)
                {
                    dt.Rows.Add(order.TableName, order.TotalPrice, order.DateCheckIn, order.DateCheckOut);
                }

                return dt;
            }
        }

        public decimal CalculateTotalRevenue(DateTime checkIn, DateTime checkOut)
        {
            decimal totalRevenue = 0;

            DataTable orders = GetOrderByDate(checkIn, checkOut);

            foreach (DataRow row in orders.Rows)
            {
                totalRevenue += Convert.ToDecimal(row["Tổng tiền"]);
            }

            return totalRevenue;
        }

        public int GetCurrentOrderId(int tableID)
        {
            using (var context = new TopokkiEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.TableID == tableID && o.Status == 0);
                return order?.ID ?? 0;
            }
        }

        public void DeleteOrderDetail(int orderID, int ticketID)
        {
            using (var context = new TopokkiEntities())
            {
                var orderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderID == orderID && od.ProductID == ticketID);
                if (orderDetail != null)
                {
                    context.OrderDetails.Remove(orderDetail);
                    //orderDetail.IsActive = false;
                    context.SaveChanges();
                }
            }
        }
    }
}
