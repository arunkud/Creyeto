using ecom.presentation.website.helper.Log.LogHelper;
using ecom.presentation.website.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ecom.presentation.website.helper
{
	/// <summary>
	/// The controller class for City
	/// </summary>
	public static class LocationHelper
	{
		#region Private Members

		private static List<City> _allCities = new List<City>();
        private static List<CityLocation> _allCityLocations = new List<CityLocation>();
        private static Dictionary<int, City> _city_IDDictionary = new Dictionary<int, City>();
		private static Dictionary<int, List<CityLocation>> _cityLocation_CityIDDictionary = new Dictionary<int, List<CityLocation>>();

        private static ReaderWriterLock rwlock = new ReaderWriterLock();
		private static DateTime? _lastUpdated;

		#endregion


		#region Private Properties

		private static bool IsOutdated
		{
			get { return !_lastUpdated.HasValue || _lastUpdated.Value.Add(CacheHelper.ResolutionListCacheDuration) < DateTime.UtcNow; }
		}

		#endregion


		#region Private Methods

		private static void SynchronizeContent()
		{
            Entities db = new Entities();
            foreach(var cityLocation in db.CityLocations.Include("City"))
            {
                _allCityLocations.Add(cityLocation);
                if (!_allCities.Contains(cityLocation.City))
                {
                    _allCities.Add(cityLocation.City);
                    _city_IDDictionary.Add(cityLocation.CityID, cityLocation.City);
                }

                if (!_cityLocation_CityIDDictionary.ContainsKey(cityLocation.CityID))
                    _cityLocation_CityIDDictionary.Add(cityLocation.CityID, new List<CityLocation>() { cityLocation });
                else
                    _cityLocation_CityIDDictionary[cityLocation.CityID].Add(cityLocation);
            }
        
			_lastUpdated = DateTime.UtcNow;
		}

		#endregion		


		#region Public Methods

		/// <summary>
		/// Gets objects from the database
		/// </summary> 
		/// <returns>List(City)</returns>
		public static List<City> GetAll()
		{
			rwlock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				if (IsOutdated)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (IsOutdated)
							SynchronizeContent();
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}

				List<City> returnedCities = new List<City>();
				returnedCities.AddRange(_allCities);

				return returnedCities;
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
		}

        /// <summary>
		/// Gets objects from the database
		/// </summary> 
		/// <returns>List(City)</returns>
		public static List<CityLocation> GetAllCityLocation()
        {
            rwlock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (IsOutdated)
                {
                    LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
                    try
                    {
                        if (IsOutdated)
                            SynchronizeContent();
                    }
                    finally
                    {
                        rwlock.DowngradeFromWriterLock(ref lc);
                    }
                }

                List<CityLocation> returnedCities = new List<CityLocation>();
                returnedCities.AddRange(_allCityLocations);

                return returnedCities;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }


        /// <summary>
        /// Returns a specific City from the cached collection based on its ID
        /// </summary>
        /// <param name="id">The City ID object</param>
        /// <returns>City</returns>
        public static City GetByID(int id)
		{
			rwlock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				bool isSynchronized = false;
				if (IsOutdated)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (IsOutdated)
						{
							SynchronizeContent();
							isSynchronized = true;
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}

				City city;
				if (!_city_IDDictionary.TryGetValue(id, out city)
					&& !isSynchronized)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (!_city_IDDictionary.TryGetValue(id, out city))
						{
							SynchronizeContent();
							isSynchronized = true;

							if (!_city_IDDictionary.TryGetValue(id, out city))
								_city_IDDictionary[id] = null;
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}
				return city;
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
		}
		
		/// <summary>
		/// Returns a set of Cities which matches a specific pattern within its own or its country's name or codes
		/// </summary>
		/// <param name="text">The search text</param>
		/// <returns>List(City)</returns>
		public static List<City> GetByKeyword(string text)
		{
			return GetByKeyword(text, null, false, true);
		}
		
		/// <summary>
		/// Returns a set of Cities which matches a specific pattern within its own or its country's name or codes
		/// </summary>
		/// <param name="text">The search text</param>
		/// <param name="isAjaxRequest">An indicator to specify if it was an Ajax request</param>
		/// <param name="logIfUnresolvable"></param>
		/// <returns>List(City)</returns>
		public static List<City> GetByKeyword(string text, bool isAjaxRequest, bool logIfUnresolvable)
		{
			return GetByKeyword(text, null, isAjaxRequest, logIfUnresolvable);
		}

		/// <summary>
		/// Returns a set of Cities (within a specific country) which matches a specific pattern within its own or its country's name or codes
		/// </summary>
		/// <param name="text">The search text</param>
		/// <param name="country">The country to search within</param>
		/// <param name="isAjaxRequest">An indicator to specify if it was an Ajax request</param>
		/// <param name="logIfUnresolvable"></param>
		/// <returns>List(City)</returns>
		public static List<City> GetByKeyword(string text, Country country, bool isAjaxRequest, bool logIfUnresolvable)
		{
			text = text.ToUpper();

			List<City> cities = GetAll();
			List<City> results = new List<City>();

			//Since if the keyword was exactly 3 characters long most likely the user is entering an IATA code for the city/airport
			//but there are a few cities which are named in 3 letters, so to avoid ignoring them if we will search for the IATA code
			//then we search through the collection of cities for ones which have the same IATA code or same city name
			if (text.Length == 3 && !isAjaxRequest)
			{
				string properText = text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
				results = cities.FindAll(item => (item.NameEN == properText) && item.IsActive);
			}

			//If there are no matches, then search through the collection of cities by the full text search string using the following enhancements
			//The (" " + text) is meant to make sure that searched text is at the beginning of the keyword, and we make sure that all keywords are located after a blank space
			//if (results.Count == 0)
			//	results = cities.FindAll(delegate(City _city) { return (_city.FullSearchTextEN.Contains(" " + text) && _city.IsActive); });

			//Sorting results by the increasing order of the position of the keyword in the FullSearchTextEN string
			//Keyword's position concept: FullSearchTextEN is a concatenation of the following attributes, and in the following order
			//City.IATACode
			//City.ICAOCode
			//City.NameEN
			//City.KeywordsEN
			//Country.NameEN
			//Country.KeywordsEN
			//Airport.KeywordsEN
			//Country.ISOCode2
			//Country.ISOCode3
			if (results.Count == 0 && !isAjaxRequest && logIfUnresolvable)
				LogHelper.Log(Logger.Application, LogLevel.Warn, "Cannot resolve City for Keyword: " + text);
			else
				results.Sort(delegate(City a, City b) { return (a.NameEN.IndexOf(text) - b.NameEN.IndexOf(text)); });

			return results;
		}

		/// <summary>
		/// Returns a set of Cities within a specific country
		/// </summary>
		/// <param name="id"></param>
		/// <returns>List(City)</returns>
		public static List<CityLocation> GetLocationByCityID(int id)
		{
			rwlock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				bool isSynchronized = false;
				if (IsOutdated)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (IsOutdated)
						{
							SynchronizeContent();
							isSynchronized = true;
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}

				List<CityLocation> cityLocationList;
				if (!_cityLocation_CityIDDictionary.TryGetValue(id, out cityLocationList)
					&& !isSynchronized)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (!_cityLocation_CityIDDictionary.TryGetValue(id, out cityLocationList))
						{
							SynchronizeContent();
							isSynchronized = true;

							if (!_cityLocation_CityIDDictionary.TryGetValue(id, out cityLocationList))
							{
								cityLocationList = new List<CityLocation>();
                                _cityLocation_CityIDDictionary[id] = cityLocationList;
							}
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}
				else
				{
					if (cityLocationList == null)
                        cityLocationList = new List<CityLocation>();
				}

				List<CityLocation> returnedCityLocationList = new List<CityLocation>();
				foreach (CityLocation city in cityLocationList)
                    returnedCityLocationList.Add(city);

				return returnedCityLocationList;
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
		}

		#endregion
	}
}