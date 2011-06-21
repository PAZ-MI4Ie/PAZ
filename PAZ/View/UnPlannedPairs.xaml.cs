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
        
        public void loadAllPairs(PairMapper pairmapper)
        {
            GridPairs.Children.Clear();
            List<Model.Pair> pairs = pairmapper.FindAllUnplanned();
            int i = 0;
            foreach (Model.Pair pair in pairs)
            {
                CustomLabel label = new CustomLabel();
                label.Id = pair.ID;
                string content = pair.Student1.Firstname + " " + pair.Student1.Surname + "\n" +
                                 pair.Student2.Firstname + " " + pair.Student2.Surname + "\n\n";
                string teachers = "";
                string experts = "";
                foreach (Model.User attachment in pair.Attachments)
                {//TODO: find out why attachment shows up if there isnt any...
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
                label.MouseMove += new MouseEventHandler(pair_MouseMove);
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

        void pair_MouseMove(object sender, MouseEventArgs e)
        {
            CustomLabel pair = sender as CustomLabel;
            if (pair != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                CalendarView.CheckAvailability(pair,false);
                DragDropEffects drag = DragDrop.DoDragDrop(pair, pair.Id, DragDropEffects.Move);
                if (drag == DragDropEffects.None)
                    CalendarView.revertCheckAvailability();
            }
        }
    }
}
