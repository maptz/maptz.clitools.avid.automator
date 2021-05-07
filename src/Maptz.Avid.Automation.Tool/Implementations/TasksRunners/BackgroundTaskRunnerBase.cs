using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsHook;
namespace Maptz.Avid.Automation.Tool
{
    public class BackgroundTaskRunner : BackgroundTaskRunnerBase
    {
        public BackgroundTaskRunner(Func<CancellationToken, Task<bool>> action)
        {
            Action = action;
        }

        public Func<CancellationToken, Task<bool>> Action { get; }

        public override async Task<bool> RunAsync()
        {
            await Action.Invoke(CancellationTokenSource.Token);
            return true;
        }
    }

    public abstract class BackgroundTaskRunnerBase : IBackgroundTaskRunner
    {
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public BackgroundTaskRunnerBase()
        {

        }



        public async Task StartAsync()
        {

            if (CancellationTokenSource != null)
            {
                throw new NotImplementedException();
            }

            var cts = new CancellationTokenSource();
            this.CancellationTokenSource = cts;
            var token = CancellationTokenSource.Token;


            var hasCancelled = await OnStarting();

            var doExit = hasCancelled;
            while (!doExit)
            {
                var isComplete = false;
                try
                {
                    isComplete = await RunAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw new Exception("Error running background task", ex);
                }
                
                await Task.Delay(200);
                hasCancelled = token.IsCancellationRequested;
                doExit |= isComplete || token.IsCancellationRequested;
            };
            await OnCompleted(hasCancelled);
            CancellationTokenSource = null;
        }

        protected virtual async Task<bool> OnStarting()
        {
            await Task.CompletedTask;
            return false;
        }

        protected virtual async Task OnCompleted(bool hasCancelled)
        {
            await Task.CompletedTask;
        }

        public bool IsRunning { get => CancellationTokenSource != null; }

        public void Cancel()
        {
            if (CancellationTokenSource == null)
            {
                return;
            }
            CancellationTokenSource.Cancel();
        }

        public abstract Task<bool> RunAsync();

    }
}