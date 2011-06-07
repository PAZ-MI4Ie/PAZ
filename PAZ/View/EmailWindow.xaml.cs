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
        private StudentMapper _studentMapper;
        private TeacherMapper _teacherMapper;
        private MysqlDb db;
        
        public EmailWindow()
        {
            InitializeComponent();

            _teachers = new List<Teacher>();
            _students = new List<Student>();
            _receivers = new List<User>();

            db = new MysqlDb("student.aii.avans.nl", "MI4Ie", "4DRcUrzV", "MI4Ie_db");

            //db = new MysqlDb("localhost", "root", "", "mi4ie_db");

            _studentMapper = new StudentMapper(db);
            _teacherMapper = new TeacherMapper(db);

            _teachers = _teacherMapper.FindAll();
            _students = _studentMapper.FindAll();

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
            Canvas canvasDocenten = new Canvas();
            int left = 0, top = 0;
            foreach (Teacher teacher in _teachers)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = teacher.Firstname + " " + teacher.Surname + " (" + teacher.Email + ")"; ;
                cb.Tag = teacher; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasDocenten.Children.Add(cb);

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
            Canvas canvasStudenten = new Canvas();
            int left = 0, top = 0;
            foreach (Student student in _students)
            {
                CheckBox cb = new CheckBox();
                cb.Checked += new RoutedEventHandler(cb_Checked);
                cb.Unchecked += new RoutedEventHandler(cb_Unchecked);
                cb.Content = student.Firstname + " " + student.Surname + " (" + student.Email + ")";
                cb.Tag = student; // koppelt object aan checkbox

                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                canvasStudenten.Children.Add(cb);

                top += 20;
            }
            scrollViewerStudenten.Content = canvasStudenten;
        }

        /**
        * Haalt de student of docent uit de _receivers lijst
        * 
        * Auteur: Gökhan 
        */
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // verwijder de gekoppelde object van de _receivers lijst
            _receivers.Remove((User)cb.Tag); 
        }

        /**
        * Voegt de geselecteerde studenten en docenten toe aan de _receivers lijst
        * 
        * Auteur: Gökhan 
        */
        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            // voeg de gekoppelde object toe aan de _receivers lijst
            _receivers.Add((User) cb.Tag); 
        }

        /**
        * Stuurt de e-mailberichten via de Gmail-server
        * 
        * Auteur: Gökhan 
        */
        private void bntVerzenden_Click(object sender, RoutedEventArgs e)
        {
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
                    emailer.To = "gokanovic@hotmail.com";   //(alleen bedoelt voor testdoeleinden)

                    // inhoud van e-mail
                    emailer.Body = "Beste " + user.Firstname + " " + user.Surname + ",<br /><br />";
                    emailer.Body += "<p>WAT KOMT HIER NOU EIGENLIJK TE STAAN?</p>";
                    emailer.Body += "Met vriendelijke groeten, <br /><br />";
                    emailer.Body += "Het PAZ-team";
   
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
    }
}
