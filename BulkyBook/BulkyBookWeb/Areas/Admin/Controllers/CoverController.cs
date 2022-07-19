using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CoverController : Controller
{
    public readonly IUnitOfWork _unitOfWork;
    public CoverController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Cover> objCoverList = _unitOfWork.Cover.GetAll();
        return View(objCoverList);
    }
    //Get
    public IActionResult Create()
    {
        return View();
    }
    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Cover obj)
    {
        if(ModelState.IsValid)
        {
            _unitOfWork.Cover.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Cover created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //Get
    public IActionResult Edit(int? id)
    {
        if(id==null)
        {
            return NotFound();
        }

        var coverFromDb = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);
        if(coverFromDb==null)
        {
            return NotFound();
        }

        return View(coverFromDb);
    }
    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Cover obj)
    {
        if(ModelState.IsValid)
        {
            _unitOfWork.Cover.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Cover updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    //Get
    public IActionResult Delete(int? id)
    {
        if (id == null || id==0)
        {
            return NotFound();
        }

        var coverFromDb = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);
        if (coverFromDb == null)
        {
            return NotFound();
        }

        return View(coverFromDb);
    }
    //Post
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var coverFromDb = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);
        if (coverFromDb == null)
        {
            return NotFound();
        }

        _unitOfWork.Cover.Remove(coverFromDb);
        _unitOfWork.Save();
        TempData["Success"] = "Covey deleted successfully";
        return RedirectToAction("Index");
    }
}
