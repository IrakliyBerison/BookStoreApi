using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace BookStoreApi.Repositories
{
    public class BookStoreLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public BookStoreLogger(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<BookStoreLogger>();
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch watch = new Stopwatch();
            string responseBody = "";
            watch.Start();
            Stream originalBody = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    memStream.Position = 0;
                    responseBody = new StreamReader(memStream).ReadToEnd();

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
                watch.Stop();
                _logger.LogInformation($"Время выполнения запроса {watch.ElapsedMilliseconds} миллисекунд. {Environment.NewLine} Запрос к {context.Request.Path}. {Environment.NewLine} Входные параметры :{context.Request.QueryString}. {Environment.NewLine} Ответ {responseBody}");
            }
        }
    }
}
