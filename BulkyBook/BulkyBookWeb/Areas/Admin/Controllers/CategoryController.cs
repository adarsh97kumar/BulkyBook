using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles=SD.Role_Admin)]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }
    //GET
    public IActionResult Create()
    {
        return View();
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        //var categoryFromDb = _db.categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Delete(int? id)
    {
        if (id == 0 || id == null)
        {
            return NotFound();
        }

        //var categoryFromDb = _db.categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);

    }
    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        //var obj = _db.categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }
        _unitOfWork.Category.Remove(categoryFromDbFirst);
        _unitOfWork.Save();
        TempData["Success"] = "Category deleted successfully";
        return RedirectToAction("Index");

    }


}
