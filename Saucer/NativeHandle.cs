using System;

namespace Saucer
{
    /// <summary>
    /// Provides access to native platform handles for advanced interop.
    /// </summary>
    public static class NativeHandle
    {
        /// <summary>
        /// Gets the native handle from an Application.
        /// </summary>
        public static IntPtr GetHandle(Application app)
        {
            return app?.GetType()
                .GetField("_nativeHandle", 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance)
                ?.GetValue(app) as IntPtr? ?? IntPtr.Zero;
        }

        /// <summary>
        /// Gets the native handle from a Window.
        /// </summary>
        public static IntPtr GetHandle(Window window)
        {
            return window?.NativeHandle ?? IntPtr.Zero;
        }

        /// <summary>
        /// Gets the native handle from a Webview.
        /// </summary>
        public static IntPtr GetHandle(Webview webview)
        {
            return webview?.NativeHandle ?? IntPtr.Zero;
        }

        /// <summary>
        /// Gets the native handle from a URL.
        /// </summary>
        public static IntPtr GetHandle(Url url)
        {
            return url?.NativeHandle ?? IntPtr.Zero;
        }

        /// <summary>
        /// Gets the native handle from an Icon.
        /// </summary>
        public static IntPtr GetHandle(Icon icon)
        {
            return icon?.NativeHandle ?? IntPtr.Zero;
        }
    }
}
