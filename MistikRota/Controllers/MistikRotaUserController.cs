using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MistikRota.Entites;
using MistikRota.BusinessLayer;
using MistikRota.BusinessLayer.Results;
using MistikRota.Filters;

namespace MistikRota.Controllers
{
    [Auth]
    [AuthAdmin]
    public class MistikRotaUserController : Controller
    {

        private MistikRotaUserManager _mistikRotaUserManager = new MistikRotaUserManager();

        // GET: MistikRotaUser
        public ActionResult Index()
        {
            return View(_mistikRotaUserManager.List());
        }

        // GET: MistikRotaUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MistikRotaUser mistikRotaUser = _mistikRotaUserManager.Find(x=>x.Id == id.Value);
            if (mistikRotaUser == null)
            {
                return HttpNotFound();
            }
            return View(mistikRotaUser);
        }

        // GET: MistikRotaUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MistikRotaUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MistikRotaUser mistikRotaUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MistikRotaUser> res = _mistikRotaUserManager.Insert(mistikRotaUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); //direkt validation üzerinden göstertmek
                    return View(mistikRotaUser);
                }

                

                return RedirectToAction("Index");
            }

            return View(mistikRotaUser);
        }

        // GET: MistikRotaUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MistikRotaUser mistikRotaUser = _mistikRotaUserManager.Find(x => x.Id == id.Value);
            if (mistikRotaUser == null)
            {
                return HttpNotFound();
            }
            return View(mistikRotaUser);
        }

        // POST: MistikRotaUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MistikRotaUser mistikRotaUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MistikRotaUser> res = _mistikRotaUserManager.Update(mistikRotaUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(mistikRotaUser);
                }

                return RedirectToAction("Index");
            }
            return View(mistikRotaUser);
        }

        // GET: MistikRotaUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MistikRotaUser mistikRotaUser = _mistikRotaUserManager.Find(x => x.Id == id.Value);
            if (mistikRotaUser == null)
            {
                return HttpNotFound();
            }
            return View(mistikRotaUser);
        }

        // POST: MistikRotaUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MistikRotaUser mistikRotaUser = _mistikRotaUserManager.Find(x => x.Id == id);
            _mistikRotaUserManager.Delete(mistikRotaUser);
            return RedirectToAction("Index");
        }

        
    }
}
