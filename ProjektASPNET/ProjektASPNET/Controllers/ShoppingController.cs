using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjektASPNET.Models;
using ProjektASPNET.ViewModel;

namespace ProjektASPNET.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingController : Controller
    {
        private ECartDBEntities1 objECartDbEntities;
        private List<ShoppingCartModel> listOfShoppingCartModels;
        public ShoppingController()
        {
            objECartDbEntities = new ECartDBEntities1();
            listOfShoppingCartModels = new List<ShoppingCartModel>();
        }
        // GET: Shopping
      /*  public ActionResult Index()
        {
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objECartDbEntities.Items
                    join
                        objCate in objECartDbEntities.Categories
                        on objItem.CategoryId equals objCate.CategoryId
                    select new ShoppingViewModel()
                    {
                        ImagePath = objItem.ImagePath,
                        ItemName = objItem.ItemName,
                        Description = objItem.Description,
                        ItemPrice = objItem.ItemPrice,
                        ItemId = objItem.ItemId,
                        Category = objCate.CategoryName,
                        ItemCode = objItem.ItemCode
                    }

                ).ToList();
            return View(listOfShoppingViewModels);
        }*/

        public ActionResult Index(string searchString, string nulek = "NULL")
        {



            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objECartDbEntities.Items
                join
                 objCate in objECartDbEntities.Categories
                on objItem.CategoryId equals objCate.CategoryId where objItem.ItemName==searchString
               select new ShoppingViewModel()
                {
                   ImagePath = objItem.ImagePath,
                   ItemName = objItem.ItemName,
                   Description = objItem.Description,
                   ItemPrice = objItem.ItemPrice,
                   ItemId = objItem.ItemId,
                   Category = objCate.CategoryName,
                   ItemCode = objItem.ItemCode
                }).ToList();
                return View(listOfShoppingViewModels);
            }


            else
            {
                IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objECartDbEntities.Items
                join
                objCate in objECartDbEntities.Categories
                on objItem.CategoryId equals objCate.CategoryId
                select new ShoppingViewModel()
                {
                   ImagePath = objItem.ImagePath,
                   ItemName = objItem.ItemName,
                   Description = objItem.Description,
                   ItemPrice = objItem.ItemPrice,
                   ItemId = objItem.ItemId,
                   Category = objCate.CategoryName,
                   ItemCode = objItem.ItemCode
                }).ToList();
                return View(listOfShoppingViewModels);
            }
        }

        [HttpPost]
        public JsonResult Index(string ItemId)
        {
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Items objItem = objECartDbEntities.Items.Single(model => model.ItemId.ToString() == ItemId);
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(new {Success = true, Counter = listOfShoppingCartModels .Count}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModels);
        }

        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderId = 0;
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            Orders orderObj = new Orders()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}",DateTime.Now)
            };
            objECartDbEntities.Orders.Add(orderObj);
            objECartDbEntities.SaveChanges();
            OrderId = orderObj.OrderId;

            foreach (var item in listOfShoppingCartModels)
            {
                OrderDetails objOrderDetail = new OrderDetails();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objECartDbEntities.OrderDetails.Add(objOrderDetail);
                objECartDbEntities.SaveChanges();
            }

            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }
    }
}