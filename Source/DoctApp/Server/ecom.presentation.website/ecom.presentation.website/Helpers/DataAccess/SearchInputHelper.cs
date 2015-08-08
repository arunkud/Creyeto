using ecom.presentation.website.helper.Log.LogHelper;
using ecom.presentation.website.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;

namespace ecom.presentation.website.helper
{
    /// <summary>
    /// The controller class for Specialization
    /// </summary>
    public static class SearchInputHelper
    {
        #region Private Members

        private static List<SearchInfo> _allSpearchInputs = new List<SearchInfo>();
        private static Dictionary<string, SearchInfo> _searchInput_IDDictionary = new Dictionary<string, SearchInfo>();

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
            foreach (var item in db.GetAllSearchInputs())
            {
                _allSpearchInputs.Add(new SearchInfo
                {
                    ID = item.ID.ToString() + item.Category,
                    Name = item.Name                    
                });

            }

            _lastUpdated = DateTime.UtcNow;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Gets objects from the database
        /// </summary> 
        /// <returns>List(Specialization)</returns>
        public static List<SearchInfo> GetAll()
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

                List<SearchInfo> returnedSearches = new List<SearchInfo>();
                returnedSearches.AddRange(_allSpearchInputs);

                return returnedSearches;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }


        public static IEnumerable<SearchInfo> GetByName(string searchText)
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
                
                return _allSpearchInputs.Where(item => item.Name.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase)).OrderBy(k => k.Name).Take(20);
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }

        #endregion
    }
}