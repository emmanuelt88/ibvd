using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace IBVD.Digital.Logic.Helper.PDFDrawing
{
    public class PdfBase
    {
        private PdfDocument doc = null;


        protected PdfDocument documento
        {
            get 
            {
                if (doc == null)
                {
                    doc = new PdfDocument();
                }

                return doc;
            }
        }



        public PdfDocument GetDocument()
        {
            return documento;
        }

        public PdfBase()
        {
            // Creo el Documento PDF
            PdfSharp.Pdf.PdfDocument documento = new PdfSharp.Pdf.PdfDocument();
        }

        public virtual void DrawHeader(string text, XGraphics graphics, PdfPage page, int pageNumber, bool showPage)
        {
            if (string.IsNullOrEmpty(text))
                return;
            XFont font = new XFont("Arial", 13, XFontStyle.Bold);
            graphics.DrawLine(new XPen(XColors.Black), 40, 20, page.Width.Point-40, 20);
            graphics.DrawLine(new XPen(XColors.Black), 40, 40, page.Width.Point - 40, 40);

            graphics.DrawString(text, font, XBrushes.Black,
            new XRect(0,23, page.Width, page.Height),
            XStringFormats.TopCenter);

            if (showPage)
            {
                font = new XFont("Arial", 10, XFontStyle.Regular);

                graphics.DrawString(string.Format("Pág. {0}", pageNumber.ToString().PadRight(3, '\0')), font, XBrushes.Black,
                new XRect(230, 25, page.Width, page.Height),
                XStringFormats.TopCenter);
            }
        }

        public virtual void DrawFooter(string text, XGraphics graphics, PdfPage page, int pageNumber, bool showPage)
        {
            if (string.IsNullOrEmpty(text))
                return;

            XFont font = new XFont("Arial", 13, XFontStyle.Bold);
            graphics.DrawLine(new XPen(XColors.Black), 40, page.Height.Point - 20, page.Width.Point - 40, page.Height.Point - 20);
            graphics.DrawLine(new XPen(XColors.Black), 40, page.Height.Point - 40, page.Width.Point - 40, page.Height.Point - 40);
            
            graphics.DrawString(text, font, XBrushes.Black,
            new XRect(0, -22, page.Width, page.Height),
            XStringFormats.BottomCenter);

            if (showPage)
            {
                font = new XFont("Arial", 10, XFontStyle.Regular);

                graphics.DrawString(string.Format("Pág. {0}", pageNumber.ToString().PadRight(3, '\0')), font, XBrushes.Black,
                new XRect(230, -25, page.Width, page.Height),
                XStringFormats.BottomCenter);
            }
        }

    }
}
