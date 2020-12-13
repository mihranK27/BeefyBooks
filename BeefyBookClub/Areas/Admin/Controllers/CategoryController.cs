using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeefyBookClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // private field of IUnitOfWork
        private readonly IUnitOfWork _unityOfWork;

        // constructor that initializes the private field as type IUnitOfWork
        public CategoryController(IUnitOfWork unitOfWork)
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
            Category category = new Category();

            //this is for create
            if (id == null)
            {               
                return View(category);
            }

            //this is for edit
            category = _unityOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }



        // Upsert POST Call for the Create and Updated based on whether there is an id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unityOfWork.Category.Add(category);                   
                }
                else
                {
                    _unityOfWork.Category.Update(category);
                }

                _unityOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        #endregion


        #region API CALLS


        // Index API GET Call to display in the Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unityOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }


        // Delete API Call to remove from the page and database
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unityOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unityOfWork.Category.Remove(objFromDb);
            _unityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
