using System.Windows.Forms;

namespace nandMMC
{
    public partial class SafeComboBox : ComboBox
    {
        public SafeComboBox()
        {
            InitializeComponent();
        }

        public override string Text
        {
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

        public void AddItem(object obj)
        {
            Invoke(new MethodInvoker(() => Items.Add(obj)));
        }

        public void Clear()
        {
            if (InvokeRequired)
            {
                VoidDelegate callback = Clear;
                BeginInvoke(callback, new object[] { });
            }
            else
            {
                Items.Clear();
            }
        }

        private void SafeSetText(string text)
        {
            base.Text = text;
        }

        #region Nested type: TextDelegate

        private delegate void TextDelegate(string text);

        #endregion Nested type: TextDelegate

        #region Nested type: VoidDelegate

        private delegate void VoidDelegate();

        #endregion Nested type: VoidDelegate
    }
}