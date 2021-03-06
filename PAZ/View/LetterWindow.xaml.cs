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
        private Dictionary<int, Expert> _receivers;
        private List<SessionRow> _sessions;
        private List<CheckBox> _expertBoxes;

        private string _selectedReceiverEmail = string.Empty;

        private IniFile _ini;
        private PAZController _controller;
        private LetterTemplate _letterTemplate;

        public LetterWindow(List<SessionRow> sessions, LetterTemplate letterTemplate)
        {
            InitializeComponent();

            tbKenmerk.Text = letterTemplate.Kenmerk;
            tbContactpersonen.Text = letterTemplate.Contactpersonen;
            tbTelefoon.Text = letterTemplate.Telefoon;
            tbEmail.Text = letterTemplate.Email;
            tbAvansAdres.Text = letterTemplate.AvansAdres;
            tbAvansLocatie.Text = letterTemplate.AvansLocatie;
            tbBeginKern.Text = letterTemplate.BeginKern;
            tbReisInformatie.Text = letterTemplate.ReisInformatie;
            tbVerdereInformatie.Text = letterTemplate.VerdereInformatie;
            tbAfzenders.Text = letterTemplate.Afzenders;
            tbBijlagen.Text = letterTemplate.Bijlagen;
            tbVoettekstLinks.Text = letterTemplate.VoettekstLinks;
            tbVoettekstMidden.Text = letterTemplate.VoettekstCenter;
            tbVoettekstRechts.Text = letterTemplate.VoettekstRechts;

            _experts = new List<Expert>();
            _receivers = new Dictionary<int, Expert>();
            _sessions = sessions;

            // Note: Dit ziet er misschien klote uit, maar een List.Contains check schijnt niet te werken(mogelijk zijn er dubbele expert objecten?)
            Dictionary<int, Expert> experts = new Dictionary<int, Expert>();

            foreach (SessionRow session in sessions)
            {
                Session sessionModel = session.GetSessionModel();

                foreach (KeyValuePair<int, Expert> keyValuePair in sessionModel.Experts)
                {
                    if(!experts.ContainsKey(keyValuePair.Key))
                        experts.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            // En nu weer naar een List omdat Dictionary faalt en niet kan sorten
            foreach (KeyValuePair<int, Expert> keyValuePair in experts)
                _experts.Add(keyValuePair.Value);

            _experts.Sort();

            ExpertsToevoegen();

            _controller = PAZController.GetInstance();
            _ini = _controller.IniReader;
            _letterTemplate = letterTemplate;

            btnSave.IsEnabled = false;
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
            double rightMost = 0.0;
            foreach (Expert expert in _experts)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = " " + expert.Firstname + " " + expert.Surname + " (" + expert.Email + ")";
                cb.Tag = expert; // koppelt object aan checkbox

                if (expert.WasChanged == true)
                    cb.IsChecked = true;

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasExperts.Children.Add(cb);

                _expertBoxes.Add(cb);

                top += 20;

                double newRightMost = left + cb.Width;
                if (rightMost < newRightMost)
                    rightMost = newRightMost;
            }
            canvasExperts.Width = rightMost;
            canvasExperts.Height = top;

            canvasExperts.HorizontalAlignment = HorizontalAlignment.Left;
            canvasExperts.VerticalAlignment = VerticalAlignment.Top;

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
            _receivers.Remove(((Expert)cb.Tag).Id);

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

            // voeg het gekoppelde object toe aan de _receivers lijst
            Expert expert = (Expert) cb.Tag;
            _receivers.Add(expert.Id, expert);

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

        private void bntMaken_Click(object sender, RoutedEventArgs e)
        {
            if (_receivers.Count == 0)
            {
                MessageBox.Show("U heeft geen geadresseerden geselecteerd, selecteer op zijn minst een persoon uit de lijst voor u probeert een brief aan te maken.", "Opmerking", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Sla de wijzigingen vast op, indien er iets gewijzigd was
            if(btnSave.IsEnabled)
                SaveTemplate();

            _controller.BriefMakenBevestigingClicked(_receivers, _letterTemplate);
        }

        /**
        * Sluit het huidige scherm
        * 
        * Auteur: Gökhan 
        */
        private void btnSluiten_Click(object sender, RoutedEventArgs e)
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
            if (!btnSave.IsEnabled)
                return;

            MessageBoxResult result = MessageBox.Show("Wilt u de wijzigingen opslaan?", "Bevestiging", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                SaveTemplate();
            else if (result == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTemplate();

            btnSave.IsEnabled = false;
        }

        private void SaveTemplate()
        {
            UpdateTemplate();

            _controller.LetterWindowSaveTemplateClicked(_letterTemplate);
        }

        private void UpdateTemplate()
        {
            _letterTemplate.Kenmerk = tbKenmerk.Text;
            _letterTemplate.Contactpersonen = tbContactpersonen.Text;
            _letterTemplate.Telefoon = tbTelefoon.Text;
            _letterTemplate.Email = tbEmail.Text;
            _letterTemplate.AvansAdres = tbAvansAdres.Text;
            _letterTemplate.AvansLocatie = tbAvansLocatie.Text;
            _letterTemplate.BeginKern = tbBeginKern.Text;
            _letterTemplate.ReisInformatie = tbReisInformatie.Text;
            _letterTemplate.VerdereInformatie = tbVerdereInformatie.Text;
            _letterTemplate.Afzenders = tbAfzenders.Text;
            _letterTemplate.Bijlagen = tbBijlagen.Text;
            _letterTemplate.VoettekstLinks = tbVoettekstLinks.Text;
            _letterTemplate.VoettekstCenter = tbVoettekstMidden.Text;
            _letterTemplate.VoettekstRechts = tbVoettekstRechts.Text;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = true;
        }
    }
}
