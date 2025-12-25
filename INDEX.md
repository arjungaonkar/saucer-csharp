# Saucer C# Bindings - Complete File Index

## Quick Navigation

### For First-Time Users
1. **Start Here**: [README.md](README.md) - Overview and quick start (5 min read)
2. **Then Read**: [Examples/HelloWorld/Program.cs](Examples/HelloWorld/Program.cs) - See working code
3. **Deep Dive**: [USAGE_GUIDE.md](USAGE_GUIDE.md) - Comprehensive tutorial (30 min read)

### For Architecture Enthusiasts
1. [ARCHITECTURE.md](ARCHITECTURE.md) - Design decisions and patterns
2. [Interop/NativeMethods.cs](Interop/NativeMethods.cs) - P/Invoke declarations
3. [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) - Component breakdown

### For Integration with Existing Code
1. [Saucer/Application.cs](Saucer/Application.cs) - Application entry point
2. [Saucer/Extensions.cs](Saucer/Extensions.cs) - Helper methods
3. [Examples/CustomScheme/Program.cs](Examples/CustomScheme/Program.cs) - Advanced patterns

---

## File Guide

### Documentation Files

#### [README.md](README.md)
**Purpose**: Quick start and feature overview  
**Length**: ~400 lines  
**Covers**:
- Installation instructions
- Feature list
- Quick start code
- API reference summary
- Platform-specific notes

**Read this if**: You're new to the project

#### [USAGE_GUIDE.md](USAGE_GUIDE.md)
**Purpose**: Comprehensive tutorial and reference  
**Length**: ~1000 lines  
**Covers**:
- Application lifecycle
- Window management (25+ properties/methods)
- Webview integration
- JavaScript communication
- Custom schemes
- Event handling
- Error handling
- Best practices
- Troubleshooting

**Read this if**: You need detailed explanations with examples

#### [ARCHITECTURE.md](ARCHITECTURE.md)
**Purpose**: Design decisions and internal patterns  
**Length**: ~500 lines  
**Covers**:
- Layer structure
- Memory management strategy
- Event marshaling
- Thread safety
- Error handling approach
- Performance characteristics

**Read this if**: You want to understand how it works internally

#### [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)
**Purpose**: High-level overview of implementation  
**Length**: ~400 lines  
**Covers**:
- Repository structure
- Key components
- Memory management strategy
- Event system
- Testing strategy
- Performance characteristics
- Known limitations
- Quality checklist

**Read this if**: You need a project overview

#### [INDEX.md](INDEX.md)
**Purpose**: This file - navigation guide  
**Length**: ~300 lines  
**Covers**:
- File organization
- Reading recommendations
- Component descriptions
- Code examples index

---

### Source Code Files

#### Interop Layer

**[Interop/NativeMethods.cs](Interop/NativeMethods.cs)**
- **Lines**: 1,100
- **Contents**:
  - 150+ P/Invoke declarations
  - 9 Enumerations
  - 12 Unmanaged delegate signatures
  - Proper marshaling attributes
- **Key Enums**: Policy, ApplicationEvent, WindowEvent, WebviewEvent, etc.
- **Key Sections**:
  - Application functions (line 1-100)
  - Window functions (line 100-350)
  - Webview functions (line 350-650)
  - Supporting functions (line 650+)

#### Core Classes

**[Saucer/Application.cs](Saucer/Application.cs)**
- **Lines**: 250
- **Purpose**: Application lifecycle and window management
- **Members**: 10 methods, 5 properties, 1 event
- **Key API**:
  ```csharp
  static Application Create(string id)
  Window CreateWindow()
  void Run(Action? onReady)
  IReadOnlyList<Screen> Screens { get; }
  bool IsThreadSafe { get; }
  event EventHandler<ApplicationQuitEventArgs>? Quit
  ```

**[Saucer/Window.cs](Saucer/Window.cs)**
- **Lines**: 350
- **Purpose**: Window management and webview hosting
- **Members**: 25+ methods, 20+ properties, 7 events
- **Key API**:
  ```csharp
  Webview CreateWebview()
  void SetSize(int w, int h)
  void SetPosition(int x, int y)
  string Title { get; set; }
  (int Width, int Height) Size { get; }
  void Show(), Hide(), Close(), Focus()
  event EventHandler<WindowResizeEventArgs>? Resized
  event EventHandler<WindowCloseEventArgs>? Closing  // Cancellable
  ```

