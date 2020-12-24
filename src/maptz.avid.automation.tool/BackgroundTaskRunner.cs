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

    public abstract class BackgroundTaskRunner
    {
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public BackgroundTaskRunner()
        {
        
        }

        

        public async Task Start()
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
            while(!doExit)
            {
                var isComplete = await DoInLoopAsync();
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

        public abstract Task<bool> DoInLoopAsync();
        
    }
}