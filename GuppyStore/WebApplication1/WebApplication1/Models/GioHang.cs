using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GioHang
    {
        //Tạo mới Cơ Sở Dữ Liệu
        dbQLGuppystoreDataContext data = new dbQLGuppystoreDataContext();
        public int iMaCa { set; get; }
        public string sTenCa { set; get; }
        public string sAnhCa { set; get; }
        public double dDonggia { set; get; }
        public int iSoLuong { set; get; }
        public double dThanhtien {
            get { return iSoLuong * dDonggia; }
        }
        public GioHang(int MaCa){
            iMaCa = MaCa;
            Ca ca = data.Cas.Single(n => n.MaCa == iMaCa);
            sTenCa = ca.TenCa;
            sAnhCa = ca.AnhCa;
            dDonggia = double.Parse(ca.GiaBan.ToString());
            iSoLuong = 1;
            }
    }
}