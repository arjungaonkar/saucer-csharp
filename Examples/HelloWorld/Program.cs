using Saucer;

// Create application
var app = Application.Create("com.example.helloworld");

// Create window
var window = app.CreateWindow();
window.Title = "Hello Saucer";
window.SetSize(1280, 720);

// Create webview
var webview = window.CreateWebview();
webview.DevToolsEnabled = true;

// Load HTML content
var html = @"
<html>
<head>
    <style>
        body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; margin: 0; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); height: 100vh; display: flex; align-items: center; justify-content: center; }
        .container { background: white; border-radius: 10px; padding: 40px; box-shadow: 0 10px 40px rgba(0,0,0,0.2); text-align: center; }
        h1 { color: #333; margin: 0 0 20px 0; }
        p { color: #666; margin: 10px 0; }
        button { background: #667eea; color: white; border: none; padding: 10px 20px; border-radius: 5px; cursor: pointer; font-size: 16px; }
        button:hover { background: #764ba2; }
    </style>
</head>
<body>
    <div class='container'>
        <h1>🛸 Welcome to Saucer!</h1>
        <p>This is a webview created with C#</p>
        <button onclick='alert("Hello from Saucer!")'>
            Click Me
        </button>
    </div>
</body>
</html>
";

webview.LoadHtml(html);

// Handle webview events
webview.TitleChanged += (s, e) =>
{
    window.Title = e.Title;
};

webview.DomReady += (s, e) =>
{
    Console.WriteLine("DOM is ready!");
};

// Handle window events
window.Resized += (s, e) =>
{
    Console.WriteLine($"Window resized to {e.Width}x{e.Height}");
};

window.Closing += (s, e) =>
{
    Console.WriteLine("Window is closing");
};

// Handle application quit
app.Quit += (s, e) =>
{
    Console.WriteLine("Application quit requested");
};

// Show window and run
window.Show();
var exitCode = app.Run(() =>
{
    Console.WriteLine($"App is ready! Saucer version: {Application.Version}");
});

// Cleanup
webview.Dispose();
window.Dispose();
app.Dispose();

Environment.Exit(exitCode);
