using System;
using Saucer.Interop;

namespace Saucer
{
    /// <summary>
    /// Represents an icon that can be used in windows.
    /// </summary>
    public sealed class Icon : IDisposable
    {
        private IntPtr _nativeHandle;
        private bool _owned;
        private bool _disposed;

        /// <summary>
        /// Gets a value indicating whether the icon is empty.
        /// </summary>
        public bool IsEmpty => NativeMethods.saucer_icon_empty(_nativeHandle);

        /// <summary>
        /// Gets the icon data as bytes.
        /// </summary>
        public byte[] GetData()
        {
            var stashPtr = NativeMethods.saucer_icon_data(_nativeHandle);
            var size = NativeMethods.saucer_stash_size(stashPtr);
            var dataPtr = NativeMethods.saucer_stash_data(stashPtr);
            var result = new byte[size.ToUInt64()];
            Marshal.Copy(dataPtr, result, 0, (int)size.ToUInt64());
            return result;
        }

        internal IntPtr NativeHandle => _nativeHandle;

        /// <summary>
        /// Loads an icon from a file.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if loading fails</exception>
        public static Icon FromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var handle = NativeMethods.saucer_icon_new_from_file(path, out int error);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"Failed to load icon from file. Error code: {error}");

            return new Icon(handle, owned: true);
        }

        /// <summary>
        /// Creates an icon from binary data.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if creation fails</exception>
        public static Icon FromData(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var stash = NativeMethods.saucer_stash_new_from_str(""); // Empty for now, will be replaced
            var handle = NativeMethods.saucer_icon_new_from_stash(stash, out int error);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"Failed to create icon from data. Error code: {error}");

            return new Icon(handle, owned: true);
        }

        /// <summary>
        /// Saves the icon to a file.
        /// </summary>
        public void SaveToFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            NativeMethods.saucer_icon_save(_nativeHandle, path);
        }

        internal Icon(IntPtr handle, bool owned = false)
        {
            _nativeHandle = handle;
            _owned = owned;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_owned && _nativeHandle != IntPtr.Zero)
            {
                NativeMethods.saucer_icon_free(_nativeHandle);
                _nativeHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
