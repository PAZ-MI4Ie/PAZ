using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PAZ.Model;
using PAZ.View;
using iText = iTextSharp.text;

namespace PAZ.Control
{
    public class PDFExporter
    {
        private const int CONTACT_INFORMATION_NUM_COLUMNS = 4;
        private const int FOOTER_NUM_COLUMNS = 3;

        private const string STANDARD_FONT_FAMILY = "Verdana";
        private const int STANDARD_FONT_SIZE = 9;

        private const string FOOTER_FONT_FAMILY = "Verdana";
        private const int FOOTER_FONT_SIZE = 6;

        private DataGrid _dataGrid;

        public PDFExporter(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        /**
         * Dit maakt de opzet voor het overzicht PDF, het werkelijke rooster wordt gemaakt in een aparte functie.
         * Auteur: Gökhan en Yorg 
         */
        public void CreateOverviewPDF(String filename)
        {
            // het document(standaard A4-formaat) maken en in landscape mode zetten
            iTextSharp.text.Document document = new iText.Document(PageSize.A4.Rotate());

            try
            {
                // De writer maken die naar het document luistert en zet de stream om in een PDF-bestand
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));

                // het document openen
                document.Open();

                // Een titel maken
                iText.Paragraph titel = new iText.Paragraph("Het PAZ-rooster", FontFactory.GetFont("Arial", 26, Font.BOLDITALIC));
                titel.Alignment = 1; // titel centeren

                // elementen toevoegen aan het document
                document.Add(titel); // de titel toevoegen
                document.Add(new iText.Paragraph(" ")); // leegruimte toevoegen
                document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen

                PdfPTable rosterTable = MakeRoster();
                if (rosterTable == null)
                {
                    MessageBox.Show("Exporteren is mislukt, het overzicht bevat geen records!", "Melding");
                    return;
                }

                document.Add(rosterTable); // het rooster

                // toon bericht dat exporteren naar PDF gelukt is
                MessageBox.Show("Exporteren gelukt! Bestand is geëxporteerd naar " + filename, "Melding");

                // open het PDF-bestand
                System.Diagnostics.Process.Start(filename);
            }
            catch (PdfException ex)
            {
                // toon bericht als er iets fout gaat
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // het document sluiten
                document.Close();
            }            
        }

        /**
         * Maakt het rooster in een tabel vorm
         * Return: De gemaakte tabel
         * Auteur: Gökhan en Yorg
        */
        private PdfPTable MakeRoster()
        {
            if (_dataGrid.Items.Count <= 0)
                return null;

            int aantalKolommen = ((SessionRow)_dataGrid.Items[0]).GetDataList().Count;

            // Maak een tabel met even veel aantal kolommen als de datagrid
            PdfPTable rosterTable = new PdfPTable(aantalKolommen);

            // bepaal de breedte voor elke kolom in volgorde
            float[] columnWidths = new float[aantalKolommen];
            for (int columnNo = 0; columnNo < aantalKolommen; ++columnNo)
                columnWidths[columnNo] = (float) _dataGrid.Columns[columnNo].Width.Value;

            rosterTable.SetWidths(columnWidths);

            // breedte van tabel instellen
            rosterTable.WidthPercentage = 100;

            // Leest de kolomnamen uit de datagrid en voegt toe aan roosterTable
            for (int columnNo = 0; columnNo < aantalKolommen; ++columnNo)
            {
                string columNaam = _dataGrid.Columns[columnNo].Header.ToString();

                Phrase ph = new Phrase(columNaam, FontFactory.GetFont("Arial", 13, Font.BOLD));
                PdfPCell cell = new PdfPCell(ph);
                cell.Padding = 5;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                rosterTable.AddCell(cell);
            }

            // Leest de rijen uit de datagrid en voegt deze toe aan roosterTable
            for (int rowNo = 0; rowNo < _dataGrid.Items.Count; ++rowNo)
            {
                SessionRow rowSession = (SessionRow)_dataGrid.Items[rowNo];

                for (int columnNo = 0; columnNo < _dataGrid.Columns.Count; ++columnNo)
                {
                    Phrase ph = new Phrase(rowSession.GetDataList()[columnNo].ToString(), FontFactory.GetFont("Arial", 13, Font.NORMAL));
                    PdfPCell cell = new PdfPCell(ph);
                    cell.Padding = 5;
                    rosterTable.AddCell(cell);
                }
            }

            return rosterTable;
        }

