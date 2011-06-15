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
    public partial class EmailWindow : Window
    {
        private List<Teacher> _teachers;
        private List<Student> _students;
        private List<User> _receivers;
        private List<SessionRow> _sessions;

        private List<CheckBox> _studentBoxes;
        private List<CheckBox> _teacherBoxes;

        private string _selectedReceiverEmail = string.Empty;

        private IniFile _ini;
        private PAZController _controller;
        private EmailTemplate _emailTemplate;

        public EmailWindow(List<SessionRow> sessions, EmailTemplate emailTemplate)
        {
            InitializeComponent();

            tbAfzender.Text = emailTemplate.Displayname;
            tbInleiding.Text += emailTemplate.Inleiding;
            tbInformatie.Text += emailTemplate.Informatie;
            tbAfsluiting.Text += emailTemplate.Afsluiting;
            tbAfzenders.Text += emailTemplate.Afzenders;

            _teachers = new List<Teacher>();
            _students = new List<Student>();
            _receivers = new List<User>();
            _sessions = sessions;

            foreach (SessionRow session in sessions)
            {
                Session sessionModel = session.GetSessionModel();

                Teacher[] teachers = sessionModel.GetTeachers();
                for (int i = 0; i < teachers.Length; ++i)
                    if(teachers[i] != null)
                        _teachers.Add(teachers[i]);

                _students.Add(sessionModel.Pair.Student1);
                _students.Add(sessionModel.Pair.Student2);
            }

            StudentenToevoegen();
            DocentenToevoegen();

            _controller = PAZController.GetInstance();
            _ini = _controller.IniReader;
            _emailTemplate = emailTemplate;

            btnSave.IsEnabled = false;
        }


        /**
        * Voegt docenten toe en weergeeft als checkboxen
        * 
        * Auteur: Gökhan 
        */
        private void DocentenToevoegen()
        {
            _teacherBoxes = new List<CheckBox>();
            Canvas canvasDocenten = new Canvas();

            int left = 0, top = 0;
            double rightMost = 0.0;
            foreach (Teacher teacher in _teachers)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = " " + teacher.Firstname + " " + teacher.Surname + " (" + teacher.Email + ")"; ;
                cb.Tag = teacher; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasDocenten.Children.Add(cb);

                _teacherBoxes.Add(cb);

                top += 20;

                double newRightMost = left + cb.Width;
                if (rightMost < newRightMost)
                    rightMost = newRightMost;
            }
            canvasDocenten.Width = rightMost;
            canvasDocenten.Height = top;

            canvasDocenten.HorizontalAlignment = HorizontalAlignment.Left;
            canvasDocenten.VerticalAlignment = VerticalAlignment.Top;

            scrollViewerDocenten.Content = canvasDocenten;
        }

       
        /**
        * Voegt studenten toe en weergeeft als checkboxen
        * 
        * Auteur: Gökhan 
        */
        private void StudentenToevoegen()
        {
            _studentBoxes = new List<CheckBox>();
            Canvas canvasStudenten = new Canvas();

            int left = 0, top = 0;
            double rightMost = 0.0;
            foreach (Student student in _students)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = " " + student.Firstname + " " + student.Surname + " (" + student.Email + ")";
                cb.Tag = student; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasStudenten.Children.Add(cb);

                _studentBoxes.Add(cb);

                top += 20;
            
                double newRightMost = left + cb.Width;
                if (rightMost < newRightMost)
                    rightMost = newRightMost;
            }
            canvasStudenten.Width = rightMost;
            canvasStudenten.Height = top;

            canvasStudenten.HorizontalAlignment = HorizontalAlignment.Left;
            canvasStudenten.VerticalAlignment = VerticalAlignment.Top;

            scrollViewerStudenten.Content = canvasStudenten;
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

            if (cb.Tag is Student)
                HandleSelectorUnchecking(cb, cbxStudentSelector, _studentBoxes);
            else
                HandleSelectorUnchecking(cb, cbxTeacherSelector, _teacherBoxes);
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

            bool isStudent = cb.Tag is Student;
            if (CheckAllChecked(isStudent ? _studentBoxes : _teacherBoxes))
            {
                if (isStudent)
                    cbxStudentSelector.IsChecked = true;
                else
                    cbxTeacherSelector.IsChecked = true;
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
        * Stuurt e-mailberichten via een mail server
        * 
        * Auteur: Gökhan en Yorg
        */
        private void bntVerzenden_Click(object sender, RoutedEventArgs e)
        {
            if (_receivers.Count == 0)
            {
                MessageBox.Show("U heeft geen geadresseerden geselecteerd, selecteer op zijn minst een persoon uit de lijst voor u probeert te verzenden.", "Opmerking", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MessageBox.Show("Weet u zeker dat u e-mailberichten wilt versturen?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Emailer emailer = _controller.Emailer;

                emailer.User = _ini["EMAILSETTINGS"]["email_user"];                     // het gebruikte account voor het versturen van de e-mails
                emailer.Password = _ini["EMAILSETTINGS"]["email_password"];             // het wachtwoord behorende bij bovenstaande account          
                emailer.From = _ini["EMAILSETTINGS"]["email_from"];                     // de afzender
                emailer.DisplayName = tbAfzender.Text;                                  // de naam van de afzender zoals die getoond wordt
                emailer.Host = _ini["EMAILSETTINGS"]["email_host"];                     // de server host
                emailer.Port = Convert.ToInt32(_ini["EMAILSETTINGS"]["email_port"]);    // het gebruikte poort nummer
                emailer.Subject = _ini["EMAILSETTINGS"]["email_onderwerp"];             // het onderwerp van het e-mailbericht
                
                foreach (User user in _receivers)
                {
                    // de geadresseerde 
                    //emailer.To = user.Email;  // let op: e-mailadressen in db zijn fake!
                    emailer.To = "ymja.kuijs@student.avans.nl";   //(alleen bedoelt voor testdoeleinden)

                    emailer.Body = CreateEmailBody(user);

                    try
                    {
                        emailer.SendEmail();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex.ToString());
                        return;
                    }                            
                }
                MessageBox.Show("E-mailberichten zijn verzonden.", "Succesvol");
            }          
        }

        /**
         * Het doel van deze functie is om een email body te maken aan de hand van een receiver
         * Input: receiver de ontvanger waarvoor de email gemaakt wordt
         * Return: de gehele body
         * Auteur: Gökhan en Yorg 
         */
        private string CreateEmailBody(User receiver)
        {
            string emailBody = "<html> \n <head> \n <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'> \n </head> \n <body> \n";
                
            // Inhoud van de brief
            emailBody += "<p>Beste " + receiver.Firstname + " " + receiver.Surname + ",</p>"; ;

            int zittingNummer = 0;
            for (int i = 0; i < _sessions.Count; ++i)
            {
                Session sessionModel = _sessions[i].GetSessionModel();

                List<User> users = new List<User>();
                Teacher[] teachers = sessionModel.GetTeachers();
                for (int j = 0; j < teachers.Length; ++j)
                    users.Add(teachers[j]);

                users.Add(sessionModel.Pair.Student1);
                users.Add(sessionModel.Pair.Student2);

                foreach (User sessionUser in users)
                {
                    if (sessionUser == receiver)
                    {
                        if(receiver is Student)
                            emailBody += "Je Afstudeerzitting";
                        else if (receiver is Teacher)
                        {
                            emailBody += "<p>";
                            emailBody += tbInleiding.Text;
                            emailBody += "</p>";

                            emailBody += "<p>U neemt deel aan de volgende zittingen: <br /> Zitting " + (++zittingNummer);
                        }

                        emailBody += " is gepland op, " + _sessions[i].Datum + " om " + sessionModel.Daytime.Starttime + ", in lokaal " + sessionModel.Classroom.Room_number + "<br />";
                    }
                }
            }

            emailBody += "</p>";

            emailBody += "<p>";
            emailBody += tbInformatie.Text;
            emailBody += "</p>";

            emailBody += "<p>";
            emailBody += tbAfsluiting.Text;
            emailBody += "</p>";

            emailBody += "<p>Met vriendelijke groet, </p>";

            emailBody += "<p>";
            emailBody += tbAfzenders.Text;
            emailBody += "<br /><i>Coördinatoren stage en afstuderen</i></p>";

            emailBody += "</body> \n </html>";

            return emailBody;
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

        private void tiPreview_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is TabItem))
                return;

            HandleLists();
        }

        private void tiPreview_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(e.Source is TabItem))
                return;

            HandleLists();
        }

        /**
        * Update de verzendlijsten wanneer er op de tabitem 'E-mail voorbeeld' geklikt wordt 
        * Auteur: Gökhan en Yorg
        */
        private void HandleLists()
        {
            listBoxStudentReceivers.Items.Clear();
            listBoxTeacherReceivers.Items.Clear();
            lblAantalReceivers.Content = "Totaal aantal geadresseerden: " + _receivers.Count;

            bool studentsEmpty = true;
            bool teachersEmpty = true;

            // Controleer of er students of teachers in de receivers lijst zitten, afhankelijk hiervan moeten de apparte geadresseerden lijsten hun status aangeven.
            foreach (User user in _receivers)
            {
                if (user is Student)
                    studentsEmpty = false;
                else
                    teachersEmpty = false;
            }

            if (studentsEmpty)
            {
                listBoxStudentReceivers.Items.Add("Lijst is leeg");
                listBoxTeacherReceivers.SelectedIndex = 0;
            }
            else
                listBoxStudentReceivers.SelectedIndex = 0;

            if (teachersEmpty)
                listBoxTeacherReceivers.Items.Add("Lijst is leeg");

            // Als de gehele receivers lijst leeg is dan hier stoppen
            if (studentsEmpty && teachersEmpty)
                return;

            ListBox listBox = null;
            foreach (User user in _receivers)
            {
                if (user is Student)
                    listBox = listBoxStudentReceivers;
                else
                    listBox = listBoxTeacherReceivers;

                listBox.Items.Add(user.Email);
            }
        }

        private void cbxStudentSelector_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(_studentBoxes);
        }

        private void cbxStudentSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUncheck(_studentBoxes);
        }

        private void cbxTeacherSelector_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(_teacherBoxes);
        }

        private void cbxTeacherSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUncheck(_teacherBoxes);
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

        private void listBoxStudentReceivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listBoxStudentReceivers.SelectedIndex;

            if (index < 0)
                return;

            // Deselecteer de docenten als er een student geselecteerd wordt
            listBoxTeacherReceivers.SelectedIndex = -1;

            _selectedReceiverEmail = listBoxStudentReceivers.Items[index].ToString();
            UpdatePreview();
        }

        private void listBoxTeacherReceivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listBoxTeacherReceivers.SelectedIndex;

            if (index < 0)
                return;

            // Deselecteer de studenten als er een docent geselecteerd wordt
            listBoxStudentReceivers.SelectedIndex = -1;

            _selectedReceiverEmail = listBoxTeacherReceivers.Items[index].ToString();
            UpdatePreview();
        }

        /** Update het preview scherm om het voorbeeld te laten zien voor de gekozen persoon
         * Auteur: Gökhan en Yorg 
         */
        private void UpdatePreview()
        {
            if (_receivers.Count == 0)
                return;

            User selectedReceiver = _receivers[0];
            foreach (User user in _receivers)
            {
                if (user.Email == _selectedReceiverEmail)
                {
                    selectedReceiver = user;
                    break;
                }
            }

            wbPreview.NavigateToString(CreateEmailBody(selectedReceiver));
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

            btnSave.IsEnabled = false;
        }

        private void Save()
        {
            _controller.EmailWindowSaveClicked(new EmailTemplate(_emailTemplate.Id, tbAfzender.Text, tbInleiding.Text, tbInformatie.Text, tbAfsluiting.Text, tbAfzenders.Text));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = true;
        }
    }
}
