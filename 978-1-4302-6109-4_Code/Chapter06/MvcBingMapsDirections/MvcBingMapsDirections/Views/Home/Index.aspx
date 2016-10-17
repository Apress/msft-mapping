<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<MvcBingMapsDirections.Models.GeoLocation>>" %>
       
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
   <head>
      <title>Create Driving Route</title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
      <script type="text/javascript">
          var map = null;
          var directionsManager;
          var directionsErrorEventObj;
          var directionsUpdatedEventObj;

          function getMap() {
              map = new Microsoft.Maps.Map(document.getElementById('myMap'), { credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT' });
          }

          function createDirectionsManager() {

              if (!directionsManager) 
              {
                  directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);
              }


              directionsManager.resetDirections();
              directionsErrorEventObj = Microsoft.Maps.Events.addHandler(directionsManager, 'directionsError' );
              directionsUpdatedEventObj = Microsoft.Maps.Events.addHandler(directionsManager, 'directionsUpdated');
          }

          function createDrivingRoute() {
              if (!directionsManager) { createDirectionsManager(); }
              directionsManager.resetDirections();
              // Set Route Mode to driving 
              directionsManager.setRequestOptions({ routeMode: Microsoft.Maps.Directions.RouteMode.transit });
              var Waypoint1 = new Microsoft.Maps.Directions.Waypoint({ address: '<%=Model[0].LocationName%>, <%=Model[0].City%>' });
              directionsManager.addWaypoint(Waypoint1);
              var Waypoint2 = new Microsoft.Maps.Directions.Waypoint({ location: new Microsoft.Maps.Location(<%=Model[1].Location.Latitude%>, <%=Model[1].Location.Longitude%>) });
              directionsManager.addWaypoint(Waypoint2);
              // Set the element in which the itinerary will be rendered
              directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('directionsItinerary') });
              directionsManager.calculateDirections();
          }

          function createDirections() {
              if (!directionsManager) {
                  Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: createDrivingRoute });
              }
              else {
                  createDrivingRoute();
              }
          }
      </script>
   </head>
 <body onload="getMap();">
      <div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
      <div>
         <input type="button" value="CreateDrivingRoute" onclick="createDirections();" />
      </div>
      <div id='directionsItinerary'> </div> 
   </body>
</html>
      
      