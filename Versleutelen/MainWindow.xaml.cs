using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;

namespace Versleutelen
{
    /// <summary>
    /// Opdracht 3: Versleutelen
    /// Maak een programma dat het versleutelen van teksten mogelijk maakt en dat als txt document
    /// bewaard kan worden.Lever ook een decryptie programma in.
    /// Wanneer je zelf een versleuteling bedenkt dan is slim algoritmiseren hier het belangrijkst.Je mag de
    /// standaard versleuteling in C# ook gebruiken.
    /// </summary>
    public partial class MainWindow : Window
    {
        //Create a UnicodeEncoder to convert between byte array and string.
        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        
        //Create byte array to hold encrypted data.
        byte[] encryptedData;

        //Encrypt button
        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            //Create byte array from textBox user input
            byte[] dataToEncrypt = ByteConverter.GetBytes(inputText.Text);

            //Pass the data to ENCRYPT and a boolean flag specifying no OAEP padding.
            encryptedData = RSAEncrypt(dataToEncrypt, false);

            //Display encrypted text in textBox
            encryptedText.Text = ByteConverter.GetString(encryptedData);
        }

        //Save button
        private void save_Click(object sender, RoutedEventArgs e)
        {
            //Save encryptedData in external .txt file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, encryptedData);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        //Encrypt function
        public static byte[] RSAEncrypt(byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;

                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Creates an RSA object containing the public key from the XML string.
                    RSA.FromXmlString(MyPublicKey());

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        private static string MyPublicKey()
        {
            return "<RSAKeyValue><Modulus>xncZR7YHbH+Q22EefnVkArlIfGYvtSp6BtUSTjTuAuksOn+gACxc" +
                "ZHfEQGqp/qczHW67zpuNxNNrOzV4Ga2m617rtxuqvURELG9RK3SujmnZBkafMwfHc1X2Ft2sz5S2D" +
                "nrxtHbQl1GVOHYu/avvjUC5UR5XNloDEKQ5Pr8r/aE=</Modulus><Exponent>AQAB</Exponent>" +
                "</RSAKeyValue>";
        }

    }
}
