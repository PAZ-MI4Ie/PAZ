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

        public SessionMapper SessionMapper { get; private set; }
        public UserMapper UserMapper { get; private set; }
        public ClassroomMapper ClassroomMapper { get; private set; }
        public StudentMapper StudentMapper { get; private set; }
        public TeacherMapper TeacherMapper { get; private set; }
        public ExpertMapper ExpertMapper { get; private set; }
        public PairMapper PairMapper { get; private set; }
        public EmailTemplateMapper EmailTemplateMapper { get; private set; }
        public LetterTemplateMapper LetterTemplateMapper { get; private set; }
        public TimeslotMapper TimeslotMapper { get; private set; }

        private MainWindow _mainWindow;

        public PAZController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            PDFexporter = new PDFExporter(mainWindow.GridOverzichtList);
            Emailer = new Emailer();
            IniReader = ReadIni();

            //TEST CODE:
            DB = new MysqlDb(IniReader["DATABASESETTINGS"]["db_host"], IniReader["DATABASESETTINGS"]["db_username"], IniReader["DATABASESETTINGS"]["db_password"], IniReader["DATABASESETTINGS"]["db_database"]);//Must be somewhere central

            SessionMapper = new SessionMapper(DB);
            UserMapper = new UserMapper(DB);
            ClassroomMapper = new ClassroomMapper(DB);
            StudentMapper = new StudentMapper(DB);
            TeacherMapper = new TeacherMapper(DB);
            ExpertMapper = new ExpertMapper(DB);
            PairMapper = new PairMapper(DB);
            EmailTemplateMapper = new EmailTemplateMapper(DB);
            LetterTemplateMapper = new LetterTemplateMapper(DB);
            TimeslotMapper = new TimeslotMapper(DB);
        }

        public void ExportRoosterClicked()
        {
            string fileName;
            if (_mainWindow.OpenNewSaveDialog("Roosteroverzicht PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                PDFexporter.CreateOverviewPDF(fileName);
            }
        }

        public void BriefMakenClicked(List<SessionRow> sessions)
        {
            LetterTemplate letterTemplate = LetterTemplateMapper.Find(1);

            LetterWindow letterWindow = new LetterWindow(sessions, letterTemplate, this);
            letterWindow.ShowDialog();
        }

        public void BriefMakenBevestigingClicked()
        {
            // dit zorgt ervoor dat er geen filters worden toegepast in de PDF uitdraai
            _mainWindow.textboxSearch.Text = "";

            string fileName;
            if (_mainWindow.OpenNewSaveDialog("Bevestigingsbrieven PAZ", ".pdf", "PDF (.pdf)|*.pdf", out fileName) == true)
            {
                // maak en exporteer als pdf
                PDFexporter.CreateLetterPDF(fileName);
            }
        }

        public void EmailVersturenClicked(List<SessionRow> sessions)
        {
            EmailTemplate emailTemplate = EmailTemplateMapper.Find(1);

            EmailWindow emailWindow = new EmailWindow(sessions, emailTemplate, this);
            emailWindow.ShowDialog();
        }

        public void EmailWindowClosed(EmailTemplate updatedTemplate)
        {
            EmailTemplateMapper.Save(updatedTemplate);
        }

        public void LetterWindowClosed(LetterTemplate updatedTemplate)
        {
            LetterTemplateMapper.Save(updatedTemplate);
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

            _mainWindow.textBoxDeadlineStart.Text = ini["DATES"]["startdate"];
            _mainWindow.textBoxDeadlineEind.Text = ini["DATES"]["enddate"];

            return ini;
        }
    }
}
