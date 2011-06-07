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

namespace PAZ.View
{
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        static string[] names;
        public SessionWindow()
        {
            InitializeComponent();
        }

        public SessionWindow(bool editing)//, Session session)
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            names = new string[] { txtStudent1.Text, txtStudent2.Text, cbTeacher1.SelectedValue.ToString(), cbTeacher2.SelectedValue.ToString(), cbExpert1.SelectedValue.ToString(), cbExpert2.SelectedValue.ToString() };
            this.Close();
        }

        public static string[] AddNewSession()
        {
            SessionWindow sw = new SessionWindow();
            sw.ShowDialog();

            return names;
        }
    }
}
