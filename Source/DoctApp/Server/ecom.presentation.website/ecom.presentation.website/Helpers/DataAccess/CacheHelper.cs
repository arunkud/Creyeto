using System;
using System.Configuration;
using System.Web;

namespace ecom.presentation.website.helper
{
    public enum CacheMethod : byte { Volatile, Persisted }

    /// <summary>
    /// Utilizes main functionalities to use cache within the application, such as Add to cache, Retrieve from cache, Remove from cache, and Clear cache
    /// </summary>
    public static class CacheHelper
	{
		#region Private Members

		private static TimeSpan _dataSetCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["DataSetCacheDurationInMinutes"]));
		private static TimeSpan _resolutionListCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ResolutionListCacheDurationInMinutes"]));
		private static TimeSpan _sensitiveObjectListCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["SensitiveObjectListCacheDurationInMinutes"]));
		private static TimeSpan _maximumDurationForDataFlush = TimeSpan.FromHours(Convert.ToDouble(ConfigurationManager.AppSettings["MaximumDurationForDataFlushInHours"]));
		private static TimeSpan _singleSensitiveObjectCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["SingleSensitiveObjectCacheDurationInMinutes"]));
		private static TimeSpan _singleHeavyObjectCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["SingleHeavyObjectCacheDurationInMinutes"]));
		private static TimeSpan _heavyObjectListCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["HeavyObjectListCacheDurationInMinutes"]));
		private static TimeSpan _shoppingCartCacheDuration = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ShoppingCartCacheDurationInMinutes"]));

		#endregion


		#region Public Property

		/// <summary>
		/// Duration for validating cached light weight data sets
		/// </summary>
		public static TimeSpan DataSetCacheDuration
		{
			get { return _dataSetCacheDuration; }
		}

		/// <summary>
		/// Duration for validating cached light weight resolution/geographical lists
		/// </summary>
		public static TimeSpan ResolutionListCacheDuration
		{
			get { return _resolutionListCacheDuration; }
		}

		/// <summary>
		/// Duration for validating cached sensitive object lists
		/// </summary>
		public static TimeSpan SensitiveObjectListCacheDuration
		{
			get { return _sensitiveObjectListCacheDuration; }
		}

		/// <summary>
		/// Duration for validating cached sensitive object lists
		/// </summary>
		public static TimeSpan MaximumDurationForDataFlush
		{
			get { return _maximumDurationForDataFlush; }
		}

		/// <summary>
		/// Duration for keeping sensitive object in cache
		/// </summary>
		public static TimeSpan SingleSensitiveObjectCacheDuration
		{
			get { return _singleSensitiveObjectCacheDuration; }
		}

		/// <summary>
		/// Duration for keeping heavy objects in cache
		/// </summary>
		public static TimeSpan SingleHeavyObjectCacheDuration
		{
			get { return _singleHeavyObjectCacheDuration; }
		}

		/// <summary>
		/// Duration for keeping heavy object lists in cache
		/// </summary>
		public static TimeSpan HeavyObjectListCacheDuration
		{
			get { return _heavyObjectListCacheDuration; }
		}		

		/// <summary>
		/// Duration for keeping shopping cart in cache
		/// </summary>
		public static TimeSpan ShoppingCartCacheDuration
		{
			get { return _shoppingCartCacheDuration; }
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Stores an object into the specified cache medium based on the provided parameters
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="keyName"></param>
		/// <param name="cacheDuration"></param>
		/// <param name="cacheMethod"></param>
		public static void Store(TimeSpan cacheDuration, string keyName, object obj)
		{
			HttpRuntime.Cache.Insert(keyName, obj, null, DateTime.UtcNow.Add(cacheDuration), System.Web.Caching.Cache.NoSlidingExpiration);			
		}

		/// <summary>
		/// Retrieves an object from the specified cache medium based on the provided parameters
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="cacheMethod"></param>
		/// <returns>T</returns>
		public static T Retrieve<T>(string keyName)
		{
			return (T)HttpRuntime.Cache[keyName];			
		}

		/// <summary>
		/// Removes a specific object from the cache based on its key name
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="cacheMethod"></param>
		public static void Remove(string keyName)
		{
			HttpRuntime.Cache.Remove(keyName);			
		}		

		#endregion
	}
}