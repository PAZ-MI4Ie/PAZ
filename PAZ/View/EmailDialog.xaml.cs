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
using System.Data;
using PAZ.Model;

namespace PAZ.View
{
    /// <summary>
    /// Interaction logic for EmailDialog.xaml
    /// </summary>
    public partial class EmailDialog : Window
    {
        public EmailDialog()
        {
            InitializeComponent();
            dataGridEmails.Items.Add(new User { Firstname = "Jen", Surname = "Piets", Email = "info@aap.nl", User_type ="Student" });
            
            CheckBox ite = new CheckBox();
            dataGridEmails.Items.Add(ite);

            dataGridEmails.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Select") });
            dataGridEmails.Columns.Add(new DataGridTextColumn { Header = "Voornaam", Binding = new Binding("Firstname") });
            dataGridEmails.Columns.Add(new DataGridTextColumn { Header = "Achternaam", Binding = new Binding("Surname") });
            dataGridEmails.Columns.Add(new DataGridTextColumn { Header = "Type", Binding = new Binding("User_type") });
            dataGridEmails.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new Binding("Email") });
        }
        
    }
}
