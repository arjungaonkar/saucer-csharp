using System;
using System.Collections.Generic;
using System.Text;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents a webview component embedded in a window.
    /// </summary>
    public sealed class Webview : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _disposed;
        private Window _window;
        private Dictionary<UIntPtr, GCHandle> _eventHandlers = new();
        private Dictionary<string, NativeMethods.SchemeHandler> _schemeHandlers = new();
        private GCHandle _gchHandle;

        #region Events

        public event EventHandler<WebviewPermissionEventArgs>? PermissionRequested;
        public event EventHandler<WebviewFullscreenEventArgs>? FullscreenRequested;
        public event EventHandler<EventArgs>? DomReady;
        public event EventHandler<WebviewNavigatedEventArgs>? Navigated;
        public event EventHandler<WebviewNavigateEventArgs>? Navigate;
        public event EventHandler<WebviewMessageEventArgs>? MessageReceived;
        public event EventHandler<WebviewRequestEventArgs>? RequestReceived;
        public event EventHandler<WebviewFaviconEventArgs>? FaviconChanged;
        public event EventHandler<WebviewTitleEventArgs>? TitleChanged;
        public event EventHandler<WebviewLoadEventArgs>? LoadStateChanged;

        #endregion

        /// <summary>
        /// Gets or sets whether developer tools are enabled.
        /// </summary>
        public bool DevToolsEnabled
        {
            get => NativeMethods.saucer_webview_dev_tools(_nativeHandle);
            set
            {
                ThrowIfDisposed();
                NativeMethods.saucer_webview_set_dev_tools(_nativeHandle, value);
            }
        }

        /// <summary>
        /// Gets or sets whether context menu is enabled.
        /// </summary>
        public bool ContextMenuEnabled
        {
            get => NativeMethods.saucer_webview_context_menu(_nativeHandle);
            set
            {
                ThrowIfDisposed();
                NativeMethods.saucer_webview_set_context_menu(_nativeHandle, value);
            }
        }

        /// <summary>
        /// Gets or sets whether force dark mode is enabled.
        /// </summary>
        public bool ForceDarkMode
        {
            get => NativeMethods.saucer_webview_force_dark(_nativeHandle);
            set
            {
                ThrowIfDisposed();
                NativeMethods.saucer_webview_set_force_dark(_nativeHandle, value);
            }
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                NativeMethods.saucer_webview_background(_nativeHandle, out byte r, out byte g, out byte b, out byte a);
                return new Color(r, g, b, a);
            }
            set
            {
                ThrowIfDisposed();
                NativeMethods.saucer_webview_set_background(_nativeHandle, value.R, value.G, value.B, value.A);
            }
        }

        /// <summary>
        /// Gets the current page title.
        /// </summary>
        public string PageTitle
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_webview_page_title(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
        }

        internal IntPtr NativeHandle => _nativeHandle;

        private Webview(IntPtr handle, Window window)
        {
            _nativeHandle = handle;
            _window = window;
            _gchHandle = GCHandle.Alloc(this);
        }

        internal static Webview Create(Window window)
        {
            var options = NativeMethods.saucer_webview_options_new(window.NativeHandle);
            try
            {
                var handle = NativeMethods.saucer_webview_new(options, out int error);
                if (handle == IntPtr.Zero)
                    throw new InvalidOperationException($"Failed to create webview. Error code: {error}");

                return new Webview(handle, window);
            }
            finally
            {
                NativeMethods.saucer_webview_options_free(options);
            }
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        public void Navigate(string url)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            NativeMethods.saucer_webview_set_url_str(_nativeHandle, url);
        }

        /// <summary>
        /// Loads HTML content into the webview.
        /// </summary>
        public void LoadHtml(string html)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(html))
                throw new ArgumentNullException(nameof(html));
            NativeMethods.saucer_webview_set_html(_nativeHandle, html);
        }

        /// <summary>
        /// Navigates back in the browsing history.
        /// </summary>
        public void GoBack()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_webview_back(_nativeHandle);
        }

        /// <summary>
        /// Navigates forward in the browsing history.
        /// </summary>
        public void GoForward()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_webview_forward(_nativeHandle);
        }

        /// <summary>
        /// Reloads the current page.
        /// </summary>
        public void Reload()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_webview_reload(_nativeHandle);
        }

        /// <summary>
        /// Executes JavaScript code in the webview.
        /// </summary>
        public void ExecuteJavaScript(string code)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            NativeMethods.saucer_webview_execute(_nativeHandle, code);
        }

        /// <summary>
        /// Injects JavaScript code that runs at a specific time.
        /// </summary>
        /// <param name="code">The JavaScript code to inject</param>
        /// <param name="runAt">When to run the script</param>
        /// <param name="noFrames">Whether to skip frames</param>
        /// <returns>The injection ID</returns>
        public ulong InjectJavaScript(string code, ScriptTime runAt = ScriptTime.Ready, bool noFrames = false)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            var id = NativeMethods.saucer_webview_inject(_nativeHandle, code, (NativeMethods.ScriptTime)runAt, noFrames, true);
            return id.ToUInt64();
        }

        /// <summary>
        /// Removes an injected script.
        /// </summary>
        public void RemoveInjectedScript(ulong id)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_webview_uninject(_nativeHandle, new UIntPtr(id));
        }

        /// <summary>
        /// Removes all injected scripts.
        /// </summary>
        public void RemoveAllInjectedScripts()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_webview_uninject_all(_nativeHandle);
        }

        /// <summary>
        /// Registers a custom scheme handler.
        /// </summary>
        public void RegisterSchemeHandler(string scheme, Action<SchemeRequest, SchemeExecutor> handler)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(scheme))
                throw new ArgumentNullException(nameof(scheme));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            NativeMethods.SchemeHandler nativeHandler = (requestPtr, executorPtr) =>
            {
                var request = new SchemeRequest(requestPtr);
                var executor = new SchemeExecutor(executorPtr);
                handler(request, executor);
            };

            _schemeHandlers[scheme] = nativeHandler;
            NativeMethods.saucer_webview_handle_scheme(_nativeHandle, scheme, nativeHandler);
        }

        /// <summary>
        /// Removes a custom scheme handler.
        /// </summary>
        public void UnregisterSchemeHandler(string scheme)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(scheme))
                throw new ArgumentNullException(nameof(scheme));
            _schemeHandlers.Remove(scheme);
            NativeMethods.saucer_webview_remove_scheme(_nativeHandle, scheme);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Webview));
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
            _schemeHandlers.Clear();

            if (_gchHandle.IsAllocated)
                _gchHandle.Free();

            if (_nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_webview_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }

    public enum ScriptTime
    {
        Creation,
        Ready
    }

    #region Event Args

    public class WebviewPermissionEventArgs : EventArgs
    {
        public required string Url { get; init; }
        public required PermissionType Permission { get; init; }
        public bool Allow { get; set; }
    }

    public class WebviewFullscreenEventArgs : EventArgs
    {
        public bool IsFullscreen { get; set; }
        public bool Allow { get; set; }
    }

    public class WebviewNavigatedEventArgs : EventArgs
    {
        public required string Url { get; init; }
    }

    public class WebviewNavigateEventArgs : EventArgs
    {
        public required string Url { get; init; }
        public bool IsNewWindow { get; init; }
        public bool IsRedirection { get; init; }
        public bool IsUserInitiated { get; init; }
        public bool Allow { get; set; }
    }

    public class WebviewMessageEventArgs : EventArgs
    {
        public required string Message { get; init; }
    }

    public class WebviewRequestEventArgs : EventArgs
    {
        public required string Url { get; init; }
    }

    public class WebviewFaviconEventArgs : EventArgs
    {
        public required byte[] Data { get; init; }
    }

    public class WebviewTitleEventArgs : EventArgs
    {
        public required string Title { get; init; }
    }

    public class WebviewLoadEventArgs : EventArgs
    {
        public bool IsFinished { get; init; }
    }

    [Flags]
    public enum PermissionType : byte
    {
        Unknown = 0,
        AudioMedia = 1 << 0,
        VideoMedia = 1 << 1,
        DesktopMedia = 1 << 2,
        MouseLock = 1 << 3,
        DeviceInfo = 1 << 4,
        Location = 1 << 5,
        Clipboard = 1 << 6,
        Notification = 1 << 7
    }

    #endregion

    #region Scheme Support

    public sealed class SchemeRequest
    {
        private IntPtr _nativeHandle;

        public string Url
        {
            get
            {
                var urlPtr = NativeMethods.saucer_scheme_request_url(_nativeHandle);
                return new Url(urlPtr).ToString();
            }
        }

        public string Method
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_scheme_request_method(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
        }

        public byte[] Content
        {
            get
            {
                var stashPtr = NativeMethods.saucer_scheme_request_content(_nativeHandle);
                var size = NativeMethods.saucer_stash_size(stashPtr);
                var dataPtr = NativeMethods.saucer_stash_data(stashPtr);
                var result = new byte[size.ToUInt64()];
                Marshal.Copy(dataPtr, result, 0, (int)size.ToUInt64());
                return result;
            }
        }

        internal SchemeRequest(IntPtr handle)
        {
            _nativeHandle = handle;
        }
    }

    public sealed class SchemeExecutor
    {
        private IntPtr _nativeHandle;

        public void Reject(SchemeError error = SchemeError.Failed)
        {
            NativeMethods.saucer_scheme_executor_reject(_nativeHandle, (NativeMethods.SchemeError)error);
        }

        public void Accept(byte[] data, string mimeType = "application/octet-stream")
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var stash = NativeMethods.saucer_stash_new_from(handle.AddrOfPinnedObject(), new UIntPtr((uint)data.Length));
                var response = NativeMethods.saucer_scheme_response_new(stash, mimeType);
                NativeMethods.saucer_scheme_executor_accept(_nativeHandle, response);
            }
            finally
            {
                handle.Free();
            }
        }

        internal SchemeExecutor(IntPtr handle)
        {
            _nativeHandle = handle;
        }
    }

    public enum SchemeError : short
    {
        NotFound = 404,
        Invalid = 400,
        Denied = 401,
        Failed = -1
    }

    #endregion
}
