using System;
using System.Web.UI;

namespace Watermango
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/WaterPlants.aspx");
        }
    }
}