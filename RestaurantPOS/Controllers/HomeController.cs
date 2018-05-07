using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPOS.Models;



namespace RestaurantPOS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static readonly Restaurant restaurant = new Restaurant();
        public static readonly OrderHistory orderHistory = new OrderHistory();
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Redirect("~/Home/Restaurant");
        }
        [AllowAnonymous]
        public IActionResult Restaurant()
        {
            return View(restaurant);
        }
        public IActionResult ManageTables()
        {
            return View(restaurant);
        }
        [AllowAnonymous]
        public IActionResult OrderHistory()
        {
            return View(orderHistory);
        }
        //[HttpPost] needed
        public IActionResult AddTable(int row, int column)
        {
            if (restaurant.TablesGrid[row, column] == null) restaurant.TablesGrid[row, column] = new Table(row, column);
            else restaurant.TablesGrid[row, column].IsActive = !restaurant.TablesGrid[row, column].IsActive;
            return Redirect("~/Home/ManageTables");
        }
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(int row, int column, int seat,string waiter)
        {
            Order order = new Order(waiter);
            Table table = restaurant.TablesGrid[row, column];
            table.TableSeats[seat].Order = order;
            List<int> seatNumbers = new List<int>();
            foreach (TableSeat ts in table.TableSeats)
                if (ts.Order != null && ts.Order.Id == order.Id) seatNumbers.Add(Array.IndexOf(table.TableSeats,ts));
            orderHistory.AddOrder(order, table.Id, seatNumbers);
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Pay")]
        public IActionResult Pay(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Pay();
            return CloseOrder(row, column, seat);
        }
        [HttpPost("Void")]
        public IActionResult Void(int row, int column, int seat)
        {

            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Void();
            return CloseOrder(row, column, seat);
        }

        private IActionResult CloseOrder(int row, int column, int seat)
        {
            var closingOrderId = restaurant.TablesGrid[row, column].TableSeats[seat].Order.Id;
            foreach (TableSeat ts in restaurant.TablesGrid[row, column].TableSeats)
                if (ts.Order!=null&&closingOrderId == ts.Order.Id)
                    ts.Order = null;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }

        [HttpPost("Ready")]
        public IActionResult Ready(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Ready();
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("CreateItem")]
        public IActionResult CreateItem(int row, int column, int seat, string name, int price)
        {

            restaurant.TablesGrid[row, column].TableSeats[seat].Order.OrderItems.Add(new OrderItem { Name = name, Price = price, CustomerMedia = new byte[0] });
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.State = Order.OrderState.Active;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("AddSeats")]
        public IActionResult AddSeats(int row, int column, int seat, string name, int additionalSeat)
        {
            restaurant.TablesGrid[row, column].TableSeats[additionalSeat].Order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            orderHistory.AddSeatToOrder(restaurant.TablesGrid[row, column].TableSeats[seat].Order.Id, additionalSeat);
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
        public IActionResult TableDetail(int row, int column, int seat)
        {
            if (!restaurant.TablesGrid[row, column].IsActive) return View("Restaurant",restaurant);
            ViewBag.row = row;
            ViewBag.column = column;
            ViewBag.curSeat = seat;

            return View(restaurant.TablesGrid[row, column]);
        }

        [AllowAnonymous]
        public IActionResult View(Guid OrderInfoId)
        {
            OrderInfo orderInfo = orderHistory.GetOrderInfo(OrderInfoId);
            if (orderInfo == null) return Redirect("~/Home/OrderHistory");
            Table curTable = null;
            foreach (Table t in restaurant.TablesGrid)
                if (t != null && t.Id == orderInfo.TableId) curTable = t.Clone() as Table;

            if (curTable == null) return BadRequest();

            ViewBag.row = curTable.Row;
            ViewBag.column = curTable.Column;
            ViewBag.curSeat = orderInfo.TableSeatsNumbers.FirstOrDefault();


            foreach (TableSeat seat in curTable.TableSeats)
                seat.Order = orderInfo.Order;

            //foreach (int seat in orderInfo.TableSeatsNumbers)
            //    curTable.TableSeats[seat].Order = orderInfo.Order;
            return View("TableDetail", curTable);
        }


    }
}
