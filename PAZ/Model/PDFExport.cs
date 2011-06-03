using System;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using iTextSharp.text;
using iText = iTextSharp.text;
using iTextSharp.text.pdf;

namespace PAZ.Model
{
    public class PDFExport
    {
        private const int CONTACT_INFORMATION_NUM_COLUMNS = 4;

        private const string STANDARD_FONT_FAMILY = "Verdana";
        private const int STANDARD_FONT_SIZE = 9;

        public enum PdfType
        {
            PDF_Overview,
            PDF_Letter
        };

        private DataGrid dataGrid;

        public PDFExport(DataGrid datagrid)
        {
            this.dataGrid = datagrid;
        }

        /**
         * Maakt een PDF-bestand aan
         * Auteur: Gökhan en Yorg
        */
        public void CreatePdf(String filename, PdfType pdfType)
        {
            iTextSharp.text.Document document = null;

            try
            {
                // het document(standaard A4-formaat) maken en instellen als landscape
                document = new iText.Document(PageSize.A4.Rotate());

                // De writer maken die naar het document luistert en zet de stream om in een PDF-bestand
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));

                // het document openen
                document.Open();

                switch(pdfType)
                {
                    case PdfType.PDF_Overview:
                        CreateOverviewPDF(document);
                        break;

                    case PdfType.PDF_Letter:
                        CreateLetterPDF(document);
                        break;
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
         * Dit maakt de opzet voor het overzicht PDF, het werkelijke rooster wordt gemaakt in een aparte functie.
         * Auteur: Gökhan en Yorg 
         */
        private void CreateOverviewPDF(iText.Document document)
        {
            // Een titel maken
            iText.Paragraph titel = new iText.Paragraph("Het PAZ-rooster", FontFactory.GetFont("Arial", 26, Font.BOLDITALIC));
            titel.Alignment = 1; // titel centeren

            // elementen toevoegen aan het document
            document.Add(titel); // de titel toevoegen
            document.Add(new iText.Paragraph(" ")); // leegruimte toevoegen
            document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
            document.Add(MakeRoster()); // het rooster
        }

        /**
         * Maakt het rooster in een tabel vorm
         * Return: De gemaakte tabel
         * Auteur: Gökhan en Yorg
        */
        private PdfPTable MakeRoster()
        {
            int aantalKolommen = dataGrid.Columns.Count;

            // Maak een tabel met even veel aantal kolommen als de datagrid
            PdfPTable rosterTable = new PdfPTable(aantalKolommen);

            // bepaal de breedte voor elke kolom in volgorde
            float[] columnWidths = new float[aantalKolommen];
            for (int columnNo = 0; columnNo < aantalKolommen; ++columnNo)
                columnWidths[columnNo] = (float) dataGrid.Columns[columnNo].Width.Value;

            rosterTable.SetWidths(columnWidths);

            // breedte van tabel instellen
            rosterTable.WidthPercentage = 100;

            // Leest de kolomnamen uit de datagrid en voegt toe aan roosterTable
            for (int columnNo = 0; columnNo < aantalKolommen; ++columnNo)
            {
                string columNaam = dataGrid.Columns[columnNo].Header.ToString();

                Phrase ph = new Phrase(columNaam, FontFactory.GetFont("Arial", 13, Font.BOLD));
                PdfPCell cell = new PdfPCell(ph);
                cell.Padding = 5;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                rosterTable.AddCell(cell);
            }

            // Leest de rijen uit de datagrid en voegt deze toe aan roosterTable
            for (int rowNo = 0; rowNo < dataGrid.Items.Count - 1; ++rowNo)
            {
                Session rowSession = (Session)dataGrid.Items[rowNo];

                // Temp oplossing, tot datalist waardes bevat
                if (rowSession.GetDataList().Count > 0)
                {
                    for (int columnNo = 0; columnNo < dataGrid.Columns.Count; ++columnNo)
                    {
                        Phrase ph = new Phrase(rowSession.GetDataList()[columnNo].ToString(), FontFactory.GetFont("Arial", 13, Font.NORMAL));
                        PdfPCell cell = new PdfPCell(ph);
                        cell.Padding = 5;
                        rosterTable.AddCell(cell);
                    }
                }
            }

            return rosterTable;
        }

        /**
         * Dit maakt de brieven om te versturen naar de experts in zijn geheel en zet ze in een PDF document
         * Auteur: Yorg 
         */
        private void CreateLetterPDF(iText.Document document)
        {
            for (int rowNo = 0; rowNo < dataGrid.Items.Count - 1; ++rowNo)
            {
                Session rowSession = (Session)dataGrid.Items[rowNo];

                // Een titel maken
                iText.Paragraph titel = new iText.Paragraph("Academie voor Management en Bestuur", FontFactory.GetFont("Arial", 12, Font.BOLD));
                titel.Alignment = 1;

                // Subtitel maken
                iText.Paragraph subTitel = new iText.Paragraph("'s-Hertogenbosch", FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 1));
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
                document.Add(new iText.Paragraph("<bedrijf>", standardFont));
                document.Add(new iText.Paragraph(rowSession.Pair.Student1.Firstname + " " + rowSession.Pair.Student1.Surname, standardFont));
                document.Add(new iText.Paragraph("<adres>", standardFont));
                document.Add(new iText.Paragraph("<pc / woonplaats>", standardFont));

                // Leeg ruimte toevoegen
                document.Add(new iText.Paragraph(" "));
                document.Add(new iText.Paragraph(" "));
                document.Add(new iText.Paragraph(" "));
                document.Add(new iText.Paragraph(" "));

                // Contact informatie tabel
                document.Add(MakeContactInformationTable());

                // Leeg ruimte toevoegen
                document.Add(new iText.Paragraph(" "));
                document.Add(new iText.Paragraph(" "));
                document.Add(new iText.Paragraph(" "));

                // Inhoud brief
                document.Add(new iText.Paragraph("Geachte heer/mevrouw " + rowSession.Pair.Student1.Surname, standardFont)); // Aanhef(moet veranderen in expert, niet student)
                document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
                document.Add(new iText.Paragraph("Hierbij nodigt Avans u uit", FontFactory.GetFont("Verdana", 8)));
            }
        }

