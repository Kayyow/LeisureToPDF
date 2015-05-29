﻿using System;
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

        public static void GeneratePDF(leisure leisure) {


            Font label = FontFactory.GetFont("georgia", 10f);
            Font titleFont = FontFactory.GetFont("Open sans", 16, new BaseColor(101, 185, 189));

            Rectangle rec = new Rectangle(PageSize.A4);
            rec.BackgroundColor = new BaseColor(240, 240, 240);

            Document doc = new Document(rec, 25, 25, 25, 25);
            FileStream fs = new FileStream(leisure.title + "PDF.pdf", FileMode.Create, FileAccess.Write, FileShare.None);

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            Paragraph title = new Paragraph(leisure.title, titleFont);
            title.Alignment = Element.ALIGN_LEFT;

           Image logo = Image.GetInstance("C:\\Users\\Juliien\\Documents\\Visual Studio 2013\\Projects\\Test ItextSharp\\Test ItextSharp\\img\\logo_Lavalloisir.png");
            logo.ScalePercent(25f);
            logo.Alignment = Image.TEXTWRAP | Image.ALIGN_RIGHT;
            logo.IndentationLeft = 9f;
            logo.SpacingAfter = 9f;
            doc.Add(logo);

            doc.Add(title);
            
            LineSeparator separatorHead = new LineSeparator();
            separatorHead.Percentage = (100);
            separatorHead.LineColor = new BaseColor(200, 200, 200);
            Chunk linebreakHead = new Chunk(separatorHead);
            doc.Add(linebreakHead);
            
            doc.Add(new Paragraph("Description:"));

            Image PictureLeisure = Image.GetInstance("C:\\Users\\Juliien\\Documents\\Visual Studio 2013\\Projects\\Test ItextSharp\\Test ItextSharp\\img\\image.jpg");
            Paragraph paragraph = new Paragraph(@leisure.description);
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;
            PictureLeisure.ScalePercent(25f);
            PictureLeisure.Alignment = Image.TEXTWRAP | Image.ALIGN_RIGHT;
            PictureLeisure.IndentationLeft = 9f;
            PictureLeisure.SpacingAfter = 9f;
            doc.Add(PictureLeisure);
            doc.Add(paragraph);

            doc.Add(linebreakHead);

            doc.Add(new Paragraph("Coordonnées : "));
            doc.Add(new Paragraph("Tel : " + leisure.phone, label));
            doc.Add(new Paragraph("Email : "+ leisure.email, label));
            doc.Add(new Paragraph("Site web : " + leisure.website, label));
            doc.Add(new Paragraph("Adresse : ", label));
            doc.Add(new Paragraph(leisure.address.number));
            doc.Add(new Paragraph(leisure.address.street));
            doc.Add(new Paragraph(leisure.address.zip_code));
            doc.Add(new Paragraph(leisure.address.city));

            doc.Add(linebreakHead);

            doc.Close();
        }


    }
}

