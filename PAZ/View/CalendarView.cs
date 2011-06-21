using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PAZ.Control;
using PAZ.Model;
using PAZMySQL;
using PAZ.Model.Mappers;
using System.Windows.Input;

namespace PAZ.View
{
    public class CalendarView : Grid
    {
        //TODO: database data schrijven + verwijderen
        public static Dictionary<string, Grid> dateGrids;
        public static PAZController _controller;
        public CalendarView()
            : base()
        {
            dateGrids = new Dictionary<string, Grid>();
        }

        #region Drag & Drop
        public void session_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CustomLabel session = sender as CustomLabel;
            if (session != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                string date = GetSessionDate(session);
                int column = GetColumn(session);
                int row = GetRow(session);
                CheckAvailability(session,true);
                DragDropEffects drag = DragDrop.DoDragDrop(session, session.Id + ";" + date + ";" + column + ";" + row + ";\n" + session.Content, DragDropEffects.Move);
                if (drag == DragDropEffects.None)
                    revertCheckAvailability();
            }
        }

        void Session_Drop(object sender, DragEventArgs e)
        {
            CustomLabel session = sender as CustomLabel;
            if (session != null && session.Background != Brushes.Red)
            {
                // If the DataObject contains string data, extract it.
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
                    if (dataString != null)
                    {
                        string[] other = dataString.Split(';');
                        Session s = _controller.SessionMapper.Find(Convert.ToInt32(other[0]));
                        Daytime d = _controller.DaytimeMapper.Find(GetSessionDate(session), Grid.GetRow(session));
                        if (d == null)
                        {
                            string[] date = other[1].Split('-');
                            d = new Daytime(0, new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])), Grid.GetRow(session) + 1);
                            _controller.DaytimeMapper.Save(d);
                        }
                        s.Daytime = d;
                        s.Classroom = _controller.ClassroomMapper.Find(Grid.GetColumn(session) + 1);
                        addSession(s, false);
                        removeSessionLabel(other[1], Convert.ToInt32(other[2]), Convert.ToInt32(other[3]));

                    }
                }
                else
                {
                    int id = (int)e.Data.GetData("System.Int32");
                    if (id != 0)
                    {
                        Session s = new Session();
                        Daytime d = _controller.DaytimeMapper.Find(GetSessionDate(session), Grid.GetRow(session));
                        if (d == null)
                        {
                            string[] date = GetSessionDate(session).Split('-');
                            d = new Daytime(0, new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])), Grid.GetRow(session) + 1);
                            _controller.DaytimeMapper.Save(d);
                        }
                        s.Daytime = d;
                        s.Classroom = _controller.ClassroomMapper.Find(Grid.GetColumn(session) + 1);
                        s.Pair = _controller.PairMapper.Find(Convert.ToInt32(id));
                        addSession(s, true);
                    }
                }
                revertCheckAvailability();
            }
        }
        
        public static void CheckAvailability(CustomLabel currentSession, bool isSession)
        {
            //getting all blocked timeslots of all users
            List<Blocked_timeslot> blockedSlots = new List<Blocked_timeslot>();
            if (isSession)
            {
                Session s = _controller.SessionMapper.Find(currentSession.Id);
                foreach (User u in s.Pair.Participants)
                {
                    foreach (Blocked_timeslot b in u.BlockedTimeslots)
                    {
                        blockedSlots.Add(b);
                    }
                }
            }
            else
            {
                Pair p = _controller.PairMapper.Find(currentSession.Id);
                foreach (User u in p.Participants)
                {
                    foreach (Blocked_timeslot b in u.BlockedTimeslots)
                    {
                        blockedSlots.Add(b);
                    }

                }
            }


            foreach (KeyValuePair<string, Grid> keyValue in dateGrids)
            {
                Grid dateGrid = keyValue.Value;

                for (int row = 1; row <= 4; row++)
                {
                    Brush background = null;

                    foreach (Blocked_timeslot b in blockedSlots)
                    {
                        if (b.Daytime.Date.ToShortDateString() == keyValue.Key && b.Daytime.Timeslot == row)
                        {
                            //A blocked timeslot found and its a hardblock
                            if (b.Hardblock == true)
                            {
                                background = Brushes.Red;
                            }
                            //A blocked timeslot found and its not a hardblock
                            else
                            {
                                background = Brushes.Orange;
                            }

                            blockedSlots.Remove(b);
                        }
                        //if there is a blocked timslot, stop looking
                        if (background != null)
                            break;
                    }
                    

                    //not blocked timeslots found, so all are available
                    if (background == null)
                        background = Brushes.LightGreen;

                    //set the color for the timeslot
                    for (int i = 0; i < 8; i++)
                    {
                        Label session = dateGrid.Children[i + ((row - 1) * 8)] as Label;
                        if (session != currentSession)
                        {

                            session.Background = background;
                        }
                    }
                }
            }
        }

        public static void revertCheckAvailability()
        {
            foreach (KeyValuePair<string, Grid> keyValue in dateGrids)
            {
                Grid dateGrid = keyValue.Value;

                for (int row = 0; row < 4; row++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Label session = dateGrid.Children[i + (row * 8)] as Label;
                        session.Background = Brushes.White;
                    }
                }
            }
        }
        #endregion
        
        public void createCalendar(Ini.IniFile ini, List<Classroom> classrooms, PAZController controller)
        {
            _controller = controller;
            DateTime startDate = DateTime.Parse(ini["DATES"]["startdate"]);
            DateTime stopDate = DateTime.Parse(ini["DATES"]["enddate"]);
            Brush headerColor = Brushes.LightGray;
            int interval = 1;
            int rows = 0;

            //Firste column color rec
            Rectangle recC = new Rectangle();
            recC.Fill = headerColor;
            Grid.SetColumn(recC, 0);
            Grid.SetRow(recC, 0);
            Children.Add(recC);

            //Defining rowdef & row height
            RowDefinition row = new RowDefinition();
            GridLength height = new GridLength(140);
            
            for (DateTime dateTime = startDate; dateTime <= stopDate; dateTime += TimeSpan.FromDays(interval))
            {
                if (dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday)
                {
                    //Adding color
                    Rectangle rec = new Rectangle();
                    rec.Fill = headerColor;
                    Grid.SetColumn(rec, 0);
                    Grid.SetRow(rec, rows);
                    Grid.SetColumnSpan(rec, classrooms.Count + 1);
                    Children.Add(rec);
                    //Add labels
                    for (int c = 0; c < classrooms.Count + 1; c++)
                    {
                        if (ColumnDefinitions.Count != classrooms.Count + 1)
                        {
                            //making columns
                            ColumnDefinition column = new ColumnDefinition();
                            GridLength width;
                            if (c == 0)
                                width = new GridLength(75);
                            else
                                width = new GridLength(120);
                            column.Width = width;
                            ColumnDefinitions.Add(column);
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
                        Children.Add(header);
                    }

                    List<Timeslot> timeslots = controller.Timeslots;

                    //Making rows
                    for (int block = 0; block < timeslots.Count ; ++block)
                    {
                        row = new RowDefinition();
                        //blok 0 = row for headers(classrooms,date)
                        if (block == 0)
                            row.Height = new GridLength(60);
                        else
                            row.Height = height;
                        RowDefinitions.Add(row);
                        rows++;

                        Label blk = new Label();
                        blk.Content = timeslots[block].Time;
                        blk.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        blk.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        Grid.SetColumn(blk, 0);
                        Grid.SetRow(blk, rows);
                        Children.Add(blk);
                    }


                    //Maken dateGrids
                    Grid dateGrid = new Grid();
                    for (int i = 0; i < ColumnDefinitions.Count - 1; i++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        GridLength width = new GridLength(120);
                        column.Width = width;
                        dateGrid.ColumnDefinitions.Add(column);
                    } 
                    for (int i = 0; i < 4; i++)
                    {
                        row = new RowDefinition();
                        row.Height = height;
                        dateGrid.RowDefinitions.Add(row);
                    }

                    for (int j = 0; j < 4; j++) 
                    {
                        for (int i = 0; i < dateGrid.ColumnDefinitions.Count; i++)
                        {
                            CustomLabel emptySession = new CustomLabel();
                            Grid.SetColumn(emptySession, i);
                            Grid.SetRow(emptySession, j);
                            emptySession.AllowDrop = true;
                            emptySession.Drop += new DragEventHandler(Session_Drop);
                            dateGrid.Children.Add(emptySession);
                        }
                    }
                    Grid.SetColumn(dateGrid, 1);
                    Grid.SetColumnSpan(dateGrid, dateGrid.ColumnDefinitions.Count);
                    Grid.SetRow(dateGrid, rows - 3);
                    Grid.SetRowSpan(dateGrid, dateGrid.RowDefinitions.Count);
                    dateGrid.ShowGridLines = true;
                    dateGrids.Add(dateTime.ToShortDateString(), dateGrid);
                    Children.Add(dateGrid);

                    //row block 4
                    row = new RowDefinition();
                    row.Height = height;
                    RowDefinitions.Add(row);
                    rows++;
                }
            }
            //set first column color over all rows
            Grid.SetRowSpan(recC, rows);
        }

        public void loadAllSessions(List<Session> sessions)
        {
            foreach (Session session in sessions)
            {
                addSessionLabel(session);
            }
        }

        public void updateCalendar()
        {
            // TODO height voor groter dan de kalender, fix it.
            dateGrids.Clear();
            Children.Clear();
            createCalendar(_controller.IniReader, _controller.ClassroomMapper.FindAll(), _controller);
            loadAllSessions(_controller.SessionMapper.FindAll());
        }
        
        void edit_Click(object sender, RoutedEventArgs e)
        {
            //Get the label
            CustomLabel session = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as CustomLabel;
            Session s = _controller.SessionMapper.Find(session.Id);
            KoppelWindow kw = new KoppelWindow(s.Pair.ID);
            if (kw.ShowDialog())
            {
                _controller.MainWindow.updateOverzicht();
                updateCalendar();
            }
        }

        void delete_Click(object sender, RoutedEventArgs e)
        {
            //Get the label
            CustomLabel session = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as CustomLabel;
            removeSession(session);
            _controller.toPlanWindow.loadAllPairs(_controller.PairMapper);
            _controller.MainWindow.updateOverzicht();
            MessageBox.Show("De zitting is succesvol verwijderd");
            
        }

        public void addSession(Session session,bool newSession)
        {
            int id = _controller.SessionMapper.Save(session);
            if (newSession)
            {
                addSessionLabel(session, id);
                _controller.toPlanWindow.loadAllPairs(_controller.PairMapper);
            }
            else
                addSessionLabel(session);
            _controller.MainWindow.updateOverzicht();
        }

        public CustomLabel addSessionLabel(Session session)
        {
            CustomLabel sessionLabel = dateGrids[session.Daytime.Date.ToShortDateString()].Children[(session.Classroom.Id - 1) + ((session.Daytime.Timeslot - 1) * 8)] as CustomLabel;
            sessionLabel.Id = session.Id;
            string[] students = new string[1];
            string[] teachers = new string[1];
            string[] experts = new string[1];
            foreach (User user in session.Pair.Participants)
            {
                string name = user.Firstname + " " + user.Surname + ",";
                if (user.User_type == "teacher")
                    teachers[0] += name;
                else if (user.User_type == "student")
                    students[0] += name;
                else
                    experts[0] += name;
            }

            students = students[0].Split(',');
            if (experts[0] != null)
                experts = experts[0].Split(',');
            else
                experts = new string[]{"GEEN EXPERTS",""};

            if (teachers[0] != null)
                teachers = teachers[0].Split(',');
            else
                teachers = new string[] { "GEEN DOCENTEN", "" };

            sessionLabel.Content = students[0] + "\n" + students[1] + "\n\n" + teachers[0] + "\n" + teachers[1] + "\n\n" + experts[0] + "\n" + experts[1];
            sessionLabel.ToolTip = "Studenten\n\nDocenten\n\nExperts";
            sessionLabel.BorderBrush = Brushes.LightGray;
            sessionLabel.BorderThickness = new Thickness(2);
            sessionLabel.MouseMove += new System.Windows.Input.MouseEventHandler(session_MouseMove);
            sessionLabel.AllowDrop = false;
            sessionLabel.Drop -= Session_Drop;
            sessionLabel.Background = Brushes.White;

            //Context Menu
            System.Windows.Controls.ContextMenu editMenu = new ContextMenu();
            //edit menu
            MenuItem edit = new MenuItem();
            edit.Header = "Wijzigen";
            edit.Click += new RoutedEventHandler(edit_Click);
            editMenu.Items.Add(edit);
            //delete menu
            MenuItem delete = new MenuItem();
            delete.Header = "Verwijderen";
            delete.Click += new RoutedEventHandler(delete_Click);
            editMenu.Items.Add(delete);
            sessionLabel.ContextMenu = editMenu;
            return sessionLabel;
        }

        public void addSessionLabel(Session session, int newId)
        {
            CustomLabel s = addSessionLabel(session);
            s.Id = newId;
        }

        public Grid HasSession(CustomLabel session)
        {
            foreach (KeyValuePair<string, Grid> pair in dateGrids)
            {
                if (pair.Value.Children.Contains(session))
                    return pair.Value;
            }
            return null;
        }

        public string GetSessionDate(CustomLabel session)
        {
            foreach (KeyValuePair<string, Grid> pair in dateGrids)
            {
                if (pair.Value.Children.Contains(session))
                    return pair.Key;
            }
            return null;
        }

        public void removeSession(CustomLabel session)
        {
            _controller.SessionMapper.Delete(session.Id);
            removeSessionLabel(session);

        }

        public bool removeSessionLabel(string date, int column, int row)
        {
            CustomLabel session = dateGrids[date].Children[(column) + ((row) * 8)] as CustomLabel;
            if (session != null)
            {
                session.Id = 0;
                session.Content = null;
                session.ToolTip = null;
                session.BorderBrush = null;
                session.BorderThickness = new Thickness(0);
                session.MouseMove -= session_MouseMove;
                session.ContextMenu = null;
                session.AllowDrop = true;
                session.Drop += new DragEventHandler(Session_Drop);
                return true;
            }
            return false;
        }

        public void removeSessionLabel(CustomLabel session)
        {
            session.Id = 0;
            session.Content = null;
            session.ToolTip = null;
            session.BorderBrush = null;
            session.BorderThickness = new Thickness(0);
            session.MouseMove -= session_MouseMove;
            session.ContextMenu = null;
            session.AllowDrop = true;
            session.Drop += new DragEventHandler(Session_Drop);

        }
    }
}
