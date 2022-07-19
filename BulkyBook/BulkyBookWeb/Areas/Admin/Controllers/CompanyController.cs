using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{
    public readonly IUnitOfWork _unitOfWork;
    public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        Company company = new();

        if(id==null || id==0)
        { 
            return View(company);
        }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            return View(company);
        }

        
    }
    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj)
    {
        if(ModelState.IsValid)
        { 

            if (obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
                TempData["Success"] = "Company added successfully";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
                TempData["Success"] = "Company updated successfully";
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
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList});
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var companyFromDb = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (companyFromDb == null)
        {
            return Json(new {success=false, message="Error while deleting"});
        }

        _unitOfWork.Company.Remove(companyFromDb);
        _unitOfWork.Save();
        return Json(new {success=true, message="Delete Successful"});
    }
    #endregion
}


