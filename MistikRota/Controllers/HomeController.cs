using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MistikRota.BusinessLayer;
using MistikRota.BusinessLayer.Results;
using MistikRota.Entites;
using MistikRota.Entites.Messages;
using MistikRota.Entites.ValueObjects;
using MistikRota.ViewModel;
using MistikRota.Filters;
using System.Threading;
using System.Globalization;

namespace MistikRota.Controllers
{
    public class HomeController : Controller
    {

        private StoryManager sm = new StoryManager();
       private CategoryManager cm = new CategoryManager();
       private MistikRotaUserManager mrum = new MistikRotaUserManager();

        // GET: Home
        public ActionResult Index()
        {
            return View(sm.ListQueryable().Where(x=>x.IsDraft == false).OrderByDescending(x => x.ModifiedDate).ToList());
        }

      



        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           
            Category cat = cm.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }

            return View("Index", cat.Stories.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedDate).ToList());

        }

        public ActionResult MostLiked()
        {
           

            return View("Index", sm.List().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {

            MistikRotaUser currentUser = Session["login"] as MistikRotaUser;

            

            BusinessLayerResult<MistikRotaUser> res = mrum.getUserById(currentUser.Id);

            if (res.Errors.Count  > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Bir hata oluştu",
                    Items = res.Errors
                };
            }


            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            
                MistikRotaUser currentUser = Session["login"] as MistikRotaUser;

               

                BusinessLayerResult<MistikRotaUser> res = mrum.getUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Bir hata oluştu",
                    Items = res.Errors
                };

            }
            return View(res.Result);
        }
        
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(MistikRotaUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                
                BusinessLayerResult<MistikRotaUser> res = mrum.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }


                Session["login"] = res.Result; //Profil güncellendiği için session güncellendi.

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            MistikRotaUser currentUser = Session["login"] as MistikRotaUser;

            
            BusinessLayerResult<MistikRotaUser> res = mrum.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear(); //Çünkü arkadaşı sildik, hala session'da kayıtlı kalmasın.

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

              
                BusinessLayerResult<MistikRotaUser> res = mrum.LoginUser(model);


                if (res.Errors.Count > 0)
                {

                    //if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    //{
                    //    ViewBag.SetLink = "ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";"
                    //}


                    res.Errors.ForEach(x =>
                        ModelState.AddModelError("", x.Message)); //her biri için ilgili string'i modelstate'in error'üne ekle.

                    

                    return View(model); //Çünkü hata var ve tekrar modele dönmeli.
                }

                Session["login"] = res.Result;       //session'a kullanıcı bilgi saklama
                return RedirectToAction("Index");    //yönlendirme...
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) //Atribbute'lar uygun mu?
            {

              
                //modelimi usermanageri içindeki register user fonk. gönderiyorum. oda bana yeni kullanıcıyı gönderiyor. (businesslayerresult(mistikrotauser) tipinde)
                BusinessLayerResult<MistikRotaUser> res = mrum.RegisterUser(model); 

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); //her biri için ilgili string'i modelstate'in error'üne ekle.
                    return View(model); //Çünkü hata var ve tekrar modele dönmeli.
                }
                
               return RedirectToAction("RegisterOk");

            }


            return View(model);
        }


        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
           
            BusinessLayerResult<MistikRotaUser> res = mrum.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors; //hatalar varsa bunları tempdata da tutup, activatecancel sayfasına yönlendircem.
                return RedirectToAction("UserActivateCancel");

            }

            return RedirectToAction("UserActivateOk");

        }

        public ActionResult UserActivateOk()
        {
            return View();
        }

        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                 errors = TempData["errors"] as List<ErrorMessageObj>;
            }

            return View(errors);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

       



    }
}
