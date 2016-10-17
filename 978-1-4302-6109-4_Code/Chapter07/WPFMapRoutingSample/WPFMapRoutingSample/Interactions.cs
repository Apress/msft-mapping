using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;

namespace WPFMapRoutingSample
{
    public class Interactions
    {
        #region RouteResult    
        public static readonly DependencyProperty RouteResultProperty = 
            DependencyProperty.RegisterAttached("RouteResult", 
            typeof(BingRouteService.RouteResult), 
            typeof(Interactions), 
            new UIPropertyMetadata(null, OnRouteResultChanged));

        public static BingRouteService.RouteResult GetRouteResult(DependencyObject target)
        {
            return (BingRouteService.RouteResult)target.GetValue(RouteResultProperty);
        }

        public static void SetRouteResult(DependencyObject target, BingRouteService.RouteResult value)
        {
            target.SetValue(RouteResultProperty, value);
        }

        private static void OnRouteResultChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnRouteResultChanged((Map)o, 
                                 (BingRouteService.RouteResult)e.OldValue, 
                                 (BingRouteService.RouteResult)e.NewValue);
        }

        private static void OnRouteResultChanged(Map map, 
            BingRouteService.RouteResult oldValue, 
            BingRouteService.RouteResult newValue)
        {
            MapPolyline routeLine = new MapPolyline();
            routeLine.Locations = new LocationCollection();
            routeLine.Opacity = 0.80;
            routeLine.Stroke = new SolidColorBrush(Colors.Magenta);
            routeLine.StrokeThickness = 5.0;

            foreach (BingRouteService.Location l in newValue.RoutePath.Points)
            {
                routeLine.Locations.Add(new Location(l.Latitude, l.Longitude));
            }

            var routeLineLayer = GetRouteLineLayer(map);
            if (routeLineLayer == null)
            {
                routeLineLayer = new MapLayer();
                 SetRouteLineLayer(map, routeLineLayer);
            }

            routeLineLayer.Children.Clear();
            routeLineLayer.Children.Add(routeLine);

            LocationRect rect = new LocationRect(routeLine.Locations[0], 
                routeLine.Locations[routeLine.Locations.Count - 1]);
            map.SetView(rect);
        }

        #endregion //RouteResult

        #region RouteLineLayer

        public static readonly DependencyProperty RouteLineLayerProperty = 
            DependencyProperty.RegisterAttached("RouteLineLayer", 
                typeof(MapLayer), 
                typeof(Interactions), 
                new UIPropertyMetadata(null, OnRouteLineLayerChanged));

        public static MapLayer GetRouteLineLayer(DependencyObject target)
        {
            return (MapLayer)target.GetValue(RouteLineLayerProperty);
        }

        public static void SetRouteLineLayer(DependencyObject target, MapLayer value)
        {
            target.SetValue(RouteLineLayerProperty, value);
        }

        private static void OnRouteLineLayerChanged(DependencyObject o, 
            DependencyPropertyChangedEventArgs e)
        {
            OnRouteLineLayerChanged((Map)o, (MapLayer)e.OldValue, (MapLayer)e.NewValue);
        }

        private static void OnRouteLineLayerChanged(Map map, MapLayer oldValue, MapLayer newValue)
        {
            if (!map.Children.Contains(newValue))
                 map.Children.Add(newValue);
        }

        #endregion //RouteLineLayer
    }
}
