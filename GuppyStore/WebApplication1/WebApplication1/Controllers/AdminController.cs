using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;


namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        dbQLGuppystoreDataContext db = new dbQLGuppystoreDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ca(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.Cas.ToList().OrderBy(n => n.MaCa).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var TenDN = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(TenDN))
            {
                ViewData["Loi1"] = "Tên Đăng Nhập Không Được Để Trống !";

            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Nhập Mật Khẩu ! ";
            }
            else
            {
                //Gắn giá trị và lấy Session
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == TenDN && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Bạn Đã Đăng Nhập Thành Công !";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Ca", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng !";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemDongCa()
        {
            ViewBag.MaDC = new SelectList(db.DongCas.ToList().OrderBy(n => n.TenDongCa),"MaDC", "TenDongCa");
            ViewBag.MaNX = new SelectList(db.NoiXuats.ToList().OrderBy(n => n.TenNX), "MaNX", "TenNX");
            return View();
        }
     [HttpPost]
     [ValidateInput(false)]
     public ActionResult ThemDongCa(Ca ca, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDC = new SelectList(db.DongCas.ToList().OrderBy(n => n.TenDongCa), "MaDC", "TenDongCa");
            ViewBag.MaNX = new SelectList(db.NoiXuats.ToList().OrderBy(n => n.TenNX), "MaNX", "TenNX");
            if(fileupload == null)
            {
                ViewBag.ThongBao = "Chọn Ảnh vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu Tên File
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu Đường Dẫn
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    ca.AnhCa = fileName;
                    //Lưu file
                    db.Cas.InsertOnSubmit(ca);
                    db.SubmitChanges();
                    
                }
                return RedirectToAction("Ca");
            }
           
        }
        [HttpGet]
        public ActionResult XoaCa(int id)
        {
            //Lấy Đối tượng cần xóa
            Ca ca = db.Cas.SingleOrDefault(n => n.MaCa == id);
            ViewBag.Maca = ca.MaCa;
            if (ca == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ca);
        }
        [HttpPost,ActionName("XoaCa")]
        public ActionResult XoaNhanXoa(int id)
        {
            //Lấy Đối tượng cần xóa
            Ca ca = db.Cas.SingleOrDefault(n => n.MaCa == id);
            ViewBag.Maca = ca.MaCa;
            if (ca == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Cas.DeleteOnSubmit(ca);
            db.SubmitChanges();
            return RedirectToAction("Ca");
        }
        [HttpGet]
        public ActionResult SuaCa(int id)
        {
            //Lấy Đối tượng cần xóa
            Ca ca = db.Cas.SingleOrDefault(n => n.MaCa == id);
            ViewBag.Maca = ca.MaCa;
            if (ca == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDC = new SelectList(db.DongCas.ToList().OrderBy(n => n.TenDongCa), "MaDC", "TenDongCa",ca.MaCa);
            ViewBag.MaNX = new SelectList(db.NoiXuats.ToList().OrderBy(n => n.TenNX), "MaNX", "TenNX",ca.MaNX);
            return View(ca);
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult SuaCa(Ca ca, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDC = new SelectList(db.DongCas.ToList().OrderBy(n => n.TenDongCa),"MaDC", "TenDongCa");
            ViewBag.MaNX = new SelectList(db.NoiXuats.ToList().OrderBy(n => n.TenNX), "MaNX", "TenNX");
            if (fileupload == null)
            {
                ViewBag.ThongBao = "Chọn Ảnh vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu Tên File
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu Đường Dẫn
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    ca.AnhCa = fileName;
                    //Lưu file
                    UpdateModel(ca);
                    db.SubmitChanges();

                }
                return RedirectToAction("Ca");
            }

        }

    }
}