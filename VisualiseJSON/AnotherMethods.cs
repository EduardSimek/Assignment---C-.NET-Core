using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualiseJSON
{
    public class AnotherMethods
    {
        public static StringBuilder htmlBuilder = new StringBuilder();
        public void BuildHTMLTable()
        {        
            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("<title>Employee Working Time</title>");
            htmlBuilder.AppendLine("<style>");
            htmlBuilder.AppendLine(".highlight { background-color: yellow; }");
            htmlBuilder.AppendLine("</style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<table>");
        }

        public void HTMLPage()
        {
            htmlBuilder.AppendLine("<td>Employee Name: " + IGlobalVariables.employeeName + "</td>");
            htmlBuilder.AppendLine("<td>Total Working Time: " + IGlobalVariables.formattedWorkingTime + "</td>");
            htmlBuilder.AppendLine("<td>Working Time Percentage: " + IGlobalVariables.workingTimePercentage.ToString("0.00") + "%</td>");
        }

        public void HTMLText()
        {
            IGlobalVariables.formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)IGlobalVariables.totalWorkingTime.TotalHours, IGlobalVariables.totalWorkingTime.Minutes);

            htmlBuilder.AppendLine("<td colspan='2'>Total Working Time of all employees: " + IGlobalVariables.formattedTotalWorkingTime + "</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</table>");

            htmlBuilder.AppendLine("<script>");

            htmlBuilder.AppendLine("function sortTable(ascending) {");
            htmlBuilder.AppendLine("    var table = document.querySelector('table');");
            htmlBuilder.AppendLine("    var rows = Array.from(table.querySelectorAll('tr'));");
            htmlBuilder.AppendLine("    rows.shift();");

            htmlBuilder.AppendLine("    rows.sort(function(a, b) {");
            htmlBuilder.AppendLine("        var aValue = a.querySelector('td:last-child').textContent;");
            htmlBuilder.AppendLine("        var bValue = b.querySelector('td:last-child').textContent;");
            htmlBuilder.AppendLine("        if (ascending) {");
            htmlBuilder.AppendLine("            return aValue.localeCompare(bValue);");
            htmlBuilder.AppendLine("        } else {");
            htmlBuilder.AppendLine("            return bValue.localeCompare(aValue);");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("    });");

            htmlBuilder.AppendLine("    rows.forEach(function(row) {");
            htmlBuilder.AppendLine("        table.appendChild(row);");
            htmlBuilder.AppendLine("    });");
            htmlBuilder.AppendLine("}");

            htmlBuilder.AppendLine("</script>");

            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");
        }
    }


}
