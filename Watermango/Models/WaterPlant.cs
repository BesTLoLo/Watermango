using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Watermango.Models
{
    /// <summary>
    /// A Business Object of WaterPlants table.
    /// </summary>
    public class WaterPlant
    {
        public int PlantId { get; set; }

        public void Add()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Insert Into WaterPlants");
            sb.AppendLine("(PlantId,WaterDate)");
            sb.AppendLine("Values (@plantId, GETDATE())");

            SqlParameter[] pars =
                        {
                          new SqlParameter("@plantId", SqlDbType.Int) { Value = PlantId }
                        };

            var dp = new DataProvider();
            dp.ExecuteScaler(sb.ToString(), pars);
        }

        /// <summary>
        /// Get Last Watered record by Plant Id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetLastWaterByPlantId(int plantId)
        {
            SqlParameter[] pars =
                        {
                          new SqlParameter("@plantId", SqlDbType.Int) { Value = plantId }
                        };

            var dp = new DataProvider();
            return dp.ExecuteDT("GetWaterPlantsByPlantId", pars);
        }
    }
}