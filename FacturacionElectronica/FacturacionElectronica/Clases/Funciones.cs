using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;//comunicación con el servicio web
using System.Xml.Linq;//procesar la respuesta del servicio web
using System.IO;//para escribir y descargar el archivo
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace FacturacionElectronica.Clases
{
    public static class Funciones
    {
        private static string usuario = "", password = "";



        public static string ObtenerFechaHora()
        {
            Uri url = new Uri("http://api.geonames.org/timezone?lat=19.432607&lng=-99.133207&username=icebeam");
            WebClient cliente = new WebClient();

            XDocument xml = XDocument.Parse(cliente.DownloadString(url));

            var consulta = from item in xml.Descendants("timezone")
                           select item.Element("time").Value;

            return consulta.First().Replace(" ", "T") + ":00";
        }



        private static bool validar(object sender, X509Certificate certificado, X509Chain cadena, SslPolicyErrors error)
        {
            return true;
        }

        private static byte[] DescargarArchivo(string archivo)
        {
            return File.ReadAllBytes(archivo);
        }

        
        public static MemoryStream Timbrar(string archivo)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(validar);
            
            byte[] xml = DescargarArchivo(archivo);

            int i = 0;
            while (xml[i] != 60)
                i++;

            xml = xml.Skip(i).ToArray();

            wsSIFAC.CFDiClient pac = new wsSIFAC.CFDiClient();
            byte[] xmlTimbrado = pac.getCfdixml(usuario, password, xml);

            MemoryStream memoria = new MemoryStream(xmlTimbrado);
            memoria.Flush();
            memoria.Position = 0;

            return memoria;
        }

    }
}