using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace nandMMC
{
    public class SafeToolStripLabel : ToolStripStatusLabel
    {
        private delegate string GetString();

        private delegate void SetText(string text);

        [Localizable(false)]
        public override string Text
        {
            get
            {
                if ((Parent != null) && (Parent.InvokeRequired))
                {
                    GetString getTextDel = () => base.Text;
                    var text = String.Empty;
                    try
                    {
                        // Invoke the SetText operation from the Parent of the ToolStripStatusLabel
                        text = (string)Parent.Invoke(getTextDel, null);
                    }
                    catch
                    {
                    }

                    return text;
                }
                return base.Text;
            }

            set
            {
                // Get from the container if Invoke is required
                if (Parent != null &&        // Make sure that the container is already built
                    Parent.InvokeRequired)   // Is Invoke required?
                {
                    SetText setTextDel = delegate(string text)
                                             {
                                                 base.Text = text;
                                             };

                    try
                    {
                        // Invoke the SetText operation from the Parent of the ToolStripStatusLabel
                        Parent.Invoke(setTextDel, new object[] { value });
                    }
                    catch
                    {
                    }
                }
                else
                    base.Text = value;
            }
        }
    }
}