using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Maps.MapControl.WPF;
using MvcBingMapTheming.Models;

namespace MvcBingMapTheming.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Theme/

        public ActionResult Index()
        {
            var locations = GetLocations();
            return View(locations);
        }

        public List<GeoLocation> GetLocations()
        {
            var locations = new List<GeoLocation>();
            var loc1 = new Location(37.788302, -122.408513);
            var geoLoc1 = new GeoLocation("Union Square", "San Francisco", loc1);
            var loc2 = new Location(37.436703, -122.160273);
            var geoLoc2 = new GeoLocation("Stanford University", "Palo Alto", loc2);
            locations.Add(geoLoc1);
            locations.Add(geoLoc2);

            return locations;
        }

    }
}
