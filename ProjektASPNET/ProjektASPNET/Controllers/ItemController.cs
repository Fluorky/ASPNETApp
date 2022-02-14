using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektASPNET.Models;
using ProjektASPNET.ViewModel;

namespace ProjektASPNET.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ItemController : Controller
    {
        private ECartDBEntities1 objECartDbEntities;
        public ItemController()
        {
            objECartDbEntities = new ECartDBEntities1();
        }
        // GET: Item
        public ActionResult Index()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in objECartDbEntities.Categories
           
           select new SelectListItem()
           {
              Text = objCat.CategoryName,
              Value = objCat.CategoryId.ToString(),
              Selected = true
           });

            objItemViewModel.SubCategorySelectListItem = (from objSubCat in objECartDbEntities.Subcategories

            select new SelectListItem()
            {
               Text = objSubCat.SubcategoryName,
              Value = objSubCat.SubcategoryId.ToString(),
               Selected = true
            });
            objItemViewModel.ItemIdSelectListItem = (from objCat in objECartDbEntities.Items
           select new SelectListItem()
           {
               Text = objCat.ItemName,
               Value = objCat.ItemId.ToString(),
               Selected = true
           });
            return View(objItemViewModel);
        }

        public ActionResult Edit()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in objECartDbEntities.Categories

            select new SelectListItem()
            {
               Text = objCat.CategoryName,
               Value = objCat.CategoryId.ToString(),
               Selected = true
            });


            objItemViewModel.SubCategorySelectListItem = (from objSubCat in objECartDbEntities.Subcategories

            select new SelectListItem()
            {
              Text = objSubCat.SubcategoryName,
              Value = objSubCat.SubcategoryId.ToString(),
               Selected = true
             });

            objItemViewModel.ItemIdSelectListItem = (from objIt in objECartDbEntities.Items
            select new SelectListItem()
            {
               Text = objIt.ItemName,
               Value = objIt.ItemId.ToString(),
               Selected = true
            });
            return View(objItemViewModel);
        }



        public ActionResult Delete()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
           
            objItemViewModel.ItemIdSelectListItem = (from objIt in objECartDbEntities.Items
            select new SelectListItem()
            {
                Text = objIt.ItemName,
                Value = objIt.ItemId.ToString(),
                Selected = true
            });
            return View(objItemViewModel);
        }

        [HttpPost]
        public JsonResult Index(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

            Items objItem = new Items();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.SubcategoryId = objItemViewModel.SubcategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemId = Guid.NewGuid();
           // objItem.ItemId = Guid.Parse("b5825747-e715-49e9-9a91-0e7576758cb3");
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemPrice = objItemViewModel.ItemPrice;

           
           // objECartDbEntities.Entry(objItem).State = EntityState.Deleted;
            // objECartDbEntities.Entry(objItem).State = EntityState.Modified;
            objECartDbEntities.Items.Add(objItem);         
            objECartDbEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public JsonResult Edit(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

            Items objItem = new Items();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.SubcategoryId = objItemViewModel.SubcategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            // objItem.ItemId = Guid.NewGuid();
           // objItem.ItemId = Guid.Parse("cf6c4cd5-ce6d-4b3f-909b-cc69deaad2c3");
            objItem.ItemId = objItemViewModel.ItemId;
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemPrice = objItemViewModel.ItemPrice;

            //objECartDbEntities.Items.Add(objItem);
            objECartDbEntities.Items.Attach(objItem);
            objECartDbEntities.Entry(objItem).State = EntityState.Modified;
            objECartDbEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is modified Successfully." }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult Delete(ItemViewModel objItemViewModel)
        {
          

            Items objItem = new Items();
  
            //objItem.ItemId = Guid.NewGuid();
           // objItem.ItemId = Guid.Parse("b5825747-e715-49e9-9a91-0e7576758cb3");
            objItem.ItemId = objItemViewModel.ItemId;
            objItem.ItemName = objItemViewModel.ItemName;
            

            //objECartDbEntities.Items.Remove(objItem);
            objECartDbEntities.Entry(objItem).State = EntityState.Deleted;
            // objECartDbEntities.Entry(objItem).State = EntityState.Modified;
            //objECartDbEntities.Items.Add(objItem);         
            objECartDbEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is removed Successfully." }, JsonRequestBehavior.AllowGet);
        }
        
        
    }
}