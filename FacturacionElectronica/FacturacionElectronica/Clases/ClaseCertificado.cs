using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace FacturacionElectronica.Clases
{
    public static class ClaseCertificado
    {
        private static string xslt = "http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_2/cadenaoriginal_3_2.xslt";

        public static string ObtenerCertificado(string certificado, out string noCertificado)
        {
            X509Certificate2 certEmisor = new X509Certificate2();
            byte[] byteCertData = LeerArchivo(certificado);
            certEmisor.Import(byteCertData);
            noCertificado = certEmisor.GetSerialNumberString();
            return Convert.ToBase64String(certEmisor.GetRawCertData());
        }

        public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);

            return encoding.GetString(bytes);
        }

        private static byte[] LeerArchivo(string certificado)
        {
            //return File.ReadAllBytes(certificado);
            return new WebClient().DownloadData(certificado);
        }

        public static string ObtenerCadenaOriginal(MemoryStream memoria)
        {
            StreamReader reader = new StreamReader(memoria);
            XPathDocument doc = new XPathDocument(reader);

            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(xslt);

            StringWriter str = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(str);

            trans.Transform(doc, null, writer);

            return str.ToString();
        }

        public static string Sellar(string strPathLlave, string strLlavePwd, string strCadenaOriginal)
        {
            try
            {
                string strSello = string.Empty;
                SecureString passwordSeguro = new System.Security.SecureString();
                passwordSeguro.Clear();
                foreach (char c in strLlavePwd.ToCharArray())
                    passwordSeguro.AppendChar(c);

                byte[] llavePrivadaBytes = System.IO.File.ReadAllBytes(strPathLlave);

                RSACryptoServiceProvider rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivadaBytes, passwordSeguro);

                if (rsa == null)
                {
                    byte[] bytes = new byte[llavePrivadaBytes.Length - 3];

                    for (int i = 3; i < llavePrivadaBytes.Length; i++)
                    {
                        bytes[i - 3] = llavePrivadaBytes[i];
                    }

                    llavePrivadaBytes = bytes;

                    rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivadaBytes, passwordSeguro);
                }

                SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider();
                byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(strCadenaOriginal), hasher);
                strSello = Convert.ToBase64String(bytesFirmados);

                return strSello;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}