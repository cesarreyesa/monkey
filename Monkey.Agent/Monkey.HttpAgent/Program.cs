using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Kayak;
using Kayak.Http;
using Monkey.HttpAgent.Commands;

namespace Monkey.HttpAgent
{
   class Program
   {
      private static readonly Repository Repository = new Repository();

      static void Main(string[] args)
      {
         Repository.CreateDbIfNotExists();

         var scheduler = new KayakScheduler(new SchedulerDelegate());
         scheduler.Post(() => KayakServer.Factory
                                 .CreateHttp(new RequestDelegate())
                                 .Listen(new IPEndPoint(IPAddress.Any, 8080)));

         // runs scheduler on calling thread. this method will block until
         // someone calls Stop() on the scheduler.
         scheduler.Start();
      }

      class SchedulerDelegate : ISchedulerDelegate
      {
         public void OnException(IScheduler scheduler, Exception e)
         {
            Debug.WriteLine("Error on scheduler.");
         }

         public void OnStop(IScheduler scheduler)
         {

         }
      }

      class RequestDelegate : IHttpRequestDelegate
      {
         public void OnRequest(HttpRequestHead request, IDataProducer requestBody,
             IHttpResponseDelegate response)
         {
            if(request.Uri == "/favicon.ico") return;

            var actionName = request.Uri.Substring(1);
            var action = Repository.GetAction(actionName);
            action.Command.Run();
            var headers = new HttpResponseHead()
                                          {
                                             Status = "200 OK",
                                             Headers = new Dictionary<string, string>() 
                                                          {
                                                             { "Content-Type", "text/plain" },
                                                             { "Content-Length", "20" },
                                                          }
                                          };
            IDataProducer body = new BufferedBody("Hello world.\r\nHello.");
            response.OnResponse(headers, body);

         }
      }

      class BufferedBody : IDataProducer
      {
         readonly ArraySegment<byte> data;

         public BufferedBody(string data) : this(data, Encoding.UTF8) { }
         private BufferedBody(string data, Encoding encoding) : this(encoding.GetBytes(data)) { }
         private BufferedBody(byte[] data) : this(new ArraySegment<byte>(data)) { }

         private BufferedBody(ArraySegment<byte> data)
         {
            this.data = data;
         }

         public IDisposable Connect(IDataConsumer channel)
         {
            // null continuation, consumer must swallow the data immediately.
            channel.OnData(data, null);
            channel.OnEnd();
            return null;
         }
      }
   }
}
