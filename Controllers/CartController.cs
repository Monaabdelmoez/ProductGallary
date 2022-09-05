using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductGallary.Constants;
using ProductGallary.Models;
using ProductGallary.Reposatories;
using ProductGallary.TDO;

namespace ProductGallary.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> userManger;

        CartInterface cartrepo;
        IReposatory<Product> reposatory;
        IFilter<Cart> filter;
        public CartController(CartInterface cartrepo, UserManager<ApplicationUser> userManger, IReposatory<Product> reposatory, IFilter<Cart> filter)
        {
            this.cartrepo = cartrepo;
            this.userManger = userManger;
            this.reposatory = reposatory;
            this.filter = filter;
        }
        public IActionResult Index()
        {
           
            UserDto infoDTO = new UserDto();
            var userID = userManger.GetUserId(HttpContext.User);
            infoDTO.user_id = userID;
            return RedirectToAction("shoppingcart",infoDTO);
        }

        [Authorize(Roles = $"{Roles.CUSTOMER_ROLE}")]


        public IActionResult shoppingcart(string user_id)
        {
            List<Guid> productsIds = getProductsIds();
            //var cart = filter.filter(user_id);
            List<ProductInfoDTO> cartProducts = new List<ProductInfoDTO>();
            foreach(Guid id in productsIds)
            {
                Product product = this.reposatory.GetById(id);
                if(product!=null)
                {
                    cartProducts.Add(
                    new ProductInfoDTO
                    {
                        Id = product.Id,
                        Name=product.Name,
                        Image=product.Image,
                        Price=product.DiscountPercentage != null?(product.Price) - ((product.DiscountPercentage / 100) * (product.Price)) :product.Price,
                        DiscountPercentage=product.DiscountPercentage==null?0: product.DiscountPercentage
                    }
                    );
                }
            }
            return View(cartProducts);
        }
       [HttpPost]
       [Authorize(Roles = $"{Roles.CUSTOMER_ROLE}")]
        public IActionResult addtocart([FromRoute]Guid id)
        {
            var userID = userManger.GetUserId(HttpContext.User);
            if (userID!=null)
            {
                //Cart cart = new Cart();
                //var p = reposatory.GetById(id);
                if (ModelState.IsValid)
                {
                    List<Guid> products;
                    if (!TempData.ContainsKey(Constant.PRODUCTS))
                    {
                        products = new List<Guid>()
                        {
                            id
                        };
                        
                    }
                    else
                    {

                        products =JsonConvert.DeserializeObject<List<Guid>>(TempData[Constant.PRODUCTS].ToString());
                        products.Add(id);
                        


                    }
                    // save Products Ids In Session
                    TempData[Constant.PRODUCTS] = JsonConvert.SerializeObject(products);
                 
                    return RedirectToAction("index");
                }
                return Redirect("Details");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [Authorize(Roles = $"{Roles.CUSTOMER_ROLE}")]
        public IActionResult RemoveFromCart(Guid id)
        {
            List<Guid> productsIds = getProductsIds();
            productsIds.Remove(id);
            // update cart
            TempData[Constant.PRODUCTS] = JsonConvert.SerializeObject(productsIds);
            return RedirectToAction("shoppingcart");
        }

        [Authorize(Roles = $"{Roles.CUSTOMER_ROLE}")]
        public IActionResult ClearCart()
        {
            TempData.Remove(Constant.PRODUCTS);
            return RedirectToAction("shoppingcart");
        }

        // this method used to get all Products Ids From Session
        private List<Guid> getProductsIds()
        {
            List<Guid> productsIds = new List<Guid>();
            if (TempData.ContainsKey(Constant.PRODUCTS))
            {
                productsIds = JsonConvert.DeserializeObject<List<Guid>>(TempData[Constant.PRODUCTS].ToString());
                TempData.Keep();
            }
            return productsIds;
        }

    }
}
