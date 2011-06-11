using System.Collections.Generic;
using Ini;
using PAZ.View;

namespace PAZ.Control
{
    public class PAZController
    {
        public PDFExporter PDFexporter { get; private set; }
        public Emailer Emailer { get; private set; }
        public IniFile IniReader { get; private set; }

        private MainWindow _mainWindow;
        private EmailWindow _emailWindow;

        public PAZController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            PDFexporter = new PDFExporter(mainWindow.GridOverzichtList);
            Emailer = new Emailer();
            IniReader = readIni();
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

        public void BriefPrintenClicked()
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
            EmailWindow emailWindow = new EmailWindow(sessions, this);
            emailWindow.ShowDialog();

            _emailWindow = emailWindow;
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

                section = new IniSection();
                section.Add("afzender", "Avans Planner Systeem");
                section.Add("inleiding", "Hierbij ontvangt u de tijd(en) waarop u aanwezig moet zijn voor de afstudeerzitting(en)");
                section.Add("informatie", "In het afstudeerlokaal wordt voor aanvang van de zitting koffie en thee geserveerd.");
                section.Add("afsluiting", "Voor eventuele vragen kunt u zich wenden tot Lilian Reuken, telefoonnummer (073) 629 5256 of Regien Blom telefoonnummer (073) 629 54 55.");
                section.Add("afzenders", "Lilian Reuken en Regien Blom");
                ini.Add("EMAILBERICHT", section);

                ini.Save();
            }

            _mainWindow.textBoxDeadlineStart.Text = ini["DATES"]["startdate"];
            _mainWindow.textBoxDeadlineEind.Text = ini["DATES"]["enddate"];

            return ini;
        }
    }
}
