using Maptz.Editing.Avid.Markers;
using Maptz.Editing.Avid.MarkerSections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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


    public class WorkEngine : IWorkEngine
    {
        public WorkEngine(IOutputWriter outputWriter, IServiceProvider serviceProvider)
        {
            OutputWriter = outputWriter;
            ServiceProvider = serviceProvider;
        }

        private Queue<IBackgroundTaskRunner> TaskQueue { get; } = new Queue<IBackgroundTaskRunner>();
        private IBackgroundTaskRunner CurrentTask { get; set; }
        public IOutputWriter OutputWriter { get; }
        public IServiceProvider ServiceProvider { get; }


        public bool IsRunning { get; private set; }
        public void Stop()
        {
            OutputWriter.WriteLine("Stopping all background tasks");
            TaskQueue.Clear();
            if (CurrentTask != null)
            {
                CurrentTask.Cancel();
            }
        }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }
            
            //var looper = ServiceProvider.GetRequiredService<LoopingTyperRunner>();
            var patternRepeater = new PatternRepeater();
            var soundService = new SoundService();
            var backgroundTask = new BackgroundTaskRunner(async ct =>
            {
                soundService.Play(SoundServiceSound.Start);
                await patternRepeater.RunAsync(ct);
                soundService.Play(SoundServiceSound.End);
                return true;
            });
            TaskQueue.Enqueue(backgroundTask);

            if (!IsRunning)
            {
                DoDequeueLoop().GetAwaiter();
            }
        }

        private async Task DoDequeueLoop()
        {
            IsRunning = true;
            while (TaskQueue.Any())
            {
                if (CurrentTask == null)
                {
                    CurrentTask = TaskQueue.Dequeue();
                    try
                    {
                        CurrentTask.StartAsync().GetAwaiter();
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Error running queue", ex);
                    }
                    
                }
                else
                {
                    if (!CurrentTask.IsRunning)
                    {
                        CurrentTask = null;
                    }
                }
                await Task.Delay(200);
            }
            IsRunning = false;
        }

       
    }
}