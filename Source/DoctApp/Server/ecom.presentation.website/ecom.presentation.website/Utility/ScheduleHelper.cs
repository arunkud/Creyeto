using ecom.presentation.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecom.presentation.website.Utility
{
    public static class ScheduleHelper
    {
        public static void UpdateScheduleTimeStampInfo(Doctor doctor, HttpRequestBase request)
        {
            doctor.SundayMorningStartTime = GetScheduleTimeStamp(request.Form["SundayMorningStartTime"]);
            doctor.SundayMorningEndTime = GetScheduleTimeStamp(request.Form["SundayMorningEndTime"]);
            doctor.SundayAfternoonStartTime = GetScheduleTimeStamp(request.Form["SundayAfternoonStartTime"]);
            doctor.SundayAfternoonEndTime = GetScheduleTimeStamp(request.Form["SundayAfternoonEndTime"]);
            doctor.SundayEveningStartTime = GetScheduleTimeStamp(request.Form["SundayEveningStartTime"]);
            doctor.SundayEveningEndTime = GetScheduleTimeStamp(request.Form["SundayEveningEndTime"]);

            doctor.MondayMorningStartTime = GetScheduleTimeStamp(request.Form["MondayMorningStartTime"]);
            doctor.MondayMorningEndTime = GetScheduleTimeStamp(request.Form["MondayMorningEndTime"]);
            doctor.MondayAfternoonStartTime = GetScheduleTimeStamp(request.Form["MondayAfternoonStartTime"]);
            doctor.MondayAfternoonEndTime = GetScheduleTimeStamp(request.Form["MondayAfternoonEndTime"]);
            doctor.MondayEveningStartTime = GetScheduleTimeStamp(request.Form["MondayEveningStartTime"]);
            doctor.MondayEveningEndTime = GetScheduleTimeStamp(request.Form["MondayEveningEndTime"]);

            doctor.TuesdayMorningStartTime = GetScheduleTimeStamp(request.Form["TuesdayMorningStartTime"]);
            doctor.TuesdayMorningEndTime = GetScheduleTimeStamp(request.Form["TuesdayMorningEndTime"]);
            doctor.TuesdayAfternoonStartTime = GetScheduleTimeStamp(request.Form["TuesdayAfternoonStartTime"]);
            doctor.TuesdayAfternoonEndTime = GetScheduleTimeStamp(request.Form["TuesdayAfternoonEndTime"]);
            doctor.TuesdayEveningStartTime = GetScheduleTimeStamp(request.Form["TuesdayEveningStartTime"]);
            doctor.TuesdayEveningEndTime = GetScheduleTimeStamp(request.Form["TuesdayEveningEndTime"]);

            doctor.WednesdayMorningStartTime = GetScheduleTimeStamp(request.Form["WednesdayMorningStartTime"]);
            doctor.WednesdayMorningEndTime = GetScheduleTimeStamp(request.Form["WednesdayMorningEndTime"]);
            doctor.WednesdayAfternoonStartTime = GetScheduleTimeStamp(request.Form["WednesdayAfternoonStartTime"]);
            doctor.WednesdayAfternoonEndTime = GetScheduleTimeStamp(request.Form["WednesdayAfternoonEndTime"]);
            doctor.WednesdayEveningStartTime = GetScheduleTimeStamp(request.Form["WednesdayEveningStartTime"]);
            doctor.WednesdayEveningEndTime = GetScheduleTimeStamp(request.Form["WednesdayEveningEndTime"]);

            doctor.ThursdayMorningStartTime = GetScheduleTimeStamp(request.Form["ThursdayMorningStartTime"]);
            doctor.ThursdayMorningEndTime = GetScheduleTimeStamp(request.Form["ThursdayMorningEndTime"]);
            doctor.ThursdayAfternoonStartTime = GetScheduleTimeStamp(request.Form["ThursdayAfternoonStartTime"]);
            doctor.ThursdayAfternoonEndTime = GetScheduleTimeStamp(request.Form["ThursdayAfternoonEndTime"]);
            doctor.ThursdayEveningStartTime = GetScheduleTimeStamp(request.Form["ThursdayEveningStartTime"]);
            doctor.ThursdayEveningEndTime = GetScheduleTimeStamp(request.Form["ThursdayEveningEndTime"]);

            doctor.FridayMorningStartTime = GetScheduleTimeStamp(request.Form["FridayMorningStartTime"]);
            doctor.FridayMorningEndTime = GetScheduleTimeStamp(request.Form["FridayMorningEndTime"]);
            doctor.FridayAfternoonStartTime = GetScheduleTimeStamp(request.Form["FridayAfternoonStartTime"]);
            doctor.FridayAfternoonEndTime = GetScheduleTimeStamp(request.Form["FridayAfternoonEndTime"]);
            doctor.FridayEveningStartTime = GetScheduleTimeStamp(request.Form["FridayEveningStartTime"]);
            doctor.FridayEveningEndTime = GetScheduleTimeStamp(request.Form["FridayEveningEndTime"]);

            doctor.SaturdayMorningStartTime = GetScheduleTimeStamp(request.Form["SaturdayMorningStartTime"]);
            doctor.SaturdayMorningEndTime = GetScheduleTimeStamp(request.Form["SaturdayMorningEndTime"]);
            doctor.SaturdayAfternoonStartTime = GetScheduleTimeStamp(request.Form["SaturdayAfternoonStartTime"]);
            doctor.SaturdayAfternoonEndTime = GetScheduleTimeStamp(request.Form["SaturdayAfternoonEndTime"]);
            doctor.SaturdayEveningStartTime = GetScheduleTimeStamp(request.Form["SaturdayEveningStartTime"]);
            doctor.SaturdayEveningEndTime = GetScheduleTimeStamp(request.Form["SaturdayEveningEndTime"]);
        }

        public static TimeSpan? GetScheduleTimeStamp(string requestParam)
        {
            if (string.IsNullOrEmpty(requestParam))
                return null;

            bool isPM = requestParam.ToUpper().EndsWith("PM");

            try
            {
                string time = requestParam.ToUpper().Replace("PM", "").Replace("AM", "").Trim();
                string[] timeVals = time.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (isPM)
                {
                    int hours = int.Parse(timeVals[0]);
                    if (hours != 12) hours += 12;
                    timeVals[0] = hours.ToString();
                }

                return TimeSpan.Parse(String.Join(":", timeVals));
            }
            catch
            {
                return null;
            }
        }

        internal static void GetScheduleTimeStamp(Doctor doctor)
        {
            if (doctor != null && doctor.DoctorSchedules != null)
            {
                foreach (var doctorShedule in doctor.DoctorSchedules)
                {
                    switch(doctorShedule.DayInWeek.ToUpper())
                    {
                        case "SUN":
                            doctor.SundayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.SundayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.SundayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.SundayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.SundayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.SundayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "MON":
                            doctor.MondayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.MondayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.MondayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.MondayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.MondayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.MondayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "TUE":
                            doctor.TuesdayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.TuesdayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.TuesdayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.TuesdayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.TuesdayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.TuesdayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "WED":
                            doctor.WednesdayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.WednesdayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.WednesdayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.WednesdayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.WednesdayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.WednesdayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "THU":
                            doctor.ThursdayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.ThursdayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.ThursdayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.ThursdayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.ThursdayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.ThursdayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "FRI":
                            doctor.FridayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.FridayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.FridayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.FridayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.FridayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.FridayEveningEndTime = doctorShedule.EveningEndTime;
                            break;

                        case "SAT":
                            doctor.SaturdayMorningStartTime = doctorShedule.MorningStartTime;
                            doctor.SaturdayMorningEndTime = doctorShedule.MorningEndTime;
                            doctor.SaturdayAfternoonStartTime = doctorShedule.AfternoonStartTime;
                            doctor.SaturdayAfternoonEndTime = doctorShedule.AfternoonEndTime;
                            doctor.SaturdayEveningStartTime = doctorShedule.EveningStartTime;
                            doctor.SaturdayEveningEndTime = doctorShedule.EveningEndTime;
                            break;                        
                    }
                }
            }
        }
    }
}