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
        public int[] ids = new int[5];
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
                else if (user.User_type == "expert")
                {
                    Expert t = _controller.ExpertMapper.Find(user.Id);
                    experts.Add(t.Id + ", " + t.ToString());
                }
            }

            ids[0] = session.Pair.ID;
            string[] temp;

            cbPairs.SelectedIndex = cbPairs.Items.IndexOf(session.Pair.ID  + ", " + session.Pair.ToString());
            if (teachers.Count > 0)
            {
                cbTeacher1.SelectedIndex = cbTeacher1.Items.IndexOf(teachers[0]);
                cbTeacher2.SelectedIndex = cbTeacher2.Items.IndexOf(teachers[1]);
                temp = teachers[0].Split(',');
                ids[1] = Convert.ToInt32(temp[0]);
                temp = teachers[1].Split(',');
                ids[2] = Convert.ToInt32(temp[0]);
            }
            if (experts.Count > 0)
            {
                cbExpert1.SelectedIndex = cbExpert1.Items.IndexOf(experts[0]);
                cbExpert2.SelectedIndex = cbExpert2.Items.IndexOf(experts[1]);
                temp = experts[0].Split(',');
                ids[3] = Convert.ToInt32(temp[0]);
                temp = experts[1].Split(',');
                ids[4] = Convert.ToInt32(temp[0]);
            }
            
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Pair pair = _controller.PairMapper.Find(ids[0]);
            List<User> newAttachment = new List<User>();
            int[] newIds = new int[4];
            string[] id = cbTeacher1.SelectedItem.ToString().Split(',');
            newIds[0] = Convert.ToInt32(id[0]);
            id = cbTeacher2.SelectedItem.ToString().Split(',');
            newIds[1] = Convert.ToInt32(id[0]);
            id = cbExpert1.SelectedItem.ToString().Split(',');
            newIds[2] = Convert.ToInt32(id[0]);
            id = cbExpert2.SelectedItem.ToString().Split(',');
            newIds[3] = Convert.ToInt32(id[0]);

            for (int i = 0; i < newIds.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    newAttachment.Add(_controller.TeacherMapper.Find(newIds[i]));
                }
                else
                {
                    newAttachment.Add(_controller.ExpertMapper.Find(newIds[i]));
                }
            }
            pair.Attachments = newAttachment;
            _controller.PairMapper.Save(pair);
            MessageBox.Show("De wijzigingen zijn opgeslagen.");
            this.Close();
        }
    }
}
