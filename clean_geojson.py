# take the geo.json data and convert it to something intelligible so it is
# easier to parse in C#
import json
countries = json.load(open("geo.json"))
output = {}
for o in countries['features']:
    i = o['id']
    name = o['properties']['ADMIN']
    abbr = o['properties']['ISO_A2']
    color = o['properties']['MAP_COLOR']

    # clean up the mess of geometry data
    geometry = o['geometry']
    if geometry['type'] == "Polygon":
        geometry['coordinates'] = [geometry['coordinates'][0]]
    else:
        geo = []
        for poly in  geometry['coordinates']:
            geo.append(poly[0])
        geometry['coordinates'] = geo

    output[i] = {
        "Name": name,
        "Abbr": abbr,
        "Id": i,
        "Geometry": geometry,
        "Color": int(color)
   }

json.dump(output, open('WorldMap/map.json', 'w'), indent=True)

