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
    * In deze klasse kun je de te verwijderen gegevens bepalen
    */
    public partial class DeleteDataWindow : Window
    {
        private List<User> _users;
        private List<object> _deleteList;

        private List<CheckBox> _userBoxes;

        private string _selectedReceiverEmail = string.Empty;

        private PAZController _controller;

        public DeleteDataWindow()
        {
            InitializeComponent();

            _controller = PAZController.GetInstance();

            _users = _controller.UserMapper.FindAll();
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

            _users.Sort();

            PersonenToevoegen();            
        }


        /**
        * Voegt personen toe en geeft weer als checkboxen
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
        * Haalt de student of docent uit de _deleteList en unchecked de selector
        */
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // verwijder de gekoppelde object van de _deleteList
            _deleteList.Remove(cb.Tag);

            HandleSelectorUnchecking(cb, cbxPersoonSelector, _userBoxes);
        }

        /**
         * Hanteer het unchecken van een selector, omdat deze alles unchecked, moeten alle boxes weer gecheckt worden behalve degene die unchecked werdt
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
        */
        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // voeg de gekoppelde object toe aan de _deleteList lijst
            _deleteList.Add(cb.Tag);

            if (CheckAllChecked(_userBoxes))
                cbxPersoonSelector.IsChecked = true;      
        }

        /**
         * Controleert of alle checkboxes gechecked zijn
         * Return: true als ze allemaal gechecked zijn
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
        */
        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            if (_deleteList.Count == 0)
            {
                MessageBox.Show("U heeft geen te verwijderen gegevens geselecteerd, selecteer op zijn minst een item uit de lijst voor u probeert te verwijderen.", "Opmerking", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MessageBox.Show("Weet u zeker dat u de geselecteerde gegevens wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
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

        /**
         * Hanteert het checken van alle checkboxen
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
         */
        private void HandleUncheck(List<CheckBox> checkBoxes)
        {
            foreach (CheckBox cb in checkBoxes)
                cb.IsChecked = false;
        }
    }
}
