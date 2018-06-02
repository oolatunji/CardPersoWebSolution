using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Utility
{
    public class StatusUtil
    {
        public enum ApprovalStatus
        {
            Pending,
            Approved,
            Declined
        }

        public enum TableNames
        {
            SYSTEMAPPROVALS,
            SYSTEMAUDITTRAIL,
            CARDACCOUNTREQUESTS
        }

        public enum ApprovalType
        {
            [Description("User Login")]
            UserLogin,
            [Description("Print Utility User Login")]
            PrintUtilityUserLogin,
            [Description("Create Branch")]
            CreateBranch,
            [Description("Update Branch")]
            UpdateBranch,
            [Description("Create User")]
            CreateUser,
            [Description("Update User")]
            UpdateUser,
            [Description("Create Role")]
            CreateRole,
            [Description("Update Role")]
            UpdateRole,           
            [Description("Create Allowed IP")]
            CreateIP,
            [Description("Update Allowed IP")]
            UpdateIP,
            [Description("Delete Allowed IP")]
            DeleteIP,
            [Description("Reset Card Print Status")]
            ResetCardPrintStatus,
            [Description("Inserted Print Records")]
            InsertedPrintRecords
        }

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

    }
}
