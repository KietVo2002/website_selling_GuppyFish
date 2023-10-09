using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NguoiDungController : Controller
    {
        //tao database
        dbQLGuppystoreDataContext db = new dbQLGuppystoreDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Dangky()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            //Gán Gía trị vào
            var hoten = collection["HotenKH"];
            var TenDN = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var Diachi = collection["Diachi"];
            var Email = collection["Email"];
            var DienThoai = collection["Dienthoai"];
            var Ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ Và Tên Không Được Để Trống !";
            }
            if (String.IsNullOrEmpty(TenDN))
            {
                ViewData["Loi2"] = "Nhập Tên Đăng Nhập ! ";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải Nhập Mật Khẩu !" ;
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Nhập Lại Mật Khẩu !";
            }
            if (String.IsNullOrEmpty(Email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống !";
            }
            if (String.IsNullOrEmpty(DienThoai))
            {
                ViewData["Loi6"] = "Nhập Số Điện Thoại !";
            }

            else
            {
                //Gắn giá trị cho đối tượng
                kh.HoTen = hoten;
                kh.TaiKhoan = TenDN;
                kh.MatKhau = matkhau;
                kh.Email = Email;
                kh.DiachiKH = Diachi;
                kh.DienThoaiKH = DienThoai;
                kh.NgaySinh = DateTime.Parse(Ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");

            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var TenDN = collection["TenDN"];
            var matkhau = collection["Matkhau"];
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
                //Gắn giá trị và lất Session
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == TenDN && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Bạn Đã Đăng Nhập Thành Công !";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index","GuppyStore");
                }
                else
                    ViewBag.Thongbao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng !";
            }
            return View();
        }
    }
}