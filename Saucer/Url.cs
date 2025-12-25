using System;
using System.Text;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents a URL that can be parsed and manipulated.
    /// </summary>
    public sealed class Url : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _owned;
        private bool _disposed;

        /// <summary>
        /// Gets the complete URL as a string.
        /// </summary>
        public string UrlString
        {
            get
            {
                var sb = new StringBuilder(1024);
                var size = new UIntPtr(1024);
                NativeMethods.saucer_url_string(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the URL scheme (e.g., "http", "https").
        /// </summary>
        public string Scheme
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_url_scheme(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the URL host.
        /// </summary>
        public string? Host
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_url_host(_nativeHandle, sb, ref size);
                return size.ToUInt64() > 0 ? sb.ToString() : null;
            }
        }

        /// <summary>
        /// Gets the URL port if present.
        /// </summary>
        public ushort? Port
        {
            get
            {
                UIntPtr port = UIntPtr.Zero;
                if (NativeMethods.saucer_url_port(_nativeHandle, ref port))
                    return (ushort)port.ToUInt32();
                return null;
            }
        }

        /// <summary>
        /// Gets the URL path.
        /// </summary>
        public string Path
        {
            get
            {
                var sb = new StringBuilder(1024);
                var size = new UIntPtr(1024);
                NativeMethods.saucer_url_path(_nativeHandle, sb, ref size);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the URL user if present.
        /// </summary>
        public string? User
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_url_user(_nativeHandle, sb, ref size);
                return size.ToUInt64() > 0 ? sb.ToString() : null;
            }
        }

        /// <summary>
        /// Gets the URL password if present.
        /// </summary>
        public string? Password
        {
            get
            {
                var sb = new StringBuilder(256);
                var size = new UIntPtr(256);
                NativeMethods.saucer_url_password(_nativeHandle, sb, ref size);
                return size.ToUInt64() > 0 ? sb.ToString() : null;
            }
        }

        internal IntPtr NativeHandle => _nativeHandle;

        /// <summary>
        /// Creates a URL from a string with strict parsing.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if parsing fails</exception>
        public static Url Parse(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var handle = NativeMethods.saucer_url_new_parse(url, out int error);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"Failed to parse URL. Error code: {error}");

            return new Url(handle, owned: true);
        }

        /// <summary>
        /// Creates a URL from a string with lenient parsing.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if parsing fails</exception>
        public static Url From(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var handle = NativeMethods.saucer_url_new_from(url, out int error);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"Failed to create URL. Error code: {error}");

            return new Url(handle, owned: true);
        }

        /// <summary>
        /// Creates a URL from individual components.
        /// </summary>
        public static Url Create(string scheme, string host, ushort? port = null, string path = "/")
        {
            if (string.IsNullOrEmpty(scheme))
                throw new ArgumentNullException(nameof(scheme));

            var portValue = new UIntPtr(port ?? 0);
            var handle = NativeMethods.saucer_url_new_opts(scheme, host ?? "", ref portValue, path ?? "/");
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create URL");

            return new Url(handle, owned: true);
        }

        internal Url(IntPtr handle, bool owned = false)
        {
            _nativeHandle = handle;
            _owned = owned;
        }

        public override string ToString() => UrlString;

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_owned && _nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_url_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
