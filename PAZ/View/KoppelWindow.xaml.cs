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
    * In deze klassen kun je de geadresseerden bepalen(docenten en studenten).
    * 
    * Auteur: Gökhan en Yorg 
    */
    public partial class KoppelWindow : Window
    {
        private PAZController _controller;

        private List<Pair> _pairs;
        private List<Teacher> _teachers;
        private List<Expert> _experts;

        private bool _isBusyCoupling = false;

        public KoppelWindow()
        {
            InitializeComponent();

            _controller = PAZController.GetInstance();

            btnSave.IsEnabled = false;

            _pairs = _controller.PairMapper.FindAll();
            _teachers = _controller.TeacherMapper.FindAll();
            _experts = _controller.ExpertMapper.FindAll();

            fillPairs();
            fillTeachers();
            fillExperts();
        }

        private void fillPairs()
        {
            cbPairs.Items.Add("Kies een paar");

            List<Pair> foundPairs = _pairs;
            foreach (Pair pair in foundPairs)
                cbPairs.Items.Add(pair);

            cbPairs.SelectedIndex = 0;
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
        * 
        * Auteur: Gökhan en Yorg
        */
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if(btnSave.IsEnabled)
                Save();

            this.Close();
        }

        /**
        * Sluit het huidige scherm
        * 
        * Auteur: Gökhan 
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

            btnSave.IsEnabled = false;
        }

        private void ResavePairAttachment()
        {
            Pair pair = (Pair) cbPairs.Items[cbPairs.SelectedIndex];

            List<User> newAttachmentList = new List<User>();

            if(cbTeacher1.SelectedIndex > 0)
                newAttachmentList.Add((User) cbTeacher1.Items[cbTeacher1.SelectedIndex]);

            if(cbTeacher2.SelectedIndex > 0)
                newAttachmentList.Add((User) cbTeacher2.Items[cbTeacher2.SelectedIndex]);

            if(cbExpert1.SelectedIndex > 0)
                newAttachmentList.Add((User) cbExpert1.Items[cbExpert1.SelectedIndex]);

            if(cbExpert2.SelectedIndex > 0)
                newAttachmentList.Add((User) cbExpert2.Items[cbExpert2.SelectedIndex]);

            pair.Attachments = newAttachmentList;
        }

        private void cbPairs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isBusyCoupling = true;

            int selectedIndex = cbPairs.SelectedIndex;
            if (selectedIndex <= 0)
            {
                cbTeacher1.SelectedIndex = 0;
                cbTeacher2.SelectedIndex = 0;
                cbExpert1.SelectedIndex = 0;
                cbExpert2.SelectedIndex = 0;
                return;
            }

            Pair selectedPair = (Pair) cbPairs.Items[selectedIndex];
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
