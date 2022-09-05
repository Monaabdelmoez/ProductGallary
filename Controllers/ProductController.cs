using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductGallary.Constants;
using ProductGallary.Models;
using ProductGallary.Reposatories;
using ProductGallary.TDO;

namespace ProductGallary.Controllers
{
    public class ProductController : Controller
    {

        //DIP
        IReposatory<Product> ProductRepo;
        IReposatory<Category> CategRepo;
        IReposatory<Gallary> reposatory;
        IFilter<Gallary> filter;
        IProduct<Product> progal;
        CartInterface cart;
        private readonly UserManager<ApplicationUser> userManger;
        IWebHostEnvironment webHostEnvironment;
        //DI dependance injection (constructor)
        public ProductController(IReposatory<Product> _productRepo, IWebHostEnvironment webHostEnvironment,
            IReposatory<Category> _CategRepo, IReposatory<Gallary> reposatory, UserManager<ApplicationUser> userManger,
            IFilter<Gallary> _filter, CartInterface _cart, IProduct<Product> _product)
       
        {
            ProductRepo = _productRepo;
            CategRepo = _CategRepo;
            this.reposatory = reposatory;
            this.filter = _filter;
            this.progal = _product;
            this.userManger = userManger;
            this.webHostEnvironment = webHostEnvironment;
            this.cart = _cart;
        }
        //image upload
        //get all products

        public IActionResult Index()
        {

            var xProduct = ProductRepo.GetAll();

            return View(xProduct);
        }

        public IActionResult Details(Guid id)
        {

            var xProduct = ProductRepo.GetById(id);

            return View(xProduct);
        }

        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult New()
        {
            var userID = userManger.GetUserId(HttpContext.User);
            var xgallary = filter.filter(userID);
            ViewData["CategoryList"] = CategRepo.GetAll();
            ViewData["GallaryList"] = xgallary;

            return View(new ProductCreateTDO());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult Savenew(ProductCreateTDO createTDO)

        {
            var userID = userManger.GetUserId(HttpContext.User);
            var xgallary = filter.filter(userID);
            ViewData["CategoryList"] = CategRepo.GetAll();
            ViewData["GallaryList"] = xgallary;

           ViewData["CategoryList"] = CategRepo.GetAll();
            ViewData["GallaryList"] = reposatory.GetAll();
            if (ModelState.IsValid == true)
            {
                Product xproduct = new Product();
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Img/ProductImages");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + createTDO.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    createTDO.Image.CopyTo(fileStream);
                    fileStream.Close();
                }
                xproduct.Name = createTDO.Name;
                xproduct.Image = uniqueFileName;
                xproduct.Price = createTDO.Price;
                xproduct.HasDiscount = createTDO.HasDiscount;
                xproduct.DiscountPercentage = createTDO.DiscountPercentage;
                xproduct.Description = createTDO.Description;
                xproduct.Category_Id = createTDO.Category_Id;
                xproduct.Gallary_Id = createTDO.Gallary_Id;
                xproduct.User_Id = userID;
                ProductRepo.Insert(xproduct);
                return RedirectToAction("Index");
            }
            else
            {
                return View("New");
            }
        }

        // Edit
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]

        public IActionResult Update(Guid id)
        {
            var userID = userManger.GetUserId(HttpContext.User);
            var xgallary = filter.filter(userID);
            ViewData["CategoryList"] = CategRepo.GetAll();
            ViewData["GallaryList"] = xgallary;

            var oldproduct = ProductRepo.GetById(id);
            return View(oldproduct);
        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]

        public IActionResult Saveupdate(Guid id, Product newproduct)
        {
            var userID = userManger.GetUserId(HttpContext.User);
            var xgallary = filter.filter(userID);
            ViewData["CategoryList"] = CategRepo.GetAll();
            ViewData["GallaryList"] = xgallary;


            if (ModelState.IsValid)
            {

                ProductRepo.Update(id, newproduct);
                return RedirectToAction("Index");
            }

            return View("Update", newproduct);

        }

        //delete

        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]

        public IActionResult Delete(Guid id)
        {
            Product oldproduct = ProductRepo.GetById(id);
            return View(oldproduct);

        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]

        public IActionResult ConfirmDelete(Guid id)
        {
            ProductRepo.Delete(id);
            return RedirectToAction("Index");
        }



        // make user modify his products only
        public IActionResult modifyproducts(string id)
        {

            var vproducts = progal.GetAllProductsWithGallaryData(id);

            return View("modifyproducts", vproducts);

        }

        //show products in specific category
        public IActionResult showproductatcategory(Guid id)
        {

            var vproducts = progal.GetAllProductsWithCategoryData(id);

            return View(vproducts);

        }

        //show products in specific gallary

        public IActionResult showproductatgallary(Guid id)
        {

            var vproducts = progal.GetAllProductsWithGallaryId(id);

            return View(vproducts);

        }


    }
}
