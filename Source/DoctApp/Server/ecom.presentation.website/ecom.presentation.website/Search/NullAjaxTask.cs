using System;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace DoctApp
{
	internal class NullAjaxTask : AjaxTask
	{
		#region AjaxTask overrides

		internal override void Begin()
		{
		}

		internal override void Finish()
		{
		}

		#endregion
	}
}
