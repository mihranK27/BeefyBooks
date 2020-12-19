using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeefyBooksClub.DataAccess.Data;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using BeefyBooksClub.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeefyBookClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        // private field of IUnitOfWork
        //private readonly IUnitOfWork _unityOfWork;

        //using the ApplicationDbContext instead of repository model
        private readonly ApplicationDbContext _db;

        // constructor that initializes the private field as type IUnitOfWork
        //public UserController(IUnitOfWork unitOfWork)
        //{
        //    _unityOfWork = unitOfWork;
        //}

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }



        #region CRUD CALLS


        // Index GET Call
        public IActionResult Index()
        {
            return View();
        }


       
        #endregion


        #region API CALLS


        // Index API GET Call to display in the Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                        
            }

            return Json(new { data = userList });
        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking " });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful." });
        }


        #endregion
    }
}
