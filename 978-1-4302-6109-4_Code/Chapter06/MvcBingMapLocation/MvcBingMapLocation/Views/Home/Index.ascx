<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
   <head>
      <title>Get location</title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
      <script type="text/javascript">
          var map = null;

          function getMap() {
              map = new Microsoft.Maps.Map(document.getElementById('myMap'), { credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT' });
          }

          function getCurrentLocation() {
              var geoLocationProvider = new Microsoft.Maps.GeoLocationProvider(map);
              geoLocationProvider.getCurrentPosition();
              //alert('Current location set, based on your browser support for geo location API');
          }
     </script>
   </head>
   <body onload="getMap();">
      <div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
      <div>
         <input type="button" value="GetCurrentLocation" onclick="getCurrentLocation();" />
      </div>
      <div id='output'> </div> 
   </body>
</html>

