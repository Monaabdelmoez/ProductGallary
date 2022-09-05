using Microsoft.AspNetCore.Mvc;
using ProductGallary.Models;
using ProductGallary.TDO;
using ProductGallary.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProductGallary.Reposatories;

namespace ProductGallary.Controllers
{
    public class GallaryController : Controller
    {
        IWebHostEnvironment env;
        private readonly UserManager<ApplicationUser> userManger;
        IReposatory<Gallary> reposatory;
        IFilter<Gallary> filter;
        public GallaryController(IReposatory<Gallary> reposatory, IWebHostEnvironment env, UserManager<ApplicationUser> userManger, IFilter<Gallary> filter)
        {
            this.reposatory = reposatory;
            this.env = env;
            this.userManger = userManger;
            this.filter = filter;
        }

        List<GalaryInfoDTO> galaryInfos = new List<GalaryInfoDTO>();
        public IActionResult Index()
        {
            var gallary = reposatory.GetAll();
            List<GalaryInfoDTO> galaryInfos = new List<GalaryInfoDTO>();
            foreach (var item in gallary)
            {
                GalaryInfoDTO dto = new GalaryInfoDTO();
                dto.Id = item.Id;
                dto.Name = item.Name;
                dto.Logo = item.Logo;
                dto.Created_Date = item.Created_Date;
                var userID = userManger.GetUserId(HttpContext.User);
                dto.user_id = userID;
                galaryInfos.Add(dto);
            }
            return View(galaryInfos);
        }
        public IActionResult Details(Guid id)
        {
            Gallary gal = reposatory.GetById(id);
            return View(gal);
        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult DetailsforBuyer(Guid id)
        {
            Gallary gal = reposatory.GetById(id);
            return View(gal);
        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult inseart()
        {
            return View( );
        }
        


        [HttpPost]
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult savegallary(GalaryCreateDTO galaryCreate)
        {

            if (ModelState.IsValid)
            {

                Gallary gallary = new Gallary();

                //upload fole to server

                string uploadimg = Path.Combine(env.WebRootPath, "img/GallaryLogo");
                string uniqe = Guid.NewGuid().ToString() + "_" + galaryCreate.Logo.FileName;
                string pathfile = Path.Combine(uploadimg, uniqe);
                using (var filestream = new FileStream(pathfile, FileMode.Create))
                {
                    galaryCreate.Logo.CopyTo(filestream);
                    filestream.Close();
                }
                
                gallary.Name = galaryCreate.name;
                gallary.Logo = uniqe;
                gallary.Created_Date = DateTime.Now;
                var userID = userManger.GetUserId(HttpContext.User);
                gallary.User_Id = userID;
                reposatory.Insert(gallary);
                return Redirect("index");
            }
            return View("inseart");

        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult edit(Guid id)
        {
            var gallary = reposatory.GetById(id);
            return View(gallary);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult update(Guid id, Gallary gallary)
        {
            if (ModelState.IsValid)
            {
                reposatory.Update(id, gallary);
                return RedirectToAction("index");
            }
            else
            {
                return View("edit");
            }

        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult delete(Guid id)
        {
            Gallary gallary = reposatory.GetById(id);
            return View(gallary);
        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]
        public IActionResult confirmdelete(Guid id)
        {
            reposatory.Delete(id);
            return RedirectToAction("index");
        }
        [Authorize(Roles = $"{Roles.BUYER_ROLE}")]

        public IActionResult Dashboard()
        {
            GalaryInfoDTO dto = new GalaryInfoDTO();
            var userID = userManger.GetUserId(HttpContext.User);
            dto.user_id = userID;
            return View(dto);


        }




        public IActionResult mygallary(string id)
        {
            var gallary = filter.filter(id);


            foreach (var item in gallary)
            {
                GalaryInfoDTO dto = new GalaryInfoDTO();
                dto.Id = item.Id;
                dto.Name = item.Name;
                dto.Logo = item.Logo;
                dto.Created_Date = item.Created_Date;
                galaryInfos.Add(dto);
            }
            return View(galaryInfos);


        }



    }
}
