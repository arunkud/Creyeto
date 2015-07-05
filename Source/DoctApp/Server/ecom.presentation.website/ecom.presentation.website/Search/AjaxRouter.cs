using System;
using System.Web;
using System.Web.SessionState;

namespace DoctApp
{
	/// <summary>
	/// AjaxRouter class whic handles all calls to AjaxRouter.ashx
	/// This class will manages and routes incoming ajax requests to appropriate AjaxTask handlers asynchronously
	/// </summary>
	public class AjaxRouter : IHttpAsyncHandler, IRequiresSessionState
	{
		#region Interface IHttpHandler

		/// <summary>
		/// Boolean flag indicating if the same instance of AjaxRouter can be used to handle more than one ajax request
		/// NOTE: This class is designed to be detached from the incoming ajax request, for that reason, this property returns 'true'
		/// </summary>
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///This method should not be called for 'IHttpAsyncHandler'
		///However, we have to provide implementation for it since 'IHttpAsyncHandler' (our base) which
		///is inherited from 'IHttpHandler' which requires this method
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			throw new InvalidOperationException();
		}

		#endregion


		#region Interface IHttpAsyncHandler

		/// <summary>
		/// Begins asynchronous ajax call handling
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		/// <param name="asyncState"></param>
		/// <returns></returns>
		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object asyncState)
		{
			AjaxTask task = null;
			string ajaxCmd = context.Request["cmd"];

			//Check for purchase restrictions
			if(!string.IsNullOrEmpty(ajaxCmd))
			{
				task = CreateTask(ajaxCmd.ToLower());	
			}
			
			if (task == null)
			{
				task = AjaxTask.NullTask;
			}
			
			//Assign task properties
			task.HttpContext = context;
			task.AsyncCallback = callback;
			task.AsyncState = asyncState;

			//Start the task
			task.Begin();

			//If task is not executing any async code then we need to call Finish() here
			if (task.AsyncWaitHandle == null)
				task.AsyncCallback(task);

			return task;
		}

		/// <summary>
		/// Called automatically when an ajax task is done executing
		/// NOTE: This function is called from the ThreadPool thread
		/// </summary>
		/// <param name="result"></param>
		public void EndProcessRequest(IAsyncResult result)
		{
			//A task has finished executing
			//Check Task.Status and proceed accordingly
			AjaxTask task = result as AjaxTask;
			task.Finish();

			//If there is a URL to redirect to then clear the response and only write the redirect url
			if (!string.IsNullOrEmpty(task.RedirectURL))
			{
				task.HttpContext.Response.Clear();
				task.HttpContext.Response.Write(task.RedirectURL);
			}
			else// Add cache-control, pragma, and expires to prevent caching of Ajax content
			{
				task.HttpContext.Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
				task.HttpContext.Response.AppendHeader("Pragma", "no-cache");
				task.HttpContext.Response.AppendHeader("Expires", "0");
			}
		}

		#endregion


		#region Private methods

		/// <summary>
		/// Create appropriate task class depending on the given ajax command
		/// </summary>
		/// <param name="ajaxCmd"></param>
		/// <returns></returns>
		static private AjaxTask CreateTask(string ajaxCmd)
		{
			switch (ajaxCmd)
			{
				case "search":
					return new SearchDoctor();				
			}

			return null;
		}

		#endregion
	}
}