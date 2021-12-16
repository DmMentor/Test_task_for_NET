using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewTask.ConsoleApp
{
    class ConsoleHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private ConsoleApp _consoleApp;
        private int _exitcode = 0;

        public ConsoleHostedService(IHostApplicationLifetime appLifetime, ConsoleApp consoleApp)
        {
            _appLifetime = appLifetime;
            _consoleApp = consoleApp;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(() =>
            {
                try
                {
                    _consoleApp.Run();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    _exitcode = 1;
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = _exitcode;
            return Task.CompletedTask;
        }
    }
}
