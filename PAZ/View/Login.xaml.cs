using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PAZ.Model;

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

            if (login.checkUsername(passwordBox1.Password))
            {
                MainWindow tt = new MainWindow();
                tt.Show();
                Application.Current.Windows[0].Close();
                this.Close();
            }
            else
                MessageBox.Show("Wachtwoord is onjuist.");
             
        }

    }
}
