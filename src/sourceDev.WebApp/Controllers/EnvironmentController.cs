using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sourceDev.WebApp.Controllers
{
    public class EnvironmentController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public EnvironmentController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        public IActionResult Routes()
        {
            //var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(x => new {
            //    Action = x.RouteValues["Action"],
            //    Controller = x.RouteValues["Controller"],
            //    Name = x.AttributeRouteInfo.Name,
            //    Template = x.AttributeRouteInfo.Template
            //}).ToList();

            var items = _actionDescriptorCollectionProvider.ActionDescriptors.Items;


            return Ok();
        }

        //[HttpGet("routes", Name = "ApiEnvironmentGetAllRoutes")]
        //[Produces(typeof(ListResult<RouteModel>))]
        //public IActionResult GetAllRoutes()
        //{

        //    var result = new ListResult<RouteModel>();
        //    var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
        //        ad => ad.AttributeRouteInfo != null).Select(ad => new RouteModel
        //        {
        //            Name = ad.AttributeRouteInfo.Name,
        //            Template = ad.AttributeRouteInfo.Template
        //        }).ToList();
        //    if (routes != null && routes.Any())
        //    {
        //        result.Items = routes;
        //        result.Success = true;
        //    }
        //    return Ok(result);
        //}
    }
}
