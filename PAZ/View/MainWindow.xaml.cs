﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using PAZMySQL;
using PAZ.Model;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using Ini;
using PAZ.View;

namespace PAZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /// 


    public partial class MainWindow : Window
    {
        private List<Session> _master;
        public ICollectionView Sessions { get; private set; }
        bool match;
        private PDFExport _pdfExport;
        private UserMapper _userMapper;
        private ClassroomMapper _classroomMapper;
        private IniFile ini;

        public MainWindow()
        {
            InitializeComponent();

            //TEST CODE:
            MysqlDb db = new MysqlDb("student.aii.avans.nl", "MI4Ie", "4DRcUrzV", "MI4Ie_db");//Must be somewhere central

            _userMapper = new UserMapper(db);
            _classroomMapper = new ClassroomMapper(db);

            SessionMapper sessionmapper = new SessionMapper(db);
			Console.WriteLine(sessionmapper.FindAll());
			_master = sessionmapper.FindAll();
            //END OF TEST CODE

			Sessions = CollectionViewSource.GetDefaultView(_master);
            GridOverzichtList.ItemsSource = Sessions;
            StudentMapper studentmapper = new StudentMapper(MysqlDb.GetInstance());
            Student verlept = new Student();
            verlept.Firstname = "Henk";
            verlept.Surname = "de Vries";
            verlept.Study = "Bierkunde";
            verlept.Studentnumber = 53290523;
            verlept.Username = "hdevries";
            verlept.Status = "accepted";
            verlept.Email = "hdevries@avans.nl";
            //studentmapper.Save(verlept);
            //END OF TEST CODE

            /*
            _master = new List<Zitting>
            {
                new Zitting
                (
                    "10-5-2011",
                    "13:30",
                    "OB002",
                    "Piet Jan \nJan Piet",
                    "Ger Saris \nKeesjan hogenboom",
                    "Ad Groot 2 \nAad Klein",
                    12
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:00",
                    "OB002",
                    "Ibrahim Boven\nJeroen Schipper",
                    "Freek Hogenboom\nSjaak Lauris",
                    "Kees Prof 2 \n Piet Hogensluiter",
                    4
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:30",
                    "OB002",
                    "Freek Netes\nMark Hos",
                    "Bruno Marks\nMandy Tregis",
                    "Kelly Bruins\nPatricia Kaai",
                    6
                ),
                new Zitting
                (
                    "10-5-2011",
                    "13:30",
                    "OB002",
                    "Piet Jan \nJan Piet",
                    "Ger Saris \nKeesjan hogenboom",
                    "Ad Groot 2 \nAad Klein",
                    10
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:00",
                    "OB002",
                    "Ibrahim Boven\nJeroen Schipper",
                    "Freek Hogenboom\nSjaak Lauris",
                    "Kees Prof 2 \n Piet Hogensluiter",
                    11
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:30",
                    "OB002",
                    "Freek Netes\nMark Hos",
                    "Bruno Marks\nMandy Tregis",
                    "Kelly Bruins\nPatricia Kaai",
                    8
                ),
                new Zitting
                (
                    "10-5-2011",
                    "13:30",
                    "OB002",
                    "Piet Jan \nJan Piet",
                    "Ger Saris \nKeesjan hogenboom",
                    "Ad Groot 2 \nAad Klein",
                    12
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:00",
                    "OB002",
                    "Ibrahim Boven\nJeroen Schipper",
                    "Freek Hogenboom\nSjaak Lauris",
                    "Kees Prof 2 \n Piet Hogensluiter",
                    3
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:30",
                    "OB002",
                    "Freek Netes\nMark Hos",
                    "Bruno Marks\nMandy Tregis",
                    "Kelly Bruins\nPatricia Kaai",
                    13
                ),
                new Zitting
                (
                    "10-5-2011",
                    "13:30",
                    "OB002",
                    "Piet Jan \nJan Piet",
                    "Ger Saris \nKeesjan hogenboom",
                    "Ad Groot 2 \nAad Klein",
                    7
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:00",
                    "OB002",
                    "Ibrahim Boven\nJeroen Schipper",
                    "Freek Hogenboom\nSjaak Lauris",
                    "Kees Prof 2 \n Piet Hogensluiter",
                    17
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:30",
                    "OB002",
                    "Freek Netes\nMark Hos",
                    "Bruno Marks\nMandy Tregis",
                    "Kelly Bruins\nPatricia Kaai",
                    3
                ),
                new Zitting
                (
                    "10-5-2011",
                    "13:30",
                    "OB002",
                    "Piet Jan \nJan Piet",
                    "Ger Saris \nKeesjan hogenboom",
                    "Ad Groot 2 \nAad Klein",
                    9
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:00",
                    "OB002",
                    "Ibrahim Boven\nJeroen Schipper",
                    "Freek Hogenboom\nSjaak Lauris",
                    "Kees Prof 2 \n Piet Hogensluiter",
                    8
                ),
                new Zitting
                (
                    "10-5-2011",
                    "14:30",
                    "OB002",
                    "Freek Netes\nMark Hos",
                    "Bruno Marks\nMandy Tregis",
                    "Kelly Bruins\nPatricia Kaai",
                    8
                )

            };
			 */

            Sessions = CollectionViewSource.GetDefaultView(_master);
            GridOverzichtList.ItemsSource = Sessions;

            // maak object
            _pdfExport = new PDFExport(GridOverzichtList);

            ini = readIni();
            List<Classroom> classrooms = new List<Classroom>();
            Classroom room = new Classroom(1, "OB202");
            classrooms.Add(room);
            room = new Classroom(2, "OB203");
            classrooms.Add(room);
            room = new Classroom(3, "OB204");
            classrooms.Add(room);
            room = new Classroom(4, "OB205");
            classrooms.Add(room);
            room = new Classroom(5, "OC201");
            classrooms.Add(room);
            room = new Classroom(6, "OB201");
            classrooms.Add(room);
            room = new Classroom(7, "OC302");
            classrooms.Add(room);
            room = new Classroom(8, "OC202");
            classrooms.Add(room);
            calendar.createCalendar(ini, classrooms);
            string[] teachers = new string[] { "Marco Huysmans", "Ger Saris" };
            string[] experts = new string[] { "Piet Janssen", "Karel Lessers" };
            calendar.addSession("2-5-2011", 1, 1, "Jeroen Schipper", "Hidde Jansen", teachers, experts);
            calendar.addSession("3-5-2011", 1, 1, "Freek Laurijssen", "Ibrahim Önder", teachers, experts);
            //calendar.loadAllSessions();
        }



        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            textboxSearch.Text = "";

            string fileName;
            if (OpenNewSaveDialog("Roosteroverzicht PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                _pdfExport.CreateOverviewPDF(fileName);
            }
        }

        private void buttonVerwijderGebruikers_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle gebruikers wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                MessageBox.Show(_userMapper.Delete() ? "Succesvol. Alle gebruikers zijn verwijderd." : "Mislukt, de gebruikers konden niet verwijderd worden.", "Gebruikers verwijderen");
        }

        private void buttonVerwijderLokalen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle lokalen wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                MessageBox.Show(_classroomMapper.Delete() ? "Succesvol. Alle lokalen zijn verwijderd." : "Mislukt, de lokalen konden niet verwijderd worden.", "Lokalen verwijderen");
        }

        private void comboBoxSelecteerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSelecteerType.SelectedIndex > 0)
            {
                verbergAlleToevoegGroupBoxs();

                switch (comboBoxSelecteerType.SelectedIndex)
                {
                    case 1: groupBoxLokaalGegevens.Visibility = Visibility.Visible; break;
                    case 2: groupBoxGebruikerGegevens.Visibility = Visibility.Visible; break;
                    case 3: groupBoxBegeleiderGegevens.Visibility = Visibility.Visible; break;
                    case 4: groupBoxExternGegevens.Visibility = Visibility.Visible; break;
                    case 5: groupBoxLeraarGegevens.Visibility = Visibility.Visible; break;
                }
            }
        }

        private void verbergAlleToevoegGroupBoxs()
        {
            groupBoxLokaalGegevens.Visibility = Visibility.Hidden;
            groupBoxGebruikerGegevens.Visibility = Visibility.Hidden;
            groupBoxBegeleiderGegevens.Visibility = Visibility.Hidden;
            groupBoxExternGegevens.Visibility = Visibility.Hidden;
            groupBoxLeraarGegevens.Visibility = Visibility.Hidden;
        }

        private void buttonEmailVersturen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Emails versturen?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Emails zijn verzonden.", "Succesvol");
            }
        }

        private void buttonBriefPrinten_Click(object sender, RoutedEventArgs e)
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            textboxSearch.Text = "";

            string fileName;
            if (OpenNewSaveDialog("Bevestigingsbrieven PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                _pdfExport.CreateLetterPDF(fileName);
            }
        }

        private void GridOverzichtList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (MessageBox.Show("Wilt u de wijzigingen opslaan?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen zijn opgeslagen.", "Succesvol");
            }
        }

        private void textboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (comboBoxSearch.SelectedIndex > 1)
            {
                Sessions.Filter = delegate(object item)
                {


                    switch (comboBoxSearch.SelectedIndex)
                    {
                        case 1: match = ((Zitting)(item)).Datum.ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 2: match = ((Zitting)(item)).Tijd.ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 3: match = ((Zitting)(item)).Lokaal.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 4: match = ((Zitting)(item)).Leerlingen.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 5: match = ((Zitting)(item)).Docenten.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 6: match = ((Zitting)(item)).Deskundige.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 7: match = ((Zitting)(item)).AantalGasten.ToString().Contains(textboxSearch.Text.ToLower()); break;

                    }

                    return match;
                };
            }
            else
                comboBoxSearch.SelectedIndex = 0;
        }

        private void comboBoxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nieuw: Gewijzigd door Yorg, dit werkt ook zo, oude manier was niet handig met nieuwe Zitting constructor
            if (comboBoxSearch.SelectedIndex == 0)
                Sessions.Filter = null; 

            // Oud
            //if (comboBoxSearch.SelectedIndex == 0)
            //    Zittingen.Filter = delegate(object item)
            //    {
            //        item = new Zitting();
            //        return true;
            //    };
        }
        public IniFile readIni()
        {
            IniFile ini = new Ini.IniFile("sys.ini");
            if (ini.Exists())
                ini.Load();
            else
            {
                IniSection section = new IniSection();
                section.Add("startdate", "1-05-2011");
                section.Add("enddate", "15-05-2011");
                ini.Add("DATES", section);

                section = new IniSection();
                section.Add("block1", "09:00-10:30");
                section.Add("block2", "11:00-12:30");
                section.Add("block3", "13:00-14:30");
                section.Add("block4", "15:00-16:30");
                ini.Add("TIME", section);

                ini.Save();
            }

            textBoxDeadlineStart.Text = ini["DATES"]["startdate"];
            textBoxDeadlineEind.Text = ini["DATES"]["enddate"];

            return ini;
        }

        /*
         * import button
         * (C) Mark de Mol
         * 
         * Shows a browse dialog. User must select a CSV file to import.
         * The CSV file must contain user data.
         * 
         * TO DO:
         *  - ADD EACH ITEM TO THE DATABASE
         */
        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Title = "Open een CSV-gebruikers bestand";
            dlg.Filter = "CSV-bestand|*.csv";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (dlg.CheckFileExists)
                {
                    string filename = dlg.FileName;
                    string line; 
                    StreamReader file = null;

                    try 
                    {
                        file = new StreamReader( filename );
                        while ((line = file.ReadLine()) != null)
                        {

                            string[] csvResult = line.Split(new Char[] { ',' });

                            for (int i = 0; i < csvResult.Length; i++)
                            {

                                /*
                                 * ADD THE RESULTS TO THE DATABASE OVER HERE,
                                 * 
                                 * I SAID, OVER HERE DAMNED!
                                 * 
                                 * FIRST 12 ITEMS ARE THE COLUMS!!!!!
                                 * this way you can easily add them.. naaisssss!
                                 */


                                /*
                                 * test to show each item
                                 * 
                                 *  MessageBox.Show(csvResult[i]);
                                 *  Console.WriteLine(csvResult[i]);
                                 *  
                                 */
                            }

                            /*
                             * test to show each line
                             * 
                             *  MessageBox.Show(line);
                             */

                        }
                    }
                    finally
                    {
                        if (file != null)
                            file.Close();
                    }

                }
                else
                    MessageBox.Show("Bestand niet gevonden.");
            }

        }
        
        /**
         * Deze functie maakt en opent een SaveFileDialog
         * @input: defaultFileName de standaard bestandsnaam om te gebruiken
         * @input: defaultExtension de standaard bestands extensie om te gebruiken
         * @input: filter het filter van bestandstypen waaruit gekozen kan worden
         * @output: outFileName het volledige pad + bestandsnaam nadat het dialoog klaar is
         * @input: appendDate als dit true is, dan wordt er de datum van vandaag aan de bestandsnaam toegevoegd
         * Return: De waarde teruggeven nadat de gebruiker het scherm sluit
         * Auteur: Yorg 
         */
        private bool? OpenNewSaveDialog(string defaultFileName, string defaultExtension, string filter, out string outFileName, bool appendDate = true)
        {
            // Maak het dialoog
            SaveFileDialog saveDialog = new SaveFileDialog();

            // Vul standaard gegevens in
            saveDialog.FileName = defaultFileName;
            if (appendDate)
                saveDialog.FileName += " " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

            saveDialog.FileName += defaultExtension;
            saveDialog.Filter = filter;

            // Open het dialoog en onthou de teruggekregen waarde
            bool? returnValue = saveDialog.ShowDialog();

            // Stel de output filename in
            outFileName = saveDialog.FileName;

            // Return de waarde teruggekregen op het moment dat het dialoog sloot
            return returnValue;
        }

        private void buttonOptiesOpslaan_Click(object sender, RoutedEventArgs e)
        {
            ini["DATES"]["startdate"] = textBoxDeadlineStart.Text;
            ini["DATES"]["enddate"] = textBoxDeadlineEind.Text;

            bool isSaved = ini.Save();
            if(isSaved)
                MessageBox.Show("Uw instellingen zijn opgeslagen.");
            else
                MessageBox.Show("Er is iets mis gegaan met het opslaan.");
        }
    }
}
