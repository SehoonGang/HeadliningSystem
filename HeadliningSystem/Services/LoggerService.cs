using CoPick.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using HeadliningSystem.ViewModels.Pages;
using System.Windows.Documents;
using System.Windows.Media;
using OpenTK.Mathematics;
using System.Windows.Forms;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using RichTextBox = System.Windows.Controls.RichTextBox;
using System.Windows;
using Application = System.Windows.Application;
using HeadliningSystem.Views.Windows;

namespace HeadliningSystem.Services
{
    public sealed class LoggerService
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        [DllImport("user32", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcblt, int nFlags);

        private static readonly Lazy<LoggerService> _instanceHolder = new Lazy<LoggerService>(() => new LoggerService());
        private readonly HashSet<char> _invalidFileNameCharsSet = new HashSet<char>(Path.GetInvalidFileNameChars());
        private LoggerService()
        {
        }

        public static LoggerService Logger
        {
            get
            {
                return _instanceHolder.Value;
            }
        }

        public Dictionary<LogLevel, SolidColorBrush> lvlCol = new Dictionary<LogLevel, SolidColorBrush>()
        {
            [LogLevel.Debug] = new SolidColorBrush(Colors.Black),
            [LogLevel.Info] = new SolidColorBrush(Colors.Black),
            [LogLevel.Warning] = new SolidColorBrush(Colors.DarkOrange),
            [LogLevel.Error] = new SolidColorBrush(Colors.Red),
            [LogLevel.Fatal] = new SolidColorBrush(Colors.Red)
        };

        public Dictionary<LogLevel, SolidColorBrush> lvlColDark = new Dictionary<LogLevel, SolidColorBrush>()
        {
            [LogLevel.Debug] = new SolidColorBrush(Colors.White),
            [LogLevel.Info] = new SolidColorBrush(Colors.White),
            [LogLevel.Warning] = new SolidColorBrush(Colors.DarkOrange),
            [LogLevel.Error] = new SolidColorBrush(Colors.Red),
            [LogLevel.Fatal] = new SolidColorBrush(Colors.Red)
        };

        private Regex localizationPattern = new Regex(@"!@.\w+", RegexOptions.Compiled);
        private object _lockFile = new object();
        private object _lockGui = new object();
        private object _lockCsv = new object();

        private string _logPath;
        private string _txtLogPath;
        private int _maxLine = -1;
        private ResourceManager _resMgr;
        private RichTextBox _rtbLog;

        private bool _writeFile;
        private bool _writeGui;
        private bool _isMainWindowSet;
        private bool _localizable;
        private bool _writeCsv;
        private SolidColorBrush _logTextColor = new SolidColorBrush(Colors.Black);
        private bool _dark = false;
        public SolidColorBrush ColorByDisplayMode
        {
            get => _logTextColor;
            set => _logTextColor = value;
        }

        public bool DarkMode
        {
            get => _dark;
            set
            {
                _dark = value;
            }
        }

        private MainWindow _mainWindow;
        public MainWindow MainWindow
        {
            get => _mainWindow;
            set 
            {
                if (value != null)
                {
                    _isMainWindowSet = true;
                    _mainWindow = value;
                }
            }
        }

        public class LogClass
        {
            public string _time;
            public LogLevel _lvl;
            public string _message;
            public string _caller;

            public LogClass(string dateTime, LogLevel lvl, string msg, string caller)
            {
                this._time = dateTime;
                this._lvl = lvl;
                this._message = msg;
                this._caller = caller;
            }
        }

        private Queue<LogClass> _logQueue = new Queue<LogClass>();
        public Queue<LogClass> LogQueue
        {
            get => _logQueue;
            set
            {
                _logQueue = value;
            }
        }

        public string LogPath
        {
            get
            {
                return _logPath;
            }

            set
            {
                _logPath = value;
                _txtLogPath = $"{_logPath}/LOG";

                try
                {
                    Directory.CreateDirectory(_txtLogPath);
                }
                catch
                {
                    Warning($"Lang.Msgs.LogPathInvalid");
                    _writeFile = false;
                    return;
                }

                var magicRule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                                                         FileSystemRights.FullControl,
                                                         InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                         PropagationFlags.None,
                                                         AccessControlType.Allow);

                DirectoryInfo dInfo = new DirectoryInfo(_logPath);
                var secu = dInfo.GetAccessControl();
                bool hasPermission = false;
                foreach (FileSystemAccessRule rule in secu.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (rule.IdentityReference == new SecurityIdentifier(WellKnownSidType.WorldSid, null))
                    {
                        hasPermission = true;
                        break;
                    }
                }
                if (!hasPermission)
                {
                    secu.AddAccessRule(magicRule);
                    dInfo.SetAccessControl(secu);
                }

                _writeFile = true;
            }
        }