        /**
         * Dit maakt de brieven om te versturen naar de experts in zijn geheel en zet ze in een PDF document
         * Auteur: Yorg 
         */
        public void CreateLetterPDF(String filename, Dictionary<int, Expert> receivers, LetterTemplate letterTemplate)
        {
            // het document(standaard A4-formaat) maken
            iTextSharp.text.Document document = new iText.Document(PageSize.A4, 75.0f, 75.0f, 0.0f, 0.0f);

            try
            {
                // Creeër een PDF pagina instantie(deze regelt de footer, meer niet)
                pdfPage page = new pdfPage(letterTemplate);  

                // De writer maken die naar het document luistert en zet de stream om in een PDF-bestand
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));

                // Zet de PageEvent van de writer klasse naar de instantie van de pagina
                writer.PageEvent = page;

                // het document openen
                document.Open();
                for (int rowNo = 0; rowNo < _dataGrid.Items.Count; ++rowNo)
                {
                    SessionRow rowSession = (SessionRow)_dataGrid.Items[rowNo];
                    Session sessionModel = rowSession.GetSessionModel();

                    Dictionary<int, Expert> experts = sessionModel.Experts;

                    foreach(KeyValuePair<int, Expert> expertKeyValuePair in experts)
                    {
                        if (!receivers.ContainsKey(expertKeyValuePair.Key))
                            continue;

                        Expert expert = expertKeyValuePair.Value;

                        document.NewPage();

                        // Een titel maken
                        iText.Paragraph titel = new iText.Paragraph("Academie voor " + sessionModel.Pair.Student1.Study, FontFactory.GetFont("Arial", 12, Font.BOLD));
                        titel.Alignment = 1;

                        // Subtitel maken
                        iText.Paragraph subTitel = new iText.Paragraph(letterTemplate.AvansLocatie, FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 1));
                        subTitel.Alignment = 1;

                        // elementen toevoegen aan het document
                        document.Add(titel); // de titel toevoegen
                        document.Add(subTitel); // de subtitel toevoegen

                        // Leeg ruimte toevoegen
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));

                        // Bepaal het standaard font om te gebruiken in het grootste deel van het document
                        Font standardFont = FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE);

                        // Adressering
                        document.Add(new iText.Paragraph(expert.Company, standardFont));
                        document.Add(new iText.Paragraph(expert.Firstname + " " + expert.Surname, standardFont));
                        document.Add(new iText.Paragraph(expert.Address, standardFont));
                        document.Add(new iText.Paragraph(expert.Postcode + " " +  expert.City, standardFont));

                        // Leeg ruimte toevoegen
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));

                        // Contact informatie tabel
                        document.Add(MakeContactInformationTable(letterTemplate));

                        // Leeg ruimte toevoegen
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));
                        document.Add(new iText.Paragraph(" "));

                        // Inhoud brief
                        document.Add(new iText.Paragraph("Geachte heer/mevrouw " + expert.Surname + ",", standardFont));

                        Teacher[] teachers = new Teacher[2];
                        int index = -1;
                        foreach (KeyValuePair<int, Teacher> teacherKeyValuePair in sessionModel.Teachers)
                            teachers[++index] = teacherKeyValuePair.Value;

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph("Hierbij ontvangt u de afstudeerscriptie van onze student " + sessionModel.Pair.Student1.Study + ", " + sessionModel.Pair.Student1.Firstname + " " + sessionModel.Pair.Student1.Surname + " van wie u de afstudeerbespreking zult bijwonen. Begeleidende docenten " + teachers[0].Firstname + " " + teachers[0].Surname + " en " + teachers[1].Firstname + " " + teachers[1].Surname + " zullen bij de zitting aanwezig zijn.", standardFont));

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph("De afstudeerzitting is gepland op, " + rowSession.Datum + " om " + sessionModel.Daytime.Starttime + ", in lokaal " + rowSession.Lokaal + " van Avans Hogeschool, " + letterTemplate.AvansAdres + " te " + letterTemplate.AvansLocatie + ".", standardFont));

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph(letterTemplate.BeginKern, standardFont));

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph(letterTemplate.ReisInformatie, standardFont));

                        // Ga verder met de inhoud van het document
                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph(letterTemplate.VerdereInformatie, standardFont));

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph("Wij danken u hartelijk voor uw medewerking.", standardFont));
                        document.Add(new iText.Paragraph("Met vriendelijke groet,", standardFont));

                        document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                        document.Add(new iText.Paragraph(letterTemplate.Afzenders, standardFont));

                        // Verander de font style tijdelijk
                        standardFont.SetStyle(Font.ITALIC);

                        document.Add(new iText.Paragraph("Coördinatoren stage en afstuderen", standardFont));

                        // Verander de font style weer terug
                        standardFont.SetStyle(Font.NORMAL);

                        document.Add(new iText.Paragraph("        Bijlage(n):	" + letterTemplate.Bijlagen, standardFont));

                        expert.WasChanged = false;
                    }
                }

                // toon bericht dat exporteren naar PDF gelukt is
                MessageBox.Show("Exporteren gelukt! Bestand is geëxporteerd naar " + filename, "Melding");

                // open het PDF-bestand
                System.Diagnostics.Process.Start(filename);
            }
            catch (PdfException ex)
            {
                // toon bericht als er iets fout gaat
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // het document sluiten
                document.Close();
            }         
        }

        /**
         * Dit maakt de contact informatie tabel zoals te zien is bovenaan de brief bij de adressering
         * Return: De gemaakte tabel
         * Auteur: Yorg 
         */
        private PdfPTable MakeContactInformationTable(LetterTemplate letterTemplate)
        {
            // Maak een tabel met het opgegeven aantal kolommen
            PdfPTable rosterTable = new PdfPTable(CONTACT_INFORMATION_NUM_COLUMNS);

            // Bepaal de breedte voor elke kolom
            rosterTable.SetWidths(new float[] { 0.15f, 0.3f, 0.15f, 0.4f });

            // Breedte van tabel instellen
            rosterTable.WidthPercentage = 100;

            // Bepaal het standaard font om te gebruiken bij deze tabel
            Font standardFont = FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 2);

            // Bepaal het standaard font(vet) om te gebruiken bij deze tabel
            Font standardBoldFont = FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 2, Font.BOLD);

            // START EERSTE RIJ

            rosterTable.AddCell(MakeTableCell("ons kenmerk", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell(letterTemplate.Kenmerk, standardFont));
            rosterTable.AddCell(MakeTableCell("contactpersonen", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell(letterTemplate.Contactpersonen, standardFont));

            // EINDE EERSTE RIJ

            // START TWEEDE RIJ

            rosterTable.AddCell(MakeTableCell("datum", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell(DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year, standardFont));
            rosterTable.AddCell(MakeTableCell("telefoon", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell(letterTemplate.Telefoon, standardFont));

            // EINDE TWEEDE RIJ

            // START DERDE RIJ

            rosterTable.AddCell(MakeTableCell("onderwerp", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell("Afstudeerzitting", standardFont));
            rosterTable.AddCell(MakeTableCell("e-mail", standardBoldFont, true));
            rosterTable.AddCell(MakeTableCell(letterTemplate.Email, standardFont));

            // EINDE DERDE RIJ

            return rosterTable;
        }

        /**
         * Dit maakt een cell voor in een tabel(dat rijmt)
         * @input: text de text die in de cell komt te staan
         * @input: font het font wat gebruikt wordt in de cell
         * @input: rightJustified waar als de text rechts gecentreerd moet staan, anders false
         * Return: De gemaakte cell
         * Auteur: Yorg 
         */
        private static PdfPCell MakeTableCell(string text, Font font, bool rightJustified = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = rightJustified ? 2 : 0;
            cell.BorderWidth = 0;

            return cell;
        }

        /**
         * Deze klasse bestaat alleen voor het invoeren van een footer door middel van het OnEndPage event, misschien lelijk, maar zo werkt ITextSharp schijnbaar :(
         * Auteur: Yorg 
         */
        private class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
        {
            LetterTemplate _letterTemplate;

            public pdfPage(LetterTemplate letterTemplate) : base ()
            {
                _letterTemplate = letterTemplate;
            }

            // Override
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                // Maak een tabel met het opgegeven aantal kolommen
                PdfPTable footerTable = new PdfPTable(FOOTER_NUM_COLUMNS);

                // Bepaal de breedte voor elke kolom
                footerTable.SetWidths(new float[] { 0.33f, 0.33f, 0.33f });

                footerTable.TotalWidth = document.PageSize.Width - (document.LeftMargin + document.RightMargin);

                //Center the table on the page
                footerTable.HorizontalAlignment = Element.ALIGN_CENTER;

                // Bepaal het font om te gebruiken voor de voettekst
                Font footerFont = FontFactory.GetFont(FOOTER_FONT_FAMILY, FOOTER_FONT_SIZE);

                footerTable.AddCell(MakeTableCell(_letterTemplate.VoettekstLinks, footerFont));
                footerTable.AddCell(MakeTableCell(_letterTemplate.VoettekstCenter, footerFont));
                footerTable.AddCell(MakeTableCell(_letterTemplate.VoettekstRechts, footerFont));

                // Schrijf de rijen naar de PDF output stream
                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 40, writer.DirectContent);
            }
        }
    }
}