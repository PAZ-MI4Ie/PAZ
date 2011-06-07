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
            ComboBoxItem selectedItem = (ComboBoxItem)(cbTeacher1.SelectedValue);
            string teacher1 = selectedItem.Content.ToString();
            selectedItem = (ComboBoxItem)(cbTeacher2.SelectedValue);
            string teacher2 = selectedItem.Content.ToString();
            selectedItem = (ComboBoxItem)(cbExpert1.SelectedValue);
            string expert1 = selectedItem.Content.ToString();
            selectedItem = (ComboBoxItem)(cbExpert2.SelectedValue);
            string expert2 = selectedItem.Content.ToString();
            names = new string[] { txtStudent1.Text, txtStudent2.Text, teacher1, teacher2, expert1, expert2 };
            this.Close();
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
