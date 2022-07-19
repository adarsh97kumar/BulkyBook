using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly IWebHostEnvironment _hostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverList = _unitOfWork.Cover.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        }; 

        if(id==null || id==0)
        {
            //create product
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
            //update product
        }

        
    }
    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if(ModelState.IsValid)
        {
            //get path of wwwroot
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                //to rename file
                string fileName = Guid.NewGuid().ToString();
                //path of upload file
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                //path file extension
                var extension = Path.GetExtension(file.FileName);

                //if image already exist
                if(obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //copy file in products folder
                using(var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                //for db
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                
            }

            if (obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);
                TempData["Success"] = "Product added successfully";
            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
                TempData["Success"] = "Product updated successfully";
            }
            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Cover");
        return Json(new { data = productList});
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (productFromDb == null)
        {
            return Json(new {success=false, message="Error while deleting"});
        }

        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productFromDb.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(productFromDb);
        _unitOfWork.Save();
        return Json(new {success=true, message="Delete Successful"});
    }
    #endregion
}


