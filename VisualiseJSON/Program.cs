﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VisualiseJSON
{
    
    public class Program : AnotherMethods, IGlobalVariables
    {
        public static async Task Main (string[] args)
        {
            await sortInformation(new DefaultHTTPClientFactory());
        }
     
        public static async Task sortInformation(HTTPClientFactoryPattern httpClientFactoryPattern)
        {
            try
            {           
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMsg = await client.GetAsync(AnotherMethods._URL);

                    if (responseMsg.IsSuccessStatusCode)
                    {
                        string jsonContent = await responseMsg.Content.ReadAsStringAsync();
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
                                HtmlBuilder.AppendLine("<tr class='highlight'>");
                                program.HTMLPage();
                                HtmlBuilder.AppendLine("</tr>");
                            }
                            else
                            {
                                HtmlBuilder.AppendLine("<tr>");
                                program.HTMLPage();
                                HtmlBuilder.AppendLine("</tr>");
                            }
                        }

                        IGlobalVariables.formattedTotalWorkingTime = string.Format("{0}:{1:00}", (int)IGlobalVariables.totalWorkingTime.TotalHours, IGlobalVariables.totalWorkingTime.Minutes);
                        program.HTMLText();
                        string htmlFile = "WorkingTime.html";
                        File.WriteAllText(htmlFile, HtmlBuilder.ToString());
                        Console.WriteLine("HTML page created successfully.");

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = htmlFile,
                            UseShellExecute = true
                        });
                        HTTPSuccessMsg.SuccessMsg(responseMsg);
                    }
                    else
                    {
                        HTTPErMsg.ErrorMsg(responseMsg);
                    }
                }
            }
            catch (Exception exception)
            {
                CommonErMsg.ErrException(exception);
            }
        }                 
    }


}









