using System;

namespace MongoDB.Driver.Util
{
    internal static class DisposableExtensions
    {
        public static void SafeDispose(this IDisposable disposable)
        {
            try
            {
                disposable?.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}