namespace FsqAutogen.Models
{
    public static class KmlSchemaModel
    {
        public static string KmlHeader =    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                                    "<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n" + 
                                                        "<Document>\n" + 
                                                            "<name>4sq2autogen4gmaps_{0}.kml</name>\n" + 
                                                            "<Style id=\"icon-1899-097138-labelson-nodesc\">\n" + 
                                                                "<IconStyle>\n" + 
                                                                    "<color>ff387109</color>\n" + 
                                                                    "<scale>1</scale>\n" + 
                                                                    "<Icon>\n" + 
                                                                    "<href>https://www.gstatic.com/mapspro/images/stock/503-wht-blank_maps.png</href>\n" + 
                                                                    "</Icon>\n" + 
                                                                    "<hotSpot x=\"32\" xunits=\"pixels\" y=\"64\" yunits=\"insetPixels\"/>\n" + 
                                                                "</IconStyle>\n" + 
                                                                "<BalloonStyle>\n" + 
                                                                    "<text><![CDATA[<h3>$[name]</h3>]]></text>\n" + 
                                                                "</BalloonStyle>\n" + 
                                                            "</Style>\n";

            // {0} :    Venue.name
            // {1} :    Location.lat
            // {2} :    Location.lng
            //
            public static string KmlEntry =   "<Placemark>\n" +
                                                        "<name>{0}</name>\n" +
                                                        "<styleUrl>#icon-1899-097138-labelson-nodesc</styleUrl>\n" +
                                                        "<Point>\n" +
                                                            "<coordinates>\n" +
                                                            "{1},{2}\n" +
                                                            "</coordinates>\n" +
                                                        "</Point>\n" +
                                                    "</Placemark>\n";

            public static string KmlFooter =   "</Document>\n" + 
                                                        "</kml>\n";
    }
}