        public int MaxLine
        {
            get
            {
                return _maxLine;
            }

            set
            {
                _maxLine = (value < -1) ? -1 : value;
            }
        }

        public ResourceManager ResMgr
        {
            get
            {
                return _resMgr;
            }

            set
            {
                _resMgr = value;
                _localizable = _resMgr != null;
            }
        }

        public System.Globalization.CultureInfo Culture { get; set; }

        public RichTextBox RtbLog
        {
            get
            {
                return _rtbLog;
            }

            set
            {
                _rtbLog = value;
                _writeGui = _rtbLog != null;
            }
        }

        public LogLevel GuiLoglevelFrom { get; set; }

        public LogLevel FileLoglevelFrom { get; set; }

        public string ProcessName { get; set; }

        public void Configure(string logPath = "",
                              LogLevel guiloglvl = LogLevel.Debug,
                              LogLevel fileloglvl = LogLevel.Debug,
                              ResourceManager resMgr = null,
                              System.Globalization.CultureInfo culture = null,
                              RichTextBox rtbLog = null,
                              int maxLine = -1,
                              string processName = null,
                              bool writeCsv = false)
        {
            LogPath = logPath;
            GuiLoglevelFrom = guiloglvl;
            FileLoglevelFrom = fileloglvl;
            ResMgr = resMgr;
            RtbLog = rtbLog;
            MaxLine = maxLine;
            ProcessName = processName;
            Culture = culture;
            _writeCsv = writeCsv;
        }

        public void Warning(string msg, [CallerMemberName] string caller = "")
        {
            WriteLog(msg, LogLevel.Warning, caller);
        }

        public void Info(string msg, [CallerMemberName] string caller = "")
        {
            WriteLog(msg, LogLevel.Info, caller);
        }

        public void Debug(string msg, [CallerMemberName] string caller = "")
        {
            WriteLog(msg, LogLevel.Debug, caller);
        }

        public void Error(string msg, [CallerMemberName] string caller = "")
        {
            WriteLog(msg, LogLevel.Error, caller);
        }

        public void Fatal(string msg, [CallerMemberName] string caller = "")
        {
            WriteLog(msg, LogLevel.Fatal, caller);
        }

        public void WriteLog(string msg, LogLevel lvl = LogLevel.Debug, [CallerMemberName] string caller = "")
        {
            string datetime = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ff}";

            if (_localizable)
            {
                msg = localizationPattern.Replace(msg, m => _resMgr.GetString(m.Value.Substring(2), Culture) ?? m.Value);
            }

            if (_writeFile && lvl >= FileLoglevelFrom)
            {
                Task.Run(() => TransportToFile(datetime, msg, lvl, caller));
            }

