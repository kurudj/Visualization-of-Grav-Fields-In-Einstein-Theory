using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using System.IO;
using System.Threading;


namespace opengl1
{
    public partial class Form1 : Form
    {
        bool two=false, three=false,threuclid = false, four=false;
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }
        Class2 camera = new Class2();
        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(255, 255, 255, 255);
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
        }
        private void button6_Click(object sender, EventArgs e)
        {

        }
        float angle = Gl.GL_IMAGE_ROTATE_ANGLE_HP;
        Point[,,] mass = new Point[1600, 100, 100];
        Point[] points = new Point[16000000];
        double[] znach = new double[16000000];
        private void button2_Click(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glColor3f(1.0f, 0, 0);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -6);
            angle = angle * 20;
            Gl.glRotated(angle, 1, 0, 0);
            Glut.glutWireSphere(2, 32, 32);
            Gl.glTranslated(0, 0, 0);
            Gl.glRotated(90, 1, 0, 0);
            Gl.glColor3d(0.0, 0.0, 1.0);
                Gl.glBegin(Gl.GL_LINES);
                for (int z = -3; z <= 3; z++)
                {
                    for (int i = -3; i <= 3; ++i)
                    {
                        Gl.glVertex3f(i, z, -3);
                        Gl.glVertex3f(i, z, 3);
                        Gl.glVertex3f(3, z, i);
                        Gl.glVertex3f(-3, z, i);
                    }
                }
                for (int k = -3; k <= 3; k++)
                {
                    for (int i = -3; i <= 3; ++i)
                    {
                        Gl.glVertex3f(k, -3, i);
                        Gl.glVertex3f(k, 3, i);
                        Gl.glVertex3f(k, i, 3);
                        Gl.glVertex3f(k, i, -3);
                    }
                }
            Gl.glEnd();
            Gl.glPopMatrix();
            Gl.glFlush();
            // fuct4(point.x,point.y)
            AnT.Invalidate();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Gl.glClearColor(255, 255, 255, 255);
            Gl.glColor3f(1.0f, 0, 0);
            Glut.glutWireCube(1.0f);
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        public Matrix g3(double x, double y, double z)
        {
                Matrix metr4 = new Matrix(3, 3);
                for (int i = 0; i <= 2; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        metr4.mat[i, j] = 0;
                    }
                }
                if (three == true)
                {
                    metr4.mat[0, 0] = 1;
                    metr4.mat[1, 1] = 1;
                    metr4.mat[2, 2] = 1;
                }
                if (threuclid == true)
                {
                   metr4.mat[0, 0] = 1 / Math.Pow((1 + (Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) / 4),2);
                    metr4.mat[1, 1] = 1;
                    metr4.mat[2, 2] = 1;
                }
                return metr4;
        }   // метрика g в треххмерном случае
        public Matrix g2(double d, double r)
        {
            Matrix metr4 = new Matrix(2, 2);
            metr4.mat[0, 1] = 0;
            metr4.mat[1, 0] = 0;
            metr4.mat[0, 0] = Math.Pow(r, 2);
            metr4.mat[1, 1] = Math.Pow(r, 2) * Math.Pow(Math.Sin(d), 2);
            return metr4;
        }   // метрика g в двумерном случае
        public Matrix g4(double d,double r)
        {
                double rg=1;
                Matrix metr4 = new Matrix(4, 4);
                    for (int i = 0; i <= 3; i++)
                    {
                        for (int j = 0; j <= 3; j++)
                        {
                            metr4.mat[i, j] = 0;
                        }
                    }
                        metr4.mat[0, 0] = 1 - (rg / r);
                        metr4.mat[1, 1] = 1 / (-(1 - (rg / r)));
                        metr4.mat[2, 2] = -Math.Pow(r, 2);
                        metr4.mat[3, 3] = -(Math.Pow(r, 2) * (Math.Pow(Math.Sin(d), 2)));
            if(metr4.Det()==0)
            {
                r += 0.1;
                metr4.mat[0, 0] = 1 - (rg / r);
                metr4.mat[1, 1] = 1 / (-(1 - (rg / r)));
                metr4.mat[2, 2] = -Math.Pow(r, 2);
                metr4.mat[3, 3] = -(Math.Pow(r, 2) * (Math.Pow(Math.Sin(d), 2)));
            }
                    return metr4;

        }  // метрика g в четырехмерном случае
        double otr1 = Math.PI/100;
        double otr = 0.0025;
        private void button1_Click(object sender, EventArgs e)
        {
            Point point = new Point();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glColor3f(1.0f, 0, 0);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -6);
            Gl.glRotated(90, 1, 0, 0);
            int c = 0;
            otr = 1;
            otr1 = Math.PI / 4;
            if (four == true)
            {
                 Glut.glutWireSphere(2, 30, 30);
                 for(t=0; t<=4;t+=otr)
                 {
                     for(s=1; s<=Math.PI;s+=otr1)
                     {
                          for(p=1;p<=2*Math.PI;p+=otr1)
                          {
                             point.x = t * Math.Sin(s) * Math.Cos(p);
                             point.y = t * Math.Sin(s) * Math.Sin(p);
                             point.z = t * Math.Cos(s);
                             points[c] = point;
                             znach[c] = znachenie(s, p, t, otr, otr1);
                             c++;
                          }
                     }
                }
                colrang(znach, points);
            }
            Gl.glTranslated(0, 0, 0);
            Gl.glRotated(90, 1, 0, 0);
            Gl.glColor3d(0.0, 0.0, 1.0);
            if (threuclid == true || three==true)
            {
                Gl.glBegin(Gl.GL_LINES);
                for (int z = -3; z <= 3; z++)
                {
                    for (int i = -3; i <= 3; ++i)
                    {
                        Gl.glVertex3f(i, z, -3);
                        Gl.glVertex3f(i, z, 3);
                        Gl.glVertex3f(3, z, i);
                        Gl.glVertex3f(-3, z, i);
                    }
                }
                for (int k = -3; k <= 3; k++)
                {
                    for (int i = -3; i <= 3; ++i)
                    {
                        Gl.glVertex3f(k, -3, i);
                        Gl.glVertex3f(k, 3, i);
                        Gl.glVertex3f(k, i, 3);
                        Gl.glVertex3f(k, i, -3);
                    }
                }
                Gl.glEnd();
            }
            Gl.glPopMatrix();
            Gl.glFlush();
           // fuct4(point.x,point.y)
            AnT.Invalidate();
        }
        // t - 0     r - 1     theta - 2     phi - 3
        public double[,,] delta4(double d,double otr,double otr1, double r)  // разность метрик при смещении на одну координату в четырехмерном случае
        {
            otr1 = Math.PI / 1000;
            otr = 0.006;
            double[,,] metr7 = new double[4,4,4];
            for (int a = 0; a <= 3; a++)
            {
                for (int b = 0; b <= 3; b++)
                {
                    metr7[a, b, 0] = (g4(d, r).mat[a, b] - g4(d, r).mat[a, b]) / (2 * otr);
                    metr7[a, b, 1] = (g4(d, r+otr).mat[a, b] - g4(d, r-otr).mat[a, b]) / (2 * otr);
                    metr7[a, b, 2] = (g4(d+otr1, r).mat[a, b] - g4(d-otr1, r).mat[a, b]) / (2 * otr1);
                    metr7[a, b, 3] = (g4(d, r).mat[a, b] - g4(d, r).mat[a, b]) / (2 * otr1);
                }
            }
            return metr7;
        }
        // x - 0      y - 1     z - 2
        public double[,,] delta3(double x, double y, double z, double otr)  // разность метрик при смещении на одну координату в трехмерном случае
        {
            otr = 0.006;
            double[,,] metr7 = new double[3, 3, 3];
            for (int a = 0; a <= 2; a++)
            {
                for (int b = 0; b <= 2; b++)
                {
                    metr7[a, b, 0] = (g3(x+otr, y, z).mat[a, b] - g3(x-otr, y, z).mat[a, b]) / (2 * otr);
                    metr7[a, b, 1] = (g3(x, y+otr, z).mat[a, b] - g3(x, y-otr, z).mat[a, b]) / (2 * otr);
                    metr7[a, b, 2] = (g3(x, y, z+otr).mat[a, b] - g3(x, y, z-otr).mat[a, b]) / (2 * otr);
                }
            }
            return metr7;
        }
        // theta - 0   phi - 1
        public double[,,] delta2(double d, double r, double otr1)  // разность метрик при смещении на одну координату в двумерном случае
        {
            otr = 0.006;
            double[,,] metr7 = new double[2, 2, 2];
            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    metr7[a, b, 0] = (g2(d+otr1, r).mat[a, b] - g2(d-otr1, r).mat[a, b]) / (2 * otr1);
                    metr7[a, b, 1] = (g2(d, r).mat[a, b] - g2(d, r).mat[a, b]) / (2 * otr);
                }
            }
            return metr7;
        }
        public double[,,,] pointdelta4(double d, double f, double otr, double otr1, double r)  // точечная разность в четырехмерном случае
        {
            double[,,,] metr8 = new double[4, 4, 4, 4];
            otr1 = Math.PI / 1000;
            double gplus = 0, gminus = 0;
            for (int a = 0; a <= 3; a++)
            {
                for (int b = 0; b <= 3; b++)
                {
                    metr8[a, b, 0, 0] = 0;
                    metr8[a, b, 1, 1] = (g4(d, r + otr).mat[a, b] - 2 * g4(d, r).mat[a, b] + g4(d, r - otr)[a, b]) / Math.Pow(otr, 2);
                    metr8[a, b, 2, 2] = (g4(d + otr1, r).mat[a, b] - 2 * g4(d, r).mat[a, b] + g4(d - otr1, r)[a, b]) / Math.Pow(otr1, 2);
                    metr8[a, b, 3, 3] = 0;
                    metr8[a, b, 0, 3] = 0;
                    metr8[a, b, 3, 0] = 0;
                    metr8[a, b, 0, 1] = 0;
                    metr8[a, b, 1, 0] = 0;
                    metr8[a, b, 0, 2] = 0;
                    metr8[a, b, 2, 0] = 0;
                    metr8[a, b, 1, 3] = 0;
                    metr8[a, b, 3, 1] = 0;
                    metr8[a, b, 2, 3] = 0;
                    metr8[a, b, 3, 2] = 0;
                    gplus = (g4(d, r - otr).mat[a, b] + g4(d + otr1, r).mat[a, b] - g4(d, r).mat[a, b] - g4(d + otr1, r - otr).mat[a, b]) / (otr * otr1);
                    gminus = (g4(d-otr1, r).mat[a, b] + g4(d, r+otr).mat[a, b] - g4(d, r).mat[a, b] - g4(d - otr1, r + otr).mat[a, b]) / (otr * otr1);
                    metr8[a, b, 1, 2] = (gplus + gminus) / 2;
                    metr8[a, b, 2, 1] = (gplus + gminus) / 2;
                }
            }
            return metr8;
        }
       public double[,,,] pointdelta3(double x, double y, double z, double otr)  // точечная разность в трехмерном случае
         {
             double[,,,] metr20 = new double[3, 3, 3, 3];
             double gplus = 0, gminus = 0;
             for (int a = 0; a <= 2; a++)
             {
                 for (int b = 0; b <= 2; b++)
                 {
                     metr20[a, b, 0, 0] = (g3(x+otr,y,z).mat[a,b] - 2 * g3(x,y,z).mat[a, b] + g3(x-otr,y,z)[a, b]) / Math.Pow(otr, 2); ;
                     metr20[a, b, 1, 1] = (g3(x, y + otr, z).mat[a, b] - 2 * g3(x,y,z).mat[a, b] + g3(x, y - otr,z)[a, b]) / Math.Pow(otr, 2);
                     metr20[a, b, 2, 2] = (g3(x, y, z + otr).mat[a, b] - 2 * g3(x, y, z).mat[a, b] + g3(x, y, z - otr)[a, b]) / Math.Pow(otr, 2);
                     metr20[a, b, 0, 1] = 0;
                     metr20[a, b, 1, 0] = 0;
                     metr20[a, b, 0, 2] = 0;
                     metr20[a, b, 2, 0] = 0;
                     gplus = (g4(d, r - otr).mat[a, b] + g4(d + otr1, r).mat[a, b] - g4(d, r).mat[a, b] - g4(d + otr1, r - otr).mat[a, b]) / (otr * otr1);
                     gminus = (g4(d - otr1, r).mat[a, b] + g4(d, r + otr).mat[a, b] - g4(d, r).mat[a, b] - g4(d - otr1, r + otr).mat[a, b]) / (otr * otr1);
                     metr20[a, b, 1, 2] = (gplus + gminus) / 2;
                     metr20[a, b, 2, 1] = (gplus + gminus) / 2;
                 }
             }
             return metr20;
         }
        public double[,,,] pointdelta2(double d, double r, double otr1)  // точечная разность в двумерном случае
        {
            double[,,,] metr8 = new double[2, 2, 2, 2];
            otr1 = Math.PI / 1000;
            double gplus = 0, gminus = 0;
            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    metr8[a, b, 0, 0] = 0;
                    metr8[a, b, 1, 1] = (g4(d, r + otr).mat[a, b] - 2 * g4(d, r).mat[a, b] + g4(d, r - otr)[a, b]) / Math.Pow(otr, 2);
                    metr8[a, b, 2, 2] = (g4(d + otr1, r).mat[a, b] - 2 * g4(d, r).mat[a, b] + g4(d - otr1, r)[a, b]) / Math.Pow(otr1, 2);
                    gplus = (g4(d, r - otr).mat[a, b] + g4(d + otr1, r).mat[a, b] - g4(d, r).mat[a, b] - g4(d + otr1, r - otr).mat[a, b]) / (otr * otr1);
                    gminus = (g4(d - otr1, r).mat[a, b] + g4(d, r + otr).mat[a, b] - g4(d, r).mat[a, b] - g4(d - otr1, r + otr).mat[a, b]) / (otr * otr1);
                    metr8[a, b, 1, 2] = (gplus + gminus) / 2;
                    metr8[a, b, 2, 1] = (gplus + gminus) / 2;
                }
            }
            return metr8;
        }
        public double[,,] christofel4(double o, double q, double r, double otr,double otr1 )
        {
            otr1 = Math.PI / 1000;
            double[,,] metr5 = new double[4,4,4];
            double rez = 0;
            for (int a=0; a<=3;a++)
            {
                for (int b = 0; b <= 3; b++)
                {
                    for (int c = 0; c <= 3; c++)
                    {
                        rez = g4(o, r).Invert().mat[a,0] * (delta4(o, otr, otr1, r)[b,0,c] + delta4(o, otr, otr1, r)[c,0,b] - delta4(o, otr, otr1, r)[b,c,0]);
                        rez += g4(o, r).Invert().mat[a, 1] * (delta4(o, otr, otr1, r)[b, 1, c] + delta4(o, otr, otr1, r)[c, 1, b] - delta4(o, otr, otr1, r)[b, c, 1]);
                        rez += g4(o, r).Invert().mat[a, 2] * (delta4(o, otr, otr1, r)[b, 2, c] + delta4(o, otr, otr1, r)[c, 2, b] - delta4(o, otr, otr1, r)[b, c, 2]);
                        rez += g4(o, r).Invert().mat[a, 3] * (delta4(o, otr, otr1, r)[b, 3, c] + delta4(o, otr, otr1, r)[c, 3, b] - delta4(o, otr, otr1, r)[b, c, 3]);
                        metr5[a,b,c] = 0.5 * rez;
                    }
                }
            }
            return metr5;
        }
        public double[,,] christofel3(double x, double y, double z, double otr)
        {
            double[,,] metr5 = new double[3, 3, 3];
            double rez = 0;
            for (int a = 0; a <= 2; a++)
            {
                for (int b = 0; b <= 2; b++)
                {
                    for (int c = 0; c <= 2; c++)
                    {
                        rez = g3(x,y,z).Invert().mat[a, 0] * (delta3(x, y, z,otr)[b, 0, c] + delta3(x, y, z, otr)[c, 0, b] - delta3(x, y, z, otr)[b, c, 0]);
                        rez += g3(x, y, z).Invert().mat[a, 1] * (delta3(x, y, z, otr)[b, 1, c] + delta3(x, y, z, otr)[c, 1, b] - delta3(x, y, z, otr)[b, c, 1]);
                        rez += g3(x, y, z).Invert().mat[a, 2] * (delta3(x, y, z, otr)[b, 2, c] + delta3(x, y, z, otr)[c, 2, b] - delta3(x, y, z, otr)[b, c, 2]);
                        metr5[a, b, c] = 0.5 * rez;
                    }
                }
            }
            return metr5;
        }
        public double[,,] christofel2(double d, double r, double otr1)
        {
            otr1 = Math.PI / 1000;
            double[,,] metr5 = new double[2, 2, 2];
            double rez = 0;
            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    for (int c = 0; c <= 1; c++)
                    {
                        rez = g2(d,r).Invert().mat[a, 0] * (delta2(d,r,otr1)[b, 0, c] + delta2(d, r, otr1)[c, 0, b] - delta2(d, r, otr1)[b, c, 0]);
                        rez += g2(d,r).Invert().mat[a, 1] * (delta2(d, r, otr1)[b, 1, c] + delta2(d, r, otr1)[c, 1, b] - delta2(d, r, otr1)[b, c, 1]);
                        metr5[a, b, c] = 0.5 * rez;
                    }
                }
            }
            return metr5;
        }
        public double[,,,] reeman4(double o, double q, double r, double otr, double otr1)
        {
            otr1 = Math.PI / 1000;
            double sum1=0;
            double[,,] chris = new double[4, 4, 4];
            chris = christofel4(o, q, r, otr, otr1);
            double[,,,] metr6 = new double[4, 4, 4, 4];
            for (int a = 0; a <= 3; a++)
            {
                for (int b = 0; b <= 3; b++)
                {
                    for (int c = 0; c <= 3; c++)
                    {
                        for (int d = 0; d <= 3; d++)
                        {
                            sum1 = 0;
                            for (int e = 0; e <= 3; e++)
                            {
                                 for (int f = 0; f <= 3; f++)
                                 {
                                        sum1 += g4(o, r).mat[e, f] * (chris[e, b, c] * chris[f, a, d] - chris[e, b, d] * chris[f, a, c]);
                                 }
                            }
                            sum1 += 0.5 * (pointdelta4(o, q, otr, otr1, r)[a, d, b, c] + pointdelta4(o, q, otr, otr1, r)[b, c, a, d] - pointdelta4(o, q, otr, otr1, r)[a, c, b, d] - pointdelta4(o, q, otr, otr1, r)[b, d, a, c]);
                            metr6[a, b, c, d] = sum1;
                        }
                    }
                }
            }
            return metr6;
        }
        public double[,,,] reeman3(double x, double y, double z, double otr,double r)
        {
            double sum1 = 0;
            double[,,] chris = new double[3, 3, 3];
            chris = christofel3(x, y, z, otr,r);
            double[,,,] metr6 = new double[3, 3, 3, 3];
            for (int a = 0; a <= 2; a++)
            {
                for (int b = 0; b <= 2; b++)
                {
                    for (int c = 0; c <= 2; c++)
                    {
                        for (int d = 0; d <= 2; d++)
                        {
                            sum1 = 0;
                            for (int e = 0; e <= 2; e++)
                            {
                                for (int f = 0; f <= 2; f++)
                                {
                                    sum1 += g3(x,y,z,r).mat[e, f] * (chris[e, b, c] * chris[f, a, d] - chris[e, b, d] * chris[f, a, c]);
                                }
                            }
                            sum1 += 0.5 * (pointdelta3(x,y,z,otr,r)[a, d, b, c] + pointdelta3(x,y,z,otr,r)[b, c, a, d] - pointdelta3(x,y,z,otr,r)[a, c, b, d] - pointdelta3(x,y,z,otr,r)[b, d, a, c]);
                            metr6[a, b, c, d] = sum1;
                        }
                    }
                }
            }
            return metr6;
        }
        public double[,,,] reeman2(double o, double r, double otr1)
        {
            double sum1 = 0;
            double[,,] chris = new double[2, 2, 2];
            chris = christofel2(o,r,otr1);
            double[,,,] metr6 = new double[2, 2, 2, 2];
            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    for (int c = 0; c <= 1; c++)
                    {
                        for (int d = 0; d <= 1; d++)
                        {
                            sum1 = 0;
                            for (int e = 0; e <= 2; e++)
                            {
                                for (int f = 0; f <= 2; f++)
                                {
                                    sum1 += g2(o,r).mat[e, f] * (chris[e, b, c] * chris[f, a, d] - chris[e, b, d] * chris[f, a, c]);
                                }
                            }
                            sum1 += 0.5 * (pointdelta2(o,r,otr1)[a, d, b, c] + pointdelta2(o, r, otr1)[b, c, a, d] - pointdelta2(o, r, otr1)[a, c, b, d] - pointdelta2(o, r, otr1)[b, d, a, c]);
                            metr6[a, b, c, d] = sum1;
                        }
                    }
                }
            }
            return metr6;
        }
        int cout = 0;
        public double znachenie(double s, double p, double t, double otr, double otr1)
        {
            otr1 = Math.PI / 1000;
            double[,,,] metr = new double[4, 4, 4, 4];
            metr = reeman4(s, p, t, otr, otr1);
            double sum4 = 0;
            for (int a = 0; a <= 3; a++)
            {
                for (int b = 0; b <= 3; b++)
                {
                    for (int c = 0; c <= 3; c++)
                    {
                        for (int d = 0; d <= 3; d++)
                        {
                            for (int e = 0; e <= 3; e++)
                            {
                                for (int f = 0; f <= 3; f++)
                                {
                                    for (int g = 0; g <= 3; g++)
                                    {
                                        for (int h = 0; h <= 3; h++)
                                        {
                                            sum4 += metr[a, b, c, d] * metr[e,f,g,h] * g4(s, t).Invert().mat[a, e] * g4(s, t).Invert().mat[b, f] * g4(s, t).Invert().mat[c, g] * g4(s, t).Invert().mat[d, h];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sum4;
        }
        int u = 0, w = 0, v = 0;
        double s;
        private void button3_Click(object sender, EventArgs e)
        {
             s = double.Parse(textBox1.Text);
        }
        double t;
        double p;
        private void button8_Click(object sender, EventArgs e)
        {
            p = double.Parse(textBox4.Text);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            t = double.Parse(textBox2.Text);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            StreamWriter file = new StreamWriter(@"C:\Users\1\Desktop\Диплом\opengl1\TestFile.txt");
            otr = 0.006;
            double otr1 = Math.PI / 100;
            String.Format("{0:0.00}", 0.06);
           // file.Write(znachenie(p));
            /* for (int i=0;i<=3;i++)
            {
                for(int j=0;j<=3;j++)
                {
                    file.Write(funct3(s, p).mat[i, j]+" ");
                }
                file.WriteLine("\n");
            }*/
           /* file.WriteLine("\n");
            file.WriteLine("\n");
            double[,,,] riman = new double[4, 4, 4, 4];
            riman = reeman(s, t, p, otr, otr1);
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    for (int k = 0; k <= 3; k++)
                    {
                        for (int h = 0; h <= 3; h++)
                        {
                            file.Write(riman[k,h,i,j] + " ");
                        }
                        file.WriteLine("\n");
                    }
                    file.WriteLine("\n");
                }
                file.WriteLine("\n");
            }
            file.WriteLine("\n");
            file.WriteLine("\n");*/
           for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    for (int k = 0; k <= 3; k++)
                    {
                        file.Write(christofel4(s, t, p, otr, otr1)[i,j,k] + " ");
                    }
                    file.WriteLine("\n");
                }
                file.WriteLine("\n");
            }
            //file.WriteLine(reeman(p, otr) + " ");
            file.Close();
            //textBox3.Text = znachenie(reeman(t,otr), s).ToString();
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 1)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
            }
            if (checkedListBox1.SelectedIndex == 0)
                two = true;
            if (checkedListBox1.SelectedIndex == 1)
                three = true;
            if (checkedListBox1.SelectedIndex == 2)
                threuclid = true;
            if (checkedListBox1.SelectedIndex == 3)
                four = true;
        }
        private void colrang(double[] metr,Point[]matric)
        {
            double temp;
            double k = 0;
            for (int i = 0; i < metr.Length; i++)
            {
                for (int j = i + 1; j < metr.Length; j++)
                {
                    if (metr[i] > metr[j])
                    {
                        temp = metr[i];
                        metr[i] = metr[j];
                        metr[j] = temp;
                    }
                }
            }
            k = metr[metr.Length] - metr[0];
            int count = 0;
            for (double h = k / 10; h < k; h += k / 10)
            {
                for (int i = 0; i <= metr.Length; i++)
                {
                    if(count==0)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(255,250,250);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if(count==1)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(250, 250, 210);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count==2)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(255, 192, 203);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count==3)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(218, 112, 214);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 4)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(153, 50, 204);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 5)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(139,0,139);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 6)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(0,0,128);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 7)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(139,69,19);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 8)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(178,34,34);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                    if (count == 9)
                        if (metr[i] <= k)
                        {
                            Gl.glColor3d(255, 0, 0);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(matric[i].x, matric[i].y, matric[i].z);
                            Gl.glEnd();
                        }
                }
                count++;
            }
        }
    }
}
