using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Drawing;
using ZXing;
using ZXing.Common;
using System.Windows.Forms;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;
using static System.Net.Mime.MediaTypeNames;

namespace QrParingTesto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("Resources/example.png");

            // Decode the QR code using ZXing
            BarcodeReader reader = new BarcodeReader();
            reader.Options = new DecodingOptions
            {
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            };
            Result result = reader.Decode(bmp);

            if (result != null)
            {
                File.WriteAllText("Resources/example2.bin", result.Text, Encoding.UTF8);


                using (var gzipStream = new GZipStream(new FileStream("Resources/example2.bin", FileMode.Open), CompressionMode.Decompress))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(memoryStream);
                        var decompressedData = memoryStream.ToArray();

                        var jsonString = Encoding.UTF8.GetString(decompressedData);

                        richTextBox1.Text = jsonString;
                    }
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            using (var gzipStream = new GZipStream(new FileStream("Resources/example.bin", FileMode.Open), CompressionMode.Decompress))
            {


                using (var memoryStream = new MemoryStream())
                {
                    gzipStream.CopyTo(memoryStream);

                    var decompressedData = memoryStream.ToArray();

                    var jsonString = Encoding.UTF8.GetString(decompressedData);
                    Console.WriteLine(jsonString);

                    richTextBox1.Text = jsonString;
                }
            }
        }
    }
}