            if (_writeGui && lvl >= GuiLoglevelFrom)
            {
                if (!_rtbLog.Dispatcher.CheckAccess())
                {
                    _rtbLog.Dispatcher.BeginInvoke(() =>
                    {
                        TransportToGui(datetime, msg, lvl, caller);
                    });
                    return;
                }
                TransportToGui(datetime, msg, lvl, caller);
            }
        }

        private void TransportToGui(string datetime, string msg, LogLevel lvl, string caller)
        {
            var paragraph = new Paragraph();
            if (DarkMode)
            {
                var runTime = new Run($"[{datetime}]") { Foreground = new SolidColorBrush(Colors.DarkOliveGreen) };
                paragraph.Inlines.Add(runTime);

                var runLvl = new Run($" [{lvl}] ") { Foreground = lvlColDark[lvl] };
                paragraph.Inlines.Add(runLvl);

                var runMsg = new Run(msg) { Foreground = new SolidColorBrush(Colors.White) };
                paragraph.Inlines.Add(runMsg);

                var runCaller = new Run(msg) { Foreground = new SolidColorBrush(Colors.Wheat) };
                paragraph.Inlines.Add(runMsg);
            }
            else
            {
                var runTime = new Run($"[{datetime}]") { Foreground = new SolidColorBrush(Colors.DarkOliveGreen) };
                paragraph.Inlines.Add(runTime);

                var runLvl = new Run($" [{lvl}] ") { Foreground = lvlCol[lvl] };
                paragraph.Inlines.Add(runLvl);

                var runMsg = new Run(msg) { Foreground = new SolidColorBrush(Colors.Black) };
                paragraph.Inlines.Add(runMsg);

                var runCaller = new Run(msg) { Foreground = new SolidColorBrush(Colors.DarkGray) };
                paragraph.Inlines.Add(runMsg);
            }

            _rtbLog.Document.Blocks.Add(paragraph);
            _rtbLog.ScrollToEnd();

            //lock (_lockGui)
            //{
            //    //if (_maxLine > 0 && LogQueue.Count > _maxLine)
            //    //{
            //    //    LogQueue.Dequeue();
            //    //}

            //    //LogQueue.Enqueue(new LogClass(datetime, lvl, msg, caller));
            //    //if (MaxLine > 0 && RtbLog.Lines.Length > MaxLine)
            //    //{
            //    //    RtbLog.Select(0, RtbLog.GetFirstCharIndexFromLine(RtbLog.Lines.Length - MaxLine));
            //    //    RtbLog.SelectedText = "\0";
            //    //}

            //    //SendMessage(RtbLog.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            //}
        }

        public void TransportToFile(string datetime, string msg, LogLevel lvl, string caller)
        {
            var filePath = $"{_txtLogPath}/{DateTime.Today:yyyy-MM-dd}.log";
            msg = $"[{datetime}] [{lvl}] {msg} (from {caller}){Environment.NewLine}";

            lock (_lockFile)
            {
                try
                {
                    File.AppendAllText(filePath, msg);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                    if (_writeGui && RtbLog != null)
                    {
                        if (!Application.Current.Dispatcher.CheckAccess())
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                TransportToGui(datetime, $"Lang.Msgs.WriteLogToFileError ({ex.Message})", LogLevel.Warning, caller);
                            });
                            return;
                        }
                        TransportToGui(datetime, $"Lang.Msgs.WriteLogToFileError ({ex.Message})", LogLevel.Warning, caller);
                    }
                }
            }
        }

        private string GenerateFullImageFilePath(string directoryName, string fileName, ImageFormat imageFormat)
        {
            try
            {
                Directory.CreateDirectory(directoryName);
            }
            catch (Exception ex)
            {
                Warning($"Lang.Msgs.CantCreateCaptureDir ({ex.Message})");
                return null;
            }

            if (fileName == null || _invalidFileNameCharsSet.Overlaps(fileName))
            {
                string tempPath = $"TEMP_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.{imageFormat}";
                Warning($"Given File Path : {fileName ?? "null"} is Invalid.");
                Warning($"Generating Temp file Path : {tempPath}.");
                fileName = tempPath;
            }

            string fullPath = $"{directoryName}/{fileName}";
            return fullPath;
        }

        public IEnumerable<string> Capture(List<string> folderPaths, List<string> fileNames, ImageFormat imageFormat = null)
        {
            if (string.IsNullOrEmpty(ProcessName))
            {
                Warning("Lang.Msgs.ProcessNameNotSet");
                return null;
            }

            IntPtr getWindow = FindWindow(null, ProcessName);

            if (getWindow == IntPtr.Zero)
            {
                Warning($"Lang.Msgs.ProcessNameInvalid ({ProcessName})");
                return null;
            }

            List<string> captureFilePaths = new List<string>();
            imageFormat = imageFormat ?? ImageFormat.Png;
            for (int i = 0; i < Math.Min(folderPaths.Count, fileNames.Count); ++i)
            {
                string fullPath = GenerateFullImageFilePath(folderPaths[i], fileNames[i], imageFormat);
                if (fullPath != null)
                {
                    captureFilePaths.Add(fullPath);
                }
            }

            if (captureFilePaths.Count() == 0)
            {
                Warning($"Zero Valid Path Available for Capture.");
                Warning($"Lang.Msgs.ScreenShotCaptureFailure");
                return null;
            }

            string captureFilePath = captureFilePaths[0];

            using (Graphics gData = Graphics.FromHwnd(getWindow))
            {
                if (gData.IsVisibleClipEmpty)
                {
                    Warning($"Lang.Msgs.NoWindowToBeCaptured");
                    return null;
                }

                Rectangle rect = Rectangle.Round(gData.VisibleClipBounds);
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    IntPtr hdc = g.GetHdc();
                    PrintWindow(getWindow, hdc, 1);
                    g.ReleaseHdc(hdc);
                    try
                    {
                        bmp.Save(captureFilePath, imageFormat);
                    }
                    catch (Exception ex)
                    {
                        Warning($"Lang.Msgs.ScreenShotCaptureFailure ({ex.Message})");
                        return null;
                    }
                }
            }

            try
            {
                for (int i = 1; i < captureFilePaths.Count; ++i)
                {
                    File.Copy(captureFilePath, captureFilePaths[i], true);
                }
                return captureFilePaths.AsEnumerable();
            }
            catch (Exception ex)
            {
                Warning($"Lang.Msgs.ScreenShotCaptureCopyError ({ex.Message})");
                return null;
            }
        }

        public void WriteCsv(string filePath, string line)
        {
            if (_writeCsv)
            {
                lock (_lockCsv)
                {
                    try
                    {
                        using (var sw = File.AppendText(filePath))
                        {
                            sw.WriteLine(line);
                        }
                    }
                    catch (Exception ex)
                    {
                        Warning($"Lang.Msgs.WriteCsvFailed ({ex.Message})");
                    }
                }
            }
        }
    }
}
