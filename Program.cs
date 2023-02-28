// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml;

Console.WriteLine("Hello, World!");

//creating object of program class to access methods  
//Program obj = new Program();
Console.WriteLine("Please Enter Input values..");
//Reading input values from console  
//int a = Convert.ToInt32(Console.ReadLine());
//int b = Convert.ToInt32(Console.ReadLine());
//Calling InvokeService method  
InvokeService();


void InvokeService()
{
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