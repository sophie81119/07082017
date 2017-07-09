using chdemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace chdemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult loaddata()
        {
            using (chdemo.Database1Entities dc = new chdemo.Database1Entities())
            {
                var data = dc.Table.OrderBy(a => a.Time).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult GetChartData()
        {
            List<Table> data = new List<Table>();
            var dt = new VisualizationDataTable();
            var chart = new ChartViewModel
            {
                Title = "Events",
                Subtitle = "per date",
                DataTable = dt
            };




            //Here MyDatabaseEntities  is our dbContext
            using (Database1Entities dc = new Database1Entities())
            {
                        
                data = dc.Table.ToList();

            }


            var sortbytime = data.OrderByDescending(x => x.Time).ToList();


            dt.AddColumn("Date", "string");
            dt.AddColumn("實際蒸汽壓力(kgf/cm2G)", "number");
            dt.AddColumn("預測蒸汽壓力(kgf/cm2G)", "number");

            int counter = 0;
            foreach (var item in sortbytime)
            {
                dt.NewRow(item.Time, item.ActualPressure, item.EstimatePressure);
                counter++;
                if (counter >= 12)
                    break;

            }






            return Content(JsonConvert.SerializeObject(chart), "application/json");
        }
    }
}