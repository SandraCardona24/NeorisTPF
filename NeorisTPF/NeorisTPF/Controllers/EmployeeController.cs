using jQueryAjaxInAsp.NETMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        IEnumerable<Employees> GetAllEmployee()
        {
            using (DBModels db = new DBModels())
            {
                return db.Employees.ToList<Employees>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Employees emp = new Employees();
            if (id != 0)
            {
                using (DBModels db = new DBModels())
                {
                    emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employees>();
                }
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Employees emp)
        {
            try
            {
                
                using (DBModels db = new DBModels())
                {
                    if (emp.EmployeeID == 0)
                    {
                        db.Employees.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModels db = new DBModels())
                {
                    Employees emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employees>();
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}