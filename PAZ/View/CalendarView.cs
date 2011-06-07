using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.ComponentModel;
using PAZ.Model;

namespace PAZ.View
{
    public class CalendarView : Grid
    {
        Point startpoint;
        public Dictionary<string, Grid> dateGrids;
        public CalendarView()
            : base()
        {
            dateGrids = new Dictionary<string, Grid>();
        }



        void session_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string[] newsession = SessionWindow.AddNewSession();
        }

        #region Drag & Drop
        public void session_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Label session = sender as Label;
            if (session != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                string date = GetSessionDate(session);
                int column = GetColumn(session);
                int row = GetRow(session);
                DragDrop.DoDragDrop(session, date + ";" + column + ";" + row + ";\n" + session.Content, DragDropEffects.Move);
            }
        }

        void CalendarView_DragEnter(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void CalendarView_DragOver(object sender, DragEventArgs e)
        {
            //set effect to none is draging over header
        }

        void CalendarView_Drop(object sender, DragEventArgs e)
        {
            //check dates
            //check classroom
            //
            Label ellipse = sender as Label;
            if (ellipse != null)
            {
                // If the DataObject contains string data, extract it.
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                    // If the string can be converted into a Brush, 
                    // convert it and apply it to the ellipse.
                    if (dataString != null)
                    {
                        string[] other = dataString.Split(';');
                        string[] users = dataString.Split('\n');
                        if(removeSession(other[0],Convert.ToInt32(other[1]),Convert.ToInt32(other[2])))
                            addSession(GetSessionDate(ellipse), GetColumn(ellipse), GetRow(ellipse), users[1], users[2], new string[] { users[4], users[5] }, new string[] { users[7], users[8] });
                    }
                }
            }
        }

        void CalendarView_DragLeave(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion
        
        public void createCalendar(Ini.IniFile ini, List<Classroom> classrooms)
        {
            DateTime startDate = DateTime.Parse(ini["DATES"]["startdate"]);
            DateTime stopDate = DateTime.Parse(ini["DATES"]["enddate"]);
            Brush headerColor = Brushes.LightGray;
            int interval = 1;
            int columns = 0;
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
                        Children.Add(header);
                    }


                    //Making rows
                    for (int block = 1; block <= 4; block++)
                    {
                        row = new RowDefinition();
                        //blok 1 = row for headers(classrooms,date)
                        if (block == 1)
                            row.Height = new GridLength(60);
                        else
                            row.Height = height;
                        RowDefinitions.Add(row);
                        rows++;

                        Label blk = new Label();
                        blk.Content = ini["TIME"]["block" + block];
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
                            Label test = new Label();
                            Grid.SetColumn(test, i);
                            Grid.SetRow(test, j);
                            test.AllowDrop = true;
                            test.DragEnter += new DragEventHandler(CalendarView_DragEnter);
                            test.DragOver += new DragEventHandler(CalendarView_DragOver);
                            test.DragLeave += new DragEventHandler(CalendarView_DragLeave);
                            test.Drop += new DragEventHandler(CalendarView_Drop);
                            test.MouseDown += new System.Windows.Input.MouseButtonEventHandler(session_MouseDown);
                            dateGrid.Children.Add(test);
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

        public void loadAllSessions()//List<Session> sessions)
        {/*
            foreach (Session session in sessions)
            {
                int classroom = session.Classroom.Id;
                string student1 = session.Pair.Student1.Firstname + " " + session.Pair.Student1.Surname;
                string student2 = session.Pair.Student2.Firstname + " " + session.Pair.Student2.Surname;
                string[,] otherPeople = new string[4, 2];
                int i = 0;
                foreach(User user in session.Pair.Attachment)
                {
                    otherPeople[i,0] = user.Firstname + " " + user.Surname;
                    otherPeople[i,1] = user.Status;
                    i++;
                }
                string[] teachers = new string[2];
                string[] experts = new string[2];
                int numberOfExpert = 0;
                for (i = 0; i < 4; i++)
                {
                    if (otherPeople[i, 1] == "teacher")
                        teachers[i] = otherPeople[i, 0];
                    else
                    {
                        experts[numberOfExpert] = otherPeople[i, 0];
                    }
                }

                addSession(0, session.Daytime.Timeslot, student1, student2, teachers, experts);
            }*/
        }

        public void addSession(string date, int classroomId, int timeslot, string student1, string student2, string[] teachers, string[] experts)
        {
            Label session = new Label();
            session = dateGrids[date].Children[(classroomId) + ((timeslot) * 8)] as Label;
            session.Content = student1 + "\n" + student2 + "\n\n" + teachers[0] + "\n" + teachers[1] +"\n\n"+ experts[0] + "\n" + experts[1];
            session.BorderBrush = Brushes.LightGray;
            session.BorderThickness = new Thickness(2);
            session.MouseDown -= session_MouseDown;
            session.MouseMove += new System.Windows.Input.MouseEventHandler(session_MouseMove);
            session.AllowDrop = false;
            session.DragEnter -= CalendarView_DragEnter;
            session.DragOver -= CalendarView_DragOver;
            session.DragLeave -= CalendarView_DragLeave;
            session.Drop -= CalendarView_Drop;

            session.ToolTip = "Studenten\n\nDocenten\n\nExperts";
        }

        public Grid HasSession(Label session)
        {
            foreach (KeyValuePair<string, Grid> pair in dateGrids)
            {
                if (pair.Value.Children.Contains(session))
                    return pair.Value;
            }
            return null;
        }

        public string GetSessionDate(Label session)
        {
            foreach (KeyValuePair<string, Grid> pair in dateGrids)
            {
                if (pair.Value.Children.Contains(session))
                    return pair.Key;
            }
            return null;
        }

        public bool removeSession(Label session)
        {
            Grid parent = HasSession(session);
            if (parent != null)
            {
                parent.Children.Remove(session);
                return true;
            }
            return false;
        }

        public bool removeSession(string date, int column, int row)
        {
            Label session = dateGrids[date].Children[(column) + ((row) * 8)] as Label;
            if (session != null)
            {
                session.Content = null;
                session.ToolTip = null;
                session.BorderBrush = null;
                session.BorderThickness = new Thickness(0);
                session.MouseMove -= session_MouseMove;
                session.AllowDrop = true;
                session.DragEnter += new DragEventHandler(CalendarView_DragEnter);
                session.DragOver += new DragEventHandler(CalendarView_DragOver);
                session.DragLeave += new DragEventHandler(CalendarView_DragLeave);
                session.Drop += new DragEventHandler(CalendarView_Drop);
                return true;
            }
            return false;
        }
    }
}
