using System;
using System.Net.Http;

namespace VisualiseJSON
{
    public class CommonErMsg
    {
        public static void ErrException(Exception exception)
        {
            Console.WriteLine($"An error occurred: {exception.Message}");
        }
    }

    public class HTTPErMsg
    {
        public static void ErrorMsg(HttpResponseMessage responseMsg)
        {
            Console.WriteLine("Request failed with status code: " + (int)responseMsg.StatusCode);
        }
    }

    public class HTTPSuccessMsg
    {
        public static void SuccessMsg(HttpResponseMessage responseMsg)
        {
            Console.WriteLine($"Request was successed with status code: {(int)responseMsg.StatusCode}");
            Console.WriteLine("HTML page opened in the default web browser.");
        }
    }
    

}

