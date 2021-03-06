﻿using BruTile.Tms;
using BruTile.Wmts;
using Mapsui.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FestiSpec.Maps.Interfaces;

namespace FestiSpec.Maps
{
    public class MapsUI : IMapsUI
    {
        private readonly string _key;
        private readonly string _base = "https://services.geodan.nl/data/geodan/gws/nld";

        public MapsUI(string key)
        {
            _key = key;
        }

        public ILayer CreateAdressLayerTms()
        {
            var url = $"{_base}/addresses/tms/1.0.0/addressesbuildings/EPSG:28992?servicekey={_key}";
            var customp = new Dictionary<string, string> {{"servicekey", _key}};

            using (var httpClient = new HttpClient())
            using (var response = httpClient.GetStreamAsync(url).Result)
            {
                var tileSources = TileMapParser.CreateTileSource(response, null, customp);
                return new TileLayer(tileSources) { Name = "Adressen" };
            }
        }
        
        public ILayer CreateWmtsLayer()
        {
            using (var httpClient = new HttpClient())
            using (var response = httpClient.GetStreamAsync("http://geodata.nationaalgeoregister.nl/wmts?VERSION=1.0.0&request=GetCapabilities").Result)
            {
                var tileSources = WmtsParser.Parse(response).ToList();
                var natura2000 = tileSources.First(t => t.Name.ToLower().Contains("natura2000"));
                return new TileLayer(natura2000) { Name = "Natura2000" };
            }
        }

        public IEnumerable<ILayer> GetPdokWmtsLayers()
        {
            var result = new List<TileLayer>();
            const string url = "http://geodata.nationaalgeoregister.nl/wmts?VERSION=1.0.0&request=GetCapabilities";

            using (var httpClient = new HttpClient())
            using (var response = httpClient.GetStreamAsync(url).Result)
            {
                var tileSources = WmtsParser.Parse(response)
                    .Where(t => t.Schema.Format == "image/png" && t.Schema.Srs.Contains("3857"));

                result.AddRange(tileSources.Select(tileSource => new TileLayer(tileSource) {Name = tileSource.Name}));
                return result;
            }
        }
    }
}