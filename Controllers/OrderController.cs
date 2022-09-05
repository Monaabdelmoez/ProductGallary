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

    public class OrderController : Controller
    {
        private readonly IReposatory<Order> orderReposatory;
        private readonly UserManager<ApplicationUser> userManger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly CartInterface cartReposatory;
        private readonly IReposatory<Product> productReposatory;

        public OrderController(UserManager<ApplicationUser> _userManger, RoleManager<IdentityRole> _roleManager, IReposatory<Order> _orderReposatory, CartInterface _cartReposatory, IReposatory<Product> _productReposatory)
        {
            orderReposatory = _orderReposatory;
            userManger = _userManger;
            roleManager = _roleManager;
            cartReposatory = _cartReposatory;
            productReposatory = _productReposatory;
        }

        private List<OrderInfoDTO> getOrders(List<Order> orders)
        {
            List<OrderInfoDTO> orderInfoDTOs = new List<OrderInfoDTO>();
            foreach (Order order in orders)
            {
                OrderInfoDTO orderInfoDTO = new OrderInfoDTO
                {
                    Id = order.Id,
                    DeliveryDate = order.DeliveryDate,
                    OrderDate = order.OrderDate,
                    IsCanceled = order.IsCanceled,
                };

                // List Of Products Info
                List<ProductInfoDTO> productInfoDTOs = new List<ProductInfoDTO>();
                foreach (Product product in order.Cart.products)
                {
                    ProductInfoDTO productInfo = new ProductInfoDTO
                    {
                        Name = product.Name,
                        Image = product.Image,
                        Price = (bool)product.HasDiscount ? (product.DiscountPercentage / 100) * product.Price : product.Price
                    };
                    productInfoDTOs.Add(productInfo);
                }
                // add products to orders
                orderInfoDTO.Products = productInfoDTOs;


                orderInfoDTOs.Add(orderInfoDTO);

            }
            return orderInfoDTOs;
        }

        //[Authorize(Roles =$"{Roles.BUYER_ROLE},{Roles.ADMIN_ROLE}")]
        public async Task<IActionResult> Index()
        {

            var userId = userManger.GetUserId(HttpContext.User);
            ApplicationUser user = await userManger.FindByIdAsync(userId);

            if(user!=null)
            if (await userManger.IsInRoleAsync(user, Roles.ADMIN_ROLE))
            {
                // print all Orders To Admin
                List<Order> orders = this.orderReposatory.GetAll();
                List<OrderInfoDTO> orderInfoDTOs = getOrders(orders);
                return View(orderInfoDTOs);

            }
            else if (await userManger.IsInRoleAsync(user, Roles.BUYER_ROLE))
            {
                List<Order> orders = this.orderReposatory.GetAll().Where(order => order.Cart.User_Id == user.Id).ToList();
                List<OrderInfoDTO> orderInfoDTOs = getOrders(orders);
                return View(orderInfoDTOs);
            }




            return View("PageNotFound");
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.CUSTOMER_ROLE}")]
        public IActionResult SaveOrder()
        {
            var userID = userManger.GetUserId(HttpContext.User);
            Cart cart = new Cart();
            cart.User_Id=userID;
            Guid cartId = Guid.NewGuid();
            cart.Id = cartId;
            List<Guid> productsIds = getProductsIds();
            cart.products=new List<Product>();  
            foreach (Guid productId in productsIds)
            {
                Product product = this.productReposatory.GetById(productId);
                if (product != null)
                    cart.products.Add(product);
            }
            if (cartReposatory.Add(cart))
            {
                Guid orderId = Guid.NewGuid();
                Order order = new Order
                {
                    Cart_Id = cartId,
                    User_Id = userID,
                    Id = orderId,
                    OrderDate = DateTime.Now,
                    DeliveryDate=DateTime.Now.AddDays(5),
                    IsCanceled=false
                };
                Bill bill = new Bill()
                {
                    OrderId=orderId
                };
                if (orderReposatory.Insert(order))
                {
                    if (TempData.ContainsKey(Constant.ORDERID))
                        TempData[Constant.ORDERID] = orderId;
                    else
                        TempData.Add(Constant.ORDERID, orderId);

                    TempData.Remove(Constant.PRODUCTS);
                    return RedirectToAction( "DisplayBill", "Bill");
                }
                
            }
               
                    return RedirectToAction("Index", "Errors");    
        }
        private List<Guid> getProductsIds()
        {
            List<Guid> productsIds = JsonConvert.DeserializeObject<List<Guid>>(TempData[Constant.PRODUCTS].ToString());
            TempData.Keep();

            return productsIds;
        }
    }
}
