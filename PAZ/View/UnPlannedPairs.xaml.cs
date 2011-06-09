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
using PAZ.Model.Mappers;

namespace PAZ.View
{
    /// <summary>
    /// Interaction logic for UnPlannedPairs.xaml
    /// </summary>
    public partial class UnPlannedPairs : Window
    {
        public UnPlannedPairs()
        {
            InitializeComponent();
        }

        public void addPair()
        {

        }

        public void RemovePair(int id)
        {/*
            Model.Pair pair = pairmapper.Find(id);
            if (pairs.Contains(pair))
                if (pairs.Remove(pair))
                {
                    update(pairs);
                    MessageBox.Show("Het paar met de studenten: " + pair.Student1.Firstname + " " + pair.Student1.Surname + ", " + pair.Student2.Firstname + " " + pair.Student2.Surname + " is verwijderd");
                }
                else
                    MessageBox.Show("Er is iets mis gegaan met het verwijderen.");*/
        }

        public void update(List<Model.Pair> pairs)
        {
            GridPairs.Children.Clear();
            int i = 0;
            foreach (Model.Pair pair in pairs)
            {
                CustomLabel label = new CustomLabel();
                label.Id = pair.ID;
                string content = pair.Student1.Firstname + " " + pair.Student1.Surname + "\n" +
                                 pair.Student2.Firstname + " " + pair.Student2.Surname + "\n\n";
                string teachers = "";
                string experts = "";
                foreach (Model.User attachment in pair.Participants)
                {
                    string name = attachment.Firstname + " " + attachment.Surname + "\n";
                    if (attachment.User_type == "teacher")
                        teachers += name;
                    else
                        experts += name;
                }
                label.Content = content + teachers + "\n" + experts;
                label.BorderBrush = Brushes.LightGray;
                label.BorderThickness = new Thickness(2);
                label.Margin = new Thickness(1, 1, 1, 1);
                Grid.SetRow(label, i);
                if (GridPairs.RowDefinitions.Count != pairs.Count)
                {
                    RowDefinition row = new RowDefinition();
                    GridLength height = new GridLength(145);
                    row.Height = height;
                    GridPairs.RowDefinitions.Add(row);
                }
                GridPairs.Children.Add(label);
                i++;
            }
        }
    }
}
