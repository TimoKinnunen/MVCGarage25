using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCGarage25.DAL;
using MVCGarage25.Models;
using System.Data.Entity.Infrastructure;

namespace MVCGarage25.Controllers
{
    public class VehiclesController : Controller
    {
        private MVCGarage25Context db = new MVCGarage25Context();

        // GET: Vehicles
        public ActionResult Index()
        {
            bool detailedView = Request["view"] == "detailed";
            ViewBag.DetailedView = detailedView;
            string itemsToShow = Request["show"] ?? "all";
            ViewBag.Show = itemsToShow;
            var vehicles = db.Vehicles.Include(v => v.Member).Include(v => v.VehicleType);
            if(itemsToShow=="checkedin")
            {
                vehicles = vehicles.Where(v => v.EndParkingTime == null);
            }
            else if(itemsToShow=="checkedout")
            {
                vehicles = vehicles.Where(v => v.EndParkingTime != null);
            }
            return View(vehicles.ToList());
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/CheckIn
        public ActionResult CheckIn()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName");
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn([Bind(Include = "Id,MemberId,VehicleTypeId,RegistrationNumber,StartParkingTime,EndParkingTime,NumberOfWheels,BrandAndModel,Color")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.StartParkingTime = DateTime.Now;
                db.Vehicles.Add(vehicle);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException e)
                {
                    ModelState.AddModelError("RegistrationNumber", e.GetBaseException().Message);
                }
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            if (vehicle.IsCheckedOut)
            {
                return RedirectToAction("Details/" + vehicle.Id);
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,VehicleTypeId,RegistrationNumber,StartParkingTime,EndParkingTime,NumberOfWheels,BrandAndModel,Color")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException e)
                {
                    ModelState.AddModelError("RegistrationNumber", e.GetBaseException().Message);
                }
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/CheckOut/5
        public ActionResult CheckOut(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            if (vehicle.IsCheckedOut)
            {
                return RedirectToAction("Details/" + vehicle.Id);
            }
            return View(vehicle);
        }

        // POST: Vehicles/CheckOut/5
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            //db.Vehicles.Remove(vehicle);
            vehicle.EndParkingTime = DateTime.Now;
            db.Entry(vehicle).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Receipt/" + vehicle.Id);
        }

        // GET: Vehicles/Receipt/5
        public ActionResult Receipt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            if (!vehicle.IsCheckedOut)
            {
                return RedirectToAction("Details/" + vehicle.Id);
            }
            return View(vehicle);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
