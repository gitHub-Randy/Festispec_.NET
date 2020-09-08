using System.Collections.Generic;
using Mapsui.Layers;

namespace FestiSpec.Maps.Interfaces
{
    public interface IMapsUI
    {
        ILayer CreateAdressLayerTms();
        ILayer CreateWmtsLayer();
        IEnumerable<ILayer> GetPdokWmtsLayers();
    }
}
