using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Controle.Models;


namespace Controle.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class EquipamentosController : Controller
    {
        private ControleEntities db = new ControleEntities();

        // GET: Equipamentos
        
        public ActionResult Index()
        {

            if (Session["Nome"] != null)
            {
                return View(db.Equipamento.ToList());
            }
            else
            {

                return RedirectToAction("ErrorLogin", "Home");
            }
            
        }

        // GET: Equipamentos/Details/5
        
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamento equipamento = db.Equipamento.Find(id);
            if (equipamento == null)
            {
                return HttpNotFound();
            }
            return View(equipamento);
        }

        // GET: Equipamentos/Create
        
        public ActionResult Create()
        {

            if (Session["Nome"] != null)
            {
                return View();
            }
            else
            {

                return RedirectToAction("ErrorLogin", "Home");
            }
        }

        // POST: Equipamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        
        public ActionResult Create([Bind(Include = "Id,Nome,Tipo,Modelo,LicensaSO,LicensaOFF,RegData,Descricao")] Equipamento equipamento)
        {
            if (ModelState.IsValid)
            {
                equipamento.RegData = DateTime.Now;
                db.Equipamento.Add(equipamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipamento);
        }

        // GET: Equipamentos/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamento equipamento = db.Equipamento.Find(id);
            if (equipamento == null)
            {
                return HttpNotFound();
            }
            return View(equipamento);
        }

        // POST: Equipamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Tipo,Modelo,LicensaSO,LicensaOFF,RegData,Descricao")] Equipamento equipamento)
        {
            if (ModelState.IsValid)
            {
                equipamento.RegData = DateTime.Now;
                db.Entry(equipamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipamento);
        }

        // GET: Equipamentos/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamento equipamento = db.Equipamento.Find(id);
            if (equipamento == null)
            {
                return HttpNotFound();
            }
            return View(equipamento);
        }

        // POST: Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Equipamento equipamento = db.Equipamento.Find(id);
            db.Equipamento.Remove(equipamento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
