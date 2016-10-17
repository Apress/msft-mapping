<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewPage<List<MvcEarthquakeMap.Models.Earthquake>>" %>

<!DOCTYPE html>

<html>
 <head>
      <title>Add default pushpin</title>
      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>

       <script type="text/javascript">
           var map = null;

           function getMap() {
               map = new Microsoft.Maps.Map(document.getElementById('myMap'), { credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT' });
           }


           function addPushpins() {      
               var offset = new Microsoft.Maps.Point(0, 5);               
               <% foreach (var item in Model)
                  {%>
               var pushpinOptions = { text: '<%=item.Magnitude%>', visible: true, textOffset: offset };
               var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(<%=item.Location.Latitude%>, <%=item.Location.Latitude%>), pushpinOptions);               
               map.entities.push(pushpin);
               <%}%>               
             
           }
       </script>

   </head>
<body onload="getMap();">
      <div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
      <div>
          <input type="button" value="AddPushpins" onclick="addPushpins();" />
      </div>
   </body>
</html>
