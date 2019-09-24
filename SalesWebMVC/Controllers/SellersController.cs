using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

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
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = _sellerservice.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
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
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = _sellerservice.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            return View(obj);
        }

        public IActionResult Edit(int? id) //GET
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = _sellerservice.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            List<Department> departments = _departmentservice.FindAll();
            SellerFormViewModel viewmodel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch!" });
            }
            try
            {
                _sellerservice.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewmodel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewmodel);
        }


    }
}