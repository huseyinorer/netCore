using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace core2.Controllers
{
    public class SessionController : Controller
    {
        public string Index()
        {
            HttpContext.Session.SetInt32("age", 27);
            HttpContext.Session.SetString("name", "huseyin");
            return "Session yaratıldı.";
        }

        public string GetSessions()
        {


            return String.Format("Hello {0}, you {1}", HttpContext.Session.GetString("name"), HttpContext.Session.GetString("age"));
        }
    }
}