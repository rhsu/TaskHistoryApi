﻿using System.Web.Mvc;

namespace AngularProto.Controllers
{
	public class RoutesDemoController : Controller
	{
		public ActionResult One()
		{
			return View();
		}

		public ActionResult Two(int donuts = 1)
		{
			ViewBag.Donuts = donuts;
			return View();
		}

		public ActionResult Three()
		{
			return View();
		}
	}
}
