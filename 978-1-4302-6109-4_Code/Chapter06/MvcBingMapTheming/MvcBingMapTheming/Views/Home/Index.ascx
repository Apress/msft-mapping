<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<MvcBingMapTheming.Models.GeoLocation>>" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
		<head>
		<title>Load map with navigation bar module</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
		<script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
		<script type="text/javascript">
		    var map = null;
		    function getMap() {
		        Microsoft.Maps.loadModule('Microsoft.Maps.Themes.BingTheme', {
		            callback: function () {
		                map = new Microsoft.Maps.Map(document.getElementById('myMap'),
                        {
                            credentials: 'At_LKBdN_d_G9Y6N53J-GtmMY8ZB-1iEc8hMlwoq6tlNldu-nkGkDPMnaye_a6XT',                      
                            theme: new Microsoft.Maps.Themes.BingTheme()
                        });
		                var pin1 = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(<%=Model[0].Location.Latitude%>,<%=Model[0].Location.Longitude%>), null);
		                map.entities.push(pin1);
		                map.entities.push(new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(<%=Model[0].Location.Latitude%>,<%=Model[0].Location.Longitude%>), { title: '<%=Model[0].LocationName%>', description: 'description here', pushpin: pin1 }));
		                var pin2 = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(<%=Model[1].Location.Latitude%>,<%=Model[1].Location.Longitude%>), null);
		                map.entities.push(pin2);
		                map.entities.push(new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(<%=Model[1].Location.Latitude%>,<%=Model[1].Location.Longitude%>), { title: '<%=Model[1].LocationName%>', description: 'description here', pushpin: pin2 }));
		                map.setView({ center: new Microsoft.Maps.Location(<%=Model[0].Location.Latitude%>,<%=Model[0].Location.Longitude%>), zoom: 9});
		            }
		        });
		    }
		</script>
		</head>
		<body onload="getMap();"> 
		<div id='myMap' style="position:relative; width:1000px; height:500px;"></div>
		</body>
</html>
				