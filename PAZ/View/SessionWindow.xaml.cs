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
using PAZ.Control;

namespace PAZ.View
{
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        private PAZController _controller;
        static string[] names;
        public SessionWindow()
        {
            InitializeComponent();
            
        }

        public SessionWindow(Session session)
        {
            InitializeComponent();
            _controller = PAZController.GetInstance();
            List<Pair> pairs = _controller.PairMapper.FindAll();
            foreach (Pair pair in pairs)
            {
                cbPairs.Items.Add(pair.ID + ", " + pair.ToString());

            }

            foreach (Teacher teacher in _controller.TeacherMapper.FindAll())
            {
                cbTeacher1.Items.Add(teacher.Id + ", " + teacher.ToString());
                cbTeacher2.Items.Add(teacher.Id + ", " + teacher.ToString());

            }

            foreach (Expert expert in _controller.ExpertMapper.FindAll())
            {
                cbExpert1.Items.Add(expert.Id + ", " + expert.ToString());
                cbExpert2.Items.Add(expert.Id + ", " + expert.ToString());

            }
            List<string> teachers = new List<string>();
            List<string> experts = new List<string>();
            foreach (User user in session.Pair.Attachments)
            {
                if (user.User_type == "teacher")
                {
                    Teacher t = _controller.TeacherMapper.Find(user.Id);
                    teachers.Add(t.Id + ", " + t.ToString());
                }
                else if (user.User_type == "teacher")
                {
                    Expert t = _controller.ExpertMapper.Find(user.Id);
                    teachers.Add(t.Id + ", " + t.ToString());
                }
            }

            cbPairs.Text = session.Pair.ToString();
            if (teachers.Count > 0)
            {
                cbTeacher1.SelectedIndex = cbTeacher1.Items.IndexOf(teachers[0]);
                cbTeacher2.SelectedIndex = cbTeacher2.Items.IndexOf(teachers[1]);
            }
            if (experts.Count > 0)
            {
                cbExpert1.SelectedIndex = cbExpert1.Items.IndexOf(experts[0]);
                cbExpert2.SelectedIndex = cbExpert2.Items.IndexOf(experts[1]);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbPairs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
