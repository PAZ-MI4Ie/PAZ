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
using System.Text.RegularExpressions;
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
        private List<SessionRow> _master;
        public ICollectionView Sessions { get; private set; }
        bool match;
        private PDFExport _pdfExport;

        private UserMapper _userMapper;
		private ClassroomMapper _classroomMapper;
		private StudentMapper _studentMapper;
		private TeacherMapper _teacherMapper;
		private ExpertMapper _expertMapper;

		private List<Student> _students;
        private List<Teacher> _teachers;
        private List<Expert> _experts;

        public MainWindow()
        {
            InitializeComponent();

            //TEST CODE:
            MysqlDb db = new MysqlDb("student.aii.avans.nl", "MI4Ie", "4DRcUrzV", "MI4Ie_db");//Must be somewhere central

            _userMapper = new UserMapper(db);
            _classroomMapper = new ClassroomMapper(db);

            SessionMapper sessionmapper = new SessionMapper(db);
			Console.WriteLine(sessionmapper.FindAll());
            List<Session> sessionModels = sessionmapper.FindAll();
            _master = new List<SessionRow>();
            foreach (Session s in sessionModels)
            {
                _master.Add(new SessionRow(s));
            }
            //END OF TEST CODE

			Sessions = CollectionViewSource.GetDefaultView(_master);
            GridOverzichtList.ItemsSource = Sessions;
			_studentMapper = new StudentMapper(MysqlDb.GetInstance());
			_teacherMapper = new TeacherMapper(MysqlDb.GetInstance());
            _expertMapper = new ExpertMapper(MysqlDb.GetInstance());
			_students = _studentMapper.FindAll();
            _teachers = _teacherMapper.FindAll();
            _experts = _expertMapper.FindAll();

			//Test code
            Student verlept = new Student();
            verlept.Firstname = "Henk";
            verlept.Surname = "de Vries";
            verlept.Study = "Bierkunde";
            verlept.Studentnumber = 53290523;
            verlept.Username = "hdevries";
            verlept.Status = "accepted";
            verlept.Email = "hdevries@avans.nl";
            //_studentmapper.Save(verlept);
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

            // 5/6/2011 Yorg: Bug fix betreft PDF export, het Overzicht DataGrid heeft geen kolommen als het nog niet in zicht is gekomen, 
            // dit is vervelend want die heeft de export nodig, daarom fop ik het systeem door de overzicht tab in de XAML als de start tab te zetten, maar hier in de constructor alsnog op de calender tab te zetten. Overbodig als het overzicht eerst getoond moet worden.
            tabCalender.Focus();

            genCalender();
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

        private void Agree(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Aanmelding goedkeuren?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol", "Succesvol");
            }
        }

        private void Decline(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Aanmelding afkeuren?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Aanmelding is afgekeurt.", "Succesvol");
            }
        }

        private void buttonEmailVersturen_Click(object sender, RoutedEventArgs e)
        {
            EmailWindow emailWindow = new EmailWindow();
            emailWindow.ShowDialog();
        }

        private void buttonBriefPrinten_Click(object sender, RoutedEventArgs e)
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            textboxSearch.Text = "";

            string fileName;
            if (OpenNewSaveDialog("Afstudeerscriptie Brieven", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
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


        /*
         * CALENDER
         */
        private void genCalender()
        {
            DateTime startDate = new DateTime(2011, 5, 1);
            DateTime stopDate = new DateTime(2011, 5, 15);
            int interval = 1;

            List<Classroom> classrooms = new List<Classroom>();
            Classroom room = new Classroom(1,"OB202");
            classrooms.Add(room);
            room = new Classroom(2,"OB203");
            classrooms.Add(room);
            room = new Classroom(3,"OB204");
            classrooms.Add(room);
            room = new Classroom(4,"OB205");
            classrooms.Add(room);
            room = new Classroom(5,"OC201");
            classrooms.Add(room);
            room = new Classroom(6,"OB201");
            classrooms.Add(room);
            room = new Classroom(7,"OC302");
            classrooms.Add(room);
            room = new Classroom(8,"OC202");
            classrooms.Add(room);

            //Making rows
            int columns = 0;
            int rows = 0;

            Rectangle rec = new Rectangle();
            rec.Fill = Brushes.LightGray;
            Grid.SetColumn(rec, 0);
            Grid.SetRow(rec, 0);
            Grid.SetRowSpan(rec, 80);
            calender.Children.Add(rec);

            RowDefinition row = new RowDefinition();
            GridLength height = new GridLength(120);

            for (DateTime dateTime = startDate; dateTime <= stopDate; dateTime += TimeSpan.FromDays(interval))
            {
                if (dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday)
                {
                    rec = new Rectangle();
                    rec.Fill = Brushes.LightGray;
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

		private void onTeacherAddClicked(object sender, RoutedEventArgs e)
		{

			//Use this for input errors
			bool hasInputError = false;


			//Check first name
			if (textBoxLeraarVoornaam.Text.Equals(String.Empty))
			{
				textBoxLeraarVoornaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxLeraarVoornaam.BorderBrush = Brushes.Gray;
			}

			//Check surname
			if (textLeraarAchternaam.Text.Equals(String.Empty))
			{
				textLeraarAchternaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textLeraarAchternaam.BorderBrush = Brushes.Gray;
			}

			//Check email adress
			if (EmailLeraar1.Text.Equals(String.Empty))
			{
				EmailLeraar1.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				EmailLeraar1.BorderBrush = Brushes.Gray;
			}
			if (!EmailLeraar1.Text.IsValidEmailAddress())
			{
				EmailLeraar1.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				EmailLeraar1.BorderBrush = Brushes.Gray;
			}


			//Create sessionspread variable
			Teacher.session_spread sessionSpread;

			//See which session spread option was chosen
			if(sessionVerspreid.IsChecked == true)
			{
				sessionSpread = Teacher.session_spread.FAR;
			}
			else if (sessionDichtBijElkaar.IsChecked == true)
			{
				sessionSpread = Teacher.session_spread.CLOSE;
			}
			else
			{
				sessionSpread = Teacher.session_spread.ANY;
			}

			if (hasInputError == false)
			{
				//Create teacher object and add values
				Teacher newTeacher = new Teacher();
				newTeacher.Firstname = textBoxLeraarVoornaam.Text;
				newTeacher.Surname = textLeraarAchternaam.Text;
				newTeacher.Email = EmailLeraar1.Text;
				newTeacher.Session_spread = sessionSpread;
				//TODO: blocked days
				//TODO
				//newTeacher.blockedDay = datePickerBlockedDay1;

				//Send to the database
				_teacherMapper.Save(newTeacher);
				MessageBox.Show("Leraar toegevoegd");
			}
		}

		private void onClassroomAddClicked(object sender, RoutedEventArgs e)
		{
			if (textBoxLokaalGegevens.Text.Equals(string.Empty))
			{
				textBoxLokaalGegevens.BorderBrush = Brushes.Red;
			}
			else
			{
				textBoxLokaalGegevens.BorderBrush = Brushes.Gray;
				Classroom newClassroom = new Classroom();
				newClassroom.Room_number = textBoxLokaalGegevens.Text;
				_classroomMapper.Save(newClassroom);
				MessageBox.Show("Lokaal toegevoegd");
			}
		}
    }

	public static class ValidatorExtensions
	{
		public static bool IsValidEmailAddress(this string s)
		{
			Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
			return regex.IsMatch(s);
		}
	}
}
