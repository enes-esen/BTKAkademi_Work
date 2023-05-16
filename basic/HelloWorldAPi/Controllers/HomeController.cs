using System;
using HelloWorldAPi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldAPi.Controllers
{
	[ApiController]
	[Route("home")]
	public class HomeController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetMessage()
		{
			var result = new ResponseModel()
			{
				HttpStatus = 200,
				Message = "ASP.NET Core Web API"
			};
			return Ok(result);
		}
	}
}

