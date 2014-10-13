using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace com.jheto.PNGEncryption
{
    public class PNGSerializer
    {

        private PNGSerializer() { }

        public static byte[] encodeText(string message)
        {
            byte[] bytes = null;
            try
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(message);
            }
            catch (Exception e)
            {
                bytes = null;
            }
            return bytes;
        }

        public static string decodeText(byte[] bytes)
        {
            string message = null;
            try
            {
                message = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (Exception e)
            {
                message = null;
            }
            return message;
        }

        public static Bitmap encodeBinary(byte[] bytes)
        {
            Bitmap bmp = null;
            try
            {
                int sqrt = (int)Math.Ceiling(Math.Sqrt(bytes.Length));
                bmp = new Bitmap(sqrt, sqrt);
                for (int w = 0, index = 0; w < sqrt; w++)
                {
                    for (int h = 0; h < sqrt; h++)
                    {
                        if (index < bytes.Length)
                        {
                            int A = (int)bytes[index];
                            Color color = Color.FromArgb(A, 0, 0, 0);
                            bmp.SetPixel(w, h, color);
                        }
                        else
                        {
                            Color color = Color.FromArgb(0, 255, 255, 255);
                            bmp.SetPixel(w, h, color);
                        }
                        index++;
                    }
                }
            }
            catch (Exception e)
            {
                bmp = null;
            }
            return bmp;
        }

        public static byte[] decodeBinary(Bitmap bmp)
        {
            byte[] bytes = null;
            try
            {
                int wSize = bmp.Width, hSize = bmp.Height;
                MemoryStream stream = new MemoryStream();
                for (int w = 0; w < wSize; w++)
                {
                    for (int h = 0; h < hSize; h++)
                    {
                        Color color = bmp.GetPixel(w, h);
                        if (color.R == 0 && color.G == 0 && color.B == 0) stream.WriteByte(color.A);
                        else break;
                    }
                }
                bytes = stream.ToArray();
                stream = null;
            }
            catch (Exception e)
            {
                bytes = null;
            }
            return bytes;
        }

        public static byte[] bitmapToArray(Bitmap bmp)
        {
            byte[] byteArray = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Close();
                    byteArray = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                byteArray = null;
            }
            return byteArray;
        }

        public static Bitmap arrayToBitmap(byte[] array)
        {
            Bitmap bmp = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(array))
                {
                    bmp = (Bitmap)Image.FromStream(ms);
                }
            }
            catch (Exception e)
            {
                bmp = null;
            }
            return bmp;
        }

        public static void doIt()
        {
            try {
                string str = @"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*";
                byte[] rawdata = PNGSerializer.encodeText(str);
                Bitmap bmp = PNGSerializer.encodeBinary(rawdata);
                bmp.Save("eicar.png", ImageFormat.Png);
                byte[] serial = PNGSerializer.bitmapToArray(bmp);
                bmp = PNGSerializer.arrayToBitmap(serial);
                rawdata = PNGSerializer.decodeBinary(bmp);
                string text = PNGSerializer.decodeText(rawdata);
                Console.WriteLine(text);
            }
            catch (Exception e) { }
        }
    
    }

    class Program
    {
        [STAThread]
        static void Main()
        {
            PNGSerializer.doIt();
        }
    }
}
