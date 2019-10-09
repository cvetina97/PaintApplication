using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace Paint
{
    public partial class Main : Form
    {
        Graphics g;
        Pen p = new Pen(Color.Black, 1);
        SolidBrush solidBrush = new SolidBrush(Color.White);
        Point startPoint = new Point(0, 0);
        Point endPoint = new Point(0, 0);
        int CurX, CurY, x, y, DiffX, DiffY;
        int control = 0;
        int controldraw = 0;
        bool closed = false;
        bool line = false;
        bool elipse = false;
        bool rectangle = false;
        bool pen = false;
        bool brush = false;
        bool eraser = false;
        bool text = false;
        Bitmap bitmapCopy = new Bitmap(300, 200);
        Bitmap bitmapDrawing;

        public Main()
        {
            InitializeComponent();
            bitmapDrawing = new Bitmap(pnlDrawing.Width, pnlDrawing.Height);
            pnlDrawing.Image = bitmapDrawing;// za da moga da vijdam kakvo risuvam 
            g = Graphics.FromImage(bitmapDrawing); // risuvam v/y Bitmapa
        }

        private void colorChange(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            p.Color = pb.BackColor;
            if(e.Button == MouseButtons.Left)
                pbDefault.BackColor = pb.BackColor;
        }

        private void pnlDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (control == 1)
            {
                x = e.X;
                y = e.Y;
                DiffX = x - CurX;
                DiffY = CurY - y;

                if (line)
                    g.DrawLine(p, CurX, CurY, x, y);
                if (elipse)
                    g.DrawEllipse(p, CurX, CurY, DiffX, -DiffY);
                if (rectangle)
                    g.DrawRectangle(p, CurX, CurY, DiffX, -DiffY);
                if(text)
                {
                    if(tbText.Text!=String.Empty)
                    {
                    string textInput = tbText.Text.Trim();
                    Font font=new Font("Times New Roman",12);
                    solidBrush.Color=pbDefault.BackColor;
                    g.DrawString(textInput, font, solidBrush, x, y);
                    tbText.ResetText();

                    }
                    else
                    {
                        MessageBox.Show("Input your text first !","Warning!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void pnlDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                if (line || elipse || rectangle || eraser || text)
                {
                    control = 1;
                    controldraw = 0;
                }
                else if(brush || pen)
                    controldraw = 1;
                this.Cursor = Cursors.Cross;
            }
            CurX = e.X;
            CurY = e.Y;
        }

        private void pnlDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            control = 0;
            controldraw = 0;
            if (pnlDrawing.Cursor == Cursors.Cross)
                pnlDrawing.Cursor = Cursors.Default;
            else
                return;
        }

        private void pnlDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (controldraw == 1)
            {
                if (brush)
                    p.Width = 10;
                else
                    p.Width = 1;
                endPoint = e.Location;
                g.DrawLine(p, startPoint, endPoint);
            }
            if(control == 1)
            {
                if(eraser)
                {
                    solidBrush.Color = Color.White;
                    g.FillRectangle(solidBrush, Math.Abs(e.X - 25), Math.Abs(e.Y - 25), 25, 25);
                }
            }
            
            startPoint = endPoint;
            pnlDrawing.Invalidate();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closed = true;
            this.Close();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closed)
                return;
            else
            {
                if (MessageBox.Show("Do you really want to quit ?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit ?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Environment.Exit(0);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename;
            openFileDialog1.Title = "Enter your picture!";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "JPEG Images|*jpg|GIF Images|*gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                pnlDrawing.BackgroundImage = Image.FromFile(filename);

            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Запис на файла";
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "JPEG Images |*jpg*;|GIF Images |*gif*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                pnlDrawing.DrawToBitmap(bitmapDrawing, pnlDrawing.ClientRectangle);

                var fileName = saveFileDialog1.FileName;
                if (!Path.HasExtension(fileName) || Path.GetExtension(fileName) != "jpg")
                    fileName = fileName + ".jpg";

                bitmapDrawing.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPaint info = new AboutPaint();
            info.ShowDialog();
        }

        private void ChoosingFormDrawing(object sender, EventArgs e)
        {
            ToolStripButton btn = (ToolStripButton)sender;
            if (btn.Name == "toolStripButton1")
            {
                line = true;
                elipse = false;
                rectangle = false;
                brush = false;
                pen = false;
                eraser = false;
                text = false;
            }
            else if (btn.Name == "toolStripButton2")
            {
                line = false;
                elipse = true;
                rectangle = false;
                brush = false;
                pen = false;
                eraser = false;
                text = false;
            }
            else if (btn.Name == "toolStripButton3")
            {
                line = false;
                elipse = false;
                rectangle = true;
                brush = false;
                pen = false;
                eraser = false;
                text = false;
            }
            else if (btn.Name == "toolStripButton4" )
            {
                line = false;
                elipse = false;
                rectangle = false;
                brush = false;
                pen = false ;
                eraser = true;
                text = false;
            }
            else if (btn.Name == "toolStripButton5")
            {
                line = false;
                elipse = false;
                rectangle = false;
                brush = false;
                pen = true;
                eraser = false;
                text = false;
                
                
            }
            else if(btn.Name=="toolStripButton6")
            {
                line = false;
                elipse = false;
                rectangle = false;
                brush = true;
                pen = false;
                eraser = false;
                text = false;
            }
            else if(btn.Name=="toolStripButton7")
            {
                line = false;
                elipse = false;
                rectangle = false;
                brush = false;
                pen = false;
                eraser = false;
                text = true;
            }
           
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rc=new Rectangle(new Point(Math.Abs(CurX - 150), Math.Abs(CurY - 100)),new Size(300,200));
            bitmapCopy = bitmapDrawing.Clone(rc,PixelFormat.Undefined);
            solidBrush.Color = Color.White;
            g.FillRectangle(solidBrush, rc);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rc = new Rectangle(new Point(Math.Abs(CurX - 150), Math.Abs(CurY - 100)), new Size(300, 200));
            bitmapCopy = bitmapDrawing.Clone(rc, PixelFormat.Undefined);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.DrawImage(bitmapCopy, Math.Abs(CurX - 150), Math.Abs(CurY - 100));
        }

        
    }
}