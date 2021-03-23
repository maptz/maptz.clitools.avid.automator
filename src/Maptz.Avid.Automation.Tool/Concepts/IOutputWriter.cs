using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Maptz.Avid.Automation.Tool
{
    public interface IOutputWriter
    {
        void WriteLine(string str);
    }
}