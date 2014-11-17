<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaginaFacturacion.aspx.cs" Inherits="FacturacionElectronica.Paginas.PaginaFacturacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table style="width: 800px">
          <tr>
              <td style="width:13%">&nbsp;</td>
              <td style="width:20%">&nbsp;</td>
              <td style="width:13%">&nbsp;</td>
              <td style="width:20%">&nbsp;</td>
              <td style="width:13%">&nbsp;</td>
              <td style="width:21%">&nbsp;</td>
          </tr>
          <tr>
              <td colspan="6"><h1>Datos del Emisor</h1></td>
          </tr>
          <tr>
              <td>Regimen Fiscal</td>
              <td colspan="5">
                <asp:DropDownList ID="ddlRegimen" runat="server">
                    <asp:ListItem Text="Regimen de Incorporacion Fiscal" />
                    <asp:ListItem Text="Regimen de las Personas Fisicas con Actividades Empresariales y Profesionales" />
                    <asp:ListItem Text="Regimen Intermedio de las Personas Físicas con Actividades Empresariales" />
                    <asp:ListItem Text="Regimen de Arrendamiento" />
                    <asp:ListItem Text="Regimen General de Ley Personas Morales" />
                    <asp:ListItem Text="Regimen Simplificado de Ley de Personas Morales" />
                    <asp:ListItem Text="Personas Morales con Fines no Lucrativos" />
                </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>RFC</td>
              <td><asp:TextBox ID="txtRFCEmisor" runat="server"></asp:TextBox></td>
              <td>Nombre</td>
              <td><asp:TextBox ID="txtNombreEmisor" runat="server" text="Antonio"></asp:TextBox></td>
          </tr>
          <tr>
              <td colspan="6"><h1>Datos del Receptor</h1></td>
          </tr>
          <tr>
              <td>RFC</td>
              <td><asp:TextBox ID="txtRFCReceptor" runat="server"></asp:TextBox></td>
              
          </tr>
          <tr>
              <td colspan="6"><h1>Datos de la Factura</h1></td>
          </tr>
          <tr>
              <td>Forma de Pago</td>
              <td colspan="2"><asp:TextBox ID="txtFormaPago" runat="server" Text="Pago en una sola exhibicion" Width="200px"></asp:TextBox></td>
              <td>Tipo de Comprobante</td>
              <td>
                  <asp:DropDownList ID="ddlTipo" runat="server">
                      <asp:ListItem Text="ingreso" />
                      <asp:ListItem Text="egreso" />
                      <asp:ListItem Text="traslado" />
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>Subtotal</td>
              <td><asp:TextBox ID="txtSubtotal" runat="server" Text="0"></asp:TextBox></td>
              <td>Total</td>
              <td><asp:TextBox ID="txtTotal" runat="server" Text="0"></asp:TextBox></td>
          </tr>
          <tr>
              <td>Metodo de Pago</td>
              <td><asp:TextBox ID="txtMetodo" runat="server" Text="No Identificado"></asp:TextBox></td>
              <td>No. Cuenta Pago</td>
              <td><asp:TextBox ID="txtCuenta" runat="server" Text="0000"></asp:TextBox></td>
              <td>Lugar Expedicion</td>
              <td><asp:TextBox ID="txtLugar" runat="server"></asp:TextBox></td>
          </tr>
          <tr>
              <td colspan="6"><h1>Conceptos</h1></td>
          </tr>
          <tr>
              <td colspan="6">
                  <asp:Table ID="tblConceptos" runat="server" Width="100%">
                      <asp:TableHeaderRow>
                          <asp:TableHeaderCell Width="10%">
                              <asp:Label ID="Label1" runat="server" Text="Cantidad"></asp:Label>
                          </asp:TableHeaderCell>
                          <asp:TableHeaderCell Width="10%">
                              <asp:Label ID="Label2" runat="server" Text="Unidad"></asp:Label>
                          </asp:TableHeaderCell>
                          <asp:TableHeaderCell Width="50%">
                              <asp:Label ID="Label3" runat="server" Text="Descripcion"></asp:Label>
                          </asp:TableHeaderCell>
                          <asp:TableHeaderCell Width="10%">
                              <asp:Label ID="Label4" runat="server" Text="Valor Unitario"></asp:Label>
                          </asp:TableHeaderCell>
                          <asp:TableHeaderCell Width="10%">
                              <asp:Label ID="Label5" runat="server" Text="Importe"></asp:Label>
                          </asp:TableHeaderCell>
                          <asp:TableHeaderCell Width="10%">
                              <asp:Label ID="Label6" runat="server" Text="Acciones"></asp:Label>
                          </asp:TableHeaderCell>
                      </asp:TableHeaderRow>
                      <asp:TableRow>
                          <asp:TableCell>
                              <asp:TextBox ID="txtCantidad" runat="server" Width="30" Text="1"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:TextBox ID="txtUnidad" runat="server" Width="80" Text="NO APLICA"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:TextBox ID="txtDescripcion" runat="server" Width="300" Text=""></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:TextBox ID="txtValorUnitario" runat="server" Width="60" Text="0"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:Label ID="lblImporte" runat="server" Text="0"></asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:Button ID="btnAgregarConcepto" runat="server" Text="Agregar" OnClick="btnAgregarConcepto_Click" />
                          </asp:TableCell>
                      </asp:TableRow>
                  </asp:Table>
              </td>
          </tr>
          <tr>
              <td colspan="6"><h1>Impuestos</h1></td>
          </tr>
          <tr>
              <td><h3>Retenciones</h3></td>
              <td>
                  <asp:Button ID="btnActualizarTotal2" runat="server" Text="Actualizar" OnClick="btnActualizarTotal_Click" />
              </td>
          </tr>
          <tr>
              <td rowspan="2"><asp:CheckBox ID="chbRetenciones" runat="server" Text="Incluir" AutoPostBack="True" OnCheckedChanged="chbRetenciones_CheckedChanged" /></td>
              <td><asp:CheckBox ID="chbRetencionIVA" runat="server" Text="IVA" AutoPostBack="True" OnCheckedChanged="chbRetencionIVA_CheckedChanged" /></td>
              <td>Importe</td>
              <td><asp:TextBox ID="txtImporteRetencionIVA" runat="server" Width="60" Text="0"></asp:TextBox></td>
          </tr>
          <tr>
              <td class="auto-style1"><asp:CheckBox ID="chbRetencionISR" runat="server" Text="ISR" AutoPostBack="True" OnCheckedChanged="chbRetencionIVA_CheckedChanged" /></td>
              <td class="auto-style1">Importe</td>
              <td class="auto-style1"><asp:TextBox ID="txtImporteRetencionISR" runat="server" Width="60" Text="0"></asp:TextBox></td>
          </tr>
          <tr>
              <td><h3>Traslados</h3></td>
              <td>
                  <asp:Button ID="btnActualizarTotal1" runat="server" Text="Actualizar" OnClick="btnActualizarTotal_Click" />
              </td>
          </tr>
          <tr>
              <td rowspan="2"><asp:CheckBox ID="chbTraslados" runat="server" Text="Incluir" AutoPostBack="True" OnCheckedChanged="chbTraslados_CheckedChanged" /></td>
              <td><asp:CheckBox ID="chbTrasladoIVA" runat="server" Text="IVA" AutoPostBack="True" OnCheckedChanged="chbTrasladoIVA_CheckedChanged" /></td>
              <td>Tasa</td>
              <td><asp:TextBox ID="txtTasaTrasladoIVA" runat="server" Width="60" Text="0"></asp:TextBox></td>
              <td>Importe</td>
              <td><asp:TextBox ID="txtImporteTrasladoIVA" runat="server" Width="60" Text="0"></asp:TextBox></td>
          </tr>
          <tr>
              <td><asp:CheckBox ID="chbTrasladoIEPS" runat="server" Text="IEPS" AutoPostBack="True" OnCheckedChanged="chbTrasladoIVA_CheckedChanged" /></td>
              <td>Tasa</td>
              <td><asp:TextBox ID="txtTasaTrasladoIEPS" runat="server" Width="60" Text="0"></asp:TextBox></td>
              <td>Importe</td>
              <td><asp:TextBox ID="txtImporteTrasladoIEPS" runat="server" Width="60" Text="0"></asp:TextBox></td>
          </tr>
          <tr>
              <td colspan="2"><asp:Button ID="btnGenerarFactura" runat="server" Text="Generar Factura" OnClick="btnGenerarFactura_Click"/></td>
              <td>&nbsp;</td>

          </tr>
          <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>
      </table>    
    </div>
    </form>
</body>
</html>
