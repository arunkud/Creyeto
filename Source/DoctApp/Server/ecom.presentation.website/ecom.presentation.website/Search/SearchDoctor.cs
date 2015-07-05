using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using ecom.presentation.website.Search;

namespace DoctApp
{
	/// <summary>
	/// The ajax task that returns flights or hotels or both based on the input parameters.
	/// It also creates a shopping cart if no spid has been provided or the shopping cart has expired. 
	/// </summary>
	internal class SearchDoctor : AjaxTask
	{		
		#region Internal Overrides

		internal override void Begin()
		{
			//#region Validate request
			//RequestModel = AjaxUtilities.GetRequestObject<SearchRequestModel>(HttpContext.Request);
			//if (RequestModel == null || RequestModel.SearchParameters == null || RequestModel.SearchParameters.Count == 0)
			//{
			//	_invalidSearchParameters.Add(InvalidSearchParameterType.NoParameters);
			//	return;
			//}

			//_dsInputValues = new Dictionary<string, string>();
			//_dsFormatAttributes = new Dictionary<string, FormAttribute>();
			
			//NameValueCollection searchParams = new NameValueCollection();

			//foreach (KeyValuePair<string, string> kvp in RequestModel.SearchParameters)
			//{
			//	searchParams.Add(kvp.Key, kvp.Value);
			//}

			//AjaxGlobalSearchParameters = ShoppingHelper.GenerateInputParameters(searchParams, out _dsInputValues, out _dsFormatAttributes);

			//foreach (KeyValuePair<string, FormAttribute> kvp in _dsFormatAttributes)
			//{
			//	if (kvp.Value.isError)
			//	{
			//		_invalidSearchParameters.Add(kvp.Value.InvalidSearchParameterType);
			//		return;
			//	}
			//}

			//if (AjaxGlobalSearchParameters == null)
			//{
			//	if (_invalidSearchParameters.Count == 0)
			//		_invalidSearchParameters.Add(InvalidSearchParameterType.Unknown);
			//	return;
			//}

			//#endregion

			////Initialize Shopping cart from session.
			//SetShoppingProcessDetails();
		}

		internal override void Finish()
		{
			////Do the actual search here.
			//if (_invalidSearchParameters.Count != 0)
			//{
			//	AjaxResponseModelBase errorModel = new AjaxResponseModelBase();
			//	errorModel.AjaxNotification.InvalidSearchParameters = _invalidSearchParameters;
			//	SetErrorMessageAndCompleteRequest(errorModel,MessageType.Error, JsonErrorCode.InvalidSearchParameters, null);
			//	return;
			//}

			//if (_isShoppingCartExpired)
			//{
			//	SetErrorMessageAndCompleteRequest(MessageType.Error, JsonErrorCode.InvalidShoppingCart, Shopping.MessageShoppingCartExpiredinHotelDescription);
			//	return;
			//}

			//GetResults();

			//SessionManager.PersistShoppingProcessDetails(_shoppingProcessDetails);
		}

		#endregion
	}
}