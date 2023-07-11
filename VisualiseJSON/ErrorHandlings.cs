using System;
using System.Net.Http;

namespace VisualiseJSON
{
    public class _ErrorHandlings
    {
        public static void CommonErMsg(Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        public static void HTTPErMsg(HttpResponseMessage responseMsg)
        {
            Console.WriteLine("Request failed with status code: " + responseMsg.StatusCode);
        }

        public static void SuccessMsg(HttpResponseMessage responseMsg)
        {
            Console.WriteLine($"Request was successed with status code: {responseMsg.StatusCode}");
            Console.WriteLine("HTML page opened in the default web browser.");
        }
    }
}
