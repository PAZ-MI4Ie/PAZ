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

        public SessionWindow(Session session)
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        public static string[] AddNewSession()
        {
            SessionWindow sw = new SessionWindow();
            sw.ShowDialog();
            return names;
        }

        private void cbPairs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)(cbPairs.SelectedValue);
            string[] students = selectedItem.Content.ToString().Split(',');
            txtStudent1.Text = students[0].Trim();
            txtStudent2.Text = students[1].Trim();
        }
    }
}
