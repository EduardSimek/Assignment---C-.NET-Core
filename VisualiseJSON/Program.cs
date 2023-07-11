using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using ZedGraph;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Runtime.CompilerServices;
using VisualiseJSON;


namespace VisualiseJSON
{
    public class Program : AnotherMethods, IGlobalVariables
    {
        public static void Main(string[] args)
        {
            //sortInformations();
            //sortInformationsWithChart_Task2();
            sortInformations2_Task1();
            
        }

        public static void sortInformations2_Task1()
        {
             string jsonFile = "JSONInfo.json";
             string jsonContent = File.ReadAllText(jsonFile);
             JArray json = JArray.Parse(jsonContent);
             
             Dictionary<string, TimeSpan> workingTimes = new Dictionary<string, TimeSpan>();
             IGlobalVariables.totalWorkingTime = TimeSpan.Zero;      
           
            foreach (JObject item in json)
            {
                IGlobalVariables.employeeName = item["EmployeeName"].ToString();
                IGlobalVariables.startTime = DateTime.Parse(item["StarTimeUtc"].ToString());
                IGlobalVariables.endTime = DateTime.Parse(item["EndTimeUtc"].ToString());
                TimeSpan duration = IGlobalVariables.endTime - IGlobalVariables.startTime;

                if (workingTimes.ContainsKey(IGlobalVariables.employeeName))
                {
                    workingTimes[IGlobalVariables.employeeName] += duration;
                }
                else
                {
                    workingTimes[IGlobalVariables.employeeName] = duration;
                }

                IGlobalVariables.totalWorkingTime += duration;
            }

            Program program = new Program();
            program.BuildHTMLTable();

            foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
            {
                IGlobalVariables.employeeName = entry.Key;
                TimeSpan workingTime = entry.Value;

                IGlobalVariables.formattedWorkingTime = string.Format("{0}:{1:00}", (int)workingTime.TotalHours, workingTime.Minutes);
                double _workingTimePercentage = ((workingTime.TotalHours + workingTime.TotalMinutes) / (IGlobalVariables.totalWorkingTime.TotalHours + IGlobalVariables.totalWorkingTime.TotalMinutes));
                IGlobalVariables.workingTimePercentage = _workingTimePercentage * 100;

                if ((int)workingTime.TotalHours < 100)
                {
                    htmlBuilder.AppendLine("<tr class='highlight'>");
                    program.HTMLPage();
                    htmlBuilder.AppendLine("</tr>");
                }
                else
                {
                    htmlBuilder.AppendLine("<tr>");
                    program.HTMLPage();
                    htmlBuilder.AppendLine("</tr>");
                }


            }

            IGlobalVariables.formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)IGlobalVariables.totalWorkingTime.TotalHours, IGlobalVariables.totalWorkingTime.Minutes);
            program.HTMLText();
            string htmlFile = "WorkingTime.html";
            File.WriteAllText(htmlFile, htmlBuilder.ToString());
            Console.WriteLine("HTML page created successfully.");
            
            Process.Start(new ProcessStartInfo
            {
                FileName = htmlFile,
                UseShellExecute = true
            });

