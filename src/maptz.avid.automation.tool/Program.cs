using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Maptz.Avid.Automation.Tool
{

    class Program
    {
        static void Main(string[] args)
        {
            AllocConsole();
            

            //See https://github.com/topstarai/WindowsHook

            var sc = new ServiceCollection();
            sc.AddSingleton<BTR>();
            sc.AddSingleton<SoundService>();
            sc.AddSingleton<KeyboardListener>();
            sc.AddSingleton<OutputWriter>();
            var sp = sc.BuildServiceProvider();


            var ow = sp.GetRequiredService<OutputWriter>();
            var kl = sp.GetRequiredService<KeyboardListener>();
            kl.Subscribe();

            ow.WriteLine("Hello world");

            Application.Run(new ApplicationContext());

            kl.Unsubscribe();

            sp.Dispose();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
