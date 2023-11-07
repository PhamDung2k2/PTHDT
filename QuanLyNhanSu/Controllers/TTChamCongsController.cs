using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;

namespace QuanLyNhanSu.Controllers
{
    public class TTChamCongsController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: TTChamCongs
        public ActionResult Index()
        {
            IQueryable<TTChamCong> tTChamCongs;

            if (Convert.ToBoolean(Session["isAdmin"]))
            {
                tTChamCongs = db.TTChamCongs.Include(t => t.NhanVien);
            }
            else
            {
                int IdNV = (int)Session["user_id"];
                tTChamCongs = db.TTChamCongs.Include(t => t.NhanVien).Where(t => t.IdNV == IdNV);
            }

            return View(tTChamCongs.ToList());
        }
        
        // GET: TTChamCongs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTChamCong tTChamCong = db.TTChamCongs.Find(id);
            if (tTChamCong == null)
            {
                return HttpNotFound();
            }
            return View(tTChamCong);
        }

        // GET: TTChamCongs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TTChamCongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNV,NgayCC,TVao")] TTChamCong tTChamCong)
        {
            if (ModelState.IsValid)
            {
                db.TTChamCongs.Add(tTChamCong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdNV = new SelectList(db.NhanViens, "IdNV", "HoTen", tTChamCong.IdNV);
            return View(tTChamCong);
        }*/
        public TimeSpan Change()
        {
            string timeString = DateTime.Now.ToString("HH:mm:ss");// Thay thế chuỗi "HH:mm:ss" bằng giá trị thời gian thực tế
            string[] timeParts = timeString.Split(':');
            if (timeParts.Length == 3)
            {
                int hours, minutes, seconds;
                if (int.TryParse(timeParts[0], out hours) &&
                    int.TryParse(timeParts[1], out minutes) &&
                    int.TryParse(timeParts[2], out seconds))
                {
                    return new TimeSpan(hours, minutes, seconds);
                    // Bây giờ bạn có một đối tượng TimeSpan từ chuỗi "HH:mm:ss"
                }
            }
            return TimeSpan.Zero;
        }
        /*public ActionResult Checkin()
        {
            TTChamCong tTChamCong = new TTChamCong();
            tTChamCong.TVao = Change();
            tTChamCong.IdNV = Convert.ToInt32(Session["user_id"]);
            tTChamCong.NgayCC = DateTime.Now.Date;
            tTChamCong.TRa = null;
            tTChamCong.ViPham = false;

            // Kiểm tra xem bản ghi có tồn tại trong TTChamCong với IdNV và NgayCC tương tự hay không
            var existingRecord = db.TTChamCongs.FirstOrDefault(c => c.IdNV == tTChamCong.IdNV && c.NgayCC == tTChamCong.NgayCC);

            if (existingRecord == null)
            {
                // Nếu không có bản ghi trùng, thì thêm mới
                db.TTChamCongs.Add(tTChamCong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                // Nếu có bản ghi trùng, bạn có thể xử lý theo ý muốn, ví dụ hiển thị thông báo lỗi
                ModelState.AddModelError("", "Nhân viên đã chấm công cùng một ngày.");
                return View(tTChamCong); // Hoặc chuyển hướng đến trang khác hoặc thực hiện hành động khác
            }

        }*/
        public ActionResult Checkin()
        {
            TTChamCong tTChamCong = new TTChamCong();
            tTChamCong.TVao = Change();
            tTChamCong.IdNV = Convert.ToInt32(Session["user_id"]);
            tTChamCong.NgayCC = DateTime.Now.Date;
            tTChamCong.TRa = TimeSpan.Zero;
            tTChamCong.ViPham = false;

            // Kiểm tra xem bản ghi có tồn tại trong TTChamCong với IdNV và NgayCC tương tự hay không
            var existingRecord = db.TTChamCongs.FirstOrDefault(c => c.IdNV == tTChamCong.IdNV && c.NgayCC == tTChamCong.NgayCC);

            if (existingRecord == null)
            {
                // Nếu không có bản ghi trùng, thì thêm mới
                db.TTChamCongs.Add(tTChamCong);
                db.SaveChanges();
                var data = new { success = true };

                // Trả về JSON với JsonRequestBehavior.AllowGet
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { success = false };

                // Trả về JSON với JsonRequestBehavior.AllowGet
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /* public ActionResult Checkout()
         {
             int id = Convert.ToInt32(Session["user_id"]);
             DateTime ngay = DateTime.Now.Date;
             TTChamCong tTChamCong = db.TTChamCongs.Where(a => a.IdNV == id && a.NgayCC == ngay).ToList().First();
             tTChamCong.TRa = Change();
             db.Entry(tTChamCong).State = EntityState.Modified;
             db.SaveChanges();
             return RedirectToAction("Index");
         }*/
        public ActionResult Checkout()
        {
                int id = Convert.ToInt32(Session["user_id"]);
                DateTime ngay = DateTime.Now.Date;
                TTChamCong tTChamCong = db.TTChamCongs.FirstOrDefault(a => a.IdNV == id && a.NgayCC == ngay);

            if (tTChamCong != null && tTChamCong.TRa == TimeSpan.Zero)
            {

                // Cập nhật TRa nếu TRa bằng TimeSpan.Zero (không được khác TimeSpan.Zero)
                tTChamCong.TRa = Change();
                db.Entry(tTChamCong).State = EntityState.Modified;
                db.SaveChanges();
                var data = new { success = true };

                // Trả về JSON với JsonRequestBehavior.AllowGet
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { success = false };

                // Trả về JSON với JsonRequestBehavior.AllowGet
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }



        // GET: TTChamCongs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTChamCong tTChamCong = db.TTChamCongs.Find(id);
            if (tTChamCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdNV = new SelectList(db.NhanViens, "IdNV", "HoTen", tTChamCong.IdNV);
            return View(tTChamCong);
        }

        // POST: TTChamCongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCC,IdNV,NgayCC,TVao,TRa,ViPham")] TTChamCong tTChamCong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tTChamCong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdNV = new SelectList(db.NhanViens, "IdNV", "HoTen", tTChamCong.IdNV);
            return View(tTChamCong);
        }

        // GET: TTChamCongs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTChamCong tTChamCong = db.TTChamCongs.Find(id);
            if (tTChamCong == null)
            {
                return HttpNotFound();
            }
            return View(tTChamCong);
        }

        // POST: TTChamCongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TTChamCong tTChamCong = db.TTChamCongs.Find(id);
            db.TTChamCongs.Remove(tTChamCong);
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
