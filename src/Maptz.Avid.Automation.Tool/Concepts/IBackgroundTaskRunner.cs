using System.Threading;
using System.Threading.Tasks;
namespace Maptz.Avid.Automation.Tool
{

    public interface IBackgroundTaskRunner
    {
        /* #region Public Properties */
        bool IsRunning { get; }
        /* #endregion Public Properties */
        /* #region Public Methods */
        void Cancel();
        Task<bool> RunAsync(CancellationToken ct);
        Task StartAsync();
        /* #endregion Public Methods */
    }
}