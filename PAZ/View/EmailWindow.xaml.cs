using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PAZMySQL;
using PAZ.Model;
using PAZ.View;

namespace PAZ
{
    /**
    * In deze klassen kun je de geadresseerden bepalen(docenten en studenten).
    * 
    * Auteur: Gökhan 
    */
    public partial class EmailWindow : Window
    {
        private List<Teacher> _teachers;
        private List<Student> _students;
        private List<User> _receivers;
        private List<SessionRow> _sessions;

        private List<CheckBox> _studentBoxes;
        private List<CheckBox> _teacherBoxes;

        public EmailWindow(List<SessionRow> sessions)
        {
            InitializeComponent();

            tbInleiding.Text += "Hierbij ontvangt u de tijd(en) waarop u aanwezig moet zijn voor de afstudeerzitting(en)";
            tbInformatie.Text += "In het afstudeerlokaal wordt voor aanvang van de zitting koffie en thee geserveerd.";
            tbAfsluiting.Text += "Voor eventuele vragen kunt u zich wenden tot Lilian Reuken, telefoonnummer (073) 629 5256 of Regien Blom telefoonnummer (073) 629 54 55.";
            tbAfzenders.Text += "Lilian Reuken en Regien Blom";

            _teachers = new List<Teacher>();
            _students = new List<Student>();
            _receivers = new List<User>();
            _sessions = sessions;

            foreach (SessionRow session in sessions)
            {
                Session sessionModel = session.GetSessionModel();

                Teacher[] teachers = sessionModel.GetTeachers();
                for (int i = 0; i < teachers.Length; ++i)
                    _teachers.Add(teachers[i]);

                _students.Add(sessionModel.Pair.Student1);
                _students.Add(sessionModel.Pair.Student2);
            }

            StudentenToevoegen();
            DocentenToevoegen();          
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
            }
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
            }
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
        * Stuurt de e-mailberichten via de Gmail-server
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
                Emailer emailer = new Emailer();            // maakt object emailer

                emailer.User = "paz.planner@gmail.com";     // het gmail-adres
                emailer.Password = "Paz.planner01";         // het gmail-wachtwoord            
                emailer.From = "paz.planner@gmail.com";     // de afzender = gmail-adres
                emailer.Host = "smtp.gmail.com";            // de gmail-server
                emailer.Port = 587;                         // het gmail-server poortnummer
                emailer.Subject = "PAZ";                    // het onderwerp van e-mailbericht
                
                foreach (User user in _receivers)
                {
                    // de geadresseerde 
                    //emailer.To = user.Email;  // let op: e-mailadressen in db zijn fake!
                    emailer.To = "ymja.kuijs@student.avans.nl";   //(alleen bedoelt voor testdoeleinden)

                    // inhoud van e-mail
                    emailer.Body += "<p>Beste " + user.Firstname + " " + user.Surname + ",</p>";

                    emailer.Body += "<p>";
                    emailer.Body += tbInleiding.Text;
                    emailer.Body += "</p>";

                    emailer.Body += "<p>De zittingen vinden plaats op: <br />";

                    for(int i = 0; i < _sessions.Count; ++i)
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
                            if (sessionUser == user)
                            {
                                emailer.Body += "Afstudeerzitting";
                                if (user is Teacher)
                                    emailer.Body += " " + (i + 1);

                                emailer.Body += " is gepland op, " + _sessions[i].Datum + " om " + sessionModel.Daytime.Time + ", in lokaal " + sessionModel.Classroom.Room_number + "<br />";
                            }
                        }
                    }

                    emailer.Body += "</p>";

                    emailer.Body += "<p>";
                    emailer.Body += tbInformatie.Text;
                    emailer.Body += "</p>";

                    emailer.Body += "<p>";
                    emailer.Body += tbAfsluiting.Text;
                    emailer.Body += "</p>";

                    emailer.Body += "<p>Met vriendelijke groet, </p>";

                    emailer.Body += "<p>";
                    emailer.Body += tbAfzenders.Text;
                    emailer.Body += "<br /><i>Coördinatoren stage en afstuderen</i></p>";

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
        * Sluit het huidige scherm
        * 
        * Auteur: Gökhan 
        */
        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /**
        * Update de verzendlijst wanneer er op de tabitem 'verzendlijst' gefocused wordt
        * 
        * Auteur: Gökhan 
        */
        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            listBoxReceivers.Items.Clear();
            lblAantalReceivers.Content = "Aantal: " + _receivers.Count;

            if (_receivers.Count == 0)
            {
                listBoxReceivers.Items.Add("Lijst is leeg");
                return;
            }
     
            foreach (User user in _receivers)
            {
                listBoxReceivers.Items.Add(user.Firstname + " " + user.Surname + "<" + user.Email + "> type: " + user.User_type);
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
    }
}
