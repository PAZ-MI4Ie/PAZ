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
         * Auteur: Yorg 
         */
        private void CreateOverviewPDF(iText.Document document)
        {
            // een paragraaf maken en aan het document toevoegen
            iText.Paragraph titel = new iText.Paragraph("Het PAZ-rooster", FontFactory.GetFont("Arial", 26, Font.BOLDITALIC));
            titel.Alignment = 1; // titel centeren

            // elementen toevoegen aan het document
            document.Add(titel); // de titel
            document.Add(new iText.Paragraph(" ")); // leegruimte toevoegen
            document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
            document.Add(MakeRoster()); // het rooster
        }

        /**
         * Maakt het rooster in een tabel vorm
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
            // elementen toevoegen aan het document
            document.Add(new iText.Paragraph("Geachte heer/mevrouw")); // Aanhef
            document.Add(new iText.Paragraph(" "));  // leegruimte toevoegen
        }
    }
}