        /**
         * Dit maakt de contact informatie tabel zoals te zien is bovenaan de brief bij de adressering
         * Return: De gemaakte tabel
         * Auteur: Yorg 
         */
        private PdfPTable MakeContactInformationTable()
        {
            int aantalKolommen = CONTACT_INFORMATION_NUM_COLUMNS;

            // Maak een tabel met het opgegeven aantal kolommen
            PdfPTable rosterTable = new PdfPTable(aantalKolommen);

            // Bepaal de breedte voor elke kolom
            rosterTable.SetWidths(new float[] { 0.075f, 0.425f, 0.075f, 0.425f }); // TO DO: Fix widths

            // Breedte van tabel instellen
            rosterTable.WidthPercentage = 100;

            // Bepaal het standaard font om te gebruiken bij deze tabel
            Font standardFont = FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 2);

            // Bepaal het standaard font(vet) om te gebruiken bij deze tabel
            Font standardBoldFont = FontFactory.GetFont(STANDARD_FONT_FAMILY, STANDARD_FONT_SIZE - 2, Font.BOLD);

            // START EERSTE RIJ

            rosterTable.AddCell(MakeContactInformationCell("ons kenmerk", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell("Afst alg/corr/ 0809/ 10.06.09", standardFont));
            rosterTable.AddCell(MakeContactInformationCell("contactpersonen", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell("Lilian Reuken en Regien Blom", standardFont));

            // EINDE EERSTE RIJ

            // START TWEEDE RIJ

            rosterTable.AddCell(MakeContactInformationCell("datum", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell(DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year, standardFont));
            rosterTable.AddCell(MakeContactInformationCell("telefoon", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell("(073) 629 52 56/ (073) 629 54 55", standardFont));

            // EINDE TWEEDE RIJ

            // START DERDE RIJ

            rosterTable.AddCell(MakeContactInformationCell("onderwerp", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell("Afstudeerzitting", standardFont));
            rosterTable.AddCell(MakeContactInformationCell("e-mail", standardBoldFont, true));
            rosterTable.AddCell(MakeContactInformationCell("ac.reuken@avans.nl/ r.blom-depoel@avans.nl", standardFont));

            // EINDE DERDE RIJ

            return rosterTable;
        }

        /**
         * Dit maakt een cell die bij de contact informatie tabel hoort
         * @input: text de text die in de cell komt te staan
         * @input: font het font wat gebruikt wordt in de cell
         * @input: rightJustified waar als de text rechts gecentreerd moet staan, anders false
         * Return: De gemaakte cell
         * Auteur: Yorg 
         */
        private PdfPCell MakeContactInformationCell(string text, Font font, bool rightJustified = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = rightJustified ? 2 : 0;
            cell.BorderWidth = 0;

            return cell;
        }
    }
}
