using System;
using System.Windows.Forms;
using WindowTools;

namespace MethodsAndClasses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Window.Restore(this);
        }

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
    }
}
