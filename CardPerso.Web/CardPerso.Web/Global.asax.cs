using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using CardPerso.Web.Models;
using CardPerso.Library.ModelLayer.Model;

namespace CardPerso.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.CreateMap<FunctionModel, Function>();
            Mapper.CreateMap<FuncModel, Function>();
            Mapper.CreateMap<RoleModel, Role>();
            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<PasswordModel, User>();
            Mapper.CreateMap<CardModel, Card>();
            Mapper.CreateMap<ApprovalModel, Approval>();
            Mapper.CreateMap<SearchFilterModel, SearhFilter>();
        }
    }
}
