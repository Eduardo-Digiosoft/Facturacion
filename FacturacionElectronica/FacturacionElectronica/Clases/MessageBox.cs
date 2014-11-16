using System.Web.UI;

namespace FacturacionElectronica.Clases
{
    public static class MessageBox
    {
        public static void Show(Page Page, string mensaje)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Script", "alert('" + mensaje + "');", true);
        }
    }
}