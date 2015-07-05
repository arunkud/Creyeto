using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecom.presentation.website
{
	/// <summary>
	/// Summary description for SearchDoctors
	/// </summary>
	public class SearchDoctors : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write("Hello World");
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}