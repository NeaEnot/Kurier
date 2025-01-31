using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Common.Interfaces
{
    public interface IApplicationLogger<T>
    {
        void LogVerbose(string message, params object[] args);
        void LogDebug(string message, params object[] args);
        void LogInfo(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
        void LogFatal(Exception ex, string message, params object[] args);
    }
}
