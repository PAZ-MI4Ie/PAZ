using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Ini;
using Microsoft.Win32;
using PAZ.Control;
using PAZ.Model;
using PAZ.Model.Mappers;
using PAZ.View;
using PAZMySQL;

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
        bool _match;
		private List<Teacher> _teachers;
		private List<Student> _students;
        private List<Classroom> _classrooms;
        private List<Pair> _pairs;

        private PAZController _controller;

        public MainWindow()
        {
            InitializeComponent();

            _controller = PAZController.GetInstance();
            _controller.Init(this);

            textBoxDeadlineStart.Text = _controller.IniReader["DATES"]["startdate"];
            textBoxDeadlineEind.Text = _controller.IniReader["DATES"]["enddate"];

            //TEST CODE:
            List<Session> tempSessions = _controller.SessionMapper.FindAll();
            Console.WriteLine(tempSessions);
            _master = new List<SessionRow>();
            foreach (Session s in tempSessions)
            {
                _master.Add(new SessionRow(s));
            }
			//END OF TEST CODE

            _teachers = _controller.TeacherMapper.FindAll();
            _students = _controller.StudentMapper.FindAll();

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

            #region test shit
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
            #endregion

            Sessions = CollectionViewSource.GetDefaultView(_master);
            GridOverzichtList.ItemsSource = Sessions;
           
            _classrooms = _controller.ClassroomMapper.FindAll();
            _pairs = _controller.PairMapper.FindAll();
            //Test CODE
            _classrooms = new List<Classroom>();
            Classroom room = new Classroom(1, "OB202");
            _classrooms.Add(room);
            room = new Classroom(2, "OB203");
            _classrooms.Add(room);
            room = new Classroom(3, "OB204");
            _classrooms.Add(room);
            room = new Classroom(4, "OB205");
            _classrooms.Add(room);
            room = new Classroom(5, "OC201");
            _classrooms.Add(room);
            room = new Classroom(6, "OB201");
            _classrooms.Add(room);
            room = new Classroom(7, "OC302");
            _classrooms.Add(room);
            room = new Classroom(8, "OC202");
            _classrooms.Add(room);
            //END TEST CODE

            CalendarView.Sessionmapper = _controller.SessionMapper;
            CalendarView.Pairmapper = _controller.PairMapper;
            CalendarView.Daytimemapper = _controller.DaytimeMapper;
            CalendarView.Classroommapper = _controller.ClassroomMapper;
            calendar.createCalendar(_controller.IniReader, _classrooms, _controller);
            calendar.loadAllSessions(tempSessions);
            UnPlannedPairs unPlannedPairs = new UnPlannedPairs();
            unPlannedPairs.loadAllPairs(_controller.PairMapper);
            unPlannedPairs.Show();
            tabCalender.Focus();
            
        }



        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            _controller.ExportRoosterClicked();
        }

        private void buttonVerwijderGegevens_Click(object sender, RoutedEventArgs e)
        {
            _controller.VerwijderGegegevensClicked();     
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
            _controller.EmailVersturenClicked(_master);
        }

        private void buttonBriefMaken_Click(object sender, RoutedEventArgs e)
        {
            _controller.BriefMakenClicked(_master);
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
                        case 1: _match = ((SessionRow)(item)).Datum.ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 2: _match = ((SessionRow)(item)).Timeslot.ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 3: _match = ((SessionRow)(item)).Lokaal.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 4: _match = ((SessionRow)(item)).Studenten.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 5: _match = ((SessionRow)(item)).Docenten.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 6: _match = ((SessionRow)(item)).Deskundigen.ToLower().ToString().Contains(textboxSearch.Text.ToLower()); break;
                        case 7: _match = ((SessionRow)(item)).AantalGasten.ToString().Contains(textboxSearch.Text.ToLower()); break;

                    }

                    return _match;
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
						int a = 0;
						List<Student> duplicates = new List<Student>();

                        while ((line = file.ReadLine()) != null)
                        {

                            string[] csvResult = line.Split(new Char[] { ',' });
							if(a != 0)
							{
								Student student = new Student();
								
								for (int i = 0; i < csvResult.Length; i++)
								{
									Console.WriteLine(csvResult[4]);
									switch (i)
									{
										case 0: student.Firstname = csvResult[0]; break;
										case 1: student.Surname = csvResult[1]; break;
										case 2: student.Studentnumber = Convert.ToInt32(csvResult[2]); break;
										case 3: student.Study = csvResult[3]; break;
										case 4: student.Email = csvResult[4]; break;
									}


								}
								if (_controller.UserMapper.FindWithDuplicateCheck(Convert.ToInt32(csvResult[2]), csvResult[4]))
								{
									duplicates.Add(student);
								}
								else
								{
									_controller.StudentMapper.Save(student);
									MessageBox.Show("Importeren is gelukt. Alle items zijn toegevoegd.");
								}
							}
							a++;
                        }

						if (duplicates.Count > 0)
						{
							String dup = "duplicaten.txt";
							if (!File.Exists(dup))
							{
								TextWriter tw = new StreamWriter(dup);
								
								foreach (Student duplicate in duplicates)
								{
									tw.WriteLine(duplicate.Studentnumber + "," + duplicate.Firstname + "" + duplicate.Surname + "," + duplicate.Email);
								}
								MessageBox.Show("Er zijn " + duplicates.Count + " duplicaten gevonden. Deze zijn niet geimporteerd, maar opgeslagen in een tekst-bestand. Dit bestand (duplicaten.txt) kunt u vinden in de map van de applicatie.\n\n De andere items zijn wel toegevoegd.");
								tw.Close();
							}
							else
							{
								/*
								 *  misschien voor later
								 *  
								 *  - to add:
								 *    - check in de duplicaten.txt of die student er
								 *      al in staat, zo niet add hem. Zo wel, doe niks.
								 *      
								 *  voor de rest werkt alles al.
								 *  
								 * (C) Mark & Mark (H)
								 * 
								 */
								StreamWriter tw = File.AppendText("duplicaten.txt");
								foreach (Student duplicate in duplicates)
								{

									tw.WriteLine(duplicate.Studentnumber + "," + duplicate.Firstname + "" + duplicate.Surname + "," + duplicate.Email);
									tw.Flush();
								}
								MessageBox.Show("Er zijn " + duplicates.Count + " duplicaten gevonden. Deze zijn niet geimporteerd, maar opgeslagen in een tekst-bestand. Dit bestand (duplicaten.txt) kunt u vinden in de map van de applicatie. \n\n De andere items zijn wel toegevoegd.");
								tw.Close();

							}
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
        public bool? OpenNewSaveDialog(string defaultFileName, string defaultExtension, string filter, out string outFileName, bool appendDate = true)
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


			//Used for date validation
			DateTime dateValue;

			//Check blocked timeslot
			if (datePickerBlockedDay1.Text.Equals(String.Empty))
			{
				datePickerBlockedDay1.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else if (!DateTime.TryParse(datePickerBlockedDay1.Text, out dateValue))
			{
				datePickerBlockedDay1.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				datePickerBlockedDay1.BorderBrush = Brushes.Gray;
				hasInputError = false;
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

			int teacher_blocktype_selected = 1;

			if (teacher_blocktype_soft.IsChecked == true)
			{
				teacher_blocktype_selected = 0;
			}
			else if (teacher_blocktype_hard.IsChecked == true)
			{
				teacher_blocktype_selected = 1;
			}

			if (hasInputError == false)
			{
				//Create teacher object and add values
				Teacher newTeacher = new Teacher();
				newTeacher.Firstname = textBoxLeraarVoornaam.Text;
				newTeacher.Surname = textLeraarAchternaam.Text;
				newTeacher.Email = EmailLeraar1.Text;
				newTeacher.Session_spread = sessionSpread;
				newTeacher.blockedTimeslot = datePickerBlockedDay1.SelectedDate.Value;
				newTeacher.BlockType = teacher_blocktype_selected;

				//Send to the database
				_controller.TeacherMapper.Save(newTeacher);
				MessageBox.Show("Leraar toegevoegd");
				textBoxLeraarVoornaam.Text = "";
				textLeraarAchternaam.Text = "";
				EmailLeraar1.Text = "";
				datePickerBlockedDay1.Text = "";
				sessionVerspreid.IsChecked = false;
				sessionDichtBijElkaar.IsChecked = false;
				teacher_blocktype_soft.IsChecked = false;
				teacher_blocktype_hard.IsChecked = false;
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
				_controller.ClassroomMapper.Save(newClassroom);
				MessageBox.Show("Lokaal toegevoegd");
			}
		}
        private void buttonOptiesOpslaan_Click(object sender, RoutedEventArgs e)
        {
            _controller.IniReader["DATES"]["startdate"] = textBoxDeadlineStart.Text;
            _controller.IniReader["DATES"]["enddate"] = textBoxDeadlineEind.Text;

            bool isSaved = _controller.IniReader.Save();
            if(isSaved)
                MessageBox.Show("Uw instellingen zijn opgeslagen.");
            else
                MessageBox.Show("Er is iets mis gegaan met het opslaan.");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnOpenUnplannedWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.Windows.Count == 1)
            {
                UnPlannedPairs upp = new UnPlannedPairs();
                upp.loadAllPairs(_controller.PairMapper);
                upp.Show();
            }
            else
                MessageBox.Show("Het scherm met de nog niet ingeplande paren staat nog open.");
        }

		private void onBegeleiderAddClicked(object sender, RoutedEventArgs e)
		{
			//Use this for input errors
			bool hasInputError = false;


			//Check first name
			if (textBoxBegeleiderVoornaam.Text.Equals(String.Empty))
			{
				textBoxBegeleiderVoornaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderVoornaam.BorderBrush = Brushes.Gray;
			}

			//Check surname
			if (textBoxBegeleiderAchternaam.Text.Equals(String.Empty))
			{
				textBoxBegeleiderAchternaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderAchternaam.BorderBrush = Brushes.Gray;
			}

			//Check email adress
			if (textBoxBegeleiderEmail.Text.Equals(String.Empty))
			{
				textBoxBegeleiderEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderEmail.BorderBrush = Brushes.Gray;
			}
			if (!textBoxBegeleiderEmail.Text.IsValidEmailAddress())
			{
				textBoxBegeleiderEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderEmail.BorderBrush = Brushes.Gray;
			}

			//Check company
			if (textBoxBegeleiderBedrijf.Text.Equals(String.Empty))
			{
				textBoxBegeleiderBedrijf.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderBedrijf.BorderBrush = Brushes.Gray;
			}

			//Check Adres
			if (textBoxBegeleiderAdres.Text.Equals(String.Empty))
			{
				textBoxBegeleiderAdres.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderAdres.BorderBrush = Brushes.Gray;
			}

			//Check Postcode
			if (textBoxBegeleiderPostcode.Text.Equals(String.Empty))
			{
				textBoxBegeleiderPostcode.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderPostcode.BorderBrush = Brushes.Gray;
			}

			//Check telephone
			if (textBoxBegeleiderTelefoonnummer.Text.Equals(String.Empty))
			{
				textBoxBegeleiderTelefoonnummer.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderTelefoonnummer.BorderBrush = Brushes.Gray;
			}

			//Check city
			if (textBoxBegeleiderCity.Text.Equals(String.Empty))
			{
				textBoxBegeleiderCity.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxBegeleiderCity.BorderBrush = Brushes.Gray;
			}

			if (hasInputError == false)
			{
				//Create expert object and add values
				Expert newExpert = new Expert();
				newExpert.Firstname = textBoxBegeleiderVoornaam.Text;
				newExpert.Surname = textBoxBegeleiderAchternaam.Text;
				newExpert.Email = textBoxBegeleiderBedrijf.Text;
				newExpert.Company = textBoxBegeleiderBedrijf.Text;
				newExpert.Address = textBoxBegeleiderAdres.Text;
				newExpert.Postcode = textBoxBegeleiderPostcode.Text;
				newExpert.Telephone = textBoxBegeleiderTelefoonnummer.Text;
				newExpert.City = textBoxBegeleiderCity.Text;

				//Send to the database
				_controller.ExpertMapper.Save(newExpert);
				MessageBox.Show("Begeleider toegevoegd");
				textBoxBegeleiderVoornaam.Text = "";
				textBoxBegeleiderAchternaam.Text = "";
				textBoxBegeleiderEmail.Text = "";
				textBoxBegeleiderBedrijf.Text = "";
				textBoxBegeleiderAdres.Text = "";
				textBoxBegeleiderPostcode.Text = "";
				textBoxBegeleiderTelefoonnummer.Text = "";
				textBoxBegeleiderCity.Text = "";
			}
		}

		private void onExpertAddClicked(object sender, RoutedEventArgs e)
		{
			//Use this for input errors
			bool hasInputError = false;

			//Check first name
			if (textBoxExternVoornaam.Text.Equals(String.Empty))
			{
				textBoxExternVoornaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternVoornaam.BorderBrush = Brushes.Gray;
			}

			//Check surname
			if (textBoxExternAchternaam.Text.Equals(String.Empty))
			{
				textBoxExternAchternaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternAchternaam.BorderBrush = Brushes.Gray;
			}

			//Check email adress
			if (textBoxExternEmail.Text.Equals(String.Empty))
			{
				textBoxExternEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternEmail.BorderBrush = Brushes.Gray;
			}
			if (!textBoxExternEmail.Text.IsValidEmailAddress())
			{
				textBoxExternEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternEmail.BorderBrush = Brushes.Gray;
			}

			//Check company
			if (textBoxExternBedrijf.Text.Equals(String.Empty))
			{
				textBoxExternBedrijf.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternBedrijf.BorderBrush = Brushes.Gray;
			}

			//Check Adres
			if (textBoxExternAdres.Text.Equals(String.Empty))
			{
				textBoxExternAdres.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternAdres.BorderBrush = Brushes.Gray;
			}

			//Check Postcode
			if (textBoxExternPostcode.Text.Equals(String.Empty))
			{
				textBoxExternPostcode.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternPostcode.BorderBrush = Brushes.Gray;
			}

			//Check telephone
			if (textBoxExternTelefoonnummer.Text.Equals(String.Empty))
			{
				textBoxExternTelefoonnummer.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternTelefoonnummer.BorderBrush = Brushes.Gray;
			}

			//Check city
			if (textBoxExpertCity.Text.Equals(String.Empty))
			{
				textBoxExpertCity.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExpertCity.BorderBrush = Brushes.Gray;
			}

			if (hasInputError == false)
			{
				//Create expert object and add values
				Expert newExpert = new Expert();
				newExpert.Firstname = textBoxExternVoornaam.Text;
				newExpert.Surname = textBoxExternAchternaam.Text;
				newExpert.Email = textBoxExternEmail.Text;
				newExpert.Company = textBoxExternBedrijf.Text;
				newExpert.Address = textBoxExternAdres.Text;
				newExpert.Postcode = textBoxExternPostcode.Text;
				newExpert.Telephone = textBoxExternTelefoonnummer.Text;
				newExpert.City = textBoxExpertCity.Text;

				//Send to the database
				_controller.ExpertMapper.Save(newExpert);
				MessageBox.Show("Expert toegevoegd");
				textBoxExternVoornaam.Text = "";
				textBoxExternAchternaam.Text = "";
				textBoxExternEmail.Text = "";
				textBoxExternBedrijf.Text = "";
				textBoxExternAdres.Text = "";
				textBoxExternPostcode.Text = "";
				textBoxExternTelefoonnummer.Text = "";
				textBoxExpertCity.Text = "";
			}
		}

		private void stageBegeleiderComboBoxMouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void docentComboBoxMouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void onStudentAddClicked(object sender, RoutedEventArgs e)
		{
			//Use this for input errors
			bool hasInputError = false;

			//Check study
			if (textBoxStudy.Text.Equals(String.Empty))
			{
				textBoxStudy.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxStudy.BorderBrush = Brushes.Gray;
			}


			//Check student number
			if (textBoxStudentennummer.Text.Equals(String.Empty))
			{
				textBoxStudentennummer.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxStudentennummer.BorderBrush = Brushes.Gray;
			}

			//Check first name
			if (textBoxExternVoornaam.Text.Equals(String.Empty))
			{
				textBoxExternVoornaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternVoornaam.BorderBrush = Brushes.Gray;
			}

			//Check surname
			if (textBoxExternAchternaam.Text.Equals(String.Empty))
			{
				textBoxExternAchternaam.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternAchternaam.BorderBrush = Brushes.Gray;
			}

			//Check email adress
			if (textBoxExternEmail.Text.Equals(String.Empty))
			{
				textBoxExternEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternEmail.BorderBrush = Brushes.Gray;
			}
			if (!textBoxExternEmail.Text.IsValidEmailAddress())
			{
				textBoxExternEmail.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				textBoxExternEmail.BorderBrush = Brushes.Gray;
			}


			//Used for date validation
			DateTime dateValue;

			//Check blocked timeslot
			if (datePickerBlockedDay.Text.Equals(String.Empty))
			{
				datePickerBlockedDay.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else if (!DateTime.TryParse(datePickerBlockedDay.Text, out dateValue))
			{
				datePickerBlockedDay.BorderBrush = Brushes.Red;
				hasInputError = true;
			}
			else
			{
				datePickerBlockedDay.BorderBrush = Brushes.Gray;
				hasInputError = false;
			}

			if (hasInputError == false)
			{
				//Create expert object and add values
				Student newStudent = new Student();
				newStudent.Firstname = textBoxVoornaam.Text;
				newStudent.Surname = textBoxAchternaam.Text;
				newStudent.Email = EmailLeering1.Text;
				newStudent.Studentnumber = Convert.ToInt32(textBoxStudentennummer.Text);
				newStudent.Study = textBoxStudy.Text;
				//TODO: Blocked Timeslots


				//Send to the database
				_controller.StudentMapper.Save(newStudent);
				MessageBox.Show("Student toegevoegd");
				textBoxVoornaam.Text = "";
				textBoxAchternaam.Text = "";
				EmailLeering1.Text = "";
				textBoxStudentennummer.Text = "";
			}
		}

        private void buttonZittingenGenereren_Click(object sender, RoutedEventArgs e)
        {
            
            //@MarkM: Schermpje dat ie bezig is laten zien aub
            // editted by MarkM
            StartWork();
            //Planner planner = new Planner();
            //planner.Plan(_controller.PairMapper.FindAll());
            
            
        }

        private bool ZittingGen()
        {
            Planner planner = new Planner();
            planner.Plan(_controller.PairMapper.FindAll());
            return true;
        }


        /*
         * 
         * 
         * 
         */
        private bool succesfull;
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            succesfull = ZittingGen();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gridLoadingSreenGen.Visibility = Visibility.Hidden;
            GridActies.Cursor = Cursors.Arrow;

            if (succesfull)
            {

                MessageBox.Show("Zittingen zijn gegenereerd.", "Actie succesvol"); 

                /*
                 *  HIER IETS DOEN ALS HET SUCCESVOL IS
                 */
            }
        }

        private void StartWork()
        {
            gridLoadingSreenGen.Visibility = Visibility.Visible;
            GridActies.Cursor = Cursors.Wait;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }
        /*
         * 
         * 
         * 
         */



        private void GridOverzichtList_Loaded(object sender, RoutedEventArgs e)
        {
            GridOverzichtList.Columns[0].SortDirection = ListSortDirection.Ascending;
            GridOverzichtList.Columns[1].SortDirection = ListSortDirection.Ascending;
        }

        private void ScrollViewer_DragOver(object sender, DragEventArgs e)
        {
            Point position = e.GetPosition(this);
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (position.Y > 0 && position.Y < this.Height)
            {
                if (position.Y < 150)
                    scrollviewer.ScrollToVerticalOffset(scrollviewer.ContentVerticalOffset - 100);
                if (position.Y > this.Height - 150)
                    scrollviewer.ScrollToVerticalOffset(scrollviewer.ContentVerticalOffset + 100);
            } if (position.X > 0 && position.X < this.Width)
            {
                if (position.X < 100)
                    scrollviewer.ScrollToHorizontalOffset(scrollviewer.ContentHorizontalOffset - 20);
                if (position.X > this.Width - 100)
                    scrollviewer.ScrollToHorizontalOffset(scrollviewer.ContentHorizontalOffset + 20);
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
