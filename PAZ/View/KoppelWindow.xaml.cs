using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Ini;
using PAZ.Control;
using PAZ.Model;
using PAZ.View;

namespace PAZ
{
    /**
    * In deze klasse kun je de koppelingen leggen(studenten, experts en docenten aan paren koppelen)
    */
    public partial class KoppelWindow : Window
    {
        private PAZController _controller;

        private List<Pair> _pairs;
        private List<Student> _students;
        private List<Teacher> _teachers;
        private List<Expert> _experts;

        private bool _isBusyCoupling = false;
        private bool _wasChanged = false;

        private int _previousStudentIndex;

        public KoppelWindow()
        {
            InitializeComponent();

            _controller = PAZController.GetInstance();

            btnSave.IsEnabled = false;

            _pairs = _controller.PairMapper.FindAll();
            _students = _controller.StudentMapper.FindAll();
            _teachers = _controller.TeacherMapper.FindAll();
            _experts = _controller.ExpertMapper.FindAll();

            fillPairs();
            fillStudents();
            fillTeachers();
            fillExperts();
        }

        public KoppelWindow(int id) : this()
        {
            cbPairs.SelectedIndex = id;
        }

        private void fillPairs()
        {
            cbPairs.Items.Add("Kies een paar");

            List<Pair> foundPairs = _pairs;
            foreach (Pair pair in foundPairs)
                cbPairs.Items.Add(pair);

            cbPairs.SelectedIndex = 0;
        }

        private void fillStudents()
        {
            cbStudent1.Items.Add("Ongekoppeld");
            cbStudent2.Items.Add("Ongekoppeld");

            List<Student> foundStudents = _students;
            foreach (Student student in foundStudents)
            {
                cbStudent1.Items.Add(student);
                cbStudent2.Items.Add(student);
            }

            cbStudent1.SelectedIndex = 0;
            cbStudent2.SelectedIndex = 0;
        }

        private void fillTeachers()
        {
            cbTeacher1.Items.Add("Ongekoppeld");
            cbTeacher2.Items.Add("Ongekoppeld");

            List<Teacher> foundTeachers = _teachers;
            foreach (Teacher teacher in foundTeachers)
            {
                cbTeacher1.Items.Add(teacher);
                cbTeacher2.Items.Add(teacher);
            }

            cbTeacher1.SelectedIndex = 0;
            cbTeacher2.SelectedIndex = 0;
        }

        private void fillExperts()
        {
            cbExpert1.Items.Add("Ongekoppeld");
            cbExpert2.Items.Add("Ongekoppeld");

            List<Expert> foundExperts = _experts;
            foreach (Expert expert in foundExperts)
            {
                cbExpert1.Items.Add(expert);
                cbExpert2.Items.Add(expert);
            }

            cbExpert1.SelectedIndex = 0;
            cbExpert2.SelectedIndex = 0;
        }

        /**
        * Sluit het scherm en slaat wijzigingen op
        */
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.IsEnabled)
                Save();

