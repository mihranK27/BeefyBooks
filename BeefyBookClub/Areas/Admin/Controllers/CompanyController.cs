using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using BeefyBooksClub.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeefyBookClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CompanyController : Controller
    {
        // private field of IUnitOfWork
        private readonly IUnitOfWork _unityOfWork;

        // constructor that initializes the private field as type IUnitOfWork
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unityOfWork = unitOfWork;
        }


        #region CRUD CALLS


        // Index GET Call
        public IActionResult Index()
        {
            return View();
        }



        // Upsert GET Call for Create and Update based on whether there is an id
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();

            //this is for create
            if (id == null)
            {               
                return View(company);
            }

            //this is for edit
            company = _unityOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }



        // Upsert POST Call for the Create and Updated based on whether there is an id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unityOfWork.Company.Add(company);                   
                }
                else
                {
                    _unityOfWork.Company.Update(company);
                }

                _unityOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        #endregion


        #region API CALLS


        // Index API GET Call to display in the Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unityOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }


        // Delete API Call to remove from the page and database
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unityOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unityOfWork.Company.Remove(objFromDb);
            _unityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
