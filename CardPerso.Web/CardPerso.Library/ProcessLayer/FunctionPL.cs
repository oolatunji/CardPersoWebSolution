using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ProcessLayer
{
    public class FunctionPL
    {
        public static Response Save(Function function, string username, bool overrideApproval)
        {
            try
            {
                if (FunctionDL.FunctionExists(function.Name))
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = string.Format("Function with name {0} already exists.", function.Name)
                    };
                }
                else
                {
                    if (FunctionDL.Save(function))
                    {
                        return new Response
                        {
                            SuccessMsg = "Function added successfully",
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

        public static Response Update(Function function, string username, bool overrideApproval)
        {
            try
            {
                if (FunctionDL.Update(function))
                {
                    return new Response
                    {
                        SuccessMsg = "Function updated successfully",
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

        public static Response RetrieveAll()
        {
            try
            {
                var functions = FunctionDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = functions }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Function>() }
                };
            }
        }
    }
}
