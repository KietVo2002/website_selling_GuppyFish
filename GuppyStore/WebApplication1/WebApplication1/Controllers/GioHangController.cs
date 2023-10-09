using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GioHangController : Controller
    {
        //Tạo Data
        dbQLGuppystoreDataContext data = new dbQLGuppystoreDataContext();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMaCa, String strURL)
        {
            //Lay Session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm Tra trong giỏ hàng đã có sản phẩm chưa
            GioHang sanpham = lstGioHang.Find(n => n.iMaCa == iMaCa);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaCa);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);

            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        //Tổng Số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong=lstGioHang.Sum(n=>n.iSoLuong);
            }

            return iTongSoLuong;
        }
        //Tính Tiền
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTongTien=lstGioHang.Sum(n=>n.dThanhtien);
            }
            return iTongTien;

        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index","GuppyStore");
            }
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return View(lstGioHang);
            }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            //kiem tra danh sach da chon
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaCa == iMaSP);
            if(lstGioHang != null)
            {
                lstGioHang.RemoveAll(n => n.iMaCa == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "GuppyStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang (int iMaSP , FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm chọn vào hay chưa
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaCa == iMaSP);
            if(sanpham != null) {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }


        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra việc đăng nhập
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "GuppyStore");
            }
            //Lấy Gior hàng
            List<GioHang> lstGioHnag = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHnag);

        }
   
        public ActionResult DatHang(FormCollection collection)
        {
            //Thêm Đơn Hàng
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH=DateTime.Now;
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Thêm chi Tiết đơn hàng
            foreach(var item in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaCa = item.iMaCa;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (Decimal) item.dDonggia;
                data.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang","GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        }

    
    }