            this.Close();
        }

        /**
        * Sluit het huidige scherm
        */
        private void btnAnnuleren(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!btnSave.IsEnabled)
                return;

            MessageBoxResult result = MessageBox.Show("Wilt u de wijzigingen opslaan?", "Bevestiging", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Save();
            else if (result == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            _controller.KoppelenWindowSaveClicked(_pairs);
            _wasChanged = true;
            btnSave.IsEnabled = false;
        }

        private void ResavePairAttachment()
        {
            Pair pair = (Pair) cbPairs.Items[cbPairs.SelectedIndex];

            List<User> newAttachmentList = new List<User>();

            pair.Student1 = (Student) cbStudent1.Items[cbStudent1.SelectedIndex];
            pair.Student1_id = pair.Student1.Id;

            if (cbStudent2.SelectedIndex <= 0)
            {
                pair.Student2 = null;
                pair.Student2_id = 0;
            }
            else
            {
                pair.Student2 = (Student)cbStudent2.Items[cbStudent2.SelectedIndex];
                pair.Student2_id = pair.Student2.Id;
            }

            if(cbTeacher1.SelectedIndex > 0)
                newAttachmentList.Add((User) cbTeacher1.Items[cbTeacher1.SelectedIndex]);

            if(cbTeacher2.SelectedIndex > 0)
                newAttachmentList.Add((User) cbTeacher2.Items[cbTeacher2.SelectedIndex]);

            if(cbExpert1.SelectedIndex > 0)
                newAttachmentList.Add((User) cbExpert1.Items[cbExpert1.SelectedIndex]);

            if(cbExpert2.SelectedIndex > 0)
                newAttachmentList.Add((User) cbExpert2.Items[cbExpert2.SelectedIndex]);

            pair.Attachments = newAttachmentList;

            int saveIndex = cbPairs.SelectedIndex;
            cbPairs.Items.Clear();
            fillPairs();
            cbPairs.SelectedIndex = saveIndex;
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _wasChanged;
        }


        private void cbPairs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isBusyCoupling = true;

            int selectedIndex = cbPairs.SelectedIndex;
            if (selectedIndex <= 0)
            {
                cbStudent1.SelectedIndex = 0;
                cbStudent2.SelectedIndex = 0;
                cbTeacher1.SelectedIndex = 0;
                cbTeacher2.SelectedIndex = 0;
                cbExpert1.SelectedIndex = 0;
                cbExpert2.SelectedIndex = 0;
                return;
            }

            Pair selectedPair = (Pair) cbPairs.Items[selectedIndex];
            for(int i = 1; i < cbStudent1.Items.Count; ++i)
            {
                Student student = (Student) cbStudent1.Items[i];
                if (student.Id == selectedPair.Student1.Id)
                    cbStudent1.SelectedIndex = i;
                else if (selectedPair.Student2 == null)
                    cbStudent2.SelectedIndex = 0;
                else if (student.Id == selectedPair.Student2.Id)
                    cbStudent2.SelectedIndex = i;
            }

            _previousStudentIndex = cbStudent1.SelectedIndex;

            SelectCoupledUsers(selectedPair, cbTeacher1, cbTeacher2);
            SelectCoupledUsers(selectedPair, cbExpert1, cbExpert2);

            _isBusyCoupling = false;
        }

        private void SelectCoupledUsers(Pair selectedPair, ComboBox firstBox, ComboBox secondBox)
        {
            bool userSelected = false;
            bool foundAllUsers = false;
            for (int i = 1; i < firstBox.Items.Count && !foundAllUsers; ++i)
            {
                User currentUser = (User)firstBox.Items[i];
                foreach (User user in selectedPair.Attachments)
                {
                    if (currentUser.Id == user.Id)
                    {
                        if (userSelected)
                        {
                            secondBox.SelectedIndex = i;
                            foundAllUsers = true;
                            break;
                        }
                        else
                        {
                            firstBox.SelectedIndex = i;
                            userSelected = true;
                        }
                    }
                }
            }

            if (!foundAllUsers)
                secondBox.SelectedIndex = 0;

            if (!userSelected)
                firstBox.SelectedIndex = 0;
        }

        private void cbStudent1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbStudent1.SelectedIndex == 0)
            {
                MessageBox.Show("Er moet minstens een student aan een paar gekoppeld zijn, gebruik het verwijderen scherm als u studenten en hun paren wilt verwijderen.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbStudent1.SelectedIndex = _previousStudentIndex;
                return;
            }


            if (cbPairs.SelectedIndex > 0)
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbStudent1.SelectedIndex == 0)
                return;

            if (cbStudent1.SelectedIndex == cbStudent2.SelectedIndex)
            {
                MessageBox.Show("Deze student is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbStudent1.SelectedIndex = _previousStudentIndex;
            }

            _previousStudentIndex = cbStudent1.SelectedIndex;
        }

        private void cbStudent2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbPairs.SelectedIndex > 0)
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbStudent2.SelectedIndex == 0)
                return;

            if (cbStudent2.SelectedIndex == cbStudent1.SelectedIndex)
            {
                MessageBox.Show("Deze student is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbStudent2.SelectedIndex = 0;
            }
        }

        private void cbTeacher1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbPairs.SelectedIndex > 0)
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbTeacher1.SelectedIndex == 0)
                return;

            if (cbTeacher1.SelectedIndex == cbTeacher2.SelectedIndex)
            {
                MessageBox.Show("Deze docent is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbTeacher1.SelectedIndex = 0;
            }
        }

        private void cbTeacher2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbPairs.SelectedIndex > 0)
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbTeacher2.SelectedIndex == 0)
                return;

            if (cbTeacher2.SelectedIndex == cbTeacher1.SelectedIndex)
            {
                MessageBox.Show("Deze docent is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbTeacher2.SelectedIndex = 0;
            }
        }

        private void cbExpert1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbPairs.SelectedIndex > 0)
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbExpert1.SelectedIndex == 0)
                return;

            if (cbExpert1.SelectedIndex == cbExpert2.SelectedIndex)
            {
                MessageBox.Show("Deze expert is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbExpert1.SelectedIndex = 0;
            }
        }

        private void cbExpert2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isBusyCoupling)
                return;

            if (cbPairs.SelectedIndex > 0 )
            {
                btnSave.IsEnabled = true;
                ResavePairAttachment();
            }

            if (cbExpert2.SelectedIndex == 0)
                return;

            if (cbExpert2.SelectedIndex == cbExpert1.SelectedIndex)
            {
                MessageBox.Show("Deze expert is al gekoppeld", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbExpert2.SelectedIndex = 0;
            }
        }
    }
}
