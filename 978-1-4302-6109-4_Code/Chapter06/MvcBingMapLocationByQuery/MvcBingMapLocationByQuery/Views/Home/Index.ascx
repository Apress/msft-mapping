<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<MvcBingMapLocationByQuery.Models.GeoLocation>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
   <head>
      <title>Find a location by query</title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
      <script type="text/javascript">
          var map = null;
          var query;
          function getMap() {
              map = new Microsoft.Maps.Map(document.getElementById('myMap'), { credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT' });            
          }

          function findLocation() {
              query = '<%=Model[0].City%>';
              map.getCredentials(callSearchService);
              query = '<%=Model[1].City%>';
              map.getCredentials(callSearchService);
              
          }

          function callSearchService(credentials) {
              var searchRequest = 'http://dev.virtualearth.net/REST/v1/Locations/' + query + '?output=json&jsonp=searchServiceCallback&key=' + credentials;
              var mapscript = document.createElement('script');
              mapscript.type = 'text/javascript';
              mapscript.src = searchRequest;
              document.getElementById('myMap').appendChild(mapscript)
          }

          function searchServiceCallback(result) {
              var output = document.getElementById("output");
              if (output) {
                  while (output.hasChildNodes()) {
                      output.removeChild(output.lastChild);
                  }
              }
              var resultsHeader = document.createElement("h5");
              output.appendChild(resultsHeader);

              if (result &&
              result.resourceSets &&
              result.resourceSets.length > 0 &&
              result.resourceSets[0].resources &&
              result.resourceSets[0].resources.length > 0) {
                  resultsHeader.innerHTML = "Bing Maps REST Search API  <br/>  Found location " + result.resourceSets[0].resources[0].name;
                  //var bbox = result.resourceSets[0].resources[0].bbox;
                  //var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(bbox[0], bbox[1]), new Microsoft.Maps.Location(bbox[2], bbox[3]));
                  map.setView({ center: new Microsoft.Maps.Location(<%=Model[1].Location.Latitude%>,<%=Model[1].Location.Longitude%>), zoom: 9 });
                  var location = new Microsoft.Maps.Location(result.resourceSets[0].resources[0].point.coordinates[0], result.resourceSets[0].resources[0].point.coordinates[1]);
                  var pushpin = new Microsoft.Maps.Pushpin(location);
                  map.entities.push(pushpin);
              }
              else {
                  if (typeof (response) == 'undefined' || response == null) {
                      alert("Invalid credentials or no response");
                  }
                  else {
                      if (typeof (response) != 'undefined' && response && result && result.errorDetails) {
                          resultsHeader.innerHTML = "Message :" + response.errorDetails[0];
                      }
                      alert("No results for the query");

                  }
              }
          }


      </script>
   </head>
   <body onload="getMap();">
      <div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
      <div>
         <input type="button" value="FindLocation" onclick="findLocation();" />
      </div>
      <div id="output"></div>
   </body>
</html>