using PaymentEngine.xTransaction;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardKnoxTest.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= 0) return;
            var response = await Task.Run<Response>(() =>
            {

                var request = GetRequest();

                request.xCommand = "cc:sale";
                request.xAmount = numericUpDown1.Value;
                return request.Manual(Require_AVS: false,
                                                Require_CVV: false,
                                                EnableDeviceInsertSwipeTap: true,
                                                Require_Pin: false,
                                                Require_Signature: false,
                                                ExitFormIfApproved: true,
                                                ExitFormIfNotApproved: true,
                                                KeepTopMost: false);


            });
            Console.WriteLine(response.xStatus);
        }
        private Request GetRequest()
        {

            Request request = new Request()
            {
                xKey = "XXXXX", // Credential
                xVersion = "4.5.4",//API Version
                xSoftwareName = "Cardknox Test", //Name of your software
                xSoftwareVersion = "1.0.0.0",
                Form_Height = 800,
                Form_Width = 1000,
                EnableSilentMode = false,
            };
            request.Settings.Device_Name = "Verifone_Vx805.4";
            request.Settings.Device_COM_Port = "COM9";
            request.Settings.Device_COM_Baud = "115200";
            request.Settings.Device_COM_Parity = "N";
            request.Settings.Device_COM_DataBits = "8";
            request.EnableKeyedEntry = true;
            request.EnableMultipleKeys = true;

            request.Settings.Listener_DeviceStatus = (s, c) =>
             {
                 Console.WriteLine(s);
             };
            request.Settings.Listener_TransactionStatus = (s, c) =>
                    {

                        Console.WriteLine(s);
                    };

            return request;
        }

    }
}
