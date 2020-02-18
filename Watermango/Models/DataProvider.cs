using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace Watermango.Models
{
    /// <summary>
    /// A Class to communicate with the database.
    /// </summary>
    public class DataProvider
    {
        //Logger to be used for logging exceptions.
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SqlConnection Connection { get; set; }

        /// <summary>
        /// Return a new connection to the SQL database.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            var con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["WatermangoConnection"].ConnectionString;
            return con;
        }

        /// <summary>
        /// Execute a stored procedure and return the results
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parameters">Parameters, pass null if none.</param>
        /// <returns></returns>
        public DataTable ExecuteDT(string spName, SqlParameter[] parameters)
        {
            var dt = new DataTable();

            try
            {
                using (var con = GetConnection())
                {
                    using (var cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        con.Open();

                        var dr = cmd.ExecuteReader();

                        using (dr)
                        {
                            dt.Load(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Exception - ExecuteDT: Stored Procedure Name ({spName}) - Error ({ex.Message})");
            }

            return dt;
        }

        /// <summary>
        /// Execute SQL query and returns the (first column - first row) value.
        /// </summary>
        /// <param name="sqlText">SQL Query Text</param>
        /// <param name="parameters">Parameters, pass null if none.</param>
        /// <returns></returns>
        public int ExecuteScaler(string sqlText, SqlParameter[] parameters)
        {
            var res = 0;

            try
            {
                using (var con = GetConnection())
                {
                    using (var cmd = new SqlCommand(sqlText, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        con.Open();
                        res = int.Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Exception - ExecuteScaler() SQLText ({sqlText}) - ErrorMessage: ({ex.Message})");
            }

            return res;
        }
    }
}