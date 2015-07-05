using System;
using System.Threading;
using System.Web;

namespace DoctApp
{
	/// <summary>
	/// Abstract base class of all AjaxTasks
	/// </summary>
	internal abstract class AjaxTask : IAsyncResult
	{
		#region Private members

		private HttpContext _httpContext = null;
		private AsyncCallback _asyncCallback = null;
		private Object _asyncState = null;
		private WaitHandle _asyncWaitHandle = null;
		private string _redirectURL = null;

		#endregion


		#region Internal Properties

		/// <summary>
		/// Return current HttpContext. This property is set once by AjaxRouter and cannot be changed afterwards
		/// </summary>
		internal HttpContext HttpContext
		{
			get
			{
				return _httpContext;
			}

			set
			{
				if (_httpContext != null)
					throw new Exception("Context cannot be set more than once");

				_httpContext = value;
			}
		}

		/// <summary>
		/// Return current AsyncCallback to be called upon task completion. This property is set once by AjaxRouter and cannot be changed afterwards
		/// </summary>
		internal AsyncCallback AsyncCallback
		{
			get
			{
				return _asyncCallback;
			}

			set
			{
				if (_asyncCallback != null)
					throw new Exception("AsyncCallback cannot be set more than once");

				_asyncCallback = value;
			}
		}

		/// <summary>
		/// Return current AsyncWaitHandle
		/// </summary>
		internal WaitHandle AsyncWaitHandle
		{
			get
			{
				return _asyncWaitHandle;
			}

			set
			{
				if (_asyncWaitHandle != null)
					throw new Exception("AsyncWaitHandle cannot be set more than once");

				_asyncWaitHandle = value;
			}
		}

		/// <summary>
		/// Return current AsyncState (which is not used as of now). This property is set once by AjaxRouter and cannot be changed afterwards
		/// </summary>
		internal Object AsyncState
		{
			get
			{
				return _asyncState;
			}

			set
			{
				if (_asyncState != null)
					throw new Exception("AsyncState cannot be set more than once");

				_asyncState = value;
			}
		}

		internal string RedirectURL
		{
			get
			{
				return _redirectURL;
			}
			set
			{
				_redirectURL = value;
			}
		}

		#endregion


		#region Interface IAsyncResult

		/// <summary>
		/// Return WaitHandle associated with current task. This property is not used and will always return 'null'
		/// </summary>
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				return _asyncWaitHandle;
			}
		}

		/// <summary>
		/// Returns state object assigned to this AjaxTask (not used and is always null)
		/// </summary>
		Object IAsyncResult.AsyncState
		{
			get
			{
				return _asyncState;
			}
		}

		/// <summary>
		/// Property to indicate if the current task has completed or not
		/// </summary>
		bool IAsyncResult.IsCompleted
		{
			get
			{
				if (_asyncWaitHandle == null)
					return true;

				return _asyncWaitHandle.WaitOne(0);
			}
		}

		/// <summary>
		/// Property to indicate if this AjaxTask has completed its work asynchronously or not.
		/// This method will always return false except if AjaxTask.Validate() return false
		/// </summary>
		bool IAsyncResult.CompletedSynchronously
		{
			get
			{
				return (_asyncWaitHandle == null);
			}
		}

		#endregion


		#region Internal Static Properties

		static internal AjaxTask NullTask
		{
			get
			{
				return new NullAjaxTask();
			}
		}

		#endregion


		#region Internal methods

		/// <summary>
		/// Task execution start routine method. This method is called by AjaxRouter to start task execution
		/// </summary>
		internal abstract void Begin();

		/// <summary>
		/// Task execution method. This method is called by AjaxRouter from the context of the thread pool to execute the task
		/// </summary>
		internal abstract void Finish();

		#endregion		
	}
}
