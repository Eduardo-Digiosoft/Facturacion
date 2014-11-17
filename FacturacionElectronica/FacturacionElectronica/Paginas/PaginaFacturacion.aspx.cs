using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

using System.Text;
using System.IO;
using FacturacionElectronica.Clases;
using System.Net;
using System.Data;

using System.Net.Security;
//using Timbrado = FacturacionElectronica.wbsSIFAC;

using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing.Imaging;


namespace FacturacionElectronica.Paginas
{
    public partial class PaginaFacturacion : System.Web.UI.Page
    {
        // Para la operación de la página
        DataTable dtConceptos;

        // Para la generación del XML
        string carpetaFacturas = @"C:\Facturacion\Facturas\";
        string prefactura = "";
        string carpetaCertificados = @"C:\Facturacion\Certificados\";
        //string certificado = "AAA010101AAA_CER.cer";
        //string llave = "AAA010101AAA_KEY.key";
        //string pass = "12345678a";
        string certificado = "certificado.cer";
        string llave = "llave.key";
        string pass = "";
        string factura = "";


        #region Conceptos y Total

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreardtConceptos();
            }
            else
            {
                dtConceptos = (DataTable)ViewState["dtConceptos"];
                MostrarTabla();                
            }
        }

        private void CreardtConceptos()
        {
            dtConceptos = new DataTable();
            dtConceptos.Columns.Add(new DataColumn() { ColumnName = "cantidad" });
            dtConceptos.Columns.Add(new DataColumn() { ColumnName = "unidad" });
            dtConceptos.Columns.Add(new DataColumn() { ColumnName = "descripcion" });
            dtConceptos.Columns.Add(new DataColumn() { ColumnName = "valorunitario" });
            dtConceptos.Columns.Add(new DataColumn() { ColumnName = "importe" });

            ViewState["dtConceptos"] = dtConceptos;
        }

        void MostrarTabla()
        {
            int i = 0;
            decimal subtotal = 0;

            foreach (DataRow r in dtConceptos.Rows)
            {
                TableRow tr = new TableRow();

                int nr = tblConceptos.Rows.Count - 1;

                for (int j = 0; j < dtConceptos.Columns.Count; j++)
                {
                    TableCell tc = new TableCell();
                    Label tb = new Label();

                    switch (j)
                    {
                        case 0:
                            tc.HorizontalAlign = HorizontalAlign.Center;
                            tb.ID = "txtCantidad" + i;
                            tb.Text = dtConceptos.Rows[i]["cantidad"].ToString();
                            break;
                        case 1:
                            tc.HorizontalAlign = HorizontalAlign.Center;
                            tb.ID = "txtUnidad" + i;
                            tb.Text = dtConceptos.Rows[i]["unidad"].ToString();
                            break;
                        case 2:
                            tb.ID = "txtDescripcion" + i;
                            tb.Text = dtConceptos.Rows[i]["descripcion"].ToString();
                            break;
                        case 3:
                            tc.HorizontalAlign = HorizontalAlign.Right;
                            tb.ID = "txtValorUnitario" + i;
                            tb.Text = dtConceptos.Rows[i]["valorunitario"].ToString();
                            break;
                        case 4:
                            tc.HorizontalAlign = HorizontalAlign.Right;
                            tb.ID = "txtImporte" + i;
                            tb.Text = dtConceptos.Rows[i]["importe"].ToString();
                            subtotal += decimal.Parse(tb.Text); ;
                            break;
                    }

                    tc.Controls.Add(tb);
                    tr.Cells.Add(tc);

                    if (j == dtConceptos.Columns.Count - 1)
                    {
                        TableCell tc2 = new TableCell();
                        
                        Button btn = new Button();
                        btn.ID = "btnEliminarConcepto" + i;
                        btn.Text = "Eliminar";
                        btn.Click += btn_Click;
                        tc2.Controls.Add(btn);
                        tr.Cells.Add(tc2);
                    }
                }

                tblConceptos.Rows.Add(tr);
                i++;
            }

            txtSubtotal.Text = subtotal.ToString();
            CalcularTotal();
        }

        void CalcularTotal()
        {
            decimal retenciones = 0, traslados = 0, subtotal, total;

            subtotal = decimal.Parse(txtSubtotal.Text);

            if (chbRetenciones.Checked)
            {
                if (chbRetencionISR.Checked)
                    retenciones += decimal.Parse(txtImporteRetencionISR.Text);

                if (chbRetencionIVA.Checked)
                    retenciones += decimal.Parse(txtImporteRetencionIVA.Text);
            }

            if (chbTraslados.Checked)
            {
                if (chbTrasladoIVA.Checked)
                    traslados += decimal.Parse(txtImporteTrasladoIVA.Text);

                if (chbTrasladoIEPS.Checked)
                    traslados += decimal.Parse(txtImporteTrasladoIEPS.Text);
            }

            total = subtotal + traslados - retenciones;

            txtTotal.Text = total.ToString();
        }

        protected void btnAgregarConcepto_Click(object sender, EventArgs e)
        {
            AgregarConcepto_dtConceptos();
            AgregarConcepto_tblConceptos();
            CalcularTotal();
            Limpiar();
        }

        void AgregarConcepto_dtConceptos()
        {
            decimal cantidad = decimal.Parse(txtCantidad.Text);
            decimal valorUnitario = decimal.Parse(txtValorUnitario.Text);

            DataRow renglon = dtConceptos.NewRow();
            renglon["cantidad"] = txtCantidad.Text;
            renglon["unidad"] = txtUnidad.Text;
            renglon["descripcion"] = txtDescripcion.Text;
            renglon["valorunitario"] = txtValorUnitario.Text;
            renglon["importe"] = (cantidad * valorUnitario).ToString();

            dtConceptos.Rows.Add(renglon);
            ViewState["dtConceptos"] = dtConceptos;
        }

        void AgregarConcepto_tblConceptos()
        {
            TableRow tr = new TableRow();

            int i = dtConceptos.Rows.Count - 1;
            decimal subtotal = decimal.Parse(txtSubtotal.Text);

            for (int j = 0; j < dtConceptos.Columns.Count; j++)
            {
                TableCell tc = new TableCell();
                Label tb = new Label();

                switch (j)
                {
                    case 0:
                        tc.HorizontalAlign = HorizontalAlign.Center;
                        tb.ID = "txtCantidad" + i;
                        tb.Text = dtConceptos.Rows[i]["cantidad"].ToString();
                        break;
                    case 1:
                        tc.HorizontalAlign = HorizontalAlign.Center;
                        tb.ID = "txtUnidad" + i;
                        tb.Text = dtConceptos.Rows[i]["unidad"].ToString();
                        break;
                    case 2:
                        tb.ID = "txtDescripcion" + i;
                        tb.Text = dtConceptos.Rows[i]["descripcion"].ToString();
                        break;
                    case 3:
                        tc.HorizontalAlign = HorizontalAlign.Right;
                        tb.ID = "txtValorUnitario" + i;
                        tb.Text = dtConceptos.Rows[i]["valorunitario"].ToString();
                        break;
                    case 4:
                        tc.HorizontalAlign = HorizontalAlign.Right;
                        tb.ID = "txtImporte" + i;
                        tb.Text = dtConceptos.Rows[i]["importe"].ToString();
                        subtotal += decimal.Parse(dtConceptos.Rows[i]["importe"].ToString());
                        break;
                }

                tc.Controls.Add(tb);
                tr.Cells.Add(tc);

                if (j == dtConceptos.Columns.Count - 1)
                {
                    TableCell tc2 = new TableCell();

                    Button btn = new Button();
                    btn.ID = "btnEliminarConcepto" + (i + 2);
                    btn.Text = "Eliminar";
                    btn.Click += new EventHandler(btn_Click);
                    tc2.Controls.Add(btn);
                    tr.Cells.Add(tc2);
                }
            }

            tblConceptos.Rows.Add(tr);
            txtSubtotal.Text = subtotal.ToString();
        }

        void Limpiar()
        {
            txtCantidad.Text = "1";
            txtUnidad.Text = "NO APLICA";
            txtDescripcion.Text = "";
            txtValorUnitario.Text = "0";
            txtCantidad.Focus();
        }

        void btn_Click(object sender, EventArgs e)
        {
            string name = ((Button)sender).ID;
            int id = int.Parse(name.Substring(19));

            dtConceptos.Rows.RemoveAt(id);
            ViewState["dtConceptos"] = dtConceptos;

            EliminarRenglones();
            MostrarTabla();
        }

        void EliminarRenglones()
        {
            int num = tblConceptos.Rows.Count - 1;

            for (int i = num; i > 1; i--)
            {
                tblConceptos.Rows.Remove(tblConceptos.Rows[i]);
            }
        }

        protected void btnActualizarTotal_Click(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        protected void chbTrasladoIVA_CheckedChanged(object sender, EventArgs e)
        {
            chbTraslados.Checked = chbTrasladoIVA.Checked || chbTrasladoIEPS.Checked;
            CalcularTotal();
        }

        protected void chbRetencionIVA_CheckedChanged(object sender, EventArgs e)
        {
            chbRetenciones.Checked = chbRetencionIVA.Checked || chbRetencionISR.Checked;
            CalcularTotal();
        }

        protected void chbTraslados_CheckedChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        protected void chbRetenciones_CheckedChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        #endregion

        #region Facturacion

        protected void btnGenerarFactura_Click(object sender, EventArgs e)
        {
            if (GenerarXML())
            {
                //MessageBox.Show(this, "La prefactura fue generada correctamente.");

                if (Timbrar())
                {
                    MessageBox.Show(this, "La prefactura fue timbrada correctamente.");
                    GuardarBD();

                    if (GenerarPDF())
                        EnviarCorreo();
                }
            }
        }

        private bool GenerarXML()
        {
            try
            {
                string cfdi = "http://www.sat.gob.mx/cfd/3";
                string xmlns = "http://www.w3.org/2000/xmlns/";
                string xsi = "http://www.w3.org/2001/XMLSchema-instance";
                string esquema = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";

                //string fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                string fecha = Funciones.ObtenerFechaHora();
                prefactura = fecha.Replace("-", "").Replace("T", "_").Replace(":", "") + ".xml";

                XmlDocument documento = new XmlDocument();
                XmlDeclaration declaracion = documento.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement raiz = documento.DocumentElement;
                documento.InsertBefore(declaracion, raiz);

                XmlElement comp = documento.CreateElement("cfdi", "Comprobante", cfdi);
                XmlAttribute ns = documento.CreateAttribute("xmlns:xsi", xmlns);
                ns.Value = xsi;
                comp.SetAttributeNode(ns);
                comp.SetAttribute("schemaLocation", xsi, esquema);

                // Parte 1: Atributos del comprobante
                string formaPago = txtFormaPago.Text;
                string subtotal = txtSubtotal.Text;
                string tipo = ddlTipo.Text;
                string total = txtTotal.Text;
                string metodoPago = txtMetodo.Text;
                string lugarExpedicion = txtLugar.Text;
                string version = "3.2"; 
                string cuentaPago = (metodoPago != "No Identificado") ? txtCuenta.Text : "";
                string noCertificado = "";
                string rutaCertificado = Path.Combine(carpetaCertificados, certificado);
                string rutaLlave = Path.Combine(carpetaCertificados, llave);

                comp.SetAttribute("certificado", ClaseCertificado.ObtenerCertificado(rutaCertificado, out noCertificado));
                comp.SetAttribute("fecha", fecha);
                comp.SetAttribute("formaDePago", formaPago);
                comp.SetAttribute("noCertificado", ClaseCertificado.ConvertHexToString(noCertificado, new UTF8Encoding(false)));
                comp.SetAttribute("subTotal", subtotal);
                comp.SetAttribute("tipoDeComprobante", tipo);
                comp.SetAttribute("total", total);
                comp.SetAttribute("metodoDePago", metodoPago);
                comp.SetAttribute("LugarExpedicion", lugarExpedicion);
                comp.SetAttribute("version", version);

                if (cuentaPago != "")
                    comp.SetAttribute("NumCtaPago", cuentaPago);

                // Parte 2: Emisor
                string rfcEmisor = txtRFCEmisor.Text;
                string regimen = ddlRegimen.Text;
                string nombreEmisor = txtNombreEmisor.Text;

                XmlElement emisor = documento.CreateElement("cfdi", "Emisor", cfdi);
                emisor.SetAttribute("rfc", rfcEmisor);
                emisor.SetAttribute("nombre", txtNombreEmisor.Text);
                
                XmlElement regimenFiscal = documento.CreateElement("cfdi", "RegimenFiscal", cfdi);
                regimenFiscal.SetAttribute("Regimen", regimen);
                
                emisor.AppendChild(regimenFiscal);
                comp.AppendChild(emisor);

                // Parte 3: Receptor
                string rfcReceptor = txtRFCReceptor.Text;
                //string nombreReceptor = txtNombreReceptor.Text;
                
                XmlElement receptor = documento.CreateElement("cfdi", "Receptor", cfdi);
                receptor.SetAttribute("rfc", rfcReceptor);
                //receptor.SetAttribute("nombre",txtNombreReceptor.Text);

                comp.AppendChild(receptor);

                // Parte 4: Conceptos
                XmlElement conceptos = documento.CreateElement("cfdi", "Conceptos", cfdi);

                foreach (DataRow registro in dtConceptos.Rows)
                {
                    decimal valorunit = decimal.Parse(registro["valorunitario"].ToString());
                    decimal importe = decimal.Parse(registro["importe"].ToString());

                    XmlElement concepto = documento.CreateElement("cfdi", "Concepto", cfdi);
                    concepto.SetAttribute("cantidad", registro["cantidad"].ToString());
                    concepto.SetAttribute("unidad", registro["unidad"].ToString());
                    concepto.SetAttribute("descripcion", registro["descripcion"].ToString());
                    concepto.SetAttribute("valorUnitario", valorunit.ToString());
                    concepto.SetAttribute("importe", importe.ToString());

                    conceptos.AppendChild(concepto);
                }

                comp.AppendChild(conceptos);

                // Parte 5: Impuestos

                bool trasladosSI = chbTrasladoIVA.Checked || chbTrasladoIEPS.Checked;
                bool retencionesSI = chbRetencionIVA.Checked || chbRetencionISR.Checked;

                decimal trasladoIVA = (chbTrasladoIVA.Checked) ? decimal.Parse(txtImporteTrasladoIVA.Text) : 0;
                decimal trasladoIEPS = (chbTrasladoIEPS.Checked) ? decimal.Parse(txtImporteTrasladoIEPS.Text) : 0;
                string tasaIVA = txtTasaTrasladoIVA.Text;
                string tasaIEPS = txtTasaTrasladoIEPS.Text;
                decimal impuestosTrasladados = trasladoIVA + trasladoIEPS;

                decimal retencionIVA = (chbRetencionIVA.Checked) ? decimal.Parse(txtImporteRetencionIVA.Text) : 0;
                decimal retencionISR = (chbRetencionISR.Checked) ? decimal.Parse(txtImporteRetencionISR.Text) : 0;
                decimal impuestosRetenidos = retencionIVA + retencionISR;

                XmlElement impuestos = documento.CreateElement("cfdi", "Impuestos", cfdi);
                impuestos.SetAttribute("totalImpuestosTrasladados", impuestosTrasladados.ToString());
                impuestos.SetAttribute("totalImpuestosRetenidos", impuestosRetenidos.ToString());

                // 5a Retenciones
                if (chbRetenciones.Checked)
                {
                    if (retencionesSI)
                    {
                        XmlElement retenciones = documento.CreateElement("cfdi", "Retenciones", cfdi);
                     
                        if (chbRetencionIVA.Checked)
                        {
                            XmlElement retencion1 = documento.CreateElement("cfdi", "Retencion", cfdi);
                            retencion1.SetAttribute("importe", retencionIVA.ToString());
                            retencion1.SetAttribute("impuesto", "IVA");
                            retenciones.AppendChild(retencion1);
                        }

                        if (chbRetencionISR.Checked)
                        {
                            XmlElement retencion2 = documento.CreateElement("cfdi", "Retencion", cfdi);
                            retencion2.SetAttribute("importe", retencionISR.ToString());
                            retencion2.SetAttribute("impuesto", "ISR");
                            retenciones.AppendChild(retencion2);
                        }

                        impuestos.AppendChild(retenciones);
                    }
                }

                // 5b Traslados
                if (chbTraslados.Checked)
                {
                    if (trasladosSI)
                    {
                        XmlElement traslados = documento.CreateElement("cfdi", "Traslados", cfdi);

                        if (chbTrasladoIVA.Checked)
                        {
                            XmlElement traslado1 = documento.CreateElement("cfdi", "Traslado", cfdi);
                            traslado1.SetAttribute("importe", trasladoIVA.ToString());
                            traslado1.SetAttribute("impuesto", "IVA");
                            traslado1.SetAttribute("tasa", tasaIVA);
                            traslados.AppendChild(traslado1);
                        }

                        if (chbTrasladoIEPS.Checked)
                        {
                            XmlElement traslado2 = documento.CreateElement("cfdi", "Traslado", cfdi);
                            traslado2.SetAttribute("importe", trasladoIEPS.ToString());
                            traslado2.SetAttribute("impuesto", "IEPS");
                            traslado2.SetAttribute("tasa", tasaIEPS);
                            traslados.AppendChild(traslado2);
                        }

                        impuestos.AppendChild(traslados);
                    }
                }

                comp.AppendChild(impuestos);

                documento.AppendChild(comp);

                MemoryStream memoria = XMLToStream(documento);
                string cadenaOriginal = ClaseCertificado.ObtenerCadenaOriginal(memoria);
                string selloCadena = ClaseCertificado.Sellar(rutaLlave, pass, cadenaOriginal);

                if (selloCadena == "")
                    return false;

                comp.SetAttribute("sello", selloCadena);
                
                MemoryStream memoria2 = XMLToStream(documento);
                GuardarXML(prefactura, memoria2);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private MemoryStream XMLToStream(XmlDocument documento)
        {
            MemoryStream memoria = new MemoryStream();
            TextWriter sw = new StreamWriter(memoria, new UTF8Encoding(false));
            documento.Save(sw);
            memoria.Flush();
            memoria.Position = 0;

            return memoria;
        }

        private void GuardarXML(string archivo, MemoryStream memoria)
        { 
            using (FileStream fs = new FileStream(Path.Combine(carpetaFacturas, archivo), FileMode.Create, FileAccess.Write))
            {
                memoria.WriteTo(fs);
            }
        }

        private bool Timbrar()
        {
            try
            {
                MemoryStream timbre = Funciones.Timbrar(
                    Path.Combine(carpetaFacturas, prefactura));
                factura = prefactura.Replace(".xml", "-timbrado.xml");
                GuardarXML(factura, timbre);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private bool GuardarBD()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private bool GenerarPDF()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private bool EnviarCorreo()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        #endregion
    }
}