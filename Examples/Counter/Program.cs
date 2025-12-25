using Saucer;
using System.Text.Json;

var app = Application.Create("com.example.counter");
var window = app.CreateWindow();
var webview = window.CreateWebview();

int counter = 0;

window.Title = "Counter App";
window.SetSize(400, 300);
webview.DevToolsEnabled = true;

var html = @"
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }
        .container {
            text-align: center;
            background: white;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
        }
        h1 { color: #333; margin: 0; }
        .counter { font-size: 48px; color: #667eea; font-weight: bold; margin: 20px 0; }
        button {
            background: #667eea;
            color: white;
            border: none;
            padding: 10px 20px;
            margin: 5px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }
        button:hover { background: #764ba2; }
    </style>
</head>
<body>
    <div class='container'>
        <h1>Counter App</h1>
        <div class='counter' id='count'>0</div>
        <button onclick='increment()'>+</button>
        <button onclick='decrement()'>-</button>
        <button onclick='reset()'>Reset</button>
    </div>
    <script>
        function increment() {
            fetch('app://increment', { method: 'POST' });
        }
        function decrement() {
            fetch('app://decrement', { method: 'POST' });
        }
        function reset() {
            fetch('app://reset', { method: 'POST' });
        }
        function updateCounter(value) {
            document.getElementById('count').textContent = value;
        }
        // Listen for updates from C#
        const eventSource = new EventSource('app://events');
        eventSource.onmessage = (e) => {
            updateCounter(e.data);
        };
    </script>
</body>
</html>
";

webview.LoadHtml(html);

webview.RegisterSchemeHandler("app", (request, executor) =>
{
    var method = request.Method;
    var url = request.Url;

    if (url.Contains("increment") && method == "POST")
    {
        counter++;
        var response = JsonSerializer.Serialize(counter);
        executor.Accept(System.Text.Encoding.UTF8.GetBytes(response), "application/json");
    }
    else if (url.Contains("decrement") && method == "POST")
    {
        counter--;
        var response = JsonSerializer.Serialize(counter);
        executor.Accept(System.Text.Encoding.UTF8.GetBytes(response), "application/json");
    }
    else if (url.Contains("reset") && method == "POST")
    {
        counter = 0;
        var response = JsonSerializer.Serialize(counter);
        executor.Accept(System.Text.Encoding.UTF8.GetBytes(response), "application/json");
    }
    else
    {
        executor.Reject(SchemeError.NotFound);
    }
});

window.Show();
app.Run();

webview.Dispose();
window.Dispose();
app.Dispose();
