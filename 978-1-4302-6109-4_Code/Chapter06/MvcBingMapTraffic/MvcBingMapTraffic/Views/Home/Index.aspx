<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<MvcBingMapTraffic.Models.GeoLocation>>" %>

<!DOCTYPE html>

<html>
   <head>
      <title>Add/Show Traffic Layer</title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
      <script type="text/javascript">
          var map = null;
          function trafficModuleLoaded() {
              setMapView();
          }
          function loadTrafficModule() {
              Microsoft.Maps.loadModule('Microsoft.Maps.Traffic', { callback: trafficModuleLoaded });
          }
          function setMapView() {
              map.setView({ zoom: 10, center: new Microsoft.Maps.Location(<%=Model[0].Location.Latitude%>,<%=Model[0].Location.Longitude%>) })
          }
          function getMap() {
              map = new Microsoft.Maps.Map(document.getElementById('myMap'), { credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT' });
              loadTrafficModule();
          }
          function showTrafficLayer() {

              var trafficLayer = new Microsoft.Maps.Traffic.TrafficLayer(map);
              // show the traffic Layer
              trafficLayer.show();
          }
      </script>
   </head>
 <body onload="getMap();">
      <div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
      <div>
         <input type="button" value="ShowTrafficLayer" onclick="showTrafficLayer();" />
      </div>
      <div id='output'> </div> 
   </body>
</html>