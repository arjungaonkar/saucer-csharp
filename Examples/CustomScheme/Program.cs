using Saucer;

var app = Application.Create("com.example.customscheme");
var window = app.CreateWindow();
var webview = window.CreateWebview();

window.Title = "Custom Scheme Example";
window.SetSize(800, 600);
webview.DevToolsEnabled = true;

// Register a custom scheme
webview.RegisterSchemeHandler("app", (request, executor) =>
{
    var path = new Saucer.Url(request.Url).Path;

    if (path == "/" || path == "")
    {
        var html = System.Text.Encoding.UTF8.GetBytes(@"
<html>
<head>
    <style>
        body { font-family: Arial; padding: 20px; background: #f5f5f5; }
        h1 { color: #333; }
        a { color: #667eea; text-decoration: none; margin-right: 20px; }
        a:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <h1>Custom Scheme Example</h1>
    <p>This content is served from a custom scheme handler!</p>
    <p>Navigation:</p>
    <ul>
        <li><a href='app://about'>About</a></li>
        <li><a href='app://settings'>Settings</a></li>
        <li><a href='app://help'>Help</a></li>
    </ul>
");
        executor.Accept(html, "text/html");
    }
    else if (path == "/about")
    {
        var html = System.Text.Encoding.UTF8.GetBytes(@"
<html>
<head>
    <style>
        body { font-family: Arial; padding: 20px; }
        h1 { color: #333; }
        a { color: #667eea; }
    </style>
</head>
<body>
    <h1>About</h1>
    <p>This is a custom scheme served page.</p>
    <p><a href='app://'>Back to Home</a></p>
</body>
</html>");
        executor.Accept(html, "text/html");
    }
    else if (path == "/settings")
    {
        var html = System.Text.Encoding.UTF8.GetBytes(@"
<html>
<head>
    <style>
        body { font-family: Arial; padding: 20px; }
        h1 { color: #333; }
        a { color: #667eea; }
    </style>
</head>
<body>
    <h1>Settings</h1>
    <p>Configure your application here.</p>
    <p><a href='app://'>Back to Home</a></p>
</body>
</html>");
        executor.Accept(html, "text/html");
    }
    else if (path == "/help")
    {
        var html = System.Text.Encoding.UTF8.GetBytes(@"
<html>
<head>
    <style>
        body { font-family: Arial; padding: 20px; }
        h1 { color: #333; }
        a { color: #667eea; }
    </style>
</head>
<body>
    <h1>Help</h1>
    <p>Need help? Check out the documentation.</p>
    <p><a href='app://'>Back to Home</a></p>
</body>
</html>");
        executor.Accept(html, "text/html");
    }
    else
    {
        executor.Reject(SchemeError.NotFound);
    }
});

// Navigate to custom scheme
webview.Navigate("app://");

window.Show();
app.Run();

webview.Dispose();
window.Dispose();
app.Dispose();
