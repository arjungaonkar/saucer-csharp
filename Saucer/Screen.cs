using System;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents a display screen.
    /// </summary>
    public sealed class Screen : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _disposed;

        /// <summary>
        /// Gets the screen name.
        /// </summary>
        public string Name
        {
            get
            {
                var namePtr = NativeMethods.saucer_screen_name(_nativeHandle);
                return Marshal.PtrToStringAnsi(namePtr) ?? "Unknown";
            }
        }

        /// <summary>
        /// Gets the screen size in pixels.
        /// </summary>
        public (int Width, int Height) Size
        {
            get
            {
                NativeMethods.saucer_screen_size(_nativeHandle, out int w, out int h);
                return (w, h);
            }
        }

        /// <summary>
        /// Gets the screen position on the virtual desktop.
        /// </summary>
        public (int X, int Y) Position
        {
            get
            {
                NativeMethods.saucer_screen_position(_nativeHandle, out int x, out int y);
                return (x, y);
            }
        }

        internal Screen(IntPtr handle)
        {
            _nativeHandle = handle;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_screen_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
