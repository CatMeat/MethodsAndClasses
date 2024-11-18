// Requires Settings.settings file:
/*
 *  <?xml version='1.0' encoding='utf-8'?>
<SettingsFile xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings" CurrentProfile="(Default)" GeneratedClassNamespace="SaveFormLocation.Properties" GeneratedClassName="Settings">
  <Profiles />
  <Settings>
    <Setting Name="WindowPosition" Type="System.Drawing.Rectangle" Scope="User">
      <Value Profile="(Default)">0, 0, 0, 0</Value>
    </Setting>
    <Setting Name="WindowState" Type="System.Windows.Forms.FormWindowState" Scope="User">
      <Value Profile="(Default)">Normal</Value>
    </Setting>
  </Settings>
</SettingsFile>
 */

using MethodsAndClasses.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowTools
{
    public static class Window
    {
        private static bool windowInitialized = false;
        public static void Restore(Form m_Window)
        {
            m_Window.WindowState = FormWindowState.Normal;
            m_Window.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (Settings.Default.WindowPosition != Rectangle.Empty &&
                IsVisibleOnAnyScreen(Settings.Default.WindowPosition))
            {
                m_Window.StartPosition = FormStartPosition.Manual;
                m_Window.DesktopBounds = Settings.Default.WindowPosition;
                m_Window.WindowState = Settings.Default.WindowState;
            }
            else
            {
                m_Window.StartPosition = FormStartPosition.WindowsDefaultLocation;

                if (Settings.Default.WindowPosition != Rectangle.Empty)
                {
                    m_Window.Size = Settings.Default.WindowPosition.Size;
                }
            }
            windowInitialized = true;
        }

        private static bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Save(Form m_Window)
        {
            switch (m_Window.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    Settings.Default.WindowState = m_Window.WindowState;
                    break;

                default:
                    Settings.Default.WindowState = FormWindowState.Normal;
                    break;
            }

            Settings.Default.Save();
        }

        public static void TrackState(Form m_Window)
        {
            // Don't record the window setup, otherwise we lose the persistent values!
            if (!windowInitialized) return; 

            if (m_Window.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowPosition = m_Window.DesktopBounds;
            }
        }

        /* Include this code in main form
         * 
                #region Windowmanager Overrides, no references required
                protected override void OnClosed(EventArgs e)
                {
                    base.OnClosed(e);
                    Window.Save(this);
                }

                protected override void OnResize(EventArgs e)
                {
                    base.OnResize(e);
                    Window.TrackState(this);
                }

                protected override void OnMove(EventArgs e)
                {
                    base.OnMove(e);
                    Window.TrackState(this);
                }
                #endregion
         */

    }
}

namespace TextTools
{
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Appends colored text to the current text of a RichTextBox
        /// </summary>
        public static void AppendText(this RichTextBox box, string text, Color color, Color? backColor = null)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionBackColor = backColor ?? Color.Black;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.SelectionBackColor = box.BackColor;
        }

        public static void TrimLines(this RichTextBox box, int numberOfLinesToKeep)
        {
            if (box.Lines.Length > (numberOfLinesToKeep))
            {
                var linestoDelete = box.Lines.Length - numberOfLinesToKeep;
                box.SelectionStart = 0;
                box.SelectionLength = box.GetFirstCharIndexFromLine(linestoDelete);
                box.SelectedText = "";
            }
        }

        public static void ScrollToBottom(this RichTextBox box)
        {
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string that has reduced consecutive internal whitespace to a single whitespace.
        /// </summary>
        public static string TrimInternal(this String _string)
        {
            while (_string.Contains("  "))
            {
                _string = _string.Replace("  ", " ");
            }
            return _string;
        }

    }
}
