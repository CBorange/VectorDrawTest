using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Geometry;
using VectorDraw.Professional.Constants;


namespace Hicom.BizDraw.DrawControls
{
    public partial class TextCtrl : UserControl
    {
        private vdFigure _vText;

        private vdDocument _vDoc;
        public event EventHandler EventFinished;
        private bool _modify;
        private string _originText;
        public TextCtrl()
        {
            InitializeComponent();
        }

        public void Show(vdDocument document, gPoint insert, double height, double rotation, bool isMultiline = false, string text = "")
        {
            _modify = false;
            Visible = true;
            TextBox.Text = text;
            _vDoc = document;

            TextBox.Multiline = isMultiline;
            if (isMultiline)
            {
                _vText = new vdMText(_vDoc, text, insert, height);
                ((vdMText)_vText).HorJustify = VdConstHorJust.VdTextHorLeft;
                ((vdMText)_vText).VerJustify = VdConstVerJust.VdTextVerCen;
                ((vdMText)_vText).Style = _vDoc.ActiveTextStyle;
            } else
            {
                _vText = new vdText(_vDoc, text, insert, height, VdConstHorJust.VdTextHorLeft, VdConstVerJust.VdTextVerCen, _vDoc.ActiveTextStyle);
            }
            
            _vDoc.ActiveLayOut.Entities.AddItem(_vText);
            TextBox.Focus();
        }

        public void Show(vdDocument document, vdFigure vText)//vdText vText)
        {
            _modify = true;
            Visible = true;
            _vDoc = document;
            _vText = vText;

            if(vText is vdText)
            {
                TextBox.Text = ((vdText)_vText).TextString;
                _originText = ((vdText)_vText).TextString;
            } else
            {
                TextBox.Text = ((vdMText)_vText).TextString;
                _originText = ((vdMText)_vText).TextString;
            }
            //TextBox.Text = _vText.TextString;
            TextBox.Focus();
            //_originText = _vText.TextString;
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    if(this.Visible)
                    {
                        if (!_modify)
                        {
                            _vDoc.ActiveLayOut.Entities.RemoveItem(_vText);
                            _vText = null;
                        }
                        else
                        {
                            if (_vText is vdText)
                                ((vdText)_vText).TextString = _originText;
                            else
                                ((vdMText)_vText).TextString = _originText;

                            //_vText.TextString = _originText;
                            _vText.Update();
                        }
                        this.Visible = false;                        
                        _vDoc.Redraw(true);
                        if (EventFinished != null)
                            EventFinished(this, new EventArgs());
                    }
                    break;
                case Keys.Enter:
                    if(TextBox.Multiline == false)
                    {
                        _vText = null;
                        this.Visible = false;
                        if (EventFinished != null)
                            EventFinished(this, new EventArgs());
                    }
                    break;
                case Keys.Shift | Keys.Enter:
                    if (TextBox.Multiline)
                    {
                        _vText = null;
                        this.Visible = false;
                        if (EventFinished != null)
                            EventFinished(this, new EventArgs());
                    }
                    break;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_vText == null)
                return;

            if (_vText is vdText)
                ((vdText)_vText).TextString = string.Format(TextBox.Text);
            else
            {
                ((vdMText)_vText).TextString = string.Format(TextBox.Text);
                Size s = TextRenderer.MeasureText(TextBox.Text, this.TextBox.Font);
                s.Height += 8;
                if (s.Width < 100) s.Width = 100;
                if (s.Height < 20) s.Height = 20;
                this.Height = s.Height;
                //this.Height = TextBox.Lines.Length <= 1 ? TextBox.PreferredHeight : (TextBox.PreferredHeight-TextBox.Margin.Bottom)*TextBox.Lines.Length;
            }
            //_vText.TextString = string.Format(TextBox.Text);
            _vText.Update();
            _vDoc.Redraw(true);
        }
    }
}
