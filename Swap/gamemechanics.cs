using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
using System.Resources;

namespace Swap
{
    class gamemechanics
    {
        private img[,] Img;
        private Rectangle rec;

        private struct img
        {
            // for inamge
            public Bitmap bmp;
            public int locx, locy;
            public bool rightloc;
            public bool hasclicked;
            // for tile shuffle
            public bool hasswitched; 
        }

        public gamemechanics(int size) // set the required values as origin
        {
            Random rnd = new Random();
            int rand = rnd.Next(1, 15);
            Img = new img[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    rec = new Rectangle(x * (300 / size), y * (300 / size), 300 / size, 300 / size);
                    Img[x, y].locx = x; Img[x, y].locy = y; // image position
                    Img[x, y].hasswitched = false; Img[x, y].rightloc = false; Img[x, y].hasclicked = false;
                    Img[x, y].bmp = (Bitmap)image(rand).Clone(rec, PixelFormat.DontCare);
                }
            }
        }

        private Bitmap image(int rand) // return 1 randomed image
        {
            return (Bitmap) Properties.Resources.ResourceManager.GetObject("I" + rand.ToString());
            // using the resourcemanager.getobject it will get the object with string equal to the parameter given to it
            // the parameter of .getobject should be a valid string ( the file name must exist in resources )
            // since the resourcemanager returns an object it must be converted to bitmap
        }

        public void drawimage(int size, Graphics g, bool hasstarted) // shuffle and draw image
        {
            if (hasstarted != true)
            { shuffleimage(size); }
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    rec = new Rectangle(x * (300 / size), y * (300 / size), 300 / size, 300 / size);
                    g.DrawImage(Img[x, y].bmp, rec);
                }
            }
            createborder(size, g);
        }

        private void shuffleimage(int size)
        {
            Random rnd = new Random();
            string[] position = new string[size * size];
            int count = 0, rndnum = 0, tmpx = 0, tmpy = 0; string temp = ""; Bitmap TEMP = null;
            // set list of location
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    position[count] = x.ToString() + y.ToString();
                    count++;
                }
            }

            count = 0; // put count back to 0

            //shuffle
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (Img[x, y].hasswitched == false)
                    {
                        rndnum = rnd.Next(0, (size * size) - count);

                        // swap images and its location
                        TEMP = Img[x, y].bmp;
                        tmpx = Img[x, y].locx; tmpy = Img[x, y].locy;

                        Img[x, y].bmp = Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].bmp;
                        Img[x, y].locx = Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locx;
                        Img[x, y].locy = Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locy;

                        Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].bmp = TEMP;
                        Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locx = tmpx;
                        Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locy = tmpy;

                        Img[x, y].hasswitched = true;
                        Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].hasswitched = true;

                        // check if image is at the right location
                        if (Img[x, y].locx == Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locx &&
                            Img[x, y].locy == Img[Convert.ToInt32(position[rndnum][0].ToString()), Convert.ToInt32(position[rndnum][1].ToString())].locy)
                        {
                            Img[x, y].rightloc = true;
                        }
                        
                        // limit the randmon max length
                        if (((size * size) - count) - 1 < 0)
                        {
                            //random selected location
                            temp = position[rndnum];
                            position[rndnum] = position[((size * size) - count) - 1];
                            position[((size * size) - count) - 1] = temp;
                            count++;
                            //sequencial selected location
                            for (int i = 0; i < (size * size) - count; i++)
                            {
                                temp = x.ToString() + y.ToString();
                                if (position[i] == temp)
                                {
                                    position[i] = position[((size * size) - count) - 1];
                                    position[((size * size) - count) - 1] = temp;
                                    break;
                                }
                            }
                            count++;
                        }
                    }
                }
            }
        }

        public void swap(int x1, int y1, int x2, int y2, int size)
        {
            Bitmap temp; int locx, locy;

            temp = Img[x1, y1].bmp;
            locx = Img[x1, y1].locx; locy = Img[x1, y1].locy;

            Img[x1, y1].bmp = Img[x2, y2].bmp;
            Img[x1, y1].locx = Img[x2, y2].locx;
            Img[x1, y1].locy = Img[x2, y2].locy;

            Img[x2, y2].bmp = temp;
            Img[x2, y2].locx = locx;
            Img[x2, y2].locy = locy;


            // for first click
            if (Img[x1, y1].locx == x1 && Img[x1, y1].locy == y1)
            { Img[x1, y1].rightloc = true; }
            else
            { Img[x1, y1].rightloc = false; }

            // for second click
            if (Img[x2, y2].locx == x2 && Img[x2, y2].locy == y2)
            { Img[x2, y2].rightloc = true; }
            else
            { Img[x2, y2].rightloc = false; }
        }

        public bool check(int size)
        {
            int count = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (Img[x, y].rightloc == true)
                    { count++; }
                }
            }
            if (count == (size * size))
            { return true; }
            return false;
        }

        private void createborder(int size, Graphics g)
        {
            Pen wp = new Pen(Color.Yellow); // tile not in right position
            Pen rp = new Pen(Color.Cyan); // tile in right position
            Pen cp = new Pen(Color.Pink, 3); // tile clicked ignore if location is right or wrong
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    rec = new Rectangle(x * (300 / size), y * (300 / size), 300 / size, 300 / size);
                    if (Img[x, y].hasclicked == false)
                    {
                        if (Img[x, y].rightloc == false)
                        { g.DrawRectangle(wp, rec); }
                        else
                        { g.DrawRectangle(rp, rec); }
                    }
                    else
                    { g.DrawRectangle(cp, rec); }
                }
            }
        }

        public void imgclicked(int x, int y, bool hasclicked)
        {
            Img[x, y].hasclicked = hasclicked;
        }
    }
}
