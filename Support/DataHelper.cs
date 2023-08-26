using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace SMG.Wikipedia.Challenge.Support
{
    public class DataHelper
    {
        /// <summary>
        /// LoadCSV
        /// </summary>
        /// <param name="name"></param>
        /// <returns>load and parse data to data table</returns>
        public DataTable LoadCSV(string name)
        {
            string dir = $"{AppContext.BaseDirectory}\\Assets";
            string[] files = Directory.GetFiles(dir, name);
            DataTable table = new DataTable();

            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(files[0])), true))
            {
                table.Load(csvReader);
            }

            return table;
        }
    }
}
