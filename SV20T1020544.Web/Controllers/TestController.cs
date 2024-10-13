using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020544.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Test",
                BirthDate = new DateTime(2002, 11, 16),
                Salary = 10.25m
            };
            return View(model);
        }
        public IActionResult Save(Models.Person model, string BirthDateInput="")
        {
            //chuyen birthdate sang kieu ngay
            DateTime? dvalue = StringToDateTime(BirthDateInput);
            if (dvalue.HasValue)
            {
                model.BirthDate = dvalue.Value;
            }
            return Json(model);
        }
        private DateTime? StringToDateTime(string s, string format = "d/M/yyyy;d-M-yyyyld.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, format.Split(';'), CultureInfo.InvariantCulture);
            }
            catch { return null; }
        }
    }
}
