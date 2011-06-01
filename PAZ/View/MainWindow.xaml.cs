using System;
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

        public MainWindow()
        {
            InitializeComponent();

            //TEST CODE:
            MysqlDb db = new MysqlDb("student.aii.avans.nl", "MI4Ie", "4DRcUrzV", "MI4Ie_db");//Must be somewhere central
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

            genCalender();
        }



        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            textboxSearch.Text = "";

            // Maak en open een save dialog
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF (.pdf)|*.pdf";
            if (dlg.ShowDialog() == true)
            {
                // maak en exporteer als pdf
                _pdfExport.createPdf(dlg.FileName);
            }
        }

        private void buttonVerwijderGebruikers_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle gebruikers wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Alle gebruikers zijn verwijderd.", "Gebruikers verwijderd");
            }
        }

        private void buttonVerwijderLokalen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle lokalen wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Alle lokalen zijn verwijderd.", "Lokalen verwijderd");
            }
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
            if (MessageBox.Show("Brief uitprinten?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Brief is uitgeprint.", "Succesvol");
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
            return ini;
        }


        /*
         * CALENDER
         */
        private void genCalender()
        {
            IniFile ini = readIni();
            DateTime startDate = DateTime.Parse(ini["DATES"]["startdate"]);
            DateTime stopDate = DateTime.Parse(ini["DATES"]["enddate"]);
            Brush headerColor = Brushes.LightGray;
            int interval = 1;

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

            //Making rows
            int columns = 0;
            int rows = 0;

            Rectangle recC = new Rectangle();
            recC.Fill = headerColor;
            Grid.SetColumn(recC, 0);
            Grid.SetRow(recC, 0);
            //Grid.SetRowSpan(rec, Convert.ToInt32(stopDate.DayOfYear - startDate.DayOfYear) * 5);
            calender.Children.Add(recC);

            RowDefinition row = new RowDefinition();
            GridLength height = new GridLength(120);

            for (DateTime dateTime = startDate; dateTime <= stopDate; dateTime += TimeSpan.FromDays(interval))
            {
                if (dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday)
                {
                    Rectangle rec = new Rectangle();
                    rec.Fill = headerColor;
                    Grid.SetColumn(rec, 0);
                    Grid.SetRow(rec, rows);
                    Grid.SetColumnSpan(rec, classrooms.Count + 1);
                    calender.Children.Add(rec);
                    //Add labels
                    for (int c = 0; c < classrooms.Count + 1; c++)
                    {
                        if (calender.ColumnDefinitions.Count != classrooms.Count + 1)
                        {
                            //making columns
                            ColumnDefinition column = new ColumnDefinition();
                            GridLength width;
                            if (c == 0)
                                width = new GridLength(75);
                            else
                                width = new GridLength(120);
                            column.Width = width;
                            calender.ColumnDefinitions.Add(column);
                            columns++;
                        }
                        //making labels
                        Label header = new Label();
                        if (c == 0)
                            header.Content = dateTime.ToString("dddd",
                      new CultureInfo("nl-NL")) + "\n" + dateTime.ToShortDateString();
                        else
                            header.Content = classrooms[c - 1].Room_number;
                        header.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        header.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        Grid.SetColumn(header, c);
                        Grid.SetRow(header, rows);
                        calender.Children.Add(header);


                    }
                    for (int blok = 1; blok <= 4; blok++)
                    {
                        row = new RowDefinition();
                        if (blok == 1)
                            row.Height = new GridLength(60);
                        else
                            row.Height = height;
                        calender.RowDefinitions.Add(row);
                        rows++;

                        Label blk = new Label();
                        blk.Content = "Blok " + blok;
                        blk.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        blk.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        Grid.SetColumn(blk, 0);
                        Grid.SetRow(blk, rows);
                        calender.Children.Add(blk);
                    }
                    row = new RowDefinition();
                    row.Height = height;
                    calender.RowDefinitions.Add(row);
                    rows++;

                }


            }

            Grid.SetRowSpan(recC, rows);
            addZitting(1, 1, "8:30", "11:00");

        }

        private void addZitting(int column, int row, string starttime, string endtime)
        {
            Label session = new Label();
            session.Content = "Mark de Mol\nFreek Laurijssen\n\nGer Saris\nMarco Huysmans";
            session.BorderBrush = Brushes.LightGray;
            session.BorderThickness = new Thickness(1);
            Grid.SetColumn(session, column);
            Grid.SetRow(session, row);
            session.ToolTip = "lol";
            calender.Children.Add(session);
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
    }
}
