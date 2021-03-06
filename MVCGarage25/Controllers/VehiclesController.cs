﻿using MVCGarage25.DAL;
using MVCGarage25.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCGarage25.Controllers
{
    public class VehiclesController : Controller
    {
        private MVCGarage25Context db = new MVCGarage25Context();

        public delegate IEnumerable<T> OrderMethod<T, TKey>(Func<T, TKey> OrderColumn);

        // POST: Vehicles/Search
        public ActionResult Search(string vehiclesearchtext)
        {
            vehiclesearchtext = vehiclesearchtext.ToLower();
            var vehicles = db.Vehicles
                .Include(v => v.Member)
                .Include(v => v.VehicleType)
                .Where(v =>
                    v.BrandAndModel.ToLower().Contains(vehiclesearchtext) ||
                    v.Color.ToLower().Contains(vehiclesearchtext) ||
                    v.NumberOfWheels.ToString().ToLower().Contains(vehiclesearchtext) ||
                    v.RegistrationNumber.ToString().ToLower().Contains(vehiclesearchtext) ||
                    v.VehicleType.Type.ToLower().Contains(vehiclesearchtext) ||
                    v.Member.FirstName.ToLower().Contains(vehiclesearchtext) ||
                    v.Member.LastName.ToLower().Contains(vehiclesearchtext)
                //v.StartParkingTime.ToString().ToLower().Contains(vehiclesearchtext) ||
                //v.EndParkingTime.ToString().ToLower().Contains(vehiclesearchtext)
                //v.ParkingTime.ToString().ToLower().Contains(vehiclesearchtext) ||
                //v.ParkingCost.ToString().ToLower().Contains(vehiclesearchtext) ||
                //v.ParkingCostPerHour.ToString().ToLower().Contains(vehiclesearchtext)
                );
            ViewBag.DetailedView = true;
            ViewBag.SearchResultText = "Search with '" + vehiclesearchtext + "' resulted in " + vehicles.Count().ToString() + " vehicles";
            return View(vehicles.ToList());
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            // Get view options

            bool detailedView = Request["view"] == "detailed";
            ViewBag.DetailedView = detailedView;
            string itemsToShow = Request["show"] ?? "all";
            ViewBag.Show = itemsToShow;

            var vehicles = db.Vehicles.Include(v => v.Member).Include(v => v.VehicleType);
            if (itemsToShow == "checkedin")
            {
                vehicles = vehicles.Where(v => v.EndParkingTime == null);
            }
            else if (itemsToShow == "checkedout")
            {
                vehicles = vehicles.Where(v => v.EndParkingTime != null);
            }

            // Get sorting options

            string sortColumn = Request["sortcolumn"] ?? "member";
            ViewBag.SortColumn = sortColumn;
            string sortOption = Request["sortoption"] ?? "ascending";
            ViewBag.SortOption = sortOption;
            var columns = new List<SelectListItem>();
            columns.Add(new SelectListItem() { Value = "memberfullname", Text = "Member's full name" });
            columns.Add(new SelectListItem() { Value = "memberfirstname", Text = "Member's first name" });
            columns.Add(new SelectListItem() { Value = "memberlastname", Text = "Member's last name" });
            columns.Add(new SelectListItem() { Value = "type", Text = "Vehicle type" });
            columns.Add(new SelectListItem() { Value = "regno", Text = "Registration number" });
            ViewBag.SortColumnList = columns;

            // This variant leads to a lot of duplicated code ... :-(

            //if (sortOption == "ascending")
            //{
            //    switch (sortColumn)
            //    {
            //        case "memberfirstname":
            //            vehicles = vehicles.OrderBy(v => v.Member.FirstName);
            //            break;
            //        case "memberlastname":
            //            vehicles = vehicles.OrderBy(v => v.Member.LastName);
            //            break;
            //        case "type":
            //            vehicles = vehicles.OrderBy(v => v.VehicleType.Type);
            //            break;
            //        case "regno":
            //            vehicles = vehicles.OrderBy(v => v.RegistrationNumber);
            //            break;
            //    }
            //}
            //else
            //{
            //    switch (sortColumn)
            //    {
            //        case "memberfirstname":
            //            vehicles = vehicles.OrderByDescending(v => v.Member.FirstName);
            //            break;
            //        case "memberlastname":
            //            vehicles = vehicles.OrderByDescending(v => v.Member.LastName);
            //            break;
            //        case "type":
            //            vehicles = vehicles.OrderByDescending(v => v.VehicleType.Type);
            //            break;
            //        case "regno":
            //            vehicles = vehicles.OrderByDescending(v => v.RegistrationNumber);
            //            break;
            //    }
            //}

            // This variant requires that we have declared this delegate:
            // public delegate IEnumerable<T> OrderMethod<T, TKey>(Func<T, TKey> OrderColumn);

            OrderMethod<Vehicle, string> orderMethod;
            Func<Vehicle, string> orderColumn;
            if (sortOption == "ascending")
            {
                orderMethod = vehicles.OrderBy;
            }
            else
            {
                orderMethod = vehicles.OrderByDescending;
            }
            switch (sortColumn)
            {
                case "memberfullname":
                    // FullName = LastName + FirstName
                    // Must combine these two columns manually,
                    // because LINQ can't order by a derived column
                    orderColumn = v => (v.Member.LastName + ", " + v.Member.FirstName);
                    break;
                case "memberfirstname":
                    orderColumn = v => v.Member.FirstName;
                    break;
                case "memberlastname":
                    orderColumn = v => v.Member.LastName;
                    break;
                case "type":
                    orderColumn = v => v.VehicleType.Type;
                    break;
                case "regno":
                    orderColumn = v => v.RegistrationNumber;
                    break;
                default:
                    orderColumn = v => (v.Member.LastName + ", " + v.Member.FirstName);
                    break;
            }
            var vehiclesToView = orderMethod(orderColumn).ToList();
            
            return View(vehiclesToView);
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
