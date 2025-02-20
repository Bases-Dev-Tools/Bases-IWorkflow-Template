namespace Bases.Utilities
{
    using System;
    using System.Text;
    using Hyland.Unity;
    using Hyland.Unity.CodeAnalysis;

    /// <summary>
    /// A Utility class for simplified logging and controls log chatter by setting logging levels automatically.
    /// </summary>
    public class BasesDiagnostics
    {
        private Diagnostics.DiagnosticsLevel _diagnosticsLevel;
        public Diagnostics.DiagnosticsLevel diagnosticsLevel { get { return _diagnosticsLevel; } set { SetDiagnosticsLevel(value); } }
        public Hyland.Unity.Application app;
        public BasesDiagnostics(Hyland.Unity.Application app)
        {
            this.app = app;\
            // To Customize: All Evnironments that are not PROD rely on the InstallID, feel free to customize this to your needs.
            if (app.SystemProperties.IsProduction)
            {
                SetDiagnosticsLevel(Diagnostics.DiagnosticsLevel.Warning);
            }
            else if (app.SystemProperties.InstallID.ToLower().Contains("uat"))
            {
                SetDiagnosticsLevel(Diagnostics.DiagnosticsLevel.Info);
            }
            else
            {

                SetDiagnosticsLevel(Diagnostics.DiagnosticsLevel.Verbose);
            }
        }
        /// <summary>
        /// Used for logging error messages. Uses the Diagnostics Level ERROR.
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Write(Diagnostics.DiagnosticsLevel.Error, message);
        }
        /// <summary>
        /// Used for logging error messages. Uses the Diagnostics Level ERROR.
        /// </summary>
        /// <param name="message"></param>
        public void Error(Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Error, ex);
        }
        /// <summary>
        /// Used for logging error messages. Uses the Diagnostics Level ERROR.
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message, Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Error, message);
            Write(Diagnostics.DiagnosticsLevel.Error, ex);
        }
        /// <summary>
        /// Used for logging warning messages. Uses the Diagnostic Level WARN.
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            Write(Diagnostics.DiagnosticsLevel.Warning, message);
        }
        /// <summary>
        /// Used for logging warning messages. Uses the Diagnostic Level WARN.
        /// </summary>
        /// <param name="ex"></param>
        public void Warn(Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Warning, ex);
        }
        /// <summary>
        /// Used for logging warning messages. Uses the Diagnostic Level WARN.
        /// </summary>
        /// <param name="ex"></param>
        public void Warn(string message, Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Warning, message);
            Write(Diagnostics.DiagnosticsLevel.Warning, ex);
        }
        /// <summary>
        /// Used for logging generic script events and messages. Uses the Diagnostic Level INFO
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            Write(Diagnostics.DiagnosticsLevel.Info, message);
        }
        /// <summary>
        /// Used for logging informational script events. Uses the Diagnostic Level INFO
        /// </summary>
        /// <param name="ex"></param>
        public void Info(Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Info, ex);
        }
        /// <summary>
        /// Used for logging informational script events. Uses the Diagnostic Level INFO
        /// </summary>
        /// <param name="ex"></param>
        public void Info(string message, Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Info, message);
            Write(Diagnostics.DiagnosticsLevel.Info, ex);
        }
        /// <summary>
        /// Used for Script debugging in non-prod and non-UAT environments. Uses the Diagnostic Level VERBOSE
        /// </summary>
        /// <param name="message"></param>
        public void Trace(string message)
        {
            Write(Diagnostics.DiagnosticsLevel.Verbose, message);
        }
        /// <summary>
        /// Used for Script debugging in non-prod and non-UAT environments. Uses the Diagnostic Level VERBOSE
        /// </summary>
        /// <param name="ex"></param>
        public void Trace(Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Verbose, ex);
        }
        /// <summary>
        /// Used for Script debugging in non-prod and non-UAT environments. Uses the Diagnostic Level VERBOSE
        /// </summary>
        /// <param name="ex"></param>
        public void Trace(string message, Exception ex)
        {
            Write(Diagnostics.DiagnosticsLevel.Verbose, message);
            Write(Diagnostics.DiagnosticsLevel.Verbose, ex);
        }
        /// <summary>
        /// USE WITH CAUTION: Overrides the environment default for Diagnostics Level.
        /// </summary>
        /// <param name="level"></param>
        private void SetDiagnosticsLevel(Diagnostics.DiagnosticsLevel level)
        {
            _diagnosticsLevel = level;
            app.Diagnostics.Level = level;
            string message = $"Diagnostics level has been overwritten to {level}";
            Write(Diagnostics.DiagnosticsLevel.Warning, message);
        }
        private void Write(Diagnostics.DiagnosticsLevel level, string message)
        {
            app.Diagnostics.WriteIf(level, message);
        }
        private void Write(Diagnostics.DiagnosticsLevel level, Exception ex)
        {
            app.Diagnostics.WriteIf(level, ex);
        }
    }
}
