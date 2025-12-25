# Saucer C# Bindings Architecture

## Overview

This project provides a complete C# wrapper around the Saucer C bindings. The architecture is designed to balance safety, performance, and usability.

## Structure

### Layer 1: P/Invoke Interop (`Interop/NativeMethods.cs`)

Direct mapping of the C API:

- **P/Invoke Declarations** - `DllImport` attributes for all Saucer C functions
- **Native Types** - Enumerations and delegates matching the C ABI
- **Marshaling** - Proper handling of strings, arrays, and opaque handles

Key decisions:
- Use `CallingConvention.Cdecl` for C function calls
- Opaque pointers represented as `IntPtr`
- String marshaling uses UTF-8 encoding (default for .NET)

### Layer 2: Managed Wrappers

#### `Application`

Root object for Saucer applications:

```
Application
  ├─ Window
  │   ├─ Webview
  │   └─ Events
  ├─ Screens
  └─ Event Loop
```

Responsibilities:
- Application lifecycle management
- Window creation
- Event loop execution
- Resource cleanup

#### `Window`

Top-level window container:

- Window properties (title, size, position)
- Webview hosting
- Window events (resize, focus, close)
- Native platform integration

#### `Webview`

Web content renderer:

- HTML/URL loading
- JavaScript execution
- Navigation history
- Custom scheme handlers
- Permission management
- Event system for web content

### Layer 3: Supporting Types

#### `Url`

Safe URL parsing:

- Parse URLs with strict validation
- Component extraction (scheme, host, path, etc.)
- URL construction from components
- Proper memory management of parsed data

#### `Icon`

Window icon handling:

- Load from file
- Create from binary data
- Save to file
- Memory-safe wrapper

#### `Screen`

Display information:

- Screen dimensions
- Position on virtual desktop
- Display name

#### `Color`

RGBA color representation:

- Value type for efficiency
- Hex string conversion
- Predefined colors
- Hash code and equality

## Memory Management

### Ownership Model

```
C Ownership      → C#Ownership     → Responsibility
─────────────────────────────────────────────────
saucer_*_free()  → IDisposable     → C# must call free
Managed by C     → Reference       → C# holds reference
```

### GCHandle Usage

GCHandles are used for callback functions:

```csharp
var gch = GCHandle.Alloc(managedObject);
var unmanagedPtr = GCHandle.ToIntPtr(gch);

// In C callback:
var obj = GCHandle.FromIntPtr(userData).Target;

// Cleanup:
gch.Free();
```

### Resource Cleanup Pattern

```csharp
public void Dispose()
{
    if (_disposed) return;
    
    // 1. Stop event listeners
    UnsubscribeAll();
    
    // 2. Free GCHandles
    FreeGCHandles();
    
    // 3. Dispose child objects
    DisposeChildren();
    
    // 4. Call native free
    if (_handle != IntPtr.Zero)
    {
        NativeMethods.saucer_*_free(_handle);
        _handle = IntPtr.Zero;
    }
    
    _disposed = true;
}
```

## Event System

### Event Marshaling

C callbacks → Managed delegates → .NET events:

```
C Callback                 Managed Delegate          .NET Event
───────────                ────────────────          ──────────
saucer_window_event_close  WindowCloseCallback       Window.Closing
  (native pointer)         (IntPtr userdata)        (EventArgs)
  (can return policy)      (must free GCHandle)     (can cancel)
```

### Event Handler Lifecycle

```csharp
// Registration
var gch = GCHandle.Alloc(nativeCallback);
var id = saucer_window_on(..., gch);
_handlers[id] = gch;

// Invocation (in callback)
var callback = GCHandle.FromIntPtr(userData);
callback.Invoke(...);

// Cleanup
gch.Free();
```

## Thread Safety

### Single-Threaded Guarantee

Saucer requires all API calls on the main thread:

- Event handlers are called on main thread
- Check `Application.IsThreadSafe`
- Use `Application.Post()` for background thread callbacks

### Implementation

```csharp
public void Post(Action callback)
{
    var gch = GCHandle.Alloc(callback);
    NativeMethods.saucer_application_post(
        _handle,
        (userdata) => {
            try {
                GCHandle.FromIntPtr(userdata).Target.Invoke();
            } finally {
                GCHandle.FromIntPtr(userdata).Free();
            }
        },
        GCHandle.ToIntPtr(gch)
    );
}
```

## Error Handling

### Error Codes

C functions return error codes via `out` parameters:

```csharp
var handle = NativeMethods.saucer_application_new(options, out int error);
if (handle == IntPtr.Zero)
    throw new InvalidOperationException($"Error: {error}");
```

### Exception Strategy

- Throw `InvalidOperationException` for API errors
- Throw `ObjectDisposedException` for disposed objects
- Throw `ArgumentNullException` for null arguments
- Throw `ArgumentException` for invalid arguments

## Performance Considerations

### String Marshaling

- Strings are marshaled via `StringBuilder` for output
- Pre-allocate buffers with reasonable capacity (256-1024 bytes)
- Handle size returned via `ref UIntPtr`

### Callback Overhead

- GCHandles have allocation overhead
- Event handlers cache native callbacks
- Unsubscribe removes handlers and frees memory

### Pointer Arithmetic

Screen array marshaling:

```csharp
for (int i = 0; i < count; i++)
{
    var ptr = Marshal.ReadIntPtr(screensPtr, i * IntPtr.Size);
    // Use ptr...
}
```

## API Design Principles

1. **Idiomatic C#** - Use C# conventions (PascalCase, properties, events)
2. **Safe by Default** - Validation, null checks, proper disposal
3. **Clear Ownership** - Comments indicate who owns resources
4. **Complete Coverage** - All C API functions have C# equivalents
5. **Type Safety** - Use enums instead of raw integers
6. **Discoverable** - XML documentation on public APIs

## Testing

### Unit Tests

Test P/Invoke declarations:

```csharp
[Test]
public void ApplicationCreate_Success()
{
    var app = Application.Create("test");
    Assert.NotNull(app);
    app.Dispose();
}
```

### Integration Tests

Test workflows:

```csharp
[Test]
public void WindowCreation_Workflow()
{
    using var app = Application.Create("test");
    using var window = app.CreateWindow();
    window.SetSize(800, 600);
    Assert.Equals((800, 600), window.Size);
}
```

## Roadmap

- [ ] NuGet package distribution
- [ ] Async/await support for long operations
- [ ] Data binding helpers
- [ ] Higher-level abstractions (MVVM support)
- [ ] Logging integration
- [ ] Performance benchmarks
- [ ] More comprehensive examples
