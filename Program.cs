using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace opg_201910_interview
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var x = Load();
            Console.WriteLine(x);
            //CreateHostBuilder(args).Build().Run();
            

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public static String Load()
        {
            DataTable dtA = new DataTable();
            DataTable dtB = new DataTable();
            List<string> lstClientA = new List<string>() { "shovel", "waghor", "blaze", "discus" };
            List<string> lstClientB = new List<string>() { "orca", "widget", "eclair", "talon" };

            dtA.Columns.Add("ClientId");
            dtA.Columns.Add("Name");
            dtA.Columns.Add("FileDirectoryPath");
            DataColumn colDateA = new DataColumn("Date");
            colDateA.DataType = System.Type.GetType("System.DateTime");
            dtA.Columns.Add(colDateA);

            dtB.Columns.Add("ClientId");
            dtB.Columns.Add("Name");
            dtB.Columns.Add("FileDirectoryPath");
            DataColumn colDateB = new DataColumn("Date");
            colDateB.DataType = System.Type.GetType("System.DateTime");
            dtB.Columns.Add(colDateB);

           

            //Client A
            string[] XMLfilesClientA = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\UploadFiles\\ClientA", "*.XML");
            foreach (string file in XMLfilesClientA)
            {
                string tmpString = Path.GetFileNameWithoutExtension(file).Replace("-", "").Replace("_", "").ToString();
                string fStringName = tmpString.Substring(0, tmpString.Length - 8);
                string tmpStringDate = tmpString.Substring(tmpString.Length - 8, 8);
                DateTime fDate = DateTime.ParseExact(tmpStringDate, "yyyyMMdd",
                CultureInfo.InvariantCulture);

                if (lstClientA.Contains(fStringName))
                {
                    DataRow tmp = dtA.NewRow();
                    tmp["ClientId"] = 1001 + dtA.Rows.Count;
                    tmp["Name"] = fStringName;
                    tmp["FileDirectoryPath"] = Path.GetDirectoryName(file);
                    tmp["Date"] = fDate;
                    dtA.Rows.Add(tmp);
                }
            }
            var resultA = dtA.AsEnumerable().OrderBy(x =>
                {
                    switch (x.Field<string>("Name"))
                    {
                        case "shovel": return 1;
                        case "waghor": return 2;
                        case "blaze": return 3;
                        case "discus": return 4;
                        default: return 9;
                    }

                }).ThenBy(y => y.Field<DateTime>("Date")).ToList();

            if (resultA != null)
            {
                dtA = resultA.CopyToDataTable();
            }
            

            //Client B
            string[] XMLfilesClientB = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\UploadFiles\\ClientB", "*.XML");
            foreach (string file in XMLfilesClientB)
            {
                string tmpString = Path.GetFileNameWithoutExtension(file).Replace("-", "").Replace("_", "").ToString();
                string fStringName = tmpString.Substring(0, tmpString.Length - 8);
                string tmpStringDate = tmpString.Substring(tmpString.Length - 8, 8);
                DateTime fDate = DateTime.ParseExact(tmpStringDate, "yyyyMMdd",
                CultureInfo.InvariantCulture);

                if (lstClientB.Contains(fStringName))
                {
                    DataRow tmp = dtB.NewRow();
                    tmp["ClientId"] = 1001 + dtB.Rows.Count;
                    tmp["Name"] = fStringName;
                    tmp["FileDirectoryPath"] = Path.GetDirectoryName(file);
                    tmp["Date"] = fDate;
                    dtB.Rows.Add(tmp);
                }
            }
            var resultB = dtB.AsEnumerable().OrderBy(x =>
            {
                switch (x.Field<string>("Name"))
                {
                    case "orca": return 1;
                    case "widget": return 2;
                    case "eclair": return 3;
                    case "talon": return 4;
                    default: return 9;
                }

            }).ThenBy(y => y.Field<DateTime>("Date")).ToList();

            if(resultB != null)
            {
                dtB = resultB.CopyToDataTable();
            }

            //Merge the 2 data tables
            DataTable dtAll = new DataTable();
            dtAll = dtA.Copy();
            dtAll.Merge(dtB);
            //dtAll.Columns.Remove("Name");
            //dtAll.Columns.Remove("Date");
            string result = JsonConvert.SerializeObject((dtAll), Newtonsoft.Json.Formatting.Indented);
            return result;
        }
    }
}
