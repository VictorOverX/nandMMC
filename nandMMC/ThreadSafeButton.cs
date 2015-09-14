using System.Windows.Forms;

namespace nandMMC
{
    public partial class SafeButton : Button
    {
        public SafeButton()
        {
            InitializeComponent();
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (InvokeRequired)
                {
                    BoolDelegate callback = SafeSetEnabled;
                    BeginInvoke(callback, new object[] { value });
                }
                else
                {
                    base.Enabled = value;
                }
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (InvokeRequired)
                {
                    TextDelegate callback = SafeSetText;
                    BeginInvoke(callback, new object[] { value });
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        private void SafeSetEnabled(bool value)
        {
            base.Enabled = value;
        }

        private void SafeSetText(string text)
        {
            base.Text = text;
        }

        #region Nested type: BoolDelegate

        private delegate void BoolDelegate(bool value);

        #endregion Nested type: BoolDelegate

        #region Nested type: TextDelegate

        private delegate void TextDelegate(string text);

        #endregion Nested type: TextDelegate
    }
}