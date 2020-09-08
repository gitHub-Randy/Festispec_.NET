using BruTile.Predefined;
using Mapsui.Geometries;
using Mapsui.Geometries.WellKnownText;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FestiSpec.Maps.Interfaces;
using FestiSpec.Entity;
using FestiSpec.Entities.Dal;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;

namespace FestiSpec.Maps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MapsView : Window
    {
        private Festival _festival;
        private readonly IGeodan _geodan;
        private Layer _lastpoint;
        public ObservableCollection<RouteData> Routes { get; set; }
        private RouteData _selectedroute;
        public RouteData SelectedRoute
        {
            get { return _selectedroute; }
            set
            {
                _selectedroute = value;
                UpdateRouteColors();
            }
        }

        private readonly string _servicekey = "6c4c63db-de9a-11e8-8aac-005056805b87";

        public MapsView()
        {
            IMapsUI mapsUi = new MapsUI(_servicekey);
            _geodan = new Geodan(_servicekey);
            _festival = DBContext.Instance.Festivals.ToList()[0];
            Routes = new ObservableCollection<RouteData>();
            InitializeComponent();

            MapControl.Map.Layers.Add(mapsUi.CreateWmtsLayer());
            MapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create()) { Name = "OpenStreetMap" });

            // Start view location
            bool success = GoToLocation($"{_festival.City} {_festival.Street} {_festival.HouseNumber}", true);
        }

        public MapsView(Festival festival, string location, out bool success)
        {
            IMapsUI mapsUi = new MapsUI(_servicekey);
            _geodan = new Geodan(_servicekey);
            _festival = festival;
            Routes = new ObservableCollection<RouteData>();
            InitializeComponent();

            MapControl.Map.Layers.Add(mapsUi.CreateWmtsLayer());
            MapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create()) { Name = "OpenStreetMap" });

            // Start view location
            success = GoToLocation(location, true);
        }

        /// <summary>
        /// Removes characters that are not allowd in ComboBoxItem.Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>More illegal characters should be added when needed.</returns>
        private string RemoveIllegalCharacters(string name)
        {
            return name.Replace("_", "");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoToLocation(Query.Text, false);
        }

        private bool GoToLocation(string location, bool addpoint)
        {
            var geo = _geodan.FindAdress(location);

            if (!geo.Equals(""))
            {
                var geometry = GeometryFromWKT.Parse(geo);

                var centroid = geometry.GetBoundingBox().GetCentroid();
                var sphericalCentroid = SphericalMercator.FromLonLat(centroid.X, centroid.Y);

                if (addpoint)
                {
                    if (_lastpoint != null)
                        MapControl.Map.Layers.Remove(_lastpoint);

                    _lastpoint = new Layer
                    {
                        DataSource = new MemoryProvider(sphericalCentroid),

                        Style = new VectorStyle
                        {
                            Fill = new Brush(new Color(150, 150, 30, 128)),
                            Outline = new Pen(Color.Red, 2),
                        }
                    };

                    // Add point
                    MapControl.Map.Layers.Add(_lastpoint);
                }

                MapControl.Map.NavigateTo(sphericalCentroid);
                MapControl.Map.NavigateTo(MapControl.Map.Resolutions[17]);

                ShowRoutes();
                return true;
            }

            return false;
        }

        public void ShowRoutes()
        {
            Routes.Clear();

            List<QuestionList> questionLists = DBContext.Instance.QuestionLists.Where(x => x.Festival.Id == _festival.Id).ToList();
            List<Inspection> inspections = new List<Inspection>();
            foreach(QuestionList list in questionLists)
            {
                List<Inspection> listInspections = DBContext.Instance.Inspections.Where(x => x.QuestionList.Id == list.Id).ToList();
                foreach(Inspection add in listInspections)
                {
                    inspections.Add(add);
                }
            }

            foreach (Inspection inspection in inspections)
            {
                Employee employee = inspection.Employee;
                try
                {
                    var responseRoute = _geodan.FindRoute($"&from={employee.City}%20{employee.Street}%20{employee.HouseNumber}&to={_festival.City}%20{_festival.Street}%20{_festival.HouseNumber}&srs=epsg:28992&returntype=coords&outputformat=json");
                    var geometry = responseRoute.features[0].geometry;
                    string code = $"{geometry.type} (";
                    foreach (var coord in geometry.coordinates)
                    {
                        var i = coord[0];
                        var sm = SphericalMercator.FromLonLat((float)coord[0], (float)coord[1]);
                        code += $"{sm.X} {sm.Y}".Replace(",", ".") + ", ";
                    }
                    code = code.Remove(code.Length - 2, 2) + ")";
                    var geoRoute = GeometryFromWKT.Parse(code);

                    var routeLayer = new Layer
                    {
                        DataSource = new MemoryProvider(geoRoute),

                        Style = new VectorStyle
                        {
                            Line = new Pen(Color.Black, 3),
                        }
                    };

                    MapControl.Map.Layers.Add(routeLayer);
                    Routes.Add(new RouteData(routeLayer, employee, (double)responseRoute.features[0].properties.route_distance, (double)responseRoute.features[0].properties.route_duration));
                }
                catch (NoRouteException)
                {
                    //No valid route was found
                    MessageBox.Show($"Geen route gevonden voor {employee.Name},\nControleer het adres van het festival en de medewerker.", "Geen route gevonden");
                    Routes.Add(new RouteData(null, employee, 999.9, 59940));
                }
            }
            UpdateRouteColors();
        }

        public void UpdateRouteColors()
        {
            foreach (RouteData route in Routes)
            {
                Layer layer = route.MapLayer;
                if (layer == null) continue;
                if (route == _selectedroute)
                {
                    layer.Style = new VectorStyle
                    {
                        Line = new Pen(Color.Red, 3),
                    };
                }
                else
                {
                    layer.Style = new VectorStyle
                    {
                        Line = new Pen(new Color(150, 150, 30, 128), 3),
                    };
                }
            }
            MapControl.Refresh();
        }
    }
}
