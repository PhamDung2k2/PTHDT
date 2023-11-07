using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;
//using cExcel = Microsoft.Office.Interop.Excel;

namespace QuanLyNhanSu.Controllers
{
    public class NhanViensController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: NhanViens
        public ActionResult Index()
        {
            
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan);
            return View(nhanViens.ToList());
        }
        public ActionResult TinhLuong(int? ThangCong)
        {
            List<Luong> luong = new List<Luong>();
            double tinhluong = 0.0;
            int tongngaycong = 0;
            int tongvipham = 0;
            double tienSum = 0.0;

            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).ToList();

            //tự nó dừng
            foreach (var nv in nhanViens)
            {
                tinhluong = db.sp_TinhLuong(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                tongngaycong = db.sp_TongNgayCong(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                tongvipham = db.sp_TongViPham(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                if (tinhluong >= 0 /*&& tongngaycong !=0*/)
                {
                    luong.Add(new Luong(tinhluong, tongngaycong, tongvipham, nv));
                    tienSum += tinhluong;
                }
                else
                {
                    luong.Add(new Luong(0.0, tongngaycong, tongvipham, nv));

                }

            }
            Session["LuongTra"] = tienSum;
            Session["ThangCong"] = ThangCong;
            return View(luong);
        }
        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.IdCV = new SelectList(db.ChucVus, "IdCV", "TenCV");
            ViewBag.IdPB = new SelectList(db.PhongBans, "IdPB", "TenPhong");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNV,HoTen,Email,Password,IsAdmin,SDT,GioiTinh,NgaySinh,QueQuan,DanToc,ChuyenNganh,IdPB,IdCV,LuongCB")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                if (db.NhanViens.Where(m => m.SDT == nhanVien.SDT).ToList().FirstOrDefault() != null)
                {
                    TempData["ErrorMessage"] = "Không Thể Thêm Nhân Viên Mới Khi Trùng Số Điện Thoại.Hãy Xem Xét Lại!";
                    return RedirectToAction("Create");
                }
                else
                {
                    nhanVien.Password = Helper.ComputeSha256Hash(nhanVien.Password);
                    db.NhanViens.Add(nhanVien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.IdCV = new SelectList(db.ChucVus, "IdCV", "TenCV", nhanVien.IdCV);
            ViewBag.IdPB = new SelectList(db.PhongBans, "IdPB", "TenPhong", nhanVien.IdPB);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCV = new SelectList(db.ChucVus, "IdCV", "TenCV", nhanVien.IdCV);
            ViewBag.IdPB = new SelectList(db.PhongBans, "IdPB", "TenPhong", nhanVien.IdPB);
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNV,HoTen,Email,Password,IsAdmin,SDT,GioiTinh,NgaySinh,QueQuan,DanToc,ChuyenNganh,IdPB,IdCV,LuongCB")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                nhanVien.Password = Helper.ComputeSha256Hash(nhanVien.Password);
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCV = new SelectList(db.ChucVus, "IdCV", "TenCV", nhanVien.IdCV);
            ViewBag.IdPB = new SelectList(db.PhongBans, "IdPB", "TenPhong", nhanVien.IdPB);
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            var hopDong = db.HopDongs.Where(h => h.IdNV == id).ToList();
            foreach (var hd in hopDong)
            {
                db.HopDongs.Remove(hd);
            }
            var cc = db.TTChamCongs.Where(h => h.IdNV == id).ToList();
            foreach (var c in cc)
            {
                db.TTChamCongs.Remove(c);
            }
            db.NhanViens.Remove(nhanVien);
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
       /* public ActionResult print()
        {
            List<Luong> luong = new List<Luong>();
            int ThangCong =Convert.ToInt32( Session["ThangCong"]);
            double tinhluong = 0.0;
            int tongngaycong = 0;
            int tongvipham = 0;
            double tienSum = 0.0;

            var nhanViens = db.NhanViens.Include(n => n.ChucVu).Include(n => n.PhongBan).ToList();

            //tự nó dừng
            foreach (var nv in nhanViens)
            {
                tinhluong = db.sp_TinhLuong(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                tongngaycong = db.sp_TongNgayCong(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                tongvipham = db.sp_TongViPham(nv.IdNV, ThangCong).FirstOrDefault().GetValueOrDefault();
                if (tinhluong >= 0 *//*&& tongngaycong !=0*//*)
                {
                    luong.Add(new Luong(tinhluong, tongngaycong, tongvipham, nv));
                    tienSum += tinhluong;
                }
                else
                {
                    luong.Add(new Luong(0.0, tongngaycong, tongvipham, nv));

                }

            }
            cExcel.Application Excel = new cExcel.Application();
            cExcel.Workbook wb = Excel.Workbooks.Add(cExcel.XlSheetType.xlWorksheet);
            cExcel.Worksheet ws = (cExcel.Worksheet)Excel.ActiveSheet;
            Excel.Visible = true;
            ws.Cells[1, 1] = "CÔNG TY TNHH LÊ VĂN VIỆT";
            ws.Cells[2, 1] = "Địa Chỉ: 451 Lê Văn Việt";
            ws.Cells[3, 1] = "Điện Thoại: 0354855783";
            ws.Cells[5, 2] = "a";
            ws.Cells[6, 1] = "b";
            ws.Cells[6, 2] = "c";
            ws.Cells[6, 3] = "d";
            ws.Cells[6, 4] = "e";
            ws.Cells[6, 5] = "f";
            ws.Cells[7, 1] = ;
            ws.Cells[7, 2] = bill.c_id;
            ws.Cells[7, 3] = bill.e_id;
            ws.Cells[7, 4] = bill.b_date;
            ws.Cells[7, 5] = bill.total;
            ws.Cells[9, 1] = "Dịch vụ chi tiết";
            ws.Cells[10, 1] = "STT";
            ws.Cells[10, 2] = "Tên dịch vụ";
            ws.Cells[10, 3] = "Số lượng";
            int i = 11;
            foreach (var item in luong)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (j == 1)
                    {
                        ws.Cells[i, 1] = i - 10;
                    }
                    else if (j == 2)
                    {
                        ws.Cells[i, j] = item.TongLuong;
                    }
                    else
                    {
                        ws.Cells[i, j] = item.TongVP;
                    }
                }
                i++;
            }
            return View("Index");
        }*/
    }
}
