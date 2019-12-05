using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Wave_Shaper
{
    public partial class Form1 : Form
    {
        int[,] points = new int[10, 2];
        int x0 = 20, y0 = 20, w0 = 800, h0 = 400;
        int nrp = 0, startingPoint = 0, selected = -2; // -2 = none; -1 = startingPoint; 0, 1, 2... = points
        SoundPlayer player = new SoundPlayer();
        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
            Redraw();
            this.label2.Text = nrp.ToString() + " points";
        }

        private void addPoint(object sender, EventArgs e)
        {
            int x = 400, y = 0;
            if(nrp < 10)
            {
                points[nrp, 0] = x;
                points[nrp, 1] = y;
                nrp++;
                SortPoints();
                Redraw();
            }
            this.label2.Text = nrp.ToString() + " point";
            if (nrp != 1)
            {
                this.label2.Text += "s";
            }
        }

        private void removePoint(object sender, EventArgs e)
        {
            int i;
            if(selected >= 0 && nrp > 0)
            {
                for(i = selected; i < nrp - 1; i++)
                {
                    points[i, 0] = points[i + 1, 0];
                    points[i, 1] = points[i + 1, 1];
                }
                nrp--;
                selected = -2;
            }
            this.label2.Text = nrp.ToString() + " point";
            if(nrp != 1)
            {
                this.label2.Text += "s";
            }
        }

        private void Redraw()
        {
            int i;
            g.Clear(this.BackColor);
            Rectangle shaper = new Rectangle(x0, y0, w0, h0);
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen blackPen = new Pen(Color.Black, 3);
            Pen redPen = new Pen(Color.Red, 1);
            Pen darkRedPen = new Pen(Color.DarkRed, 1);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush darkRedBrush = new SolidBrush(Color.DarkRed);
            g.FillRectangle(whiteBrush, shaper);
            g.DrawRectangle(blackPen, shaper);

            if (nrp > 0)
            {
                g.DrawLine(bluePen, x0, y0 + h0 / 2 - startingPoint, x0 + points[0, 0], y0 + h0 / 2 - points[0, 1]);
                for (i = 0; i < nrp - 1; i++)
                {
                    g.DrawLine(bluePen, x0 + points[i, 0], y0 + h0 / 2 - points[i, 1], x0 + points[i + 1, 0], y0 + h0 / 2 - points[i + 1, 1]);
                }
                g.DrawLine(bluePen, x0 + points[nrp - 1, 0], y0 + h0 / 2 - points[nrp - 1, 1], x0 + w0, y0 + h0 / 2 - startingPoint);
            }
            else
            {
                g.DrawLine(bluePen, x0, y0 + h0 / 2 - startingPoint, x0 + w0, y0 + h0 / 2 - startingPoint);
            }

            if (selected == -1)
            {
                g.DrawEllipse(darkRedPen, x0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.FillEllipse(darkRedBrush, x0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.DrawEllipse(darkRedPen, x0 + w0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.FillEllipse(darkRedBrush, x0 + w0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
            }
            else
            {
                g.DrawEllipse(redPen, x0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.FillEllipse(redBrush, x0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.DrawEllipse(redPen, x0 + w0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
                g.FillEllipse(redBrush, x0 + w0 - 8, y0 + h0 / 2 - startingPoint - 8, 16, 16);
            }
            for (i = 0; i < nrp; i++)
            {
                if(selected == i)
                {
                    g.DrawEllipse(darkRedPen, x0 + points[i, 0] - 8, y0 + h0 / 2 - points[i, 1] - 8, 16, 16);
                    g.FillEllipse(darkRedBrush, x0 + points[i, 0] - 8, y0 + h0 / 2 - points[i, 1] - 8, 16, 16);
                }
                else
                {
                    g.DrawEllipse(redPen, x0 + points[i, 0] - 8, y0 + h0 / 2 - points[i, 1] - 8, 16, 16);
                    g.FillEllipse(redBrush, x0 + points[i, 0] - 8, y0 + h0 / 2 - points[i, 1] - 8, 16, 16);
                }
            }
        }

        private void SortPoints()
        {
            int i, j, aux;
            for (i = 0; i < nrp - 1; i++)
            {
                for(j = i + 1; j < nrp; j++)
                {
                    if(points[i, 0] > points[j, 0])
                    {
                        aux = points[i, 0];
                        points[i, 0] = points[j, 0];
                        points[j, 0] = aux;

                        aux = points[i, 1];
                        points[i, 1] = points[j, 1];
                        points[j, 1] = aux;

                        if(selected == i)
                        {
                            selected = j;
                        }
                        else if(selected == j)
                        {
                            selected = i;
                        }
                    }
                }
            }
        }

        private void WriteWavFile()
        {
            FileStream stream = File.Create("Wave.wav");
            BinaryWriter writer = new BinaryWriter(stream);

            int i, j;
            int NumSamples;
            int frequency = (int)this.numericUpDown1.Value;
            double ratio_w, ratio_h, sample;
            short short_sample;
            // wav
            int ChunkID = 0x46464952;
            int ChunkSize;
            int Format = 0x45564157;
            int Subchunck1ID = 0x20746D66;
            int Subchunk1Size = 16;
            short AudioFormat = 1;
            short NumChannels = 2;
            int SampleRate = 44100;
            NumSamples = SampleRate / frequency;
            int ByteRate;
            short BlockAlign;
            short BitsPerSample = 16;
            BlockAlign = (short)((int)NumChannels * (int)BitsPerSample / 8);
            ByteRate = SampleRate * NumChannels * BitsPerSample / 8;
            int Subchunk2ID = 0x61746164;
            int Subchunk2Size = NumSamples * NumChannels * BitsPerSample / 8;
            ChunkSize = 36 + Subchunk2Size;

            writer.Write(ChunkID);
            writer.Write(ChunkSize);
            writer.Write(Format);
            writer.Write(Subchunck1ID);
            writer.Write(Subchunk1Size);
            writer.Write(AudioFormat);
            writer.Write(NumChannels);
            writer.Write(SampleRate);
            writer.Write(ByteRate);
            writer.Write(BlockAlign);
            writer.Write(BitsPerSample);
            writer.Write(Subchunk2ID);
            writer.Write(Subchunk2Size);

            ratio_w = (double)w0 / (double)NumSamples;
            ratio_h = 32767.0 / (h0 / 2);
            for (i = 0; i < NumSamples; i++)
            {
                j = 0;
                while (j < nrp && i * ratio_w >= points[j, 0])
                {
                    j++;
                }
                if(nrp == 0)
                {
                    sample = ratio_h * startingPoint;
                }
                else if (j == 0)
                {
                    sample = ratio_h * ((points[j, 1] - startingPoint) / points[j, 0] * ratio_w * i);
                }
                else if (j == nrp)
                {
                    sample = ratio_h * (points[j - 1, 1] + ((startingPoint - points[j - 1, 1]) / (w0 - points[j - 1, 0]) * (ratio_w * i - points[j - 1, 0])));
                }
                else
                {
                    sample = ratio_h * (points[j - 1, 1] + ((points[j, 1] - points[j - 1, 1]) / (points[j, 0] - points[j - 1, 0]) * (ratio_w * i - points[j - 1, 0])));
                }

                short_sample = (short)sample;
                stream.WriteByte((byte)(short_sample & 0xff));
                stream.WriteByte((byte)(short_sample >> 8));
                stream.WriteByte((byte)(short_sample & 0xff));
                stream.WriteByte((byte)(short_sample >> 8));
            }
            stream.Close();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            int i, s = 0;
            if (e.Button == MouseButtons.Left)
            {
                if ((e.X >= x0 - 8 && e.X <= x0 + 8 || e.X >= x0 + w0 - 8 && e.X <= x0 + w0 + 8) && e.Y >= y0 + h0 / 2 - 8 - startingPoint && e.Y <= y0 + h0 / 2 + 8 - startingPoint)
                {
                    selected = -1;
                }
                else
                {
                    for (i = nrp - 1; i >= 0; i--)
                    {
                        if (e.X >= x0 + points[i, 0] - 8 && e.X <= x0 + points[i, 0] + 8 && e.Y >= y0 + h0 / 2 - points[i, 1] - 8 && e.Y <= y0 + h0 / 2 - points[i, 1] + 8)
                        {
                            selected = i;
                            s = 1;
                            break;
                        }
                    }
                    if (s == 0)
                    {
                        selected = -2;
                    }
                }
                Redraw();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if(selected == -1)
                {
                    startingPoint = y0 + h0 / 2 - e.Y;
                    if(startingPoint > 200)
                    {
                        startingPoint = 200;
                    }
                    if (startingPoint < -200)
                    {
                        startingPoint = -200;
                    }
                }
                else if(selected >= 0)
                {
                    points[selected, 0] = e.X - x0;
                    if (points[selected, 0] > 800)
                    {
                        points[selected, 0] = 800;
                    }
                    if (points[selected, 0] < 0)
                    {
                        points[selected, 0] = 0;
                    }

                    points[selected, 1] = y0 + h0 / 2 - e.Y;
                    if (points[selected, 1] > 200)
                    {
                        points[selected, 1] = 200;
                    }
                    if (points[selected, 1] < -200)
                    {
                        points[selected, 1] = -200;
                    }
                }
                SortPoints();
                Redraw();
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            player.Stop();
            WriteWavFile();
            player.SoundLocation = "Wave.wav";
            player.PlayLooping();
        }

        private void OnStop(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Redraw();
        }  
    }
}
