using System;
using System.Windows;
using System.Data.SqlClient;


namespace WeatherClock
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String temperature = "";


        public MainWindow()
        {
            InitializeComponent();

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 30;
            this.Top = 50;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0,1);
            dispatcherTimer.Start();

            System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer1_Tick);
            dispatcherTimer1.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer1.Start();

            


        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Clock1.Content = DateTime.Now.ToLongTimeString();
        }

        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            sqlSelect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the window that contains the button.
            Window window = (Window)this;// sender;

            // Close the window.
            window.Close();
        }

        private void sqlSelect() {
            using (SqlConnection conn = new SqlConnection("Password=passWeatherDaemonAero;Persist Security Info=True;User ID=WeatherDaemonAero;Initial Catalog=WebInstanceNVAero;Data Source=APPAERO")) 
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 cast( M.TemperatureMax as VarChar(4)),   M.TemperatureMax, ForecastTime, M.TemperatureMin  FROM[WebInstanceNVAero].[dbo].[GIS_Metering] MT " +
            "join[WebInstanceNVAero].[dbo].GIS_Meter M on M.Metering = MT.ID  where Town = 23471  and ForecastTime <= GETDATE()  order by ForecastTime desc, MT.[ID] desc", conn);
        try
                {
                    conn.Open();
                    temperature = (String)cmd.ExecuteScalar();
                    Temperature.Content = String.Format("{0} °C", temperature);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
