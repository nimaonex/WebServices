using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AMLReport.Entity;
using AMLReport.Tools;

namespace AMLReport.DAL
{
    public class ReportConditionsDal
    {
        public ReportParameters GetConditions1()
        {
            var result = new ReportParameters();

            using (var connection =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["AntiMoneyLaunderingConnectionString"].ConnectionString))
            {
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "[AML].[GetParameterForReport1]";


                var fromDateParameter = new SqlParameter("@FromDate", SqlDbType.NVarChar, 30)
                {
                    Direction = ParameterDirection.Output
                };
                var toDateParameter = new SqlParameter("@ToDate", SqlDbType.NVarChar, 30)
                {
                    Direction = ParameterDirection.Output
                };
                var tranTypeParameter = new SqlParameter("@TranType", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                var customerTypeParameter = new SqlParameter("@customerType", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                var accountnumberParameter = new SqlParameter("@Accountnumber", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                var tranStatusParameter = new SqlParameter("@TranStatus", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                var reprtnumberParameter = new SqlParameter("@Reprtnumber", SqlDbType.NVarChar, 4000)
                {
                    Direction = ParameterDirection.Output
                };


                command.Parameters.Add(fromDateParameter);
                command.Parameters.Add(toDateParameter);
                command.Parameters.Add(tranTypeParameter);
                command.Parameters.Add(customerTypeParameter);
                command.Parameters.Add(accountnumberParameter);
                command.Parameters.Add(tranStatusParameter);
                command.Parameters.Add(reprtnumberParameter);


                try
                {
                    connection.Open();

                    command.ExecuteNonQuery();

                    result.Accountnumber = accountnumberParameter.Value.ToString();
                    result.customerType = customerTypeParameter.Value.ToString();
                    result.FromDate = fromDateParameter.Value.ToString();
                    result.Reprtnumber = reprtnumberParameter.Value.ToString();
                    result.ToDate = toDateParameter.Value.ToString();
                    result.TranStatus = tranStatusParameter.Value.ToString();
                    result.TranType = tranTypeParameter.Value.ToString();
                }
                catch (Exception ex)
                {
                    DpkLogger.Log(ex);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }

                    command.Dispose();
                }
            }


            return result;
        }
}