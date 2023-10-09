using Lhh.DataAccess.Data;
using Lhh.DataAccess.Repository.IRepository;
using Lhh.Models;
using Lhh.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objCategoriesList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objCategoriesList);
        }

        ////get
        //public IActionResult Upsert(int? id)
        //{
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.Name,
        //        Value = u.Id.ToString(),
        //    });
        //    ViewBag.CategoryList = CategoryList;
        //    Product product = new Product();


        //    if (id == null|| id == 0)
        //    {
        //        return View(product);
        //    } 
        //    else
        //    {
        //        //update
        //        product = _unitOfWork.Product.Get(u => u.Id == id);
        //        return View(product);
        //    }


        //    ProductViewModel productViewModel = new()
        //    {
        //        CatagoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        }),
        //        Product = new Product()
        //    };
        //    return View(productViewModel);
        //}

        ////post
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Product product, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPaath = _webHostEnvironment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath = Path.Combine(wwwRootPaath, @"Images\Product");

        //            if (!string.IsNullOrEmpty(product.Image))
        //            {
        //                //delete the old image
        //                var oldImagePath = Path.Combine(wwwRootPaath,product.Image.TrimStart('\\'));
        //                if(System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //            product.Image = @"\Images\Product\"+ fileName;
        //        }

        //        if (product.Id == 0)    {_unitOfWork.Product.Add(product);}
        //                        else { _unitOfWork.Product.Update(product);}

        //        _unitOfWork.Save();
        //        TempData["success"] = "Product create success";
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);


        //}


        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            try {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images/product");
                        if (!string.IsNullOrEmpty(productViewModel.Product.Image))
                        {
                            //delete old imageURL	
                            var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productViewModel.Product.Image = @"\images\product\" + fileName;
                    }

                    if (productViewModel.Product.Id == 0)
                    {

                        _unitOfWork.Product.Add(productViewModel.Product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productViewModel.Product);
                    }
                    _unitOfWork.Save();
                    TempData["success"] = "Product was created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    });
                    return View(productViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(productViewModel);
            }
            
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objCategoriesList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objCategoriesList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string? oldImagePath = null;
            if (productToBeDeleted.Image != null)
            {
                oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.Image.TrimStart('\\'));
            }

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}
