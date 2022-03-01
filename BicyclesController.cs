using _01_MVCWebAppIntro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace _01_MVCWebAppIntro.Controllers
{
    public class BicyclesController : Controller
    {
        // List
        public ActionResult Index()
        {
            //var list = new List<Bicycle>
            //{
            //    new Bicycle {Id = 1, Size = 24.5D,
            //        ProductionDate = DateTime.Now },
            //    new Bicycle {Id = 2, Size = 25.5D,
            //        ProductionDate = DateTime.Now },
            //    new Bicycle {Id = 3, Size = 26.5D,
            //        ProductionDate = DateTime.Now },
            //};

            //var result = string.Empty;
            //foreach (Bicycle b in list)
            //{
            //    result += b.Id + " " + b.Size.ToString();
            //    result += "<br/>";
            //}
            //ViewBag.Test = "Test";
            var result = new ApplicationDbContext().Bicycles.ToList();
            return View(result);

        }

        // Create => Display form
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Bicycle model)
        {
            if (model.Size < 10)
            {
                ModelState.AddModelError("0", "Size has to be greater than 10.");
            }
            if (ModelState.IsValid)
            {
                // Add to DB
                using (var db = new ApplicationDbContext())
                {
                    db.Bicycles.Add(new Bicycle
                    {
                        Size = model.Size,
                        ProductionDate = model.ProductionDate
                    });

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                var find = db.Bicycles.Find(id);
                if (find != null)
                {
                    return View(find);
                }

                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(Bicycle model, int? id = 0)
        {
            // Add to DB
            using (var db = new ApplicationDbContext())
            {
                var find = db.Bicycles.Find(id);
                if (find != null)
                {
                    find.Size = model.Size;
                    find.ProductionDate = model.ProductionDate;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                var find = db.Bicycles.Find(id);
                if (find != null)
                {
                    return View(find);
                }

                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id = 0)
        {
            // Add to DB
            using (var db = new ApplicationDbContext())
            {
                var find = db.Bicycles.Find(id);
                if (find != null)
                {
                    db.Bicycles.Remove(find);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // 1- ViewModel
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(BicycleSearchVM model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var l = int.Parse(model.LowerSize);
                    var h = int.Parse(model.UpperSize);
                    var result = db.Bicycles.Where(b => b.Size >= l && b.Size <= h).ToList();
                    // find 
                    return View("SearchResult", result);
                }
            }
            return View(model);
        }

        public ActionResult Search2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search2(BicycleSearchVM model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var l = double.Parse(model.LowerSize);
                    var h = double.Parse(model.UpperSize);
                    var result = db.Bicycles
                        .Where(b => b.Size >= l && b.Size <= h)
                        .ToList();
                    // find 
                    ViewBag.Results = result;
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult SPA()
        {
            return View();
        }

        // Create Ajax
        [HttpPost]
        public void CreateAjax(Bicycle model)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Bicycles.Add(model);
                db.SaveChanges();
            }
        }

        // Get All Objects Ajax
        public ActionResult GetAll()
        {
            Thread.Sleep(500);
            using (var db = new ApplicationDbContext())
            {
                var result = db.Bicycles.ToList();
                return PartialView(result);
            }
        }
    }
}