# Saucer C# Usage Guide

## Table of Contents

1. [Getting Started](#getting-started)
2. [Application Lifecycle](#application-lifecycle)
3. [Window Management](#window-management)
4. [Webview Integration](#webview-integration)
5. [JavaScript Communication](#javascript-communication)
6. [Custom Schemes](#custom-schemes)
7. [Event Handling](#event-handling)
8. [Error Handling](#error-handling)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)

## Getting Started

### Basic Setup

```csharp
using Saucer;

// 1. Create application
var app = Application.Create("com.mycompany.myapp");

// 2. Create window
var window = app.CreateWindow();
window.Title = "My App";
window.SetSize(1280, 720);

// 3. Create webview
var webview = window.CreateWebview();

// 4. Load content
webview.Navigate("https://example.com");

// 5. Show and run
window.Show();
app.Run();

// 6. Cleanup
webview.Dispose();
window.Dispose();
app.Dispose();
```

### Using 'using' Statements

```csharp
using (var app = Application.Create("id"))
using (var window = app.CreateWindow())
using (var webview = window.CreateWebview())
{
    // Use the objects
} // Automatically disposed
```

## Application Lifecycle

### Creating an Application

```csharp
var app = Application.Create("com.example.myapp");
```

**Important:** Application ID should be unique and follow reverse domain notation.

### Running the Event Loop

```csharp
// Simple run
var exitCode = app.Run();

// With initialization callback
var exitCode = app.Run(onReady: () => 
{
    Console.WriteLine("Application is ready!");
    // Perform initialization
});
```

### Handling Application Quit

```csharp
app.Quit += (sender, e) =>
{
    Console.WriteLine("User wants to quit");
    // Can set e.Cancel = true to prevent quit
};
```

### Getting Application Info

```csharp
var version = Application.Version;  // "1.0.0"
var isThreadSafe = app.IsThreadSafe; // true/false
```

### Working with Screens

```csharp
var screens = app.Screens;
foreach (var screen in screens)
{
    Console.WriteLine($"Screen: {screen.Name}");
    var (width, height) = screen.Size;
    var (x, y) = screen.Position;
    Console.WriteLine($"  Size: {width}x{height}");
    Console.WriteLine($"  Position: ({x}, {y})");
}
```

## Window Management

### Creating Windows

```csharp
var window = app.CreateWindow();
```

You can create multiple windows:

```csharp
var window1 = app.CreateWindow();
var window2 = app.CreateWindow();
```

### Window Properties

```csharp
// Title
window.Title = "My Application";
var title = window.Title;

// Size
window.SetSize(800, 600);
var (width, height) = window.Size;

// Position
window.SetPosition(100, 100);
var (x, y) = window.Position;

// Background color
window.BackgroundColor = new Color(255, 255, 255);
```

### Window State

```csharp
// Visibility
window.Show();
window.Hide();

// Window state
window.SetMinimized(true);
window.SetMaximized(false);
window.SetFullscreen(false);
window.SetResizable(true);
window.SetAlwaysOnTop(true);
window.SetClickThrough(false);

// Query state
if (window.IsVisible)
    Console.WriteLine("Window is visible");
```

### Window Sizing Constraints

```csharp
// Set size constraints
window.SetSize(800, 600);
window.SetMinSize(400, 300);
window.SetMaxSize(1920, 1080);

// Get constraints
var (w, h) = window.Size;
var (minW, minH) = window.MinSize; // Requires accessing native bounds
```

### Window Decorations

```csharp
// Control window decorations
window.SetDecorations(WindowDecoration.Full);
// Options: None, Partial, Full
```

### Window Interactions

```csharp
// Focus and activation
window.Focus();

// Drag operations
window.StartDrag();  // Drag entire window
window.StartResize(WindowEdge.TopRight);  // Drag from corner

// Available edges:
// Top, Bottom, Left, Right
// TopLeft, TopRight, BottomLeft, BottomRight
```

### Centering on Screen

```csharp
// Using extension method
window.CenterOnScreen();

// Or manually
var screen = window.Application.Screens.FirstOrDefault();
if (screen != null)
{
    var (screenW, screenH) = screen.Size;
    var (windowW, windowH) = window.Size;
    window.SetPosition(
        (screenW - windowW) / 2,
        (screenH - windowH) / 2
    );
}
```

## Webview Integration

### Creating a Webview

```csharp
var webview = window.CreateWebview();
```

Each window can have one webview.

### Loading Content

```csharp
// Load from URL
webview.Navigate("https://example.com");

// Load HTML string
var html = @"<html><body><h1>Hello</h1></body></html>";
webview.LoadHtml(html);

// Safe navigation
if (webview.TryNavigate(url))
    Console.WriteLine("Navigation started");
else
    Console.WriteLine("Invalid URL");
```

### Navigation History

```csharp
// Navigate
webview.Navigate("https://example.com");
webview.Navigate("https://example.com/page2");

// Go back/forward
webview.GoBack();     // Back to page1
webview.GoForward();  // Forward to page2

// Reload
webview.Reload();
```

### Webview Configuration

```csharp
// Developer tools
webview.DevToolsEnabled = true;   // Enable F12
// Or use extension:
webview.EnableDeveloperMode();

// Context menu
webview.ContextMenuEnabled = true;

// Dark mode
webview.ForceDarkMode = true;

// Background color
webview.BackgroundColor = Color.White;
```

### Page Information

```csharp
// Get page title
var title = webview.PageTitle;

// Get current URL
var url = webview.Url;  // Returns Url object

// Get favicon
var favicon = webview.Favicon;  // Returns Icon
```

## JavaScript Communication

### Executing JavaScript

```csharp
// Simple execution
webview.ExecuteJavaScript("console.log('Hello from C#')");

// With variables
webview.ExecuteJavaScript("alert('Value: ' + 42)");

// Using extension
webview.LogToConsole("Debug message");
```

### Injecting Scripts

```csharp
// Inject at page creation
var id = webview.InjectJavaScript(
    code: "window.myVar = 42;",
    runAt: ScriptTime.Creation,
    noFrames: false
);

// Inject when DOM is ready
var id = webview.InjectJavaScript(
    code: "document.addEventListener('DOMContentLoaded', () => {...})",
    runAt: ScriptTime.Ready
);

// Inject into frames
var id = webview.InjectJavaScript(
    code: "console.log('In iframe')",
    runAt: ScriptTime.Ready,
    noFrames: true  // Skip iframes
);

// Remove injection
webview.RemoveInjectedScript(id);
webview.RemoveAllInjectedScripts();
```

### Injecting Data

```csharp
var data = new { Name = "John", Age = 30 };
var json = System.Text.Json.JsonSerializer.Serialize(data);
webview.InjectJson("userData", json);

// In JavaScript:
// console.log(window.userData.Name)  // "John"
```

## Custom Schemes

### Registering a Scheme

```csharp
webview.RegisterSchemeHandler("app", (request, executor) =>
{
    var path = request.Url;  // e.g., "app://index"
    
    if (path.Contains("index"))
    {
        var html = Encoding.UTF8.GetBytes("<html>...");
        executor.Accept(html, "text/html");
    }
    else
    {
        executor.Reject(SchemeError.NotFound);
    }
});

// Navigate to custom scheme
webview.Navigate("app://index");
```

### Parsing Request URLs

```csharp
webview.RegisterSchemeHandler("app", (request, executor) =>
{
    // Get URL components
    var url = new Url(request.Url);
    var path = url.Path;       // /api/users
    var scheme = url.Scheme;   // app
    
    // Get request details
    var method = request.Method;     // POST, GET, etc.
    var content = request.Content;   // Byte array
    
    // Handle request
    if (path == "/api/data")
    {
        executor.Accept(GetData(), "application/json");
    }
});
```

### Response Headers

```csharp
webview.RegisterSchemeHandler("app", (request, executor) =>
{
    var stash = NativeMethods.saucer_stash_new_from_str("{\"status\":\"ok\"}");
    var response = NativeMethods.saucer_scheme_response_new(stash, "application/json");
    
    NativeMethods.saucer_scheme_response_append_header(response, "X-Custom", "value");
    NativeMethods.saucer_scheme_response_set_status(response, 200);
    
    executor.Accept(response);
});
```

### Error Responses

```csharp
webview.RegisterSchemeHandler("app", (request, executor) =>
{
    if (!ValidateRequest(request))
    {
        executor.Reject(SchemeError.Denied);       // 401 Denied
        // Or:
        executor.Reject(SchemeError.NotFound);     // 404 Not Found
        executor.Reject(SchemeError.Invalid);      // 400 Bad Request
        executor.Reject(SchemeError.Failed);       // -1 Failed
    }
});
```

## Event Handling

### Window Events

```csharp
// Lifecycle events
window.Decorated += (s, e) => 
    Console.WriteLine($"Decorations: {e.Decoration}");
window.Maximized += (s, e) => 
    Console.WriteLine($"Maximized: {e.IsMaximized}");
window.Minimized += (s, e) => 
    Console.WriteLine($"Minimized: {e.IsMinimized}");
window.Closed += (s, e) => 
    Console.WriteLine("Window closed");

// State events
window.Resized += (s, e) => 
    Console.WriteLine($"New size: {e.Width}x{e.Height}");
window.FocusChanged += (s, e) => 
    Console.WriteLine($"Focused: {e.IsFocused}");

// Control events
window.Closing += (s, e) => 
{
    if (!CanClose)
        e.Cancel = true;  // Prevent closing
};
```

### Webview Events

```csharp
// Content events
webview.DomReady += (s, e) => 
    Console.WriteLine("DOM is ready");
webview.TitleChanged += (s, e) => 
    window.Title = e.Title;  // Keep window title in sync
webview.LoadStateChanged += (s, e) => 
    Console.WriteLine($"Finished: {e.IsFinished}");

// Navigation events
webview.Navigate += (s, e) => 
{
    Console.WriteLine($"Navigating to: {e.Url}");
    if (!AllowNavigation(e.Url))
        e.Allow = false;  // Block navigation
};
webview.Navigated += (s, e) => 
    Console.WriteLine($"Navigated to: {e.Url}");

// Security events
webview.PermissionRequested += (s, e) =>
{
    if (e.Permission == PermissionType.Clipboard)
        e.Allow = true;
};
webview.FullscreenRequested += (s, e) => 
{
    e.Allow = true;
};

// Content events
webview.FaviconChanged += (s, e) => 
    Console.WriteLine($"Favicon updated");
webview.MessageReceived += (s, e) => 
    Console.WriteLine($"Message: {e.Message}");
```

## Error Handling

### Try-Catch Pattern

```csharp
try
{
    var app = Application.Create("id");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Failed to create app: {ex.Message}");
}
```

### Validation

```csharp
public void SetupWebview(Webview webview)
{
    if (webview == null)
        throw new ArgumentNullException(nameof(webview));
    
    try
    {
        webview.Navigate("https://invalid-url/");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

### Resource Safety

```csharp
public void SafeSetup()
{
    Application app = null;
    try
    {
        app = Application.Create("id");
        var window = app.CreateWindow();
        // Use objects...
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    finally
    {
        app?.Dispose();
    }
}
```

## Best Practices

### 1. Always Use Using Statements

```csharp
using (var app = Application.Create("id"))
using (var window = app.CreateWindow())
using (var webview = window.CreateWebview())
{
    // Safe - everything cleaned up
}
```

### 2. Unique Application IDs

```csharp
// Good
var app = Application.Create("com.mycompany.myapp");
var app2 = Application.Create("com.mycompany.myapp-v2");

// Bad
var app = Application.Create("app");  // Too generic
```

### 3. Handle Events Early

```csharp
var window = app.CreateWindow();

// Register event handlers before showing
window.Closing += (s, e) => SaveState();
window.Resized += (s, e) => ResizeContent();

// Then show
window.Show();
```

### 4. Check Thread Safety

```csharp
if (!app.IsThreadSafe)
{
    // Use Application.Post() for background work
    app.Post(() => DoWork());
}
else
{
    // Can call directly from any thread
    window.SetSize(800, 600);
}
```

### 5. Validate URLs Before Navigation

```csharp
var url = userInput;
if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
{
    webview.Navigate(url);
}
else
{
    Console.WriteLine("Invalid URL");
}
```

### 6. Use Extension Methods

```csharp
// Instead of:
window.Title = Title;  // Manual

// Use provided extensions:
window.CenterOnScreen();
webview.LogToConsole("Debug");
webview.InjectJson("data", json);
```

## Troubleshooting

### Application Won't Start

**Problem:** `InvalidOperationException` when creating Application

**Solution:** Ensure Saucer runtime library is installed and in PATH

### Window Not Showing

**Problem:** Window created but not visible

**Solution:** Call `window.Show()` before `app.Run()`

### JavaScript Not Executing

**Problem:** `ExecuteJavaScript()` has no effect

**Solution:** Wait for DomReady event:

```csharp
webview.DomReady += (s, e) => 
    webview.ExecuteJavaScript("console.log('Ready')");
```

### Memory Leaks

**Problem:** Application consumes increasing memory

**Solution:** Always dispose objects properly:

```csharp
// Good
webview?.Dispose();
window?.Dispose();
app?.Dispose();

// Bad - Missing dispose
var webview = window.CreateWebview();
// ... webview never disposed
```

### Events Not Firing

**Problem:** Event handlers never called

**Solution:** Register before relevant actions:

```csharp
window.Show();  // After this
window.Resized += (s, e) => { };  // Events may be missed

// Correct order:
window.Resized += (s, e) => { };
window.Show();
```
