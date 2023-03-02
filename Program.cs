// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Text;
using System.Xml;

Console.WriteLine("Hello, World!");

//Calling InvokeService method (WebRequest)
//InvokeService();

//Calling InvokeService method (HttpClient)
InvokeSoapService();

void InvokeService()
{
    Console.WriteLine("Calling InvokeService method (WebRequest)");
    //Calling CreateSOAPWebRequest method
    HttpWebRequest request = CreateSOAPWebRequest();

    XmlDocument SOAPReqBody = new();
    //SOAP Body Request
    SOAPReqBody.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body><login xmlns=\"http://tempuri.org/\"><email>angella.freeman@email.com</email><password>string</password></login></soap:Body></soap:Envelope>");

    using (Stream stream = request.GetRequestStream())
    {
        SOAPReqBody.Save(stream);
    }
    //Geting response from request
    using WebResponse Serviceres = request.GetResponse();
    using StreamReader rd = new(Serviceres.GetResponseStream());
    //reading stream
    var ServiceResult = rd.ReadToEnd();
    //writting stream result on console
    Console.WriteLine(ServiceResult);
    Console.ReadLine();
}

HttpWebRequest CreateSOAPWebRequest()
{
    //Making Web Request
    HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://localhost/DemoSoap.asmx?op=login");
    //SOAPAction
    Req.Headers.Add(@"SOAPAction:http://tempuri.org/login");
    //Content_type
    Req.ContentType = "text/xml;charset=\"utf-8\"";
    Req.Accept = "text/xml";
    //HTTP method
    Req.Method = "POST";
    //return HttpWebRequest
    return Req;
}

void InvokeSoapService()
{
    Console.WriteLine("Calling InvokeSoapService method (HttpClient)");
    string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body><login xmlns=\"http://tempuri.org/\"><email>angella.freeman@email.com</email><password>string</password></login></soap:Body></soap:Envelope>";
    try
    {
        string url = @"http://localhost/DemoSoap.asmx?op=login";
        string soapAction = "http://tempuri.org/login";

        string result = PostSoapRequestAsync(url, xml, soapAction).Result;
        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

async Task<string> PostSoapRequestAsync(string url, string xml, string soapAction)
{
    try
    {
        HttpClient httpClient = new();
        var byteArray = Encoding.ASCII.GetBytes("username:password1234");
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        using HttpContent content = new StringContent(xml, Encoding.UTF8, "text/xml");
        using HttpRequestMessage request = new(HttpMethod.Post, url);
        request.Headers.Add("SOAPAction", soapAction);
        request.Content = content;

        using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
        return await response.Content.ReadAsStringAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return ex.Message;
    }
}