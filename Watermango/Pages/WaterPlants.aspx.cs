using System;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Watermango.Models;

namespace Watermango.Pages
{
    public partial class WaterPlants : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Fill the grid with plants.
                var p = new Plant();
                var dt = p.GetAllPlants();
                Plants_grd.DataSource = dt;
                Plants_grd.DataBind();

                AddParametersJS(dt.Rows.Count);

                AddPlant();
            }
        }

        /// <summary>
        /// Save a watering to the database and return the WaterDate
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string SaveWatering(int plantId)
        {
            //Save a watering.
            var wp = new WaterPlant();
            wp.PlantId = plantId;
            wp.Add();

            //Get all 
            var dt = wp.GetLastWaterByPlantId(plantId);

            return dt.Rows[0]["WaterDate"].ToString() + "," + dt.Rows[0]["WaterCount"].ToString();
        }

        /// <summary>
        /// Add global variables to the Javascript for each plant
        /// </summary>
        /// <param name="count"></param>
        private void AddParametersJS(int count)
        {
            //Define the name and type of the client script on the page.
            var csName = "GlobalJSParameters";
            var csType = this.GetType();

            //Get a ClientScriptManager reference from the Page class.
            var cs = Page.ClientScript;

            //Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(csType, csName))
            {
                var csText = new StringBuilder();
                csText.AppendLine("<script type=\"text/javascript\"> ");
                for (int i = 0; i < count; i++)
                {
                    csText.AppendLine($"var p{i};");
                }
                csText.Append("</script>");
                cs.RegisterClientScriptBlock(csType, csName, csText.ToString());
            }
        }

        /// <summary>
        /// Create ClientClicks events and handle empty values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Plants_grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var row = (DataRowView)e.Row.DataItem;

                //Fill PlantId
                var plantLbl = (Label)e.Row.FindControl("plantId_lbl");
                var plantId = row["ID"].ToString();
                plantLbl.Text = plantId;

                var waterCount = (Label)e.Row.FindControl("waterCount_lbl");
                waterCount.Text = row["WaterCount"].ToString();

                //Fill last watered date.
                var lastTime = (Label)e.Row.FindControl("lastTime_txt");
                var dateValue = row["LastWatered"].ToString();
                var d = new DateTime();
                if (!string.IsNullOrEmpty(dateValue))
                {
                    d = DateTime.Parse(dateValue);
                }
                else
                {
                    dateValue = "Never.";
                }
                lastTime.Text = dateValue;

                //Define click events
                var chk = (HtmlInputCheckBox)e.Row.FindControl("select_chk");
                chk.Attributes.Add("onclick", "selectPlant(this.id," + plantId + ")");

                var btn = (HtmlInputButton)e.Row.FindControl("start_btn");
                btn.Attributes.Add("onclick", "startWatering(this.id," + plantId + ")");

                btn = (HtmlInputButton)e.Row.FindControl("stop_btn");
                btn.Attributes.Add("onclick", "saveWatering(this.id," + plantId + ", false)");

                //Check Water Status
                var statusLbl = (HtmlGenericControl)e.Row.FindControl("status_lbl");
                if (dateValue == "Never.")
                {
                    statusLbl.InnerText = "Attention! Needs water.";
                    statusLbl.Style.Add("color", "red");
                }
                else if ((DateTime.Now - d).TotalHours > 5)
                {
                    statusLbl.InnerText = "Attention! It's been more than 6 hours.";
                    statusLbl.Style.Add("color", "red");
                }
                else
                {
                    statusLbl.InnerText = "Good.";
                    statusLbl.Style.Add("color", "green");
                }
            }
        }

        /// <summary>
        /// Add a plant to the database.
        /// </summary>
        private void AddPlant()
        {
            if (Request.QueryString["addPlant"] != null && !string.IsNullOrEmpty(Request.QueryString["addPlant"].ToString()))
            {
                var plantName = Request.QueryString["addPlant"].ToString();
                var rnd = new Random();
                var p = new Plant();
                p.PlantName = plantName;
                p.Image = rnd.Next(1, 6).ToString() + ".png";
                p.Add();

                var url = Request.Url.ToString();
                Response.Redirect(url.Substring(0, url.IndexOf("?")));
            }
        }
    }
}
