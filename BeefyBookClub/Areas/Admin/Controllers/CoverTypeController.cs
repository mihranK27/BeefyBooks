using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using BeefyBooksClub.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeefyBookClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        // private field of IUnitOfWork
        private readonly IUnitOfWork _unityOfWork;

        // constructor that initializes the private field as type IUnitOfWork
        public CoverTypeController(IUnitOfWork unitOfWork)
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
            CoverType coverType = new CoverType();

            //this is for create
            if (id == null)
            {               
                return View(coverType);
            }

            //this is for edit
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            coverType = _unityOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }



        // Upsert POST Call for the Create and Updated based on whether there is an id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", coverType.Name);

                if (coverType.Id == 0)
                {
                    _unityOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, parameter);         
                }
                else
                {
                    parameter.Add("Id", coverType.Id);
                    _unityOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, parameter);
                }

                _unityOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(coverType);
        }

        #endregion


        #region API CALLS


        // Index API GET Call to display in the Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unityOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);
            return Json(new { data = allObj });
        }


        // Delete API Call to remove from the page and database
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            var objFromDb = _unityOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unityOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, parameter);
            _unityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
