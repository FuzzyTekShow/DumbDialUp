using System;
using System.ComponentModel;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Resources;

namespace DumbDialUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        NetworkController networkController;

        // Close window button stuff
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;


        public MainWindow()
        {
            InitializeComponent();
            InitGUI();
            InitSound();
        }

        /// <summary>
        /// Initialise the GUI
        /// </summary>
        private void InitGUI()
        {
            // Setting the network controller will DISABLE the network connection.
            networkController = new NetworkController();
            ToggleText();
        }

        /// <summary>
        /// Start the famous dialup sounds
        /// </summary>
        private void InitSound()
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("pack://application:,,,/DumbDialUp;component/dialup.wav"));

            if (sri != null)
            {
                SoundPlayer player = new SoundPlayer(sri.Stream);
                player.Load();
                player.Play();
            }
        }


        private async void ToggleText()
        {
            tbWaitText.Text = "Please wait.\nDialing";
            int waitTime = 500;

            for (int i = 0; i < 3; i++)
            {
                tbStatusText.Text = string.Empty;
                await Task.Delay(waitTime);
                tbStatusText.Text = ".";
                await Task.Delay(waitTime);
                tbStatusText.Text = "..";
                await Task.Delay(waitTime);
                tbStatusText.Text = "...";
                await Task.Delay(waitTime);
            }

            tbWaitText.Text = "Please wait.\nEstablishing connection";

            for (int i = 0; i < 9; i++)
            {
                tbStatusText.Text = string.Empty;
                await Task.Delay(waitTime);
                tbStatusText.Text = ".";
                await Task.Delay(waitTime);
                tbStatusText.Text = "..";
                await Task.Delay(waitTime);
                tbStatusText.Text = "...";
                await Task.Delay(waitTime);
            }

            if (networkController != null)
            {
                networkController.Enable();
            }
            
            tbWaitText.FontWeight = FontWeights.Bold;
            tbWaitText.Text = "\nConnection established!\n";
            pbProgress.Visibility = Visibility.Hidden;
            tbStatusText.Text = string.Empty;
            await Task.Delay(3250);
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Be sure to RE-ENABLE connection on window close!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (networkController != null)
            {
                networkController.Enable();
            }
        }


        /// <summary>
        /// Called as soon as the window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }


        // Window close button stuff

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
