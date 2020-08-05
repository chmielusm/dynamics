using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoleMembershipsLoader
{
    public static class Helper
    {
        public static Tuple<int, int, int> ToTuple(this string value)
        {
            return FindCell(value);
        }

        /// <summary>
        /// Converts cells address range to Tuple<col,row,col>
        /// </summary>
        /// <param name="cellStr"></param>
        /// <returns></returns>
        public static Tuple<int, int, int> FindCell(string cellStr)
        {
            if (string.IsNullOrWhiteSpace(cellStr)) return new Tuple<int, int, int>(0, 0, 0);

            var parts = cellStr.Split(':');
            int counter = 0;
            int retValCol = 0;
            int retValRow = 0;
            int retValColEnd = 0;

            while (counter < parts.Length)
            {
                var match = Regex.Match(parts[counter], @"(?<col>[A-Z]+)(?<row>\d+)");
                var colStr = match.Groups["col"].ToString();
                var col = Convert.ToInt32(colStr.Select((t, i) => (colStr[i] - 64) * Math.Pow(26, colStr.Length - i - 1)).Sum());
                var row = int.Parse(match.Groups["row"].ToString());

                if (counter == 0)
                {
                    retValCol = col;
                    retValRow = row;
                }
                else
                {
                    retValColEnd = col;
                }

                counter++;
            }

            return new Tuple<int, int, int>(retValRow - 1, retValCol - 1, retValColEnd - 1);
        }

        public static bool ParseBoolean(string word)
        {
            bool state = false;
            if (bool.TryParse(word, out state))
            {
                return state;
            }
            else
            {
                if (word.ToLower() == "no") return false;
                if (word.ToLower() == "yes") return true;
            }
            return state;
        }

        public static Guid FetchRecord(IOrganizationService service, string query, string filedName)
        {
            var result = service.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return Guid.Empty;
            foreach (var record in result.Entities)
            {
                return record.GetAttributeValue<Guid>(filedName);
            }
            return Guid.Empty;
        }
    }
}
