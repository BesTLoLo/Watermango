using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace Watermango.Models
{
    /// <summary>
    /// A Business Object represents Plants table.
    /// </summary>
    public class Plant
    {
        public string PlantName { get; set; }
        public string Image { get; set; }

        /// <summary>
        /// Add a plant to the database
        /// </summary>
        /// <returns></returns>
        public int Add()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Insert Into Plants");
            sb.AppendLine("(PlantName, Image, CreatedOnDate)");
            sb.AppendLine("Values (@plantName, @image, GETDATE())");
            sb.AppendLine("Select @@IDENTITY");

            SqlParameter[] pars =
                        {
                          new SqlParameter("@plantName", SqlDbType.VarChar) { Value = PlantName },
                          new SqlParameter("@image", SqlDbType.VarChar) { Value = Image }
                        };
            var dp = new DataProvider();
            return dp.ExecuteScaler(sb.ToString(), pars);
        }

        /// <summary>
        /// Get All Plants from the database
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPlants()
        {
            var dp = new DataProvider();
            return dp.ExecuteDT("GetAllPlants", null);
        }

    }
}