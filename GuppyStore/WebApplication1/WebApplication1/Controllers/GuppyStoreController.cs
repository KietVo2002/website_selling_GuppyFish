using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GuppyStoreController : Controller
    {
        //Tao doi tuong quan ly CSDL
        dbQLGuppystoreDataContext data = new dbQLGuppystoreDataContext();
        private List<Ca> Laycamoi(int count)
        {
            //Sap xep
            return data.Cas.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
           
        }
        public ActionResult Index()
        {
            //lay 20 dong ca moi
            var camoi = Laycamoi(20);
            return View(camoi);
        }
        public ActionResult Dongca()
        {
            var dongca = from dc in data.DongCas select dc;
            return PartialView(dongca);
        }
        public ActionResult SPTheodongca(int id)
        {
            var ca = from c in data.Cas where c.MaDC == id select c;
            return View(ca);
        }
     public ActionResult Details(int id)
        {
            var ca = from c in data.Cas
                     where c.MaCa == id 
                     select c;
            return View(ca.Single());
        }
    }
}