**[Saucer/Webview.cs](Saucer/Webview.cs)**
- **Lines**: 400
- **Purpose**: Web content rendering and JavaScript integration
- **Members**: 20+ methods, 5 properties, 10 events
- **Key API**:
  ```csharp
  void Navigate(string url)
  void LoadHtml(string html)
  void ExecuteJavaScript(string code)
  ulong InjectJavaScript(string code, ScriptTime when, bool noFrames)
  void RegisterSchemeHandler(string scheme, Action<SchemeRequest, SchemeExecutor>)
  bool DevToolsEnabled { get; set; }
  event EventHandler<WebviewNavigateEventArgs>? Navigate  // Cancellable
  event EventHandler<WebviewTitleEventArgs>? TitleChanged
  ```

#### Supporting Types

**[Saucer/Url.cs](Saucer/Url.cs)**
- **Lines**: 120
- **Purpose**: URL parsing and component extraction
- **API**:
  ```csharp
  static Url Parse(string url)          // Strict parsing
  static Url From(string url)           // Lenient parsing
  static Url Create(string scheme, string host, ushort? port, string path)
  string Scheme { get; }
  string? Host { get; }
  ushort? Port { get; }
  string Path { get; }
  ```

**[Saucer/Icon.cs](Saucer/Icon.cs)**
- **Lines**: 80
- **Purpose**: Window icon management
- **API**:
  ```csharp
  static Icon FromFile(string path)
  static Icon FromData(byte[] data)
  void SaveToFile(string path)
  byte[] GetData()
  bool IsEmpty { get; }
  ```

**[Saucer/Screen.cs](Saucer/Screen.cs)**
- **Lines**: 50
- **Purpose**: Display screen information
- **API**:
  ```csharp
  string Name { get; }
  (int Width, int Height) Size { get; }
  (int X, int Y) Position { get; }
  ```

**[Saucer/Color.cs](Saucer/Color.cs)**
- **Lines**: 40
- **Purpose**: RGBA color representation
- **API**:
  ```csharp
  Color(byte r, byte g, byte b, byte a = 255)
  Color(int hex)  // From hex code
  string ToString()  // "#AARRGGBB"
  static Color Black, White, Red, Green, Blue, Transparent
  ```

#### Extensions and Utilities

**[Saucer/Extensions.cs](Saucer/Extensions.cs)**
- **Lines**: 150
- **Purpose**: Helper methods and common patterns
- **Methods**:
  - `window.CenterOnScreen()` - Center on primary screen
  - `webview.OnMessage(Action<string>)` - Message handler
  - `webview.InjectJson(string varName, string json)` - Inject data
  - `webview.LogToConsole(string)` - Debug logging
  - `webview.EnableDeveloperMode()` - Setup dev tools
  - `webview.TryNavigate(string)` - Safe navigation
  - `ParseColor(string hex)` - Hex to Color
  - `app.GetPrimaryScreen()` - Get primary display

**[Saucer/NativeHandle.cs](Saucer/NativeHandle.cs)**
- **Lines**: 50
- **Purpose**: Low-level native handle access
- **Use Case**: Advanced P/Invoke scenarios

**[Saucer/GlobalUsings.cs](Saucer/GlobalUsings.cs)**
- **Lines**: 5
- **Purpose**: Global using statements

**[Saucer/StringBuilder.cs](Saucer/StringBuilder.cs)**
- **Lines**: 3
- **Purpose**: Convenient alias for System.Text.StringBuilder

#### Project Configuration

**[saucer-csharp.csproj](saucer-csharp.csproj)**
- **Purpose**: .NET project configuration
- **Content**:
  - Target framework: net6.0
  - XML documentation enabled
  - Unsafe code allowed
  - NuGet package metadata

---

### Example Applications

#### [Examples/HelloWorld/Program.cs](Examples/HelloWorld/Program.cs)
- **Lines**: 90
- **Demonstrates**:
  - Basic application setup
  - Window creation
  - HTML loading
  - Event handling
  - Window lifecycle
- **Learning Value**: ⭐⭐⭐ (Start here)
- **Run**: `cd Examples/HelloWorld && dotnet run`

#### [Examples/Counter/Program.cs](Examples/Counter/Program.cs)
- **Lines**: 150
- **Demonstrates**:
  - Custom scheme handlers
  - Bidirectional communication
  - State management
  - JSON serialization
- **Learning Value**: ⭐⭐⭐⭐
- **Run**: `cd Examples/Counter && dotnet run`

