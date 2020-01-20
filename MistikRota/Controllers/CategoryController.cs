using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MistikRota.BusinessLayer;
using MistikRota.Entites;
using MistikRota.Filters;

namespace MistikRota.Controllers
{
    [Auth] //Controller'ın başına ekleyerek bütün actionlar için geçerli kılıyorum. 
    [AuthAdmin]

    public class CategoryController : Controller
    {
        
        CategoryManager categoryManager = new CategoryManager();
        //bizim context'e direkt erişimimiz yok, ui katmanın bildiği şey business layerdır. bizim busines layer'da kategori ile 
        //ilgili işlemler category manager denilen, repository(tabloların nesnelerinin bulunduğu) classı kullanmam gerekiyor. 
        //Fakat UI katmanı repository tanımadığı için category manager içinde varolan sınırlı sayıdaki methodlara erişebilecektir. Bizim diğer ekleme, silme, d
        //düzenleme methodlarına erişebilmemiz için, dataacceslayer'daki IRepository interface'ine erişmemiz gerekiyor.
        //bunun için bir core projesi oluşturup IRepository nesnelerini buraya taşıdık. bu projeye her katman ulaşabilecek ve içindeki methodları kullanabilecek.
        //bu interface'i kullanabilmek için managerbase isimli yapıyı oluşturup, category manager'da miras edindik.

      
        public ActionResult Index()
        {
            return View(categoryManager.List()); //bana liste olarak category'leri getiriyor.
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x=>x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x=>x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {

                Category cat = categoryManager.Find(x => x.Id == category.Id);


                cat.Title = category.Title;
                cat.Description = category.Description; 

                categoryManager.Update(category);

                return RedirectToAction("Index");
            }
            return View(category);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x=>x.Id == id);
            categoryManager.Delete(category);

            return RedirectToAction("Index");
        }

        
    }
}
