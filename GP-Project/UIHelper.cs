
using ScottPlot.Styles;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GP_Project
{
    public static class UIHelper
    {
        // Borderradius (Panel, Form ve Button)
        public static void SetRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
                path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
                path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }
        }

        // Placeholder
        public static void SetPlaceholder(TextBox textBox, string placeholder)
        {
            if (textBox == null) return;

            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.GotFocus -= TextBox_GotFocus;
            textBox.LostFocus -= TextBox_LostFocus;

            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;

            textBox.Tag = placeholder;
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 400,
                    Height = 180,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                Button confirmation = new Button() { Text = "Tamam", Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK };

                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }
        private static void TextBox_GotFocus(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == (string)textBox.Tag)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.White;
            }
        }

        private static void TextBox_LostFocus(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = (string)textBox.Tag;
                textBox.ForeColor = Color.Gray;
            }
        }

        // Underline hareketi
        public static void MoveUnderlineTo(Panel underlinePanel, Button targetButton)
        {
            if (underlinePanel == null || targetButton == null)
                return;

            underlinePanel.Width = targetButton.Width;
            underlinePanel.Left = targetButton.Left;
            underlinePanel.Top = targetButton.Top + targetButton.Height - 1;
        }

        // Uyarı Popup
        public static void ShowPopup(string message, string type = "Info")
        {
            customWarningForm popup = new customWarningForm();
            popup.SetPopupStyle(type, message);
            popup.ShowDialog();
        }

        // Menü Buton aktifleştirici
        public static void ActivateMenuButton(ref Button activeButton, Button clickedButton)
        {
            if (activeButton != null)
                activeButton.BackColor = Color.Black;

            clickedButton.BackColor = Color.SlateBlue;

            activeButton = clickedButton;
        }
    }
}
