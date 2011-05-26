using System;
using System.Windows.Controls;
using System.IO;
using System.Windows;

// imports van iTextSharp
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PAZ.Model
{
    public class PDFExport
    {
        private DataGrid datagrid;

        public PDFExport(DataGrid datagrid)
        {
            this.datagrid = datagrid;
        }

        /**
         * Maakt het rooster in een tabel vorm
         * Auteur: Gökhan en Yorg
        */
        private PdfPTable MaakRooster()
        {
            // een tabel met 7 kolommen
            PdfPTable roosterTable = new PdfPTable(datagrid.Columns.Count);

            // bepaal de breedte voor elke kolom in volgorde
            roosterTable.SetWidths(new float[] { 0.5f, 0.35f, 0.4f, 1, 1, 1, 0.6f });

            // breedte van tabel instellen
            roosterTable.WidthPercentage = 100;

            // Leest de kolomnamen uit de datagrid en voegt toe roosterTable
            for (int columnNo = 0; columnNo < datagrid.Columns.Count; ++columnNo)
            {
                string columNaam = datagrid.Columns[columnNo].Header.ToString();

                Phrase ph = new Phrase(columNaam, FontFactory.GetFont("Arial", 13, Font.BOLD));
                PdfPCell cell = new PdfPCell(ph);
                cell.Padding = 5;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                roosterTable.AddCell(cell);
            }

            // Lees de rijen uit de datagrid en voegt toe aan roosterTable
            for (int rowNo = 0; rowNo < datagrid.Items.Count; ++rowNo)
            {
                Zitting rijZitting = (Zitting)datagrid.Items[rowNo];
                for (int columnNo = 0; columnNo < datagrid.Columns.Count; ++columnNo)
                {
                    Phrase ph = new Phrase(rijZitting.GetDataList()[columnNo].ToString(), FontFactory.GetFont("Arial", 13, Font.NORMAL));
                    PdfPCell cell = new PdfPCell(ph);
                    cell.Padding = 5;
                    roosterTable.AddCell(cell);
                }
            }

            return roosterTable;
        }



        /**
         * Maakt een PDF-bestand aan
         * Auteur: Gökhan en Yorg
        */
        public void createPdf(String filename)
        {
            iTextSharp.text.Document document = null;

            try
            {
                // het document(standaard A4-formaat) maken en instellen als landscape
                document = new iTextSharp.text.Document(PageSize.A4.Rotate());

                // De writer maken die naar het document luistert en zet de stream om in een PDF-bestand
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));

                // het document openen
                document.Open();

                // een paragraaf maken en aan het document toevoegen
                iTextSharp.text.Paragraph titel = new iTextSharp.text.Paragraph("Het PAZ-rooster", FontFactory.GetFont("Arial", 26, Font.BOLDITALIC));
                titel.Alignment = 1; // titel centeren

                // elementen toevoegen aan het document
                document.Add(titel); // de titel
                document.Add(new iTextSharp.text.Paragraph(" ")); // leegruimte toevoegen
                document.Add(new iTextSharp.text.Paragraph(" "));  // leegruimte toevoegen
                document.Add(MaakRooster()); // het rooster

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
    }

}
