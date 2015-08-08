using ecom.presentation.website.helper.Log.LogHelper;
using ecom.presentation.website.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ecom.presentation.website.helper
{
	/// <summary>
	/// The controller class for Specialization
	/// </summary>
	public static class SpecialityHelper
	{
		#region Private Members

		private static List<Specialization> _allSpecialities = new List<Specialization>();
        private static Dictionary<int, Specialization> _specialization_IDDictionary = new Dictionary<int, Specialization>();
		
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
            foreach(var specialization in db.Specializations)
            {
                _allSpecialities.Add(specialization);
                if (!_allSpecialities.Contains(specialization))
                {
                    _allSpecialities.Add(specialization);
                    _specialization_IDDictionary.Add(specialization.ID, specialization);
                }                
            }
        
			_lastUpdated = DateTime.UtcNow;
		}

		#endregion		


		#region Public Methods

		/// <summary>
		/// Gets objects from the database
		/// </summary> 
		/// <returns>List(Specialization)</returns>
		public static List<Specialization> GetAll()
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

				List<Specialization> returnedCities = new List<Specialization>();
				returnedCities.AddRange(_allSpecialities);

				return returnedCities;
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
		}

        /// <summary>
        /// Returns a specific Specialization from the cached collection based on its ID
        /// </summary>
        /// <param name="id">The Specialization ID object</param>
        /// <returns>Specialization</returns>
        public static Specialization GetByID(int id)
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

				Specialization specialization;
				if (!_specialization_IDDictionary.TryGetValue(id, out specialization)
					&& !isSynchronized)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (!_specialization_IDDictionary.TryGetValue(id, out specialization))
						{
							SynchronizeContent();
							isSynchronized = true;

							if (!_specialization_IDDictionary.TryGetValue(id, out specialization))
								_specialization_IDDictionary[id] = null;
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}
				return specialization;
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
		/// <returns>List(Specialization)</returns>
		public static List<Specialization> GetByKeyword(string text)
		{
			return GetByKeyword(text, null, false, true);
		}
		
		/// <summary>
		/// Returns a set of Cities which matches a specific pattern within its own or its country's name or codes
		/// </summary>
		/// <param name="text">The search text</param>
		/// <param name="isAjaxRequest">An indicator to specify if it was an Ajax request</param>
		/// <param name="logIfUnresolvable"></param>
		/// <returns>List(Specialization)</returns>
		public static List<Specialization> GetByKeyword(string text, bool isAjaxRequest, bool logIfUnresolvable)
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
		/// <returns>List(Specialization)</returns>
		public static List<Specialization> GetByKeyword(string text, Country country, bool isAjaxRequest, bool logIfUnresolvable)
		{
			text = text.ToUpper();

			List<Specialization> cities = GetAll();
			List<Specialization> results = new List<Specialization>();

			//Since if the keyword was exactly 3 characters long most likely the user is entering an IATA code for the specialization/airport
			//but there are a few cities which are named in 3 letters, so to avoid ignoring them if we will search for the IATA code
			//then we search through the collection of cities for ones which have the same IATA code or same specialization name
			if (text.Length == 3 && !isAjaxRequest)
			{
				string properText = text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
				results = cities.FindAll(item => (item.NameEN == properText) && item.IsActive);
			}

			//If there are no matches, then search through the collection of cities by the full text search string using the following enhancements
			//The (" " + text) is meant to make sure that searched text is at the beginning of the keyword, and we make sure that all keywords are located after a blank space
			//if (results.Count == 0)
			//	results = cities.FindAll(delegate(Specialization _specialization) { return (_specialization.FullSearchTextEN.Contains(" " + text) && _specialization.IsActive); });

			//Sorting results by the increasing order of the position of the keyword in the FullSearchTextEN string
			//Keyword's position concept: FullSearchTextEN is a concatenation of the following attributes, and in the following order
			//Specialization.IATACode
			//Specialization.ICAOCode
			//Specialization.NameEN
			//Specialization.KeywordsEN
			//Country.NameEN
			//Country.KeywordsEN
			if (results.Count == 0 && !isAjaxRequest && logIfUnresolvable)
				LogHelper.Log(Logger.Application, LogLevel.Warn, "Cannot resolve Specialization for Keyword: " + text);
			else
				results.Sort(delegate(Specialization a, Specialization b) { return (a.NameEN.IndexOf(text) - b.NameEN.IndexOf(text)); });

			return results;
		}
        
		#endregion
	}
}