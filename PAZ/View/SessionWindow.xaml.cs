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
                cbPairs.Items.Add(pair);

            }

            foreach (Teacher teacher in _controller.TeacherMapper.FindAll())
            {
                cbTeacher1.Items.Add(teacher);
                cbTeacher2.Items.Add(teacher);

            }

            foreach (Expert expert in _controller.ExpertMapper.FindAll())
            {
                cbExpert1.Items.Add(expert);
                cbExpert2.Items.Add(expert);

            }
            List<Teacher> teachers = new List<Teacher>();
            List<Expert> experts = new List<Expert>();
            foreach (User user in session.Pair.Attachments)
            {
                if (user.User_type == "teacher")
                    teachers.Add(_controller.TeacherMapper.Find(user.Id));
                else
                    experts.Add(_controller.ExpertMapper.Find(user.Id));
            }

            cbPairs.Text = session.Pair.ToString();
            cbTeacher1.Text = teachers[0].ToString();
            cbTeacher2.Text = teachers[1].ToString();
            cbExpert1.Text = experts[0].ToString();
            cbExpert2.Text = experts[1].ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbPairs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
