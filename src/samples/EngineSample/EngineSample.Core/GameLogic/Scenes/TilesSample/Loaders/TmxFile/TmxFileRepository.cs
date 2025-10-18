using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample.Loaders.TmxFile;

/// <summary>
///     Repositorio para carga de un archivo TMX
/// </summary>
public class TmxFileRepository
{
    /// <summary>
    ///     Carga los datos de un mapa
    /// </summary>
    public MapModel? Load(string xml)
    {
        MapModel? map = null;
        MLFile fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

            // Carga los datos del nodo
            if (fileML is not null)
                foreach (MLNode rootML in fileML.Nodes)
                    if (rootML.Name == "map")
                    {
                        // Crea el objeto del mapa
                        map = new MapModel()
                                    {
                                        Orientation = rootML.Attributes["orientation"].Value.TrimIgnoreNull(),
                                        Width = rootML.Attributes["width"].Value.GetInt(0),
                                        Height = rootML.Attributes["height"].Value.GetInt(0),
                                        TileWidth = rootML.Attributes["tilewidth"].Value.GetInt(0),
                                        TileHeight = rootML.Attributes["tileheight"].Value.GetInt(0)
                                    };
                        // Carga las propiedades
                        map.Properties.AddRange(LoadProperties(rootML));
                        // Carga el resto de objetos
                        foreach (MLNode nodeML in rootML.Nodes)
                            switch (nodeML.Name)
                            {
                                case "tileset":
                                        map.Tilesets.Add(LoadTileset(nodeML));
                                    break;
                                case "layer":
                                        map.Layers.Add(LoadLayer(nodeML));
                                    break;
                                case "objectgroup":
                                        map.ObjectGroups.Add(LoadObjectGroup(nodeML));
                                    break;
                            }
                    }
            // Devuelve el mapa
            return map;
    }

    /// <summary>
    ///     Carga los datos de un tileset
    /// </summary>
    private MapTilesetModel LoadTileset(MLNode rootML)
    {
        MapTilesetModel tileset = new()
                                    {
                                        FirstGid = rootML.Attributes["firstgid"].Value.GetInt(0),
                                        Name = rootML.Attributes["name"].Value.TrimIgnoreNull(),
                                        TileWidth = rootML.Attributes["tilewidth"].Value.GetInt(0),
                                        TileHeight = rootML.Attributes["tileheight"].Value.GetInt(0),
                                        ImageSource = rootML.Attributes["source"].Value.TrimIgnoreNull(),
                                        ImageWidth = rootML.Attributes["width"].Value.GetInt(0),
                                        ImageHeight = rootML.Attributes["height"].Value.GetInt(0)
                                    };

            // Devuelve el tileset (no está cargando de archivos externos)
            return tileset;
    }

    /// <summary>
    ///     Carga los datos de una capa
    /// </summary>
    private MapLayerModel LoadLayer(MLNode rootML)
    {
        MapLayerModel layer = new()
                                {
                                    Name = rootML.Attributes["name"].Value.TrimIgnoreNull(),
                                    Width = int.Parse(rootML.Attributes["width"].Value),
                                    Height = int.Parse(rootML.Attributes["height"].Value)
                                };

            // Carga el contenido
            layer.Tiles.AddRange(LoadLayerData(rootML.Nodes["data"]));
            layer.Properties.AddRange(LoadProperties(rootML));
            // Devuelve la capa
            return layer;
    }

    /// <summary>
    ///     Carga los datos de la capa
    /// </summary>
    private List<int> LoadLayerData(MLNode rootML)
    {
        List<int> tileIds = [];
        string encoding = rootML.Attributes["encoding"].Value.TrimIgnoreNull();

            // Si está codificado como CSV ...
            if (!string.IsNullOrWhiteSpace(encoding) && encoding.Equals("csv", StringComparison.CurrentCultureIgnoreCase))
            {
                string csvData = rootML.Value.TrimIgnoreNull();
                
                    // Si hay datos en la cadena del CSV
                    if (!string.IsNullOrWhiteSpace(csvData))
                    {
                        // Quita los saltos de línea
                        csvData = csvData.Replace('\n', ' ').Replace('\r', ' ');
                        // Añade los Ids
                        foreach (string data in csvData.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                            tileIds.Add(data.GetInt(0));
                    }
            }
            // Devuelve la lista de Id
            return tileIds;
    }

    /// <summary>
    ///     Carga la información de un grupo de objetos
    /// </summary>
    private MapObjectGroupModel LoadObjectGroup(MLNode rootML)
    {
        MapObjectGroupModel group = new()
                                        {
                                            Name = rootML.Attributes["name"].Value.TrimIgnoreNull()
                                        };

            // Carga las propiedades
            group.Properties.AddRange(LoadProperties(rootML));
            // Carga los objetos
            foreach (MLNode nodeML in rootML.Nodes)
                if (nodeML.Name == "object")
                {
                    MapObjectModel mapObject = new()
                                                {
                                                    Id = nodeML.Attributes["id"].Value.GetInt(0),
                                                    Name = nodeML.Attributes["name"].Value.TrimIgnoreNull(),
                                                    X = (float) nodeML.Attributes["x"].Value.GetDouble(0),
                                                    Y = (float) nodeML.Attributes["y"].Value.GetDouble(0),
                                                    Width = (float) nodeML.Attributes["width"].Value.GetDouble(0),
                                                    Height = (float) nodeML.Attributes["height"].Value.GetDouble(0)
                                                };

                        // Carga las proiedades
                        mapObject.Properties.AddRange(LoadProperties(nodeML));
                        // Carga los datos de polígono
                        foreach (MLNode childML in nodeML.Nodes)
                            if (childML.Name == "polygon")
                            {
                                string pointsStr = childML.Attributes["points"].Value.TrimIgnoreNull();

                                    if (!string.IsNullOrWhiteSpace(pointsStr))
                                    foreach (string pair in pointsStr.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                                        if (!string.IsNullOrWhiteSpace(pair))
                                        {
                                            string[] coords = pair.Split(',');

                                                if (coords.Length == 2)
                                                    mapObject.PolygonPoints.Add(new Vector2((float) coords[0].GetDouble(0), 
                                                                                            (float) coords[0].GetDouble(0))
                                                                               );
                                        }
                            }
                        // Añade el objeto al grupo
                        group.Objects.Add(mapObject);
                }
            // Devuelve el grupo leido
            return group;
    }

    /// <summary>
    ///     Carga las propiedades
    /// </summary>
    private List<MapPropertyModel> LoadProperties(MLNode rootML)
    {
        List<MapPropertyModel> properties = [];

            // Carga las propiedades
            foreach (MLNode nodeML in rootML.Nodes)
                if (nodeML.Name == "properties")
                    foreach (MLNode propertyML in nodeML.Nodes)
                        if (propertyML.Name == "property")
                            properties.Add(new MapPropertyModel
                                                    {
                                                        Name = propertyML.Attributes["name"].Value.TrimIgnoreNull(),
                                                        Value = propertyML.Attributes["value"].Value.TrimIgnoreNull(),
                                                        Type = propertyML.Attributes["type"].Value.TrimIgnoreNull()
                                                    }
                                           );
            // Devuelve la lista de propiedades
            return properties;
    }
}
