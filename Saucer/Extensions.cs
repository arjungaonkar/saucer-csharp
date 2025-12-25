using System;
using System.Text;

namespace Saucer
{
    /// <summary>
    /// Extension methods for Saucer types.
    /// </summary>
    public static class SaucerExtensions
    {
        /// <summary>
        /// Executes JavaScript and returns a result (simplified version).
        /// </summary>
        public static void ExecuteJavaScriptAsync(this Webview webview, string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            webview.ExecuteJavaScript(code);
        }

        /// <summary>
        /// Injects JSON data into the page.
        /// </summary>
        public static void InjectJson(this Webview webview, string variableName, string jsonData)
        {
            if (string.IsNullOrEmpty(variableName))
                throw new ArgumentNullException(nameof(variableName));
            if (string.IsNullOrEmpty(jsonData))
                throw new ArgumentNullException(nameof(jsonData));

            var code = $"window.{variableName} = {jsonData}; console.log('Injected {variableName}')";
            webview.InjectJavaScript(code, ScriptTime.Ready);
        }

        /// <summary>
        /// Creates a centered window.
        /// </summary>
        public static void CenterOnScreen(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            var screen = window.Application.Screens.FirstOrDefault();
            if (screen != null)
            {
                var (screenWidth, screenHeight) = screen.Size;
                var (windowWidth, windowHeight) = window.Size;
                var (screenX, screenY) = screen.Position;

                var x = screenX + (screenWidth - windowWidth) / 2;
                var y = screenY + (screenHeight - windowHeight) / 2;

                window.SetPosition(x, y);
            }
        }

        /// <summary>
        /// Registers a simple message handler that receives strings from JavaScript.
        /// </summary>
        public static void OnMessage(this Webview webview, Action<string> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            webview.MessageReceived += (s, e) => handler(e.Message);
        }

        /// <summary>
        /// Converts a hexadecimal color string to Color.
        /// </summary>
        public static Color ParseColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentNullException(nameof(hex));

            hex = hex.TrimStart('#');
            if (hex.Length != 6 && hex.Length != 8)
                throw new ArgumentException("Hex color must be 6 or 8 characters (RGB or ARGB)", nameof(hex));

            if (hex.Length == 6)
                return new Color(Convert.ToByte(hex.Substring(0, 2), 16),
                                Convert.ToByte(hex.Substring(2, 2), 16),
                                Convert.ToByte(hex.Substring(4, 2), 16));

            return new Color(Convert.ToByte(hex.Substring(2, 2), 16),
                            Convert.ToByte(hex.Substring(4, 2), 16),
                            Convert.ToByte(hex.Substring(6, 2), 16),
                            Convert.ToByte(hex.Substring(0, 2), 16));
        }

        /// <summary>
        /// Converts a Color to a hexadecimal string.
        /// </summary>
        public static string ToHexString(this Color color)
        {
            return color.ToString(); // Uses the Color.ToString() implementation
        }

        /// <summary>
        /// Gets the application's primary screen.
        /// </summary>
        public static Screen? GetPrimaryScreen(this Application app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            return app.Screens.FirstOrDefault();
        }

        /// <summary>
        /// Loads a URL safely, handling exceptions.
        /// </summary>
        public static bool TryNavigate(this Webview webview, string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                webview.Navigate(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Shows developer tools with a keyboard shortcut handler.
        /// </summary>
        public static void EnableDeveloperMode(this Webview webview)
        {
            webview.DevToolsEnabled = true;
            webview.ExecuteJavaScript(@"
                document.addEventListener('keydown', (e) => {
                    if ((e.ctrlKey || e.metaKey) && e.key === 'F12') {
                        console.log('DevTools shortcut pressed');
                    }
                });
            ");
        }

        /// <summary>
        /// Creates a simple console logger that posts messages to JavaScript console.
        /// </summary>
        public static void LogToConsole(this Webview webview, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var escaped = message.Replace("\"", "\\\"").Replace("\n", "\\n");
            webview.ExecuteJavaScript($"console.log(\"[C#] {escaped}\");");
        }
    }
}
