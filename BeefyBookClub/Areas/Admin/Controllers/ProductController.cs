using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeefyBooksClub.DataAccess.Repository.IRepository;
using BeefyBooksClub.Models;
using BeefyBooksClub.Models.ViewModels;
using BeefyBooksClub.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeefyBookClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        // private fields
        private readonly IUnitOfWork _unityOfWork;
        private readonly IWebHostEnvironment _hostEnviornment;

        // constructor that initializes the private field as type IUnitOfWork
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unityOfWork = unitOfWork;
            _hostEnviornment = hostEnvironment;
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
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unityOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unityOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            //this is for create
            if (id == null)
            {               
                return View(productVM);
            }

            //this is for edit
            productVM.Product = _unityOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }



        // Upsert POST Call for the Create and Updated based on whether there is an id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnviornment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        //this means that this is an edit and we need to remove old image
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                else
                {
                    //update when they do not change the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unityOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    _unityOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unityOfWork.Product.Update(productVM.Product);
                }

                _unityOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = _unityOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.CoverTypeList = _unityOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unityOfWork.Product.Get(productVM.Product.Id);
                }
            }

            return View(productVM);
        }

        #endregion


        #region API CALLS


        // Index API GET Call to display in the Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unityOfWork.Product.GetAll(includeProperties: "Category,CoverType" );
            return Json(new { data = allObj });
        }


        // Delete API Call to remove from the page and database
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unityOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string webRootPath = _hostEnviornment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unityOfWork.Product.Remove(objFromDb);
            _unityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
