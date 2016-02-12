using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ProcessLayer
{
    public class ApprovalConfigurationPL
    {
        public static Response RetrieveAll()
        {
            try
            {
                var confs = ApprovalConfigurationDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = confs }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<ApprovalConfiguration>() }
                };
            }
        }

        public static Response RetrieveByType(string type)
        {
            try
            {
                var conf = ApprovalConfigurationDL.RetrieveByType(type);
                return new Response
                {
                    DynamicList = new { data = conf }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new ApprovalConfiguration() }
                };
            }
        }

        public static Response Update(List<ApprovalConfiguration> confs)
        {
            try
            {
                if (ApprovalConfigurationDL.Update(confs))
                {
                    return new Response
                    {
                        SuccessMsg = "Approval configuration updated successfully",
                        ErrorMsg = string.Empty
                    };
                }
                else
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = "Operation failed"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message
                };
            }
        }
    }
}
