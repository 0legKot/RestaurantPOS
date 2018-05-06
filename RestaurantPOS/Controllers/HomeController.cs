using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantPOS.Models;

namespace RestaurantPOS.Controllers
{

    public class HomeController : Controller
    {
        static Restaurant restaurant = new Restaurant();
        static OrderHistory orderHistory = new OrderHistory();
        public IActionResult Index()
        {
            return View("Restaurant",restaurant);
        }

        public IActionResult OrderHistory()
        {
            
            return View(orderHistory);
        }
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(int row, int column, int seat)
        {
            Order order = new Order();
            Table table = restaurant.TablesGrid[row, column];
            table.TableSeats[seat].Order = order;
            orderHistory.AddOrder(order, table.Id, new List<int>() { seat });
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Pay")]
        public IActionResult Pay(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Pay();
            restaurant.TablesGrid[row, column].TableSeats[seat].Order=null;
            return  Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Void")]
        public IActionResult Void(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Void();
            restaurant.TablesGrid[row, column].TableSeats[seat].Order = null;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Ready")]
        public IActionResult Ready(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Ready();
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("CreateItem")]
        public IActionResult TableDetail(int row, int column, int seat, string name,int price)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.OrderItems.Add(new OrderItem {Name=name,Price=price,CustomerMedia=new byte[1] });
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.State = Order.OrderState.Active;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncDiscount")]
        public IActionResult IncDiscount(int row, int column, int seat, string name, decimal discount)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Discount += discount;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncTips")]
        public IActionResult IncTips(int row, int column, int seat, string name, decimal tips)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Tips += tips;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpGet]
        public IActionResult TableDetail(int row, int column,int seat)
        {
            ViewBag.row = row;
            ViewBag.column = column;
            ViewBag.curSeat = seat;
            return View(restaurant.TablesGrid[row,column]);
        }

        [Authorize]
        public IActionResult View(Guid OrderInfoId)
        {
            OrderInfo orderInfo = orderHistory.GetOrderInfo(OrderInfoId);
            Table curTable = null;
            foreach (Table t in restaurant.TablesGrid)
                if (t!=null&&t.Id == orderInfo.TableId) curTable = t.Clone() as Table;

            if (curTable == null) return BadRequest();

            ViewBag.row = curTable.Row;
            ViewBag.column = curTable.Column;
            ViewBag.curSeat = orderInfo.TableSeatsNumbers.FirstOrDefault();
            
            foreach (int seat in orderInfo.TableSeatsNumbers)
            curTable.TableSeats[seat].Order = orderInfo.Order;
            return View("TableDetail", curTable);
        }


    }
}
