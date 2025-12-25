# Saucer C# Bindings

**Production-ready C# wrapper for the Saucer webview framework**

This project provides complete P/Invoke interop and idiomatic .NET bindings for [Saucer](https://github.com/saucer/saucer), a lightweight cross-platform webview framework.

## Features

✅ **Full C API Coverage** - All Saucer C bindings mapped to P/Invoke

✅ **Idiomatic .NET API** - Events, properties, and exception handling

✅ **Memory Management** - Proper resource disposal and lifetime management

✅ **Event Support** - Application, Window, and Webview event systems

✅ **Custom Scheme Handlers** - Register custom URL schemes

✅ **JavaScript Integration** - Execute and inject scripts

✅ **Cross-Platform** - Windows, macOS, Linux via Saucer runtime

## Installation

### Prerequisites

- .NET 6.0 or higher
- Saucer runtime library (`libsaucer.so`, `libsaucer.dylib`, or `saucer.dll`)

### Adding to Your Project

```bash
git clone https://github.com/arjungaonkar/saucer-csharp
cd saucer-csharp
dotnet build
```

Or reference the NuGet package (when available):

```bash
dotnet add package Saucer.Bindings
```

## Quick Start

### Creating a Simple Application

```csharp
using Saucer;

var app = Application.Create("com.example.myapp");
var window = app.CreateWindow();
var webview = window.CreateWebview();

window.Title = "Hello Saucer";
window.SetSize(1280, 720);

webview.Navigate("https://example.com");
webview.DevToolsEnabled = true;

window.Show();

app.Quit += (sender, e) => {
    Console.WriteLine("Application closing");
};

app.Run();

window.Dispose();
app.Dispose();
```

### Loading HTML Content

```csharp
var html = """
<html>
  <head><title>Hello</title></head>
  <body>
    <h1>Hello from C#!</h1>
    <button onclick="callCSharp()">Click Me</button>
  </body>
</html>
""";

webview.LoadHtml(html);
```

### Executing JavaScript

```csharp
webview.ExecuteJavaScript("console.log('Hello from C#')");

var injectionId = webview.InjectJavaScript(
    "window.myVariable = 42;",
    ScriptTime.Ready,
    noFrames: false
);

// Later: remove the injected script
webview.RemoveInjectedScript(injectionId);
```

### Handling Events

```csharp
window.Resized += (sender, e) => {
    Console.WriteLine($"Window resized to {e.Width}x{e.Height}");
};

window.Closing += (sender, e) => {
    if (!AllowClose)
        e.Cancel = true;
};

window.FocusChanged += (sender, e) => {
    Console.WriteLine($"Focus: {e.IsFocused}");
};

webview.TitleChanged += (sender, e) => {
    window.Title = e.Title;
};

webview.DomReady += (sender, e) => {
    Console.WriteLine("DOM is ready");
};

webview.LoadStateChanged += (sender, e) => {
    Console.WriteLine($"Load finished: {e.IsFinished}");
};
```

### Custom Scheme Handlers

```csharp
webview.RegisterSchemeHandler("app", (request, executor) => {
    var content = Encoding.UTF8.GetBytes("<h1>Hello</h1>");
    executor.Accept(content, "text/html");
});

// Now you can navigate to: app://anything
webview.Navigate("app://index");
```

### Handling Permissions

```csharp
webview.PermissionRequested += (sender, e) => {
    if (e.Permission == PermissionType.Clipboard)
    {
        e.Allow = true;
    }
};
```

## API Reference

### Core Classes

#### Application

- `static Application.Create(string id)` - Create application instance
- `Window CreateWindow()` - Create a window
- `void Run(Action? onReady = null)` - Start event loop
- `void Quit()` - Exit application
- `void Post(Action callback)` - Post callback to main thread
- `IReadOnlyList<Screen> Screens` - Get connected screens
- `bool IsThreadSafe` - Check thread safety
- `event EventHandler<ApplicationQuitEventArgs>? Quit` - Quit event
- `string Version` - Get Saucer version

#### Window

Properties:
- `string Title` - Window title
- `(int Width, int Height) Size` - Window dimensions
- `(int X, int Y) Position` - Window position
- `Color BackgroundColor` - Background color
- `bool IsVisible` - Visibility state
- `bool IsFocused` - Focus state
- `bool IsMinimized` - Minimized state
- `bool IsMaximized` - Maximized state
- `bool IsFullscreen` - Fullscreen state
- `bool IsResizable` - Resizable state
- `bool IsAlwaysOnTop` - Always on top state
- `bool IsClickThrough` - Click-through state

Methods:
- `Webview CreateWebview()` - Create webview
- `void Show()` - Show window
- `void Hide()` - Hide window
- `void Close()` - Close window
- `void Focus()` - Focus window
- `void SetSize(int width, int height)`
- `void SetPosition(int x, int y)`
- `void SetMinimized(bool minimized)`
- `void SetMaximized(bool maximized)`
- `void SetFullscreen(bool fullscreen)`
- `void SetResizable(bool resizable)`
- `void StartDrag()` - Start window drag
- `void StartResize(WindowEdge edge)` - Start resize

Events:
- `Decorated` - Decoration changed
- `Maximized` - Maximized state changed
- `Minimized` - Minimized state changed
- `Closed` - Window closed
- `Resized` - Window resized
- `FocusChanged` - Focus changed
- `Closing` - Before close (can cancel)

#### Webview

Properties:
- `bool DevToolsEnabled` - Developer tools
- `bool ContextMenuEnabled` - Context menu
- `bool ForceDarkMode` - Force dark mode
- `Color BackgroundColor` - Background color
- `string PageTitle` - Current page title

Methods:
- `void Navigate(string url)` - Navigate to URL
- `void LoadHtml(string html)` - Load HTML content
- `void GoBack()` - Navigate back
- `void GoForward()` - Navigate forward
- `void Reload()` - Reload page
- `void ExecuteJavaScript(string code)` - Execute JS
- `ulong InjectJavaScript(string code, ScriptTime runAt, bool noFrames)` - Inject JS
- `void RemoveInjectedScript(ulong id)` - Remove injected script
- `void RemoveAllInjectedScripts()` - Remove all injected scripts
- `void RegisterSchemeHandler(string scheme, Action<SchemeRequest, SchemeExecutor> handler)` - Custom scheme
- `void UnregisterSchemeHandler(string scheme)` - Remove custom scheme

Events:
- `PermissionRequested` - Permission request
- `FullscreenRequested` - Fullscreen request
- `DomReady` - DOM ready
- `Navigated` - Navigation completed
- `Navigate` - Before navigation (can block)
- `MessageReceived` - Message from JS
- `RequestReceived` - Resource request
- `FaviconChanged` - Favicon changed
- `TitleChanged` - Title changed
- `LoadStateChanged` - Load state changed

#### Supporting Types

- `Url` - URL parsing and manipulation
- `Icon` - Window icons
- `Screen` - Display information
- `Color` - RGBA color representation
- `SchemeRequest` - Custom scheme request details
- `SchemeExecutor` - Custom scheme response handler

## Thread Safety

- Check `Application.IsThreadSafe` before calling from non-UI threads
- Use `Application.Post()` to schedule callbacks on the main thread
- Event handlers are called on the main thread

## Memory Management

All classes implement `IDisposable` for proper resource cleanup:

```csharp
using (var app = Application.Create("id"))
using (var window = app.CreateWindow())
using (var webview = window.CreateWebview())
{
    // Use the objects
} // Automatically disposed in reverse order
```

## Error Handling

Most operations throw `InvalidOperationException` with error codes:

```csharp
try
{
    var app = Application.Create("id");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Failed to create application: {ex.Message}");
}
```

## Platform-Specific Notes

### Windows
- Uses WebView2 runtime
- Requires Edge WebView2 runtime installed

### macOS
- Uses WKWebView
- Requires macOS 10.11+

### Linux
- Uses WebKit via GTK
- Requires libwebkit2gtk-4.0

## Building Saucer

For instructions on building the Saucer runtime library, see the [Saucer repository](https://github.com/saucer/saucer).

## Examples

See the `Examples` directory for complete working examples:

- `HelloWorld` - Basic window and webview
- `Counter` - Bidirectional communication with JavaScript
- `CustomScheme` - Custom URL scheme handler
- `FileServer` - Serve local files
- `Markdown` - Markdown preview application

## Contributing

Contributions are welcome! Please ensure:

1. Code follows C# conventions
2. All public APIs are documented
3. Memory is properly managed
4. P/Invoke signatures match the C API

## License

MIT License - See LICENSE file for details

## Acknowledgments

- [Saucer](https://github.com/saucer/saucer) - The underlying webview framework
- P/Invoke patterns based on established .NET interop practices
