﻿using System;
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
    * In deze klassen kun je de geadresseerden bepalen voor de brief aan de experts
    * 
    * Auteur: Gökhan en Yorg 
    */
    public partial class LetterWindow : Window
    {
        private List<Expert> _experts;
        private List<User> _receivers;
        private List<SessionRow> _sessions;
        private List<CheckBox> _expertBoxes;

        private string _selectedReceiverEmail = string.Empty;

        private IniFile _ini;
        private PAZController _controller;
        private EmailTemplate _emailTemplate;

        public LetterWindow(List<SessionRow> sessions, EmailTemplate emailTemplate, PAZController controller)
        {
            InitializeComponent();

            tbInleiding.Text += emailTemplate.Inleiding;
            tbInformatie.Text += emailTemplate.Informatie;
            tbAfzenders.Text += emailTemplate.Afzenders;

            _experts = new List<Expert>();
            _receivers = new List<User>();
            _sessions = sessions;

            foreach (SessionRow session in sessions)
            {
                Session sessionModel = session.GetSessionModel();

                Expert[] experts = sessionModel.GetExperts();
                for (int i = 0; i < experts.Length; ++i)
                    if (experts[i] != null)
                        _experts.Add(experts[i]);
            }

            ExpertsToevoegen();

            _ini = controller.IniReader;
            _controller = controller;
            _emailTemplate = emailTemplate;
        }


        /**
        * Voegt docenten toe en weergeeft als checkboxen
        * 
        * Auteur: Gökhan 
        */
        private void ExpertsToevoegen()
        {
            _expertBoxes = new List<CheckBox>();
            Canvas canvasExperts = new Canvas();
            int left = 0, top = 0;
            foreach (Expert expert in _experts)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = " " + expert.Firstname + " " + expert.Surname + " (" + expert.Email + ")"; ;
                cb.Tag = expert; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasExperts.Children.Add(cb);

                _expertBoxes.Add(cb);

                top += 20;
            }
            scrollViewerExperts.Content = canvasExperts;
        }

        /**
        * Haalt de student of docent uit de _receivers lijst en unchecked de selector
        * Auteur: Gökhan en Yorg
        */
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // verwijder de gekoppelde object van de _receivers lijst
            _receivers.Remove((User)cb.Tag);

            HandleSelectorUnchecking(cb, cbxExpertSelector, _expertBoxes);
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
        * Voegt de geselecteerde studenten en docenten toe aan de _receivers lijst en markeert de selector als checked als alle checkboxes checked zijn.
        * Auteur: Gökhan en Yorg
        */
        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // voeg de gekoppelde object toe aan de _receivers lijst
            _receivers.Add((User) cb.Tag);

            if (CheckAllChecked(_expertBoxes))
                cbxExpertSelector.IsChecked = true;
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

        private void bntVerzenden_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tijdelijk buiten werking tot Teun de mappers update.");
            //_controller.BriefPrintenBevestigingClicked();
        }

        /**
        * Sluit het huidige scherm
        * 
        * Auteur: Gökhan 
        */
        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbxExpertSelector_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(_expertBoxes);
        }

        private void cbxExpertSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUncheck(_expertBoxes);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_controller.EmailWindowClosed(new EmailTemplate(_emailTemplate.Id, tbAfzender.Text, tbInleiding.Text, tbInformatie.Text, tbAfsluiting.Text, tbAfzenders.Text));
        }
    }
}
