<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<MvcBingMapsModules.Models.GeoLocation>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
   <head>
      <title></title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>

      <script type="text/javascript">

          var map;

          function myModuleLoaded() {             
              var polygonModule = new PolygonModule(map);
              polygonModule.drawPolygon(new Microsoft.Maps.Location(37.788327, -122.408447),
                  new Microsoft.Maps.Location(37.788531, -122.406837),
                  new Microsoft.Maps.Location(37.787607, -122.406676),
                  new Microsoft.Maps.Location(37.787412, -122.408264));
              
              map.setView({zoom: 15, center: new Microsoft.Maps.Location(<%=Model[0].Location.Latitude%>,<%=Model[0].Location.Longitude%>) })              
          }

          function GetMap() {
              // Initialize the map
              var options = { credentials: "At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT" };
              map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), options);

              // Register and load the arrow module
              Microsoft.Maps.registerModule("PolygonModule", "https://dl.dropboxusercontent.com/u/26170114/polygonmodule.js");
              Microsoft.Maps.loadModule("PolygonModule", { callback: myModuleLoaded });

          }

      </script>
   </head>
   <body onload="GetMap();">
      <div id='mapDiv' style="position:relative; width:1000px; height:500px;"></div>       
   </body>
</html>
