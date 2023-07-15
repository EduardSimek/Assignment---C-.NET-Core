using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VisualiseJSON
{
    public abstract class HTTPClientFactoryPattern
    {
        public abstract HttpClient CreateHTTPClient();
    }

    public class DefaultHTTPClientFactory : HTTPClientFactoryPattern
    {
        public override HttpClient CreateHTTPClient()
        {
            return new HttpClient();
        }
    }
}
