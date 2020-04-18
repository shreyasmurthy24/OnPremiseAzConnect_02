using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace FnAppDBDemo
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //Inserting the data into ASQL
            InsertData(data);

            string msg = "Data inserted";
            string responseMessage = string.IsNullOrEmpty(msg)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {msg}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        private static void InsertData(dynamic data)
        {
            string fname = data.FName;
            string lname = data.LName;
            int age = data.Age;

            try
            {
                var strConn = Environment.GetEnvironmentVariable("sqldb_connection");
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_name", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fname", fname));
                    cmd.Parameters.Add(new SqlParameter("@lname", lname));
                    cmd.Parameters.Add(new SqlParameter("@age", age));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }
}
