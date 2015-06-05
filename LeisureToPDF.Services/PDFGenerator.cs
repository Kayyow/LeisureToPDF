using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeisureToPDF.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;

namespace LeisureToPDF.Services {
    public static class PDFGenerator {

        public static BaseColor BLUE = new BaseColor(95, 154, 181);
        public static BaseColor TURQUOISE = new BaseColor(101, 185, 189);
        public static BaseColor GREY = new BaseColor(221, 221, 221);

        public static void GeneratePDF(leisure leisure, string path) {

            
			// Uses to get directory
			string dirLTPDF = @"LeisureToPDF\";
			string currentDir = Directory.GetCurrentDirectory();
			int idxLTPDF = currentDir.IndexOf(dirLTPDF) + dirLTPDF.Length;
            string dir = currentDir.Substring(0, idxLTPDF);
			string dirAssets = dir + @"LeisureToPDF\Assets\";
            string directory = path;
			//
            
            System.Diagnostics.ProcessStartInfo psi;
            psi = new System.Diagnostics.ProcessStartInfo(directory, "");

            Font labelFont = FontFactory.GetFont("georgia", 10f, new BaseColor(255, 255, 255));
            Font titleFont = FontFactory.GetFont("Open sans", 16, new BaseColor(101, 185, 189));

            Rectangle rec = new Rectangle(PageSize.A4);
            rec.BackgroundColor = new BaseColor(240, 240, 240);

            Document doc = new Document(rec, 25, 25, 25, 25);
            FileStream fs = new FileStream(directory, FileMode.Create, FileAccess.Write, FileShare.None);

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            try {              

                doc.Open();

                PdfPTable tab = new PdfPTable(4);

                PdfPCell cellHeader = new PdfPCell(new Phrase(leisure.title, titleFont));
                cellHeader.Colspan = 4;
                cellHeader.HorizontalAlignment = 0;
                cellHeader.Border = Rectangle.BOTTOM_BORDER;
                cellHeader.Padding = 10;

                PdfPCell cellSeparator = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 24F)));
                cellSeparator.Colspan = 4;
                cellSeparator.Border = Rectangle.TOP_BORDER;
                cellSeparator.Padding = 10;

                /* Création des cellule 'Label' */
                PdfPCell cellCategoryLabel = new PdfPCell();
                SetCellLabelStyle(cellCategoryLabel, 2, 1);

                PdfPCell cellEmailLabel = new PdfPCell();
                SetCellLabelStyle(cellEmailLabel, 2, 1);

                PdfPCell cellPhoneLabel = new PdfPCell();
                SetCellLabelStyle(cellPhoneLabel, 2, 1);

                PdfPCell cellWebsiteLabel = new PdfPCell();
                SetCellLabelStyle(cellWebsiteLabel, 4, 1);

                PdfPCell cellAddressLabel = new PdfPCell();
                SetCellLabelStyle(cellAddressLabel, 4, 1);

                PdfPCell cellDescriptionLabel = new PdfPCell();
                SetCellLabelStyle(cellDescriptionLabel, 4, 1);

                /* Création des cellule 'Value' */
                PdfPCell cellCategoryValue = new PdfPCell();
                SetCellValueStyle(cellCategoryValue, 2, 1);

                PdfPCell cellEmailValue = new PdfPCell();
                SetCellValueStyle(cellEmailValue, 2, 1);

                PdfPCell cellPhoneValue = new PdfPCell();
                SetCellValueStyle(cellPhoneValue, 2, 1);

                PdfPCell cellWebsiteValue = new PdfPCell();
                SetCellValueStyle(cellWebsiteValue, 4, 1);

                PdfPCell cellAddressValue = new PdfPCell();
                SetCellValueStyle(cellAddressValue, 4, 1);

                PdfPCell cellDescriptionValue = new PdfPCell();
                SetCellValueStyle(cellDescriptionValue, 4, 1);

