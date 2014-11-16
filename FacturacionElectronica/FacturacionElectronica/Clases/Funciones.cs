using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionElectronica.Clases
{
    public class Funciones
    {
        public static string ObtenerFechaHora()
        {
            Uri url = new Uri("http://api.geonames.org/timezone?lat=19.432607&lng=-99.133207&username=icebeam");
            WebClient cliente = new WebClient();

            XDocument xml = XDocument.Parse(cliente.DownloadString(url));

            var consulta = from item in xml.Descendants("timezone")
                           select item.Element("time").Value;

            return consulta.First().Replace(" ", "T") + ":00";
        }

    }
}