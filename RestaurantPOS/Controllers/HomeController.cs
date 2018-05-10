using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPOS.Models;



namespace RestaurantPOS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public static readonly Restaurant restaurant = new Restaurant();
        //public static readonly OrderHistory orderHistory=new OrderHistory( );
        private AppDbContext db;
        public HomeController(AppDbContext context)
        {
            db = context;
        }

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
            foreach (Table t in restaurant.TablesGrid)
                if (t != null) ViewData[t.Id.ToString()] = $"Row:{t.Row} Column: {t.Column}";
            
            return View(db.OrderHistory.Include("Order").Include(x=>x.Order.OrderItems).Include(x=>x.TableSeatsNumbers).ToList());
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
            List<SeatNumber> seatNumbers = new List<SeatNumber>();
            foreach (TableSeat ts in table.TableSeats)
                if (ts.Order != null && ts.Order.Id == order.Id) seatNumbers.Add(new SeatNumber { Id = Guid.NewGuid(), Number = Array.IndexOf(table.TableSeats, ts) });
            db.OrderHistory.Add(new OrderInfo(order,table.Id,seatNumbers));
            db.SaveChanges();
            //orderHistory.AddOrder(order, table.Id,  seatNumbers);
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Pay")]
        public IActionResult Pay(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Pay();
            return CloseOrder(row, column, seat, Order.OrderState.Paid);
        }
        [HttpPost("Void")]
        public IActionResult Void(int row, int column, int seat)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Void();
            return CloseOrder(row, column, seat,Order.OrderState.Voided);
        }

        private IActionResult CloseOrder(int row, int column, int seat, Order.OrderState state)
        {
            var closingOrderId = restaurant.TablesGrid[row, column].TableSeats[seat].Order.Id;
            db.OrderHistory.Include("Order").FirstOrDefault(x => x.Order.Id == closingOrderId).Order.State = state;
            db.OrderHistory.Include("Order").FirstOrDefault(x => x.Order.Id == closingOrderId).Order.ClosedDate = restaurant.TablesGrid[row, column].TableSeats[seat].Order.ClosedDate;
            db.SaveChanges();
            foreach (TableSeat ts in restaurant.TablesGrid[row, column].TableSeats)
                if (ts.Order!=null&&closingOrderId == ts.Order.Id)
                    ts.Order = null;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }

        [HttpPost("Ready")]
        public IActionResult Ready(int row, int column, int seat)
        {
            var order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            order.Ready();
            db.OrderHistory.Include(x => x.Order).FirstOrDefault(x => x.Order.Id == order.Id).Order.State = Order.OrderState.Ready;
            db.SaveChanges();
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("CreateItem")]
        public IActionResult CreateItem(int row, int column, int seat, string name, string price)
        {
            var order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            var orderItem = new OrderItem { Name = name, Price = decimal.Parse(price, new CultureInfo("en-US")), CustomerMedia = new byte[0] };
            order.OrderItems.Add(orderItem);
            order.State = Order.OrderState.Active;
            db.OrderHistory.Include(x => x.Order).FirstOrDefault(x=>x.Order.Id==order.Id).Order.State=Order.OrderState.Active;
            db.OrderHistory.Include(x => x.Order).FirstOrDefault(x => x.Order.Id == order.Id).Order.OrderItems.Add(orderItem);
            db.SaveChanges();
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("AddSeats")]
        public IActionResult AddSeats(int row, int column, int seat, string name, int additionalSeat)
        {
            restaurant.TablesGrid[row, column].TableSeats[additionalSeat].Order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            var tmp = db.OrderHistory.Include("Order").Include(x=>x.TableSeatsNumbers).FirstOrDefault((x) => x.Order.Id == restaurant.TablesGrid[row, column].TableSeats[seat].Order.Id);
            var addSeatNumber = new SeatNumber { Id = Guid.NewGuid(), Number = additionalSeat };
            tmp.TableSeatsNumbers.Add(addSeatNumber);
            db.SaveChanges();
            //orderHistory.AddSeatToOrder(restaurant.TablesGrid[row, column].TableSeats[seat].Order.Id, additionalSeat);
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncDiscount")] 
        public IActionResult IncDiscount(int row, int column, int seat, string name, string discount)
        {
            var order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            order.Discount += decimal.Parse(discount, new CultureInfo("en-US"));
            db.OrderHistory.Include("Order").FirstOrDefault(x=>x.Order.Id==order.Id).Order.Discount += decimal.Parse(discount, new CultureInfo("en-US"));
            db.SaveChanges();
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncTips")]
        public IActionResult IncTips(int row, int column, int seat, string name, string tips)
        {
            var order = restaurant.TablesGrid[row, column].TableSeats[seat].Order;
            order.Tips += decimal.Parse(tips, new CultureInfo("en-US"));
            db.OrderHistory.Include("Order").FirstOrDefault(x => x.Order.Id == order.Id).Order.Tips += decimal.Parse(tips, new CultureInfo("en-US"));
            db.SaveChanges();
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
            OrderInfo orderInfo = (OrderInfo)db.OrderHistory.Include("Order").Include(x=>x.Order.OrderItems).Include(x => x.TableSeatsNumbers).FirstOrDefault(x => x.Id == OrderInfoId)?.Clone(); //orderHistory.GetOrderInfo(OrderInfoId);
            if (orderInfo == null) return Redirect("~/Home/OrderHistory");
            bool orderClosed = orderInfo.Order.ClosedDate != new DateTime();
            Table curTable = null;
            foreach (Table t in restaurant.TablesGrid)
                if (t != null && t.Id == orderInfo.TableId) {
                    if (orderClosed)
                    { curTable = t.Clone() as Table; }
                    else curTable = t;
                }

            if (curTable == null) return BadRequest();

            ViewBag.row = curTable.Row;
            ViewBag.column = curTable.Column;
            ViewBag.curSeat = orderInfo.TableSeatsNumbers.FirstOrDefault().Number;

            if (orderClosed)
            foreach (TableSeat seat in curTable.TableSeats)
                seat.Order = orderInfo.Order;
            return View("TableDetail", curTable);
        }


    }
}
