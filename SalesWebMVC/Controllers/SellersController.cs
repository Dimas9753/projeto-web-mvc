using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerservice;
        private readonly DepartmentService _departmentservice;

        public SellersController(SellerService sellerservice, DepartmentService departmentservice)
        {
            _sellerservice = sellerservice;
            _departmentservice = departmentservice;
        }
        public IActionResult Index()
        {
            var list = _sellerservice.FindAll();
            return View(list);
        }

        public IActionResult Create() // GET
        {
            var departments = _departmentservice.FindAll();
            var viewmodel = new SellerFormViewModel { Departments = departments };
            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller) // POST
        {
            _sellerservice.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)  //GET
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerservice.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)  //POST
        {
            _sellerservice.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerservice.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

         




    }
}