#### [Examples/CustomScheme/Program.cs](Examples/CustomScheme/Program.cs)
- **Lines**: 200
- **Demonstrates**:
  - Complex scheme routing
  - Multi-page applications
  - URL path parsing
  - Response handling
- **Learning Value**: ⭐⭐⭐⭐⭐
- **Run**: `cd Examples/CustomScheme && dotnet run`

---

## Code Organization

```
Main Classes (5)
├── Application (root lifecycle)
├── Window (top-level container)
├── Webview (web content)
├── [4 supporting types]
└── Event arguments (10 types)

Interop Layer (1)
├── NativeMethods
│   ├── 150+ P/Invoke declarations
│   ├── 9 enumerations
│   └── 12 unmanaged delegates

Extensions (1)
└── SaucerExtensions (8 methods)

Examples (3)
├── HelloWorld
├── Counter
└── CustomScheme

Documentation (4)
├── README.md
├── USAGE_GUIDE.md
├── ARCHITECTURE.md
└── IMPLEMENTATION_SUMMARY.md
```

---

## API Coverage Matrix

| Component | Saucer C API | C# Wrapper | Coverage |
|-----------|-------------|-----------|----------|
| Application | 15 functions | Application class | 100% |
| Window | 40 functions | Window class | 100% |
| Webview | 35 functions | Webview class | 100% |
| URL | 10 functions | Url class | 100% |
| Icon | 7 functions | Icon class | 100% |
| Stash | 8 functions | Internal use | 100% |
| Permission | 6 functions | Event args | 100% |
| Scheme | 8 functions | Webview methods | 100% |
| Screen | 4 functions | Screen class | 100% |
| **Total** | **133** | **150+ members** | **100%** |

---

## Learning Path

### Beginner (30 minutes)
1. Read: [README.md](README.md) (5 min)
2. Run: [Examples/HelloWorld/Program.cs](Examples/HelloWorld/Program.cs) (5 min)
3. Study: HelloWorld source code (10 min)
4. Try: Modify window title and size (10 min)

### Intermediate (2 hours)
1. Read: [USAGE_GUIDE.md](USAGE_GUIDE.md) - Section 1-3 (30 min)
2. Run: [Examples/Counter/Program.cs](Examples/Counter/Program.cs) (10 min)
3. Study: Custom scheme implementation (30 min)
4. Implement: Simple scheme handler (20 min)

### Advanced (4 hours)
1. Read: [ARCHITECTURE.md](ARCHITECTURE.md) (45 min)
2. Study: [Interop/NativeMethods.cs](Interop/NativeMethods.cs) (1 hour)
3. Review: Memory management patterns (30 min)
4. Implement: Complex multi-page app (1.5 hours)

---

## Quick Reference

### Most Common Operations

**Create Application**
```csharp
var app = Application.Create("com.example.app");
```
[Source: Application.cs line 80]

**Create Window**
```csharp
var window = app.CreateWindow();
window.Show();
```
[Source: Window.cs line 200]

**Load Web Content**
```csharp
var webview = window.CreateWebview();
webview.Navigate("https://example.com");
```
[Source: Webview.cs line 150]

**Execute JavaScript**
```csharp
webview.ExecuteJavaScript("console.log('hello')");
```
[Source: Webview.cs line 180]

**Register Custom Scheme**
```csharp
webview.RegisterSchemeHandler("app", (req, exec) => {
    exec.Accept(data, "text/html");
});
```
[Source: Webview.cs line 230]

**Handle Events**
```csharp
window.Resized += (s, e) => Console.WriteLine($"{e.Width}x{e.Height}");
window.Closing += (s, e) => e.Cancel = !CanClose();
webview.TitleChanged += (s, e) => window.Title = e.Title;
```
[Source: Window.cs line 280]

---

## Troubleshooting Guide Location

For common issues, see [USAGE_GUIDE.md - Troubleshooting](USAGE_GUIDE.md#troubleshooting)

- Application won't start
- Window not showing
- JavaScript not executing
- Memory leaks
- Events not firing

---

## Contributing

For contribution guidelines, see:
- Issue reporting: GitHub Issues
- Code style: Match existing code
- Tests: Add unit tests for new features
- Docs: Update relevant documentation

---

## License

MIT License - See LICENSE file

---

## Last Updated

December 25, 2025

For the latest changes, check the [commit history](https://github.com/arjungaonkar/saucer-csharp/commits/main)
