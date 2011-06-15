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
    * In deze klassen kun je de te verwijderen gegevens bepalen
    * 
    * Auteur: Gökhan en Yorg 
    */
    public partial class DeleteDataWindow : Window
    {
        private List<User> _users;
        private List<Classroom> _classrooms;
        private List<object> _deleteList;

        private List<CheckBox> _userBoxes;
        private List<CheckBox> _classroomBoxes;

        private string _selectedReceiverEmail = string.Empty;

        private PAZController _controller;

        public DeleteDataWindow()
        {
            InitializeComponent();

            _controller = PAZController.GetInstance();

            _users = _controller.UserMapper.FindAll();
            _classrooms = _controller.ClassroomMapper.FindAll();
            _deleteList = new List<object>();

            List<User> _admins = new List<User>();
            foreach (User user in _users)
            {
                if (user.User_type == "admin")
                    _admins.Add(user);
            }

            // Dit moet wel zo, omdat de admins niet direct in de users list kunnen worden verwijderd tijdens een loop
            foreach (User admin in _admins)
                _users.Remove(admin);

            LokalenToevoegen();
            PersonenToevoegen();            
        }


        /**
        * Voegt personen toe en geeft weer als checkboxen
        * 
        * Auteur: Gökhan en Yorg
        */
        private void PersonenToevoegen()
        {
            _userBoxes = new List<CheckBox>();
            Canvas canvasUsers = new Canvas();

            int left = 0, top = 0;
            double rightMost = 0.0;
            foreach (User user in _users)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);

                string gebruikerType = user.User_type;
                if(gebruikerType == "teacher")
                    gebruikerType = "docent";

                cb.Content = " " + user.Firstname + " " + user.Surname + " (" + user.Email + ") : " + gebruikerType;
                cb.Tag = user; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasUsers.Children.Add(cb);

                _userBoxes.Add(cb);

                top += 20;

                double newRightMost = left + cb.Width;
                if(rightMost < newRightMost)
                    rightMost = newRightMost;
            }
            canvasUsers.Width = rightMost;
            canvasUsers.Height = top;

            canvasUsers.HorizontalAlignment = HorizontalAlignment.Left;
            canvasUsers.VerticalAlignment = VerticalAlignment.Top;

            scrollViewerPersonen.Content = canvasUsers;
        }

       
        /**
        * Voegt lokalen toe en geeft weer als checkboxen
        * 
        * Auteur: Gökhan en Yorg
        */
        private void LokalenToevoegen()
        {
            _classroomBoxes = new List<CheckBox>();
            Canvas canvasClassrooms = new Canvas();

            int left = 0, top = 0;
            double rightMost = 0;
            foreach (Classroom classroom in _classrooms)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = classroom.Room_number;
                cb.Tag = classroom; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasClassrooms.Children.Add(cb);

                _classroomBoxes.Add(cb);

                top += 20;
                double newRightMost = left + cb.Width;
                if (rightMost < newRightMost)
                    rightMost = newRightMost;
            }
            canvasClassrooms.Width = rightMost;
            canvasClassrooms.Height = top;

            canvasClassrooms.HorizontalAlignment = HorizontalAlignment.Left;
            canvasClassrooms.VerticalAlignment = VerticalAlignment.Top;

            scrollViewerLokalen.Content = canvasClassrooms;
        }

        /**
        * Haalt de student of docent uit de _deleteList en unchecked de selector
        * Auteur: Gökhan en Yorg
        */
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // verwijder de gekoppelde object van de _deleteList
            _deleteList.Remove(cb.Tag);

            if (cb.Tag is User)
                HandleSelectorUnchecking(cb, cbxPersoonSelector, _userBoxes);
            else
                HandleSelectorUnchecking(cb, cbxLokaalSelector, _classroomBoxes);
        }

        /**
         * Hanteer het unchecken van een selector, omdat deze alles unchecked, moeten alle boxes weer gecheckt worden behalve degene die unchecked werdt
         * Auteur: Gökhan en Yorg 
         */
        private void HandleSelectorUnchecking(CheckBox uncheckedBox, CheckBox selectorBox, List<CheckBox> checkBoxes)
        {
            bool selectorWasChecked = (bool)selectorBox.IsChecked;

            selectorBox.IsChecked = false;

            if (selectorWasChecked)
                HandleCheck(checkBoxes, uncheckedBox);
        }

        /**
        * Voegt de geselecteerde data toe aan de _deleteList en markeert de selector als checked als alle checkboxes checked zijn.
        * Auteur: Gökhan en Yorg
        */
        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // voeg de gekoppelde object toe aan de _deleteList lijst
            _deleteList.Add(cb.Tag);

            bool isUser = cb.Tag is User;
            if (CheckAllChecked(isUser ? _userBoxes : _classroomBoxes))
            {
                if (isUser)
                    cbxPersoonSelector.IsChecked = true;
                else
                    cbxLokaalSelector.IsChecked = true;          
            }
        }

        /**
         * Controleert of alle checkboxes gechecked zijn
         * Return: true als ze allemaal gechecked zijn
         * Auteur: Gökhan en Yorg 
         */
        private bool CheckAllChecked(List<CheckBox> checkBoxes)
        {       
            foreach (CheckBox cb in checkBoxes)
            {
                if (cb.IsChecked == false)
                    return false;
            }

            return true;
        }

        /**
        * Stuurt het commando om de geselecteerde gegevens te verwijderen
        * 
        * Auteur: Gökhan en Yorg
        */
        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            if (_deleteList.Count == 0)
            {
                MessageBox.Show("U heeft geen te verwijderen gegevens geselecteerd, selecteer op zijn minst een item uit de lijst voor u probeert te verwijderen.", "Opmerking", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MessageBox.Show("Weet u zeker dat u alle gebruikers wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (_controller.VerwijderGegevensBevestigingClicked(_deleteList))
                    MessageBox.Show("Succesvol. Alle de gekozen gegevens zijn verwijderd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Mislukt, de gekozen gegevens konden niet (helemaal) verwijderd worden.", "Verwijderen", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSluiten_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbxPersoonSelector_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(_userBoxes);
        }

        private void cbxPersoonSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUncheck(_userBoxes);
        }

        private void cbxLokalenSelector_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(_classroomBoxes);
        }

        private void cbxLokalenSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUncheck(_classroomBoxes);
        }

        /**
         * Hanteert het checken van alle checkboxen
         * Auteur: Gökhan en Yorg 
         */
        private void HandleCheck(List<CheckBox> checkBoxes, CheckBox ignoreBox = null)
        {
            foreach (CheckBox cb in checkBoxes)
            {
                if(cb != ignoreBox)
                    cb.IsChecked = true;
            }
        }

        /**
         * Hanteert het unchecken van alle checkboxen
         * Auteur: Gökhan en Yorg 
         */
        private void HandleUncheck(List<CheckBox> checkBoxes)
        {
            foreach (CheckBox cb in checkBoxes)
                cb.IsChecked = false;
        }
    }
}
