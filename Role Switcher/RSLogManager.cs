using XrmToolBox.Extensibility;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Role_Switcher
{
    public enum LogLevel
    {
        Error,
        Warning,
        Information
    }

    public class LogMessage
    {
        public string Message { get; set; }
        public LogLevel Level { get; set; }

        public override string ToString() => $"[{Level}] {Message}";
    }

    public class RSLogManager
    {
        private readonly LogManager _logger;
        public BindingList<LogMessage> Messages;

        public RSLogManager()
        {
            _logger = new LogManager(typeof(RoleSwitcher));
            Messages = new BindingList<LogMessage>();
        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Error:
                    _logger.LogError(message); break;
                case LogLevel.Warning:
                    _logger.LogWarning(message); break;
                case LogLevel.Information:
                    _logger.LogInfo(message); break;
                default:
                    _logger.LogInfo(message); break;
            }
            Messages.Add(new LogMessage { Level = level, Message = message });
        }

        public static void DrawMessage(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox listBox = sender as ListBox;
            LogMessage logEntry = listBox.Items[e.Index] as LogMessage;

            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            Brush logLevelBrush;
            Brush messageBrush = Brushes.Black;

            switch (logEntry.Level)
            {
                case LogLevel.Information:
                    logLevelBrush = messageBrush;
                    break;

                case LogLevel.Warning:
                    logLevelBrush = Brushes.DarkGoldenrod;
                    break;

                case LogLevel.Error:
                    logLevelBrush = Brushes.Red;
                    break;

                default:
                    logLevelBrush = messageBrush;
                    break;
            }

            string logLevel = $"[{logEntry.Level}]";
            float logLevelWidth = e.Graphics.MeasureString(logLevel, e.Font).Width;

            e.Graphics.DrawString(logLevel, e.Font, logLevelBrush, e.Bounds, StringFormat.GenericDefault);
            e.Graphics.DrawString(logEntry.Message, e.Font, messageBrush, new RectangleF(e.Bounds.X + logLevelWidth, e.Bounds.Y, e.Bounds.Width - logLevelWidth, e.Bounds.Height), StringFormat.GenericDefault);
        }
    }
}