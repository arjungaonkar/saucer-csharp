using System;
using System.Collections.Generic;
using System.Linq;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents a Saucer application instance.
    /// </summary>
    public sealed class Application : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _disposed;
        private List<Window> _windows = new();
        private Dictionary<UIntPtr, GCHandle> _eventHandlers = new();
        private GCHandle _gchHandle;

        private event EventHandler<ApplicationQuitEventArgs>? _onQuit;
        private NativeMethods.ApplicationQuitCallback? _quitCallback;

        /// <summary>
        /// Occurs when the application is about to quit.
        /// </summary>
        public event EventHandler<ApplicationQuitEventArgs>? Quit
        {
            add
            {
                _onQuit += value;
                if (_onQuit?.GetInvocationList().Length == 1)
                {
                    RegisterQuitHandler();
                }
            }
            remove => _onQuit -= value;
        }

        /// <summary>
        /// Gets the version of Saucer.
        /// </summary>
        public static string Version
        {
            get
            {
                var versionPtr = NativeMethods.saucer_version();
                return Marshal.PtrToStringAnsi(versionPtr) ?? "unknown";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the application is thread-safe.
        /// </summary>
        public bool IsThreadSafe => NativeMethods.saucer_application_thread_safe(_nativeHandle);

        /// <summary>
        /// Gets the list of screens.
        /// </summary>
        public IReadOnlyList<Screen> Screens
        {
            get
            {
                NativeMethods.saucer_application_screens(_nativeHandle, out var screensPtr, out var size);
                var result = new List<Screen>();

                if (screensPtr != IntPtr.Zero && size.ToUInt64() > 0)
                {
                    for (int i = 0; i < (int)size.ToUInt64(); i++)
                    {
                        var screenPtr = Marshal.ReadIntPtr(screensPtr, i * IntPtr.Size);
                        result.Add(new Screen(screenPtr));
                    }
                }

                return result.AsReadOnly();
            }
        }

        private Application(IntPtr handle)
        {
            _nativeHandle = handle;
            _gchHandle = GCHandle.Alloc(this);
        }

        /// <summary>
        /// Creates a new application instance.
        /// </summary>
        /// <param name="id">The application ID</param>
        /// <returns>The created application</returns>
        /// <exception cref="InvalidOperationException">Thrown if initialization fails</exception>
        public static Application Create(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var options = NativeMethods.saucer_application_options_new(id);
            try
            {
                var handle = NativeMethods.saucer_application_new(options, out int error);
                if (handle == IntPtr.Zero)
                    throw new InvalidOperationException($"Failed to create application. Error code: {error}");

                return new Application(handle);
            }
            finally
            {
                NativeMethods.saucer_application_options_free(options);
            }
        }

        /// <summary>
        /// Creates a new window for this application.
        /// </summary>
        /// <returns>The created window</returns>
        public Window CreateWindow()
        {
            ThrowIfDisposed();
            var window = Window.Create(this);
            _windows.Add(window);
            return window;
        }

        /// <summary>
        /// Posts a callback to be executed on the main thread.
        /// </summary>
        /// <param name="callback">The callback to execute</param>
        public void Post(Action callback)
        {
            ThrowIfDisposed();
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var gch = GCHandle.Alloc(callback);
            NativeMethods.PostCallback nativeCallback = (userdata) =>
            {
                try
                {
                    var cb = GCHandle.FromIntPtr(userdata).Target as Action;
                    cb?.Invoke();
                }
                finally
                {
                    GCHandle.FromIntPtr(userdata).Free();
                }
            };

            NativeMethods.saucer_application_post(_nativeHandle, nativeCallback, GCHandle.ToIntPtr(gch));
        }

        /// <summary>
        /// Runs the application event loop.
        /// </summary>
        /// <param name="onReady">Callback when application is ready</param>
        /// <returns>The exit code</returns>
        public int Run(Action? onReady = null)
        {
            ThrowIfDisposed();

            NativeMethods.RunCallback? runCb = null;
            NativeMethods.FinishCallback? finishCb = null;

            if (onReady != null)
            {
                runCb = (app, userdata) => onReady();
            }

            return NativeMethods.saucer_application_run(_nativeHandle, runCb, finishCb, IntPtr.Zero);
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void Quit()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_application_quit(_nativeHandle);
        }

        private void RegisterQuitHandler()
        {
            _quitCallback = (app, userdata) =>
            {
                var args = new ApplicationQuitEventArgs();
                _onQuit?.Invoke(this, args);
                return args.Cancel ? NativeMethods.Policy.Block : NativeMethods.Policy.Allow;
            };

            var nativePtr = Marshal.GetFunctionPointerForDelegate(_quitCallback);
            var id = NativeMethods.saucer_application_on(
                _nativeHandle,
                NativeMethods.ApplicationEvent.Quit,
                nativePtr,
                true,
                GCHandle.ToIntPtr(_gchHandle)
            );
            _eventHandlers[id] = GCHandle.Alloc(_quitCallback);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Application));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var handler in _eventHandlers.Values)
            {
                if (handler.IsAllocated)
                    handler.Free();
            }
            _eventHandlers.Clear();

            foreach (var window in _windows)
            {
                window?.Dispose();
            }
            _windows.Clear();

            if (_gchHandle.IsAllocated)
                _gchHandle.Free();

            if (_nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_application_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Event arguments for application quit event.
    /// </summary>
    public class ApplicationQuitEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether to cancel the quit.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
