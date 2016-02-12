using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWithOracleTest
{
    public class RoleFunctions
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
    }

    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PageLink { get; set; }

        public static List<Function> DefaultFunctions()
        {
            var functions = new List<Function>();

            var function = new Function()
            {
                Name = "Manage System Functions",
                PageLink = "/Function/AddFunction"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "View System Functions",
                PageLink = "/Function/ViewFunction"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Manage System Roles",
                PageLink = "/Role/AddRole"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "View System Roles",
                PageLink = "/Role/ViewRole"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Manage System Users",
                PageLink = "/User/AddUser"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "View System Users",
                PageLink = "/User/ViewUser"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Reset Card Print Status",
                PageLink = "/Card/RetrieveCards"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Manage Approval Configuration",
                PageLink = "/Approval/Configuration"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Approve Request",
                PageLink = "/Approval/ApprovalList"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "View System Audit Trail",
                PageLink = "/System/AuditTrail"
            };
            functions.Add(function);

            function = new Function()
            {
                Name = "Manage System Configuration",
                PageLink = "/System/SystemConfiguration"
            };
            functions.Add(function);

            return functions;
        }
    }
}
