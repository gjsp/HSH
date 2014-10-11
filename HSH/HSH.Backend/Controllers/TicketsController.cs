using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HSH.Data.Models;

namespace HSH.Backend.Controllers
{
    public class TicketsController : Controller
    {
        private HSHEntities db = new HSHEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            var ticket = db.Ticket.Include(t => t.AspNetUsers).Include(t => t.AspNetUsers1).Include(t => t.AspNetUsers2).Include(t => t.Member);
            return View(ticket.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.ApprovePayId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.ApproveId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.MemberId = new SelectList(db.Member, "MemberId", "MemberRef");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,TicketRef,TicketType,MemberId,TradeRef,Status,Purity,Quantity,Price,Amount,CreateDate,CreateBy,Active,PayStatus,UseDeposit,DueDate,PayType,PayId,PayDate,ApprovePayId,ApprovePayDate,ApproveId,ApproveDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketId = Guid.NewGuid();
                db.Ticket.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApprovePayId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApprovePayId);
            ViewBag.ApproveId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApproveId);
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.CreateBy);
            ViewBag.MemberId = new SelectList(db.Member, "MemberId", "MemberRef", ticket.MemberId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApprovePayId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApprovePayId);
            ViewBag.ApproveId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApproveId);
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.CreateBy);
            ViewBag.MemberId = new SelectList(db.Member, "MemberId", "MemberRef", ticket.MemberId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketId,TicketRef,TicketType,MemberId,TradeRef,Status,Purity,Quantity,Price,Amount,CreateDate,CreateBy,Active,PayStatus,UseDeposit,DueDate,PayType,PayId,PayDate,ApprovePayId,ApprovePayDate,ApproveId,ApproveDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApprovePayId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApprovePayId);
            ViewBag.ApproveId = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.ApproveId);
            ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.CreateBy);
            ViewBag.MemberId = new SelectList(db.Member, "MemberId", "MemberRef", ticket.MemberId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Ticket ticket = db.Ticket.Find(id);
            db.Ticket.Remove(ticket);
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
