using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MistikRota.Entites;
using MistikRota.Models;
using MistikRota.BusinessLayer;
using MistikRota.Filters;

namespace MistikRota.Controllers
{
    public class StoryController : Controller
    {

        StoryManager storyManager = new StoryManager();
        //Aşağıda create action'ında category'leri görüntülememizi istiyor, sonuçta kullanıcı category seçecek.
        //bunun için;
        CategoryManager categoryManager = new CategoryManager();


        [Auth]
        public ActionResult Index()
        {
            //Sessiondaki kullanıcının id'sini alıp, o kişinin id'sine ait notların görünmesi gerekiyor. 
            //Yalnız bunu sürekli yazmak yerine UI(Web) katmanımda harici bir şekilde bunun kontrolünü yapıp getiriyim. Model'de bunun için bir class oluşturdum.
            //Include sorgu çekerken kullanılacak diğer tabloları da kullan. Stroy'e, category'i Join ediyoruz yani. 
            //ListQueryable, Iqueryable türünde bir sistemde hazır kayıtlı interface'i kullanıyor. Görevi ise bize aslında story tablosuna o an select ediyor. 
            var stories = storyManager.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(x => x.ModifiedDate);

            return View(stories.ToList());
        }


        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = storyManager.Find(x=>x.Id == id.Value);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Story story)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                story.Owner = CurrentSession.User;

                storyManager.Insert(story);

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", story.CategoryId);
            return View(story);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = storyManager.Find(x => x.Id == id.Value);
            if (story == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", story.CategoryId);
            return View(story);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Story story)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Story db_note = storyManager.Find(x => x.Id == story.Id);
                db_note.IsDraft = story.IsDraft;
                db_note.CategoryId = story.CategoryId;
                db_note.Content = story.Content;
                db_note.Title = story.Title;

                storyManager.Update(db_note);

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", story.CategoryId);
            return View(story);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = storyManager.Find(x => x.Id == id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = storyManager.Find(x => x.Id == id);
            storyManager.Delete(story);
            return RedirectToAction("Index");
        }

        //public ActionResult GetNoteText(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Story story = storyManager.Find(x => x.Id == id.Value);

        //    if (story == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return PartialView("_PartialStoryText", story);
        //}

    }
}
