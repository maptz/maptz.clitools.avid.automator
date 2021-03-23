using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Maptz.Avid.Automation.Tool
{

    public class OutputWriter : IOutputWriter
    {
        public void WriteLine(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
            Console.WriteLine(str);
        }
    }
}