                PdfPCell cellImage = new PdfPCell();
                cellImage.FixedHeight = 80;
                cellImage.Padding = 0;
                cellImage.Rowspan = 6;
                cellImage.Colspan = 2;

                cellCategoryLabel.AddElement(new Paragraph(new Phrase("Categorie :", labelFont)));
                cellEmailLabel.AddElement(new Paragraph(new Phrase("Email :  ", labelFont)));
                cellPhoneLabel.AddElement(new Paragraph(new Phrase("Telephone : ", labelFont)));
                cellWebsiteLabel.AddElement(new Paragraph(new Phrase("Site web : ", labelFont)));
                cellAddressLabel.AddElement(new Paragraph(new Phrase("Adresse : ", labelFont)));
                cellDescriptionLabel.AddElement(new Paragraph(new Phrase("Description : ", labelFont)));

                cellCategoryValue.AddElement(new Paragraph(new Phrase(leisure.category.label)));
                cellEmailValue.AddElement(new Paragraph(new Phrase(leisure.email)));
                cellPhoneValue.AddElement(new Paragraph(new Phrase(leisure.phone)));
                cellWebsiteValue.AddElement(new Paragraph(new Phrase(leisure.website)));
                cellAddressValue.AddElement(new Paragraph(new Phrase(leisure.address.number + " " + leisure.address.street + " " + leisure.address.zip_code + " " + leisure.address.city)));
                cellDescriptionValue.AddElement(new Paragraph(new Phrase(@leisure.description)));

                Image PictureLeisure = Image.GetInstance("C:\\Users\\Juliien\\Documents\\Visual Studio 2013\\Projects\\Test ItextSharp\\Test ItextSharp\\img\\Capture1.png");
                cellImage.AddElement(PictureLeisure);


                tab.AddCell(cellHeader);
                tab.AddCell(cellSeparator);

                tab.AddCell(cellCategoryLabel);
                tab.AddCell(cellImage);
                tab.AddCell(cellCategoryValue);
                tab.AddCell(cellEmailLabel);
                tab.AddCell(cellEmailValue);
                tab.AddCell(cellPhoneLabel);
                tab.AddCell(cellPhoneValue);

                tab.AddCell(cellWebsiteLabel);
                tab.AddCell(cellWebsiteValue);
                tab.AddCell(cellAddressLabel);
                tab.AddCell(cellAddressValue);

                tab.AddCell(cellDescriptionLabel);
                tab.AddCell(cellDescriptionValue);

                /* Creation of the footer */
                Image footer = Image.GetInstance(dirAssets + "logo_Lavalloisir.jpg");
                footer.Alignment = Element.ALIGN_CENTER;
                footer.ScalePercent(25f);

                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = 300;

                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.PaddingLeft = 10;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, 240, 30, writer.DirectContent);

                doc.Add(tab);

                doc.Close();


                System.Diagnostics.Process.Start(psi);

            } catch (Exception) {

                throw new Exception("Une erreur est survenue lors de la créations de doccument.");

            } finally {
                writer.Close();
                fs.Close();
                doc.Close();
            }
            
        }
        
        private static void SetCellLabelStyle(PdfPCell cell, int nbColumn, int nbRows) {


            cell.Colspan = nbColumn;
            cell.Rowspan = nbRows;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidth = 0;
            cell.BorderWidthLeft = 4;
            cell.BorderColorLeft = BLUE;
            cell.BackgroundColor = TURQUOISE;
            cell.PaddingLeft = 10;
            cell.PaddingBottom = 10;
        }

        private static void SetCellValueStyle(PdfPCell cell, int nbColumn, int nbRows) {

            cell.Colspan = nbColumn;
            cell.Rowspan = nbRows;
            cell.BackgroundColor = GREY;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 0;
            cell.PaddingLeft = 10;
            cell.PaddingBottom = 10;
        }



    }
}

