using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace eFilingMailServiceSetting
{
    class ComBox
    {
        #region ComboBoxItem
        /// <summary>
        /// Combo Box Item
        /// </summary>
        public class ComboBoxItem
        {
            /// <summary>
            /// Text
            /// </summary>
            private string _Text = null;
            /// <summary>
            /// Text
            /// </summary>
            public string Text { get { return this._Text; } set { this._Text = value; } }
            /// <summary>
            /// Value
            /// </summary>
            private object _Value = null;
            /// <summary>
            /// Value
            /// </summary>
            public object Value { get { return this._Value; } set { this._Value = value; } }
            /// <summary>
            /// Tag
            /// </summary>
            private object _Tag = null;
            /// <summary>
            /// Tag
            /// </summary>
            public object Tag { get { return this._Tag; } set { this._Tag = value; } }
            /// <summary>
            /// 字體顏色
            /// </summary>
            private Color _ForeColor = Color.Black;
            /// <summary>
            /// 字體顏色
            /// </summary>
            public Color ForeColor { get { return this._ForeColor; } set { this._ForeColor = value; } }
            /// <summary>
            /// return Text
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this._Text;
            }
        }
        #endregion

        #region SkinComboBox()
        /// <summary>
        /// 重繪 Combo Box
        /// </summary>
        /// <param name="ComBox"></param>
        public void SkinComboBox(ComboBox ComBox)
        {
            ComBox.DrawItem += new DrawItemEventHandler(this.ComboBox_DrawItem);

            ComBox.DrawMode = DrawMode.OwnerDrawFixed;
        }
        #endregion

        #region ComboBox_DrawItem()
        /// <summary>
        /// 重繪 Combo Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            ComboBox comBox = (ComboBox)sender;

            Graphics g = e.Graphics;

            Rectangle rec = e.Bounds;

            Font font = comBox.Font;

            if (e.Index > -1)
            {
                if (comBox.Items[e.Index] != null && (ComBox.ComboBoxItem)comBox.Items[e.Index] != null)
                {
                    ComBox.ComboBoxItem comBoxItem = (ComBox.ComboBoxItem)comBox.Items[e.Index];

                    SolidBrush solidBrush = new SolidBrush(Color.LightSkyBlue);
                    
                    if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                    {
                        solidBrush = new SolidBrush(Color.LightSkyBlue);

                        e.Graphics.FillRectangle(solidBrush, rec);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(solidBrush, rec);
                    }
                    solidBrush.Dispose();

                    System.Drawing.StringFormat sf = new System.Drawing.StringFormat();

                    SolidBrush sb = new SolidBrush(comBoxItem.ForeColor);

                    sf.Alignment = StringAlignment.Near;

                    e.Graphics.DrawString(comBoxItem.Text, font, sb, rec, sf);

                    e.DrawFocusRectangle();

                    sf = null; sb = null;
                }
            }
        }
        #endregion
    }
}