            Console.WriteLine("HTML page opened in the default web browser.");
        }
       

        public static void sortInformations()
        {
            string jsonFile = "JSONInfo.json";
            JArray json = LoadJsonFromFile(jsonFile);
            Dictionary<string, TimeSpan> workingTimes = new Dictionary<string, TimeSpan>();
            TimeSpan totalWorkingTime = TimeSpan.Zero;

            JArray LoadJsonFromFile(string jsonFile)
            {
                string jsonContent = File.ReadAllText(jsonFile);
                return JArray.Parse(jsonContent);
            }

            void calculatingWorkingTimes(JArray json)
            {
                foreach (JObject item in json)
                {
                    string employeeName = item["EmployeeName"].ToString();
                    DateTime startTime = DateTime.Parse(item["StarTimeUtc"].ToString());
                    DateTime endTime = DateTime.Parse(item["EndTimeUtc"].ToString());

                    TimeSpan duration = endTime - startTime;

                    if (workingTimes.ContainsKey(employeeName))
                    {
                        workingTimes[employeeName] += duration;
                    }
                    else
                    {
                        workingTimes[employeeName] = duration;
                    }

                    totalWorkingTime += duration;
                }
            }

            string GenerateHtmlTable(Dictionary<string, TimeSpan> workingTimes)
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("<title>Employee Working Time</title>");
                htmlBuilder.AppendLine("<style>");
                htmlBuilder.AppendLine(".highlight { background-color: yellow; }");

                htmlBuilder.AppendLine("</style>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");
                htmlBuilder.AppendLine("<table>");

                foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
                {
                    string employeeName = entry.Key;
                    TimeSpan workingTime = entry.Value;

                    string formattedWorkingTime = string.Format("{0}:{1:00}", (int)workingTime.TotalHours, workingTime.Minutes);
                    double _workingTimePercentage = ((workingTime.TotalHours + workingTime.TotalMinutes) / (totalWorkingTime.TotalHours + totalWorkingTime.TotalMinutes));
                    double workingTimePercentage = _workingTimePercentage * 100;

                    if ((int)workingTime.TotalHours < 100)
                    {                      
                        htmlBuilder.AppendLine("<tr class='highlight'>");
                        htmlBuilder.AppendLine("<td>Employee Name: " + employeeName + "</td>");
                        htmlBuilder.AppendLine("<td>Total Working Time: " + formattedWorkingTime + "</td>");
                        htmlBuilder.AppendLine("<td>Working Time Percentage: " + workingTimePercentage.ToString("0.00") + "%</td>");
                        htmlBuilder.AppendLine("</tr>");
                    }
                    else
                    {
                        htmlBuilder.AppendLine("<tr>");
                        htmlBuilder.AppendLine("<td>Employee Name: " + employeeName + "</td>");
                        htmlBuilder.AppendLine("<td>Total Working Time: " + formattedWorkingTime + "</td>");
                        htmlBuilder.AppendLine("<td>Working Time Percentage: " + workingTimePercentage.ToString("0.00") + "%</td>");
                        htmlBuilder.AppendLine("</tr>");
                    }

                }

                string formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)totalWorkingTime.TotalHours, totalWorkingTime.Minutes);

                htmlBuilder.AppendLine("<td colspan='2'>Total Working Time of all employees: " + formattedTotalWorkingTime + "</td>");
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

                return htmlBuilder.ToString();
            }

            void SaveHtmlToFile(string htmlFile, string htmlContent)
            {
                File.WriteAllText(htmlFile, htmlContent);
                Console.WriteLine("HTML page created successfully.");
            }

            void OpenHtmlFileInBrowser(string htmlFile)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = htmlFile,
                    UseShellExecute = true
                });
                Console.WriteLine("HTML page opened in the default web browser.");
            }

            calculatingWorkingTimes(json);
            string htmlFile = "WorkingTime.html";

            OpenHtmlFileInBrowser(htmlFile);
        }

       public static void sortInformationsWithChart_Task2()
        {
            string jsonFile = "JSONInfo.json";
            string jsonContent = File.ReadAllText(jsonFile);
            JArray json = JArray.Parse(jsonContent);

            Dictionary<string, TimeSpan> workingTimes = new Dictionary<string, TimeSpan>();
            TimeSpan totalWorkingTime = TimeSpan.Zero;

            foreach (JObject item in json)
            {
                string employeeName = item["EmployeeName"].ToString();
                DateTime startTime = DateTime.Parse(item["StarTimeUtc"].ToString());
                DateTime endTime = DateTime.Parse(item["EndTimeUtc"].ToString());

                TimeSpan duration = endTime - startTime;

                if (workingTimes.ContainsKey(employeeName))
                {
                    workingTimes[employeeName] += duration;
                }
                else
                {
                    workingTimes[employeeName] = duration;
                }

                totalWorkingTime += duration;
            }

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("<title>Employee Working Time</title>");
            htmlBuilder.AppendLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>"); 
            htmlBuilder.AppendLine("<style>");
            htmlBuilder.AppendLine(".highlight { background-color: yellow; }");

            htmlBuilder.AppendLine("</style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<canvas id='chart'></canvas>");   
            htmlBuilder.AppendLine("<table>");

            foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
            {
                string employeeName = entry.Key;
                TimeSpan workingTime = entry.Value;

                string formattedWorkingTime = string.Format("{0}:{1:00}", (int)workingTime.TotalHours, workingTime.Minutes);
                double _workingTimePercentage = ((workingTime.TotalHours + workingTime.TotalMinutes) / (totalWorkingTime.TotalHours + totalWorkingTime.TotalMinutes));
                double workingTimePercentage = _workingTimePercentage * 100;

                if ((int)workingTime.TotalHours < 100)
                {
                    htmlBuilder.AppendLine("<tr class='highlight'>");
                    
                    htmlBuilder.AppendLine("<td>Employee Name: " + employeeName + "</td>");
                    htmlBuilder.AppendLine("<td>Total Working Time: " + formattedWorkingTime + "</td>");
                    htmlBuilder.AppendLine("<td>Working Time Percentage: " + workingTimePercentage.ToString("0.00") + "%</td>");
                    htmlBuilder.AppendLine("</tr>");
                }
                else
                {
                    htmlBuilder.AppendLine("<tr>");
                    htmlBuilder.AppendLine("<td>Employee Name: " + employeeName + "</td>");
                    htmlBuilder.AppendLine("<td>Total Working Time: " + formattedWorkingTime + "</td>");
                    htmlBuilder.AppendLine("<td>Working Time Percentage: " + workingTimePercentage.ToString("0.00") + "%</td>");
                    htmlBuilder.AppendLine("</tr>");
                }

            }

            string formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)totalWorkingTime.TotalHours, totalWorkingTime.Minutes);

            htmlBuilder.AppendLine("<td colspan='2'>Total Working Time of all employees: " + formattedTotalWorkingTime + "</td>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</table>");

            htmlBuilder.AppendLine("<script>");

            htmlBuilder.AppendLine("var ctx = document.getElementById('chart').getContext('2d');");
            htmlBuilder.AppendLine("var chart = new Chart (ctx, {");
            htmlBuilder.AppendLine("    type: 'pie',");
            htmlBuilder.AppendLine("    data: {");
            htmlBuilder.AppendLine("        labels: [");

            foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
            {
                string employeeName = entry.Key;
                double workingTimePercentage = (entry.Value.TotalHours / totalWorkingTime.TotalHours) * 100;
                htmlBuilder.AppendLine("            '" + employeeName + "',");
            }

            htmlBuilder.AppendLine("        ],");
            htmlBuilder.AppendLine("        datasets: [{");
            htmlBuilder.AppendLine("            data: [");

            foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
            {
                double workingTimePercentage = (entry.Value.TotalHours / totalWorkingTime.TotalHours) * 100;
                htmlBuilder.AppendLine("            " + workingTimePercentage.ToString("0.00") + ",");
            }

            htmlBuilder.AppendLine("            ],");
            htmlBuilder.AppendLine("            backgroundColor: [");

            foreach (KeyValuePair<string, TimeSpan> entry in workingTimes.OrderBy(x => x.Value))
            {
                htmlBuilder.AppendLine("            getRandomColor(),"); 
            }

            htmlBuilder.AppendLine("            ],");
            htmlBuilder.AppendLine("        }],");
            htmlBuilder.AppendLine("    },");
            htmlBuilder.AppendLine("    options: {");
            htmlBuilder.AppendLine("        responsive: true,");
            htmlBuilder.AppendLine("        maintainAspectRatio: false,");
            htmlBuilder.AppendLine("    },");
            htmlBuilder.AppendLine("});");
            htmlBuilder.AppendLine("function getRandomColor() {");
            htmlBuilder.AppendLine("    var letters = '0123456789ABCDEF';");
            htmlBuilder.AppendLine("    var color = '#';");
            htmlBuilder.AppendLine("    for (var i = 0; i < 6; i++) {");
            htmlBuilder.AppendLine("        color += letters[Math.floor(Math.random() * 16)];");
            htmlBuilder.AppendLine("    }");
            htmlBuilder.AppendLine("    return color;");
            htmlBuilder.AppendLine("}");

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

            string htmlFile = "WorkingTime.html";
            File.WriteAllText(htmlFile, htmlBuilder.ToString());
            Console.WriteLine("HTML page created successfully.");

            Process.Start(new ProcessStartInfo
            {
                FileName = htmlFile,
                UseShellExecute = true
            });

            Console.WriteLine("HTML page opened in the default web browser.");
        }

        
    }

}
