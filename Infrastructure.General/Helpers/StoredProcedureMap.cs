using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.General.Implementation.Helpers
{
    public static class StoredProcedureMap
    {
        /// <summary>
        ///     Get Employee Details (i.e. first name, familiar name, last name) by Employee Code
        /// </summary>
        /// 
        public const string GetEmployee = "spGXCCMS_GetEmployeeDataTemp";

        public const string GetUserAccessInfo = "GetUserAccessInfo";

        public const string GetRegistrationsReport = "getRegistrationsReport";

        public static string GetInvestments = "getBainInvestments";

        public static string GetSeeNotes = "getSeeNotes";

        public static string GetRegistrationFrequencyInfo = "getRegistrationFrequencyInfo";

        public static string GetRegistrations = "getRegistrations";

        public static string GetAllWorkType = "getAllWorkType";

        public static string GetAllWorkToStart = "getAllWorkToStart";

        public static string GetPartnersByName = "getPartnersByName";

        public static string UpdateRegistration = "updateRegistration";

        public static string GetRegistrationStatus = "GetRegistrationStatus";

        public static string GetDashboard = "getDashboard";

        public static string GetNotes = "getNotes";

        public static string GetRegistrationStage = "GetRegistrationStage"; 
    }
}
