using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult TableDetail(int row, int column, int seat,bool x)
        {
            Order order = new Order();
            Table table = restaurant.TablesGrid[row, column];
            table.TableSeats[seat].Order = order;
            orderHistory.AddOrder(order, table,new List<TableSeat> { table.TableSeats[seat] });
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Pay")]
        public IActionResult TableDetail(int row, int column, int seat,string nothing)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Pay();
            restaurant.TablesGrid[row, column].TableSeats[seat].Order=null;
            return  Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("Void")]
        public IActionResult TableDetail(int row, int column, int seat, double nothing)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Void();
            restaurant.TablesGrid[row, column].TableSeats[seat].Order = null;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("CreateItem")]
        public IActionResult TableDetail(int row, int column, int seat, string name,int price)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.OrderItems.Add(new OrderItem {Name=name,Price=price,CustomerMedia=new byte[1] });
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncDiscount")]
        public IActionResult TableDetail(int row, int column, int seat, string name, decimal discount)
        {
            restaurant.TablesGrid[row, column].TableSeats[seat].Order.Discount += discount;
            return Redirect($"~/Home/TableDetail?row={row}&column={column}&seat={seat}");
        }
        [HttpPost("IncTips")]
        public IActionResult TableDetail(int row, int column, int seat, string name, decimal tips, bool x)
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
        [HttpGet("View")]
        public IActionResult TableDetail(int row, int column, int seat,Guid id)
        {
            ViewBag.row = row;
            ViewBag.column = column;
            ViewBag.curSeat = seat;
            Table table = new Table(row, column);
            table.TableSeats[seat].Order = orderHistory.GetOrders().Where(x => x.Id == id).Select(x=>x.Order).FirstOrDefault();
            return View(table);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
