using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;


namespace WeatherClock
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isBackgroundVideo = true; // Поле для отслеживания состояния фона
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

            //обновление дня недели,месяц,год раз в день
            System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimerDateTime_Tick);
            dispatcherTimer1.Interval = new TimeSpan(24, 0, 0);
            dispatcherTimer1.Start();

            //отслеживание перемещение активного окна
            this.MouseDown += new MouseButtonEventHandler(MainWindow_MouseDown);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Clock1.Content = DateTime.Now.ToLongTimeString();
        }

        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            sqlSelect();
        }

        //обновление через 24 часа
        private void dispatcherTimerDateTime_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DayOfWeek.Content = now.ToString("dddd");
            dataTime.Content = now.ToString("d MMMM yyyy");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the window that contains the button.
            Window window = (Window)this;// sender;

            // Close the window.
            window.Close();
        }

        //отслеживание триггера на движение активного окна
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //Вкыл-выкл фона
        private void ToggleBackground_Click(object sender, RoutedEventArgs e)
        {
            if (userControl1 != null)
            {
                if (isBackgroundVideo)
                {
                    userControl1.ToggleBackground(false); 
                    this.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
                    isBackgroundVideo = false;
                }
                else
                {
                    userControl1.ToggleBackground(true);
                    this.Background = new SolidColorBrush(Color.FromArgb(7, 255, 255, 255));
                    isBackgroundVideo = true;
                }
            }
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
