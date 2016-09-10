using System;
using System.ServiceModel;
using System.Windows.Forms;
using GroceryValue.Client.ServiceReference;

namespace GroceryValue.Client
{
    internal static class Program
    {
        private static ServiceClient _client;

        [STAThread]
        private static void Main()
        {
            try
            {
                StartServiceClient();
                RunApplication();
                FinishServiceClient();
            }
            catch (Exception exception) when
            (
                exception is GroceryValueException ||
                exception is InvalidOperationException ||
                exception is CommunicationException ||
                exception is TimeoutException
            )
            {
                exception.Display();
            }
            finally
            {
                FinalizeServiceClient();
            }
        }

        private static void StartServiceClient()
        {
            _client = new ServiceClient("NetNamedPipeEndpoint");
        }

        private static void FinishServiceClient()
        {
            _client.Close();
        }

        private static void FinalizeServiceClient()
        {
            if (_client != null && _client.State != CommunicationState.Closed)
            {
                _client.Abort();
            }
        }

        private static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GroceryValueForm(_client));
        }

        private static void Display(this Exception exception)
        {
            MessageBox.Show(exception.Message, exception.GetType().ToString());
        }
    }
}
