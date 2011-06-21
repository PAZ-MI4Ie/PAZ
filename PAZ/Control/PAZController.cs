using System.Collections.Generic;
using Ini;
using PAZ.Model;
using PAZ.Model.Mappers;
using PAZ.View;
using PAZMySQL;

namespace PAZ.Control
{
    public class PAZController
    {
        public PDFExporter PDFexporter { get; private set; }
        public Emailer Emailer { get; private set; }
        public IniFile IniReader { get; private set; }
        public MysqlDb DB { get; private set; }

        public BlockedTimeslotMapper BlockedTimeslotMapper { get; private set; }
        public SessionMapper SessionMapper { get; private set; }
        public UserMapper UserMapper { get; private set; }
        public ClassroomMapper ClassroomMapper { get; private set; }
        public StudentMapper StudentMapper { get; private set; }
        public DaytimeMapper DaytimeMapper { get; private set; }
        public TeacherMapper TeacherMapper { get; private set; }
        public ExpertMapper ExpertMapper { get; private set; }
        public PairMapper PairMapper { get; private set; }
        public EmailTemplateMapper EmailTemplateMapper { get; private set; }
        public LetterTemplateMapper LetterTemplateMapper { get; private set; }
        public TimeslotMapper TimeslotMapper { get; private set; }
        public UnPlannedPairs toPlanWindow { get; set; }

        public List<Timeslot> Timeslots { get; private set; }

        public MainWindow MainWindow { get; private set; }

        private static PAZController _controller;

        public static PAZController GetInstance()
        {
            if (PAZController._controller == null)
                PAZController._controller = new PAZController();

            return PAZController._controller;
        }

        public PAZController()
        {
            Emailer = new Emailer();
            IniReader = ReadIni();
        }

        public void Init(MainWindow mainWindow)
        {
            PDFexporter = new PDFExporter(mainWindow.GridOverzichtList);

            DB = MysqlDb.GetInstance();

            BlockedTimeslotMapper = new BlockedTimeslotMapper(DB);
            SessionMapper = new SessionMapper(DB);
            UserMapper = new UserMapper(DB);
            ClassroomMapper = new ClassroomMapper(DB);
            StudentMapper = new StudentMapper(DB);
            DaytimeMapper = new DaytimeMapper(DB);
            TeacherMapper = new TeacherMapper(DB);
            ExpertMapper = new ExpertMapper(DB);
            PairMapper = new PairMapper(DB);
            EmailTemplateMapper = new EmailTemplateMapper(DB);
            LetterTemplateMapper = new LetterTemplateMapper(DB);
            TimeslotMapper = new TimeslotMapper(DB);

            Timeslots = TimeslotMapper.FindAll();
            toPlanWindow = new UnPlannedPairs();

            MainWindow = mainWindow;
        }

        public void ExportRoosterClicked()
        {
            string fileName;
            if (MainWindow.OpenNewSaveDialog("Roosteroverzicht PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                PDFexporter.CreateOverviewPDF(fileName);
            }
        }

        public void BriefMakenClicked(List<SessionRow> sessions)
        {
            LetterTemplate letterTemplate = LetterTemplateMapper.Find(1);

            LetterWindow letterWindow = new LetterWindow(sessions, letterTemplate);
            letterWindow.ShowDialog();
        }

        public void BriefMakenBevestigingClicked(Dictionary<int, Expert> receivers, LetterTemplate letterTemplate)
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            MainWindow.textboxSearch.Text = "";

            string fileName;
            if (MainWindow.OpenNewSaveDialog("Bevestigingsbrieven PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                PDFexporter.CreateLetterPDF(fileName, receivers, letterTemplate);
            }
        }

        public void EmailVersturenClicked(List<SessionRow> sessions)
        {
            EmailTemplate emailTemplate = EmailTemplateMapper.Find(1);

            EmailWindow emailWindow = new EmailWindow(sessions, emailTemplate);
            emailWindow.ShowDialog();
        }

        public void EmailWindowSaveClicked(EmailTemplate updatedTemplate)
        {
            EmailTemplateMapper.Save(updatedTemplate);
        }

        public void LetterWindowSaveTemplateClicked(LetterTemplate updatedTemplate)
        {
            LetterTemplateMapper.Save(updatedTemplate);
        }

        public void VerwijderGegegevensClicked()
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.ShowDialog();
        }

        // TO DO: Voor teun, functionaliteit please :)
        public bool VerwijderGegevensBevestigingClicked(List<object> deleteList)
        {
            foreach (object dataItem in deleteList)
            {
                int id;

                if (dataItem is User)
                    id = ((User)dataItem).Id;
                else
                    id = ((Classroom)dataItem).Id;
            }

            return true;
        }

        public IniFile ReadIni()
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
                section.Add("email_user", "paz.planner@gmail.com");
                section.Add("email_password", "Paz.planner01");
                section.Add("email_from", "paz.planner@gmail.com");
                section.Add("email_host", "smtp.gmail.com");
                section.Add("email_port", "587");
                section.Add("email_onderwerp", "Afstudeerzitting(en)");
                ini.Add("EMAILSETTINGS", section);

                section = new IniSection();
                section.Add("db_host", "student.aii.avans.nl");
                section.Add("db_username", "MI4Ie");
                section.Add("db_password", "4DRcUrzV");
                section.Add("db_database", "MI4Ie_db");
                ini.Add("DATABASESETTINGS", section);

                ini.Save();
            }

            return ini;
        }
    }
}
