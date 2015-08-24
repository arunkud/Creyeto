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
	public static class DoctorHelper
	{
		#region Private Members

		private static Dictionary<int, List<DoctorSearchResponseInfo>> _doctor_CityLocationIDDictionary = new Dictionary<int, List<DoctorSearchResponseInfo>>();
        private static List<DoctorSearchResponseInfo> _allDoctors = new List<DoctorSearchResponseInfo>();

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
            var doctors = db.Doctors.Include("Hospital").Include("Specialization");
            foreach (var doctor in doctors)
            {
                DoctorSearchResponseInfo searchResponseItem = new DoctorSearchResponseInfo {
                    Name = doctor.NameEN,
                    Specialization = doctor.Specialization.NameEN,
                    Qualification = doctor.Qualification,
                    ContactNumber = doctor.Hospital.ContactNumber,
                    Hospital = doctor.Hospital.NameEN,
                    Fee = doctor.Fee,
                    Image = !string.IsNullOrEmpty(doctor.ImageExt) ? "~/Images/" + doctor.HospitalID + "/" + doctor.ID + doctor.ImageExt : null
                };

                _allDoctors.Add(searchResponseItem);

                int cityLocationId = doctor.Hospital.CityLocationID;
                if (_doctor_CityLocationIDDictionary.ContainsKey(cityLocationId))
                    _doctor_CityLocationIDDictionary[cityLocationId].Add(searchResponseItem);
                else
                    _doctor_CityLocationIDDictionary.Add(cityLocationId, new List<DoctorSearchResponseInfo> { searchResponseItem });
            }
        
			_lastUpdated = DateTime.UtcNow;
		}

        #endregion


        #region Public Methods

        /// <summary>
        /// Returns a specific Specialization from the cached collection based on its ID
        /// </summary>
        /// <param name="id">The Specialization ID object</param>
        /// <returns>Specialization</returns>
        public static List<DoctorSearchResponseInfo> GetAllDoctors()
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

                List<DoctorSearchResponseInfo> doctorSearchResponses = new List<DoctorSearchResponseInfo>();
                doctorSearchResponses.AddRange(_allDoctors);                
                return doctorSearchResponses;
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
        public static List<DoctorSearchResponseInfo> GetByCityLocationID(int id)
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

				List<DoctorSearchResponseInfo> doctorSearchResponses;
				if (!_doctor_CityLocationIDDictionary.TryGetValue(id, out doctorSearchResponses)
					&& !isSynchronized)
				{
					LockCookie lc = rwlock.UpgradeToWriterLock(Timeout.Infinite);
					try
					{
						if (!_doctor_CityLocationIDDictionary.TryGetValue(id, out doctorSearchResponses))
						{
							SynchronizeContent();
							isSynchronized = true;

							if (!_doctor_CityLocationIDDictionary.TryGetValue(id, out doctorSearchResponses))
                                _doctor_CityLocationIDDictionary[id] = null;
						}
					}
					finally
					{
						rwlock.DowngradeFromWriterLock(ref lc);
					}
				}
				return doctorSearchResponses;
			}
			finally
			{
				rwlock.ReleaseReaderLock();
			}
		}
		
		#endregion
	}
}