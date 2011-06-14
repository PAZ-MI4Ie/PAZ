using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PAZ.Control;
using PAZMySQL;

namespace PAZ
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        
        public Login()
        {
            InitializeComponent();
        }
       
       
        private void labelButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartWork();
        }

        public bool login()
        {
            AdminMapper adminmapper = new AdminMapper(MysqlDb.GetInstance());
            
            if (adminmapper.CheckLogin("admin", passwordBox1.Password))
            {
                return true;
                
            }
            else
            { 
                MessageBox.Show("Wachtwoord is onjuist.");
                return false;
            }
        }


        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
                StartWork();
        }

        private bool succesfull;
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            succesfull = login();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gridLoadingSreen.Visibility = Visibility.Hidden;
            GridLogin.Cursor = Cursors.Arrow;

            if (succesfull)
            {
                MainWindow tt = new MainWindow();
                tt.Show();
                Application.Current.Windows[0].Close();
                this.Close();
            }
        }

        private void StartWork()
        {
            gridLoadingSreen.Visibility = Visibility.Visible;
            GridLogin.Cursor = Cursors.Wait;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }


    }
}
