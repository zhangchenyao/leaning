using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioctest
{
    public class Worker : BackgroundService
    {
        private readonly MessageWriter _messageWriter = new MessageWriter();

        protected override async Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            _messageWriter.Write($"Worker running at: {DateTimeOffset.Now}");
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
