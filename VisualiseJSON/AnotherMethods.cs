using System.Text;

namespace VisualiseJSON
{
    public class AnotherMethods
    {
        private static StringBuilder htmlBuilder = new StringBuilder();

        public static StringBuilder HtmlBuilder { get => htmlBuilder; set => htmlBuilder = value; }

        public static string _URL = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

        public void BuildHTMLTable()
        {        
            HtmlBuilder.AppendLine("<html>");
            HtmlBuilder.AppendLine("<head>");
            HtmlBuilder.AppendLine("<title>Employee Working Time</title>");
            HtmlBuilder.AppendLine("<style>");
            HtmlBuilder.AppendLine(".highlight { background-color: yellow; }");
            HtmlBuilder.AppendLine("</style>");
            HtmlBuilder.AppendLine("</head>");
            HtmlBuilder.AppendLine("<body>");
            HtmlBuilder.AppendLine("<table>");
        }

        public void HTMLPage()
        {
            HtmlBuilder.AppendLine("<td>Employee Name: " + IGlobalVariables.employeeName + "</td>");
            HtmlBuilder.AppendLine("<td>Total Working Time: " + IGlobalVariables.formattedWorkingTime + "</td>");
            HtmlBuilder.AppendLine("<td>Working Time Percentage: " + IGlobalVariables.workingTimePercentage.ToString("0.00") + "%</td>");
        }

        public void HTMLText()
        {
            IGlobalVariables.formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)IGlobalVariables.totalWorkingTime.TotalHours, IGlobalVariables.totalWorkingTime.Minutes);

            HtmlBuilder.AppendLine("<td colspan='2'>Total Working Time of all employees: " + IGlobalVariables.formattedTotalWorkingTime + "</td>");
            HtmlBuilder.AppendLine("</tr>");
            HtmlBuilder.AppendLine("</table>");

            HtmlBuilder.AppendLine("<script>");

            HtmlBuilder.AppendLine("function sortTable(ascending) {");
            HtmlBuilder.AppendLine("    var table = document.querySelector('table');");
            HtmlBuilder.AppendLine("    var rows = Array.from(table.querySelectorAll('tr'));");
            HtmlBuilder.AppendLine("    rows.shift();");

            HtmlBuilder.AppendLine("    rows.sort(function(a, b) {");
            HtmlBuilder.AppendLine("        var aValue = a.querySelector('td:last-child').textContent;");
            HtmlBuilder.AppendLine("        var bValue = b.querySelector('td:last-child').textContent;");
            HtmlBuilder.AppendLine("        if (ascending) {");
            HtmlBuilder.AppendLine("            return aValue.localeCompare(bValue);");
            HtmlBuilder.AppendLine("        } else {");
            HtmlBuilder.AppendLine("            return bValue.localeCompare(aValue);");
            HtmlBuilder.AppendLine("        }");
            HtmlBuilder.AppendLine("    });");

            HtmlBuilder.AppendLine("    rows.forEach(function(row) {");
            HtmlBuilder.AppendLine("        table.appendChild(row);");
            HtmlBuilder.AppendLine("    });");
            HtmlBuilder.AppendLine("}");

            HtmlBuilder.AppendLine("</script>");

            HtmlBuilder.AppendLine("</body>");
            HtmlBuilder.AppendLine("</html>");
        }
    }
    

    

}




