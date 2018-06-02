using System.Web;
using System.Web.Optimization;

namespace CardPerso.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/common_libraries").Include(
                      "~/Scripts/lib/jquery.js",
                      "~/Scripts/lib/jquery.datetimepicker.full.js",
                      "~/Scripts/lib/lodash.min.js",
                      "~/Scripts/lib/jquery-ui.min.js",
                      "~/Scripts/lib/bootstrap.min.js",
                      "~/Scripts/lib/jquery.slimscroll.min.js",
                      "~/Scripts/lib/jquery.dataTables.min.js",
                      "~/Scripts/lib/dataTables.tableTools.js",
                      "~/Scripts/lib/toastr.min.js",
                      "~/Scripts/lib/nav.js",
                      "~/Scripts/lib/jstree.min.js",
                      "~/Scripts/app/Utility/configFile.js",
                      "~/Scripts/app/Utility/messageBox.js",
                      "~/Scripts/app/Utility/SystemConfiguration.js",
                      "~/Scripts/lib/custom.js",
                      "~/Scripts/app/Login/Login.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/configureSystem").Include(
                     "~/Scripts/app/Utility/ConfigureSystem.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                   "~/Scripts/app/Login/Layout.js",
                   "~/Scripts/app/DynamicMenu/Layout.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                    "~/Scripts/app/DynamicMenu/Dashboard.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/addfunctions").Include(
                      "~/Scripts/app/Function/AddFunction.js"                    
                      ));            

            bundles.Add(new ScriptBundle("~/bundles/viewfunctions").Include(
                      "~/Scripts/app/Function/ViewFunction.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addroles").Include(
                      "~/Scripts/app/Role/AddRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewroles").Include(
                      "~/Scripts/app/Role/ViewRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addbranches").Include(
                     "~/Scripts/app/Branch/AddBranch.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/viewbranches").Include(
                      "~/Scripts/app/Branch/ViewBranch.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addusers").Include(
                     "~/Scripts/app/User/AddUser.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/viewusers").Include(
                      "~/Scripts/app/User/ViewUser.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/changepassword").Include(
                    "~/Scripts/app/Password/ChangePassword.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/forgotpassword").Include(
                    "~/Scripts/app/Password/ForgotPassword.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/resetpassword").Include(
                    "~/Scripts/app/Password/PasswordReset.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/retrievecards").Include(
                    "~/Scripts/app/Card/RetrieveCards.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/approvalconfiguration").Include(
                      "~/Scripts/app/Approval/Configuration.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/approvallist").Include(
                      "~/Scripts/app/Approval/ApprovalList.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewapproval").Include(
                      "~/Scripts/app/Approval/ViewApproval.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/audittrail").Include(
                      "~/Scripts/app/Utility/AuditTrail.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addips").Include(
                    "~/Scripts/app/IP/AddIP.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/viewips").Include(
                      "~/Scripts/app/IP/ViewIP.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/jquery.datetimepicker.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/themes/default/style.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/animate.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/owl.transitions.css",
                       "~/Content/jquery.dataTables.min.css",
                      "~/Content/dataTables.tableTools.css",
                      "~/Content/clndr.css",
                      "~/Content/toastr.min.css",
                      "~/Content/style.css",
                      "~/Content/Wobblebar.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
