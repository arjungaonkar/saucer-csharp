using System;
using System.Collections.Generic;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents a window in the application.
    /// </summary>
    public sealed class Window : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _disposed;
        private Application _application;
        private Dictionary<UIntPtr, GCHandle> _eventHandlers = new();
        private GCHandle _gchHandle;

        #region Events

        public event EventHandler<WindowDecoratedEventArgs>? Decorated;
        public event EventHandler<WindowMaximizeEventArgs>? Maximized;
        public event EventHandler<WindowMinimizeEventArgs>? Minimized;
        public event EventHandler<EventArgs>? Closed;
        public event EventHandler<WindowResizeEventArgs>? Resized;
        public event EventHandler<WindowFocusEventArgs>? FocusChanged;
        public event EventHandler<WindowCloseEventArgs>? Closing;

        #endregion

        /// <summary>
        /// Gets a value indicating whether the window is visible.
        /// </summary>
        public bool IsVisible => NativeMethods.saucer_window_visible(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window has focus.
        /// </summary>
        public bool IsFocused => NativeMethods.saucer_window_focused(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window is minimized.
        /// </summary>
        public bool IsMinimized => NativeMethods.saucer_window_minimized(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window is maximized.
        /// </summary>
        public bool IsMaximized => NativeMethods.saucer_window_maximized(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window is resizable.
        /// </summary>
        public bool IsResizable => NativeMethods.saucer_window_resizable(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window is fullscreen.
        /// </summary>
        public bool IsFullscreen => NativeMethods.saucer_window_fullscreen(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window is always on top.
        /// </summary>
        public bool IsAlwaysOnTop => NativeMethods.saucer_window_always_on_top(_nativeHandle);

        /// <summary>
        /// Gets a value indicating whether the window allows click-through.
        /// </summary>
        public bool IsClickThrough => NativeMethods.saucer_window_click_through(_nativeHandle);

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_window_title(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
            set
            {
                ThrowIfDisposed();
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value));
                NativeMethods.saucer_window_set_title(_nativeHandle, value);
            }
        }

        /// <summary>
        /// Gets the window size in pixels.
        /// </summary>
        public (int Width, int Height) Size
        {
            get
            {
                NativeMethods.saucer_window_size(_nativeHandle, out int w, out int h);
                return (w, h);
            }
        }

        /// <summary>
        /// Gets the window position on the screen.
        /// </summary>
        public (int X, int Y) Position
        {
            get
            {
                NativeMethods.saucer_window_position(_nativeHandle, out int x, out int y);
                return (x, y);
            }
        }

        /// <summary>
        /// Gets the window background color.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                NativeMethods.saucer_window_background(_nativeHandle, out byte r, out byte g, out byte b, out byte a);
                return new Color(r, g, b, a);
            }
            set
            {
                ThrowIfDisposed();
                NativeMethods.saucer_window_set_background(_nativeHandle, value.R, value.G, value.B, value.A);
            }
        }

        internal IntPtr NativeHandle => _nativeHandle;
        internal Application Application => _application;

        private Window(IntPtr handle, Application application)
        {
            _nativeHandle = handle;
            _application = application;
            _gchHandle = GCHandle.Alloc(this);
        }

        internal static Window Create(Application app)
        {
            var handle = NativeMethods.saucer_window_new(app.NativeHandle, out int error);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"Failed to create window. Error code: {error}");

            return new Window(handle, app);
        }

        /// <summary>
        /// Creates a webview in this window.
        /// </summary>
        /// <returns>The created webview</returns>
        public Webview CreateWebview()
        {
            ThrowIfDisposed();
            return Webview.Create(this);
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_show(_nativeHandle);
        }

        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_hide(_nativeHandle);
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_close(_nativeHandle);
        }

        /// <summary>
        /// Focuses the window.
        /// </summary>
        public void Focus()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_focus(_nativeHandle);
        }

        /// <summary>
        /// Sets the window size.
        /// </summary>
        public void SetSize(int width, int height)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_size(_nativeHandle, width, height);
        }

        /// <summary>
        /// Sets the window position.
        /// </summary>
        public void SetPosition(int x, int y)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_position(_nativeHandle, x, y);
        }

        /// <summary>
        /// Sets whether the window is minimized.
        /// </summary>
        public void SetMinimized(bool minimized)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_minimized(_nativeHandle, minimized);
        }

        /// <summary>
        /// Sets whether the window is maximized.
        /// </summary>
        public void SetMaximized(bool maximized)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_maximized(_nativeHandle, maximized);
        }

        /// <summary>
        /// Sets whether the window is fullscreen.
        /// </summary>
        public void SetFullscreen(bool fullscreen)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_fullscreen(_nativeHandle, fullscreen);
        }

        /// <summary>
        /// Sets whether the window is resizable.
        /// </summary>
        public void SetResizable(bool resizable)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_set_resizable(_nativeHandle, resizable);
        }

        /// <summary>
        /// Starts window drag.
        /// </summary>
        public void StartDrag()
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_start_drag(_nativeHandle);
        }

        /// <summary>
        /// Starts window resize from the specified edge.
        /// </summary>
        public void StartResize(WindowEdge edge)
        {
            ThrowIfDisposed();
            NativeMethods.saucer_window_start_resize(_nativeHandle, (NativeMethods.WindowEdge)edge);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Window));
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

            if (_gchHandle.IsAllocated)
                _gchHandle.Free();

            if (_nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_window_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Represents a window edge for resize operations.
    /// </summary>
    [Flags]
    public enum WindowEdge : byte
    {
        Top = 1 << 0,
        Bottom = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        TopLeft = Top | Left,
        TopRight = Top | Right
    }

    #region Event Args

    public class WindowDecoratedEventArgs : EventArgs
    {
        public WindowDecoration Decoration { get; set; }
    }

    public class WindowMaximizeEventArgs : EventArgs
    {
        public bool IsMaximized { get; set; }
    }

    public class WindowMinimizeEventArgs : EventArgs
    {
        public bool IsMinimized { get; set; }
    }

    public class WindowResizeEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class WindowFocusEventArgs : EventArgs
    {
        public bool IsFocused { get; set; }
    }

    public class WindowCloseEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }

    public enum WindowDecoration
    {
        None = 0,
        Partial = 1,
        Full = 2
    }

    #endregion
}
