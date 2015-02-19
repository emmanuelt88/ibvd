using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Globalization;
using System.Drawing;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.Logic.Helper.PDFDrawing
{
    
    public class Grid : PdfBase, IDrawablePDFSharp
    {
        private Dictionary<string, Column> columns;
        private List<Row> rows;
        private double rowHeight;
        private double headerRowHeight;
        private int pageSize;
        private string textHeader;
        private string textFooter;

        /// <summary>
        /// 
        /// </summary>
        public double PageHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double PageWidth { get; set; }
        /// <summary>
        /// Get or set if header text have center alignment
        /// </summary>
        public bool HeaderAlignCenter { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Grid(double headerHeight, double rowHeight, int pageSize, string header, string footer)
        {
            PageHeight = 0;
            PageWidth = 0;
            columns = new Dictionary<string, Column>();
            rows = new List<Row>();
            this.rowHeight = rowHeight;
            this.headerRowHeight = headerHeight;
            this.pageSize = pageSize;
            this.textHeader = header;
            this.textFooter = footer;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Grid(double headerHeight, double rowHeight, int pageSize, string header, string footer, double pageWidth, double pageHeight)
        {
            PageHeight = pageHeight;
            PageWidth = pageWidth;
            columns = new Dictionary<string, Column>();
            rows = new List<Row>();
            this.rowHeight = rowHeight;
            this.headerRowHeight = headerHeight;
            this.pageSize = pageSize;
            this.textHeader = header;
            this.textFooter = footer;
        }

        /// <summary>
        /// Generate grid header
        /// </summary>
        public void GenerateHeader()
        {
            rows.Add(new Row(30, columns));
        }
        /// <summary>
        /// Return true if column header exists in the GridBase.
        /// </summary>
        /// <param name="header">Column header</param>
        /// <returns>If exists column</returns>
        public bool ColumnExists(string header)
        {
            return columns.ContainsKey(header);
        }


        public void CreateColumn(string header, double width, bool alignCenter)
        {
            if (columns.ContainsKey(header))
            {
                throw new ColumnException("No se puede crear una columna repetida", ColumnException.TYPE.COLUMN_DUPLICATE);
            }
            else
            {
                Column nueva = new Column(header, width, true, alignCenter,0,0) ;
                columns.Add(header, nueva);
            }
        }

        public override void DrawHeader(string text, XGraphics graphics, PdfPage page, int pageNumber, bool showPage)
        {
            if (string.IsNullOrEmpty(text))
                return;
            XFont font = new XFont("Arial", 13, XFontStyle.Bold);
            
            graphics.DrawLine(new XPen(XColors.Black), 150, 40, page.Width.Point - 40, 40);

            graphics.DrawString(text, font, XBrushes.Black,
            new XRect(0, 23, page.Width, page.Height),
            XStringFormats.TopCenter);

            if (showPage)
            {
                font = new XFont("Arial", 10, XFontStyle.Regular);

                graphics.DrawString(string.Format("Pág. {0}", pageNumber.ToString().PadRight(3, '\0')), font, XBrushes.Black,
                new XRect(230, 25, page.Width, page.Height),
                XStringFormats.TopCenter);
            }
        }
        /// <summary>
        /// Create a row width a dictionary of values 
        /// where key is header and value is text to display on the grid
        /// </summary>
        /// <param name="values">Column values</param>
        public void CreateRow(Dictionary<string, Column> values, double marginLeft, double marginRight)
        {
            rows.Add(new Row(rowHeight, values));
        }
        /// <summary>
        /// Grid column
        /// </summary>
        public class Column
        {
            public double MarginLeft { get; set; }
            public double MarginRight { get; set; }

            /// <summary>
            /// Value.
            /// </summary>
            public string Value { get; private set; }
            /// <summary>
            /// Column width.
            /// </summary>
            public double Width { get; private set; }
            /// <summary>
            /// Return true if this is a column header
            /// </summary>
            public bool IsColumnHeader { get; private set; }
            /// <summary>
            /// If column align center
            /// </summary>
            public bool AlignCenter { get; private set; }
            internal Column(string header, double width, bool isColumnHeader, bool alignCenter,double marginLeft, double marginRight)
            {
                this.Value = header;
                this.Width = width;
                this.IsColumnHeader = isColumnHeader;
                this.AlignCenter = alignCenter;
                this.MarginLeft = marginLeft;
                this.MarginRight = marginRight;
            }

            public Column(string value, bool alignCenter, double marginLeft, double marginRight)
            {
                this.Value = value;
                this.AlignCenter = alignCenter;
                this.MarginLeft = marginLeft;
                this.MarginRight = marginRight;
            }

        }
        /// <summary>
        /// Exception 
        /// </summary>
        public class ColumnException : Exception
        {
            /// <summary>
            /// Exception types
            /// </summary>
            public enum TYPE
            {
                /// <summary>
                /// Error occurs when try to create a duplicate column.
                /// </summary>
                COLUMN_DUPLICATE
            }
            /// <summary>
            /// Type error occurred
            /// </summary>
            public TYPE ErrorType { get; private set; }

            public ColumnException(string message, TYPE type)
                : base(message)
            {
                ErrorType = type;
            }
        }

        /// <summary>
        /// Grid row
        /// </summary>
        private class Row
        {
            public Dictionary<string, Column> Values { get; private set; }
            /// <summary>
            /// Row height
            /// </summary>
            public double Height { get; private set; }

            public Row(double height, Dictionary<string, Column> values)
            {
                Height = height;
                this.Values = values;
            }
        }

        #region IDrawablePDFSharp Members

         public void DrawDatosReunion(Reunion reunion, double left, double top, double pageWidth, double pageHeight)
        {

            PdfPage page = documento.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            if (pageWidth > 0)
                page.Width = new XUnit(pageWidth, XGraphicsUnit.Centimeter);
            if (pageHeight > 0)
                page.Height = new XUnit(pageHeight, XGraphicsUnit.Centimeter);
             
            // Obtengo el objeto XGraphics para poder dibujar sobre el PDF.
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont fontLabel = new XFont("Arial", 11, XFontStyle.Bold);
            XFont fontText = new XFont("Arial", 11, XFontStyle.Regular);
            int i = 0;

            gfx.DrawString("Preside:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30));
            gfx.DrawString((reunion.Encargado.FullNameUser).PadRight(80, '\0'), fontText, XBrushes.Black, new XPoint(150, 100 + i * 30));
            
            i++;

            gfx.DrawString("Título:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100+i*30));
            gfx.DrawString(reunion.Titulo.PadRight(50, '\0'), fontText, XBrushes.Black, new XPoint(150, 100 + i * 30));
            i++;
            fontText = new XFont("Arial", 11, XFontStyle.Regular);
            gfx.DrawString("Fecha del culto:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30));
            gfx.DrawString(string.Format(reunion.FechaCulto.ToString("dddd, dd {0} MMMM {1} yyyy {2} hh:mm tt", new CultureInfo("es-AR")), "de", "de", "a las").PadRight(50, '\0'), fontText, XBrushes.Black, new XPoint(150, 100 + i * 30));
            i++;
            gfx.DrawString("Fecha de ensayo:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30));
            gfx.DrawString(string.Format(reunion.FechaEnsayo.ToString("dddd, dd {0} MMMM {1} yyyy {2} hh:mm tt", new CultureInfo("es-AR")), "de", "de", "a las").PadRight(50, '\0'), fontText, XBrushes.Black, new XPoint(150, 100 + i * 30));
            i++;
            //gfx.DrawString("Participantes:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30));

            //foreach (U persona in reunion.Usuarios)
            //{
            //    gfx.DrawString(string.Format("{0} {1}", persona.Nombre, persona.Apellido), fontText, XBrushes.Black, new XPoint(150, 100 + i * 20 + 32));
            //    i++;
            //}

            //int extra = reunion.Usuarios.Count * 10;
            int extra = 0;

            gfx.DrawString("Hay cena:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30 - extra));
            gfx.DrawString((reunion.HayCena) ? "SI" : "NO", fontText, XBrushes.Black, new XPoint(150, 100 + i * 30 - extra));
            i++;
            gfx.DrawString("Descripción:".PadRight(20, '\0'), fontLabel, XBrushes.Black, new XPoint(50, 100 + i * 30 - extra));
            string palabras= string.Empty;
            
            foreach (string palabra in reunion.Descripcion.Replace("\r\n"," ").Split(' '))
            {
                palabras += " " + palabra;

                if (palabras.Length > 50)
                {
                    i++;
                    gfx.DrawString(palabras,fontText, XBrushes.Black, new XPoint(150, 100 + i * 11 + 95));
                    
                    palabras = string.Empty;
                }
            }
            i++;
            gfx.DrawString(palabras, fontText, XBrushes.Black, new XPoint(150, 100 + i * 11 + 95));
            fontText = new XFont("Arial", 11, XFontStyle.Regular);
            DrawHeader("Datos de la reunión" , gfx, page, 1, false);
            DrawFooter(textFooter, gfx, page, 1, false);
        }
        public void Draw(double left, double top)
        {
            double marginTop = 30;
            double aHeight = -1;
            double aLeft = 0;
            double aTop = 0;
            PdfPage page = documento.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            if (PageWidth > 0)
            {
                page.Width = new XUnit(PageWidth, XGraphicsUnit.Centimeter);
            }
            if (PageHeight > 0)
            {
                page.Height = new XUnit(PageHeight, XGraphicsUnit.Centimeter);
            }

            // Obtengo el objeto XGraphics para poder dibujar sobre el PDF.
            XGraphics gfx = XGraphics.FromPdfPage(page);

            int pages;
            double result = rows.Count / (pageSize + 0.0);

            pages = int.Parse(result.ToString("0"));

            if (result.ToString().Contains(","))
            {
                if (int.Parse(result.ToString().Split(',')[1][0].ToString()) < 5)
                {
                    pages++;
                }
            }
            

            if (pages == 0)
            {
                pages++;
            }
            
            int itemsCount = 0;
            int pageNumber = documento.Pages.Count;
            DrawHeader(textHeader, gfx, page, pageNumber, false);
            DrawFooter(textFooter, gfx, page, pageNumber, true);
            for (int i = 0; i < pages; i++)
            {
                
                

                XFont font = new XFont("Arial", 12, XFontStyle.Bold);
                aLeft = 0;
                foreach (string header in columns.Keys)
                {
                    DrawRectangle(gfx, left + aLeft, top + marginTop, columns[header].Width, headerRowHeight, header, font, HeaderAlignCenter,0,0);
                    aLeft += columns[header].Width;
                }

                font = new XFont("Arial", 10, XFontStyle.Regular);

                foreach (Row row in rows.Skip(pageSize * (i)).Take(pageSize))
                {
                    itemsCount++;

                    aTop += (aTop == 0) ? headerRowHeight : rowHeight;
                    aLeft = 0;
                    foreach (string key in row.Values.Keys)
                    {
                        DrawRectangle(gfx, left + aLeft, top + aTop + marginTop, columns[key].Width, rowHeight, row.Values[key].Value, font, columns[key].AlignCenter, row.Values[key].MarginLeft, row.Values[key].MarginRight);

                        aLeft += columns[key].Width;
                    }

                    aHeight += rowHeight;

                    if (itemsCount % pageSize == 0)
                    {
                        aHeight = 0;
                        page = documento.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        page.Size = PdfSharp.PageSize.A4;
                        if (PageWidth > 0)
                        {
                            page.Width = new XUnit(PageWidth, XGraphicsUnit.Centimeter);
                        }
                        if (PageHeight > 0)
                        {
                            page.Height = new XUnit(PageHeight, XGraphicsUnit.Centimeter);
                        }
                        DrawHeader(textHeader, gfx, page, i+2, false);
                        DrawFooter(textFooter, gfx, page, i+2, true);
                        pageNumber++;
                        aTop = 0;
                    }
                }
            }

        }

        private void DrawRectangle(XGraphics graphicsPage, double left, double top, double width, double height, string content, XFont font, bool alignmentCenter, double marginLeft, double marginRight)
        {
            graphicsPage.DrawLines(XPens.Black, new XPoint[]{ 
                new XPoint(left, top), 
                new XPoint(left + width, top), 
                new XPoint(left + width, top+ height), 
                new XPoint(left, top+height),
                new XPoint(left, top)
            });


            if (alignmentCenter)
            {
                width = (width - 12 - content.Length * 7) / 2;
                left = left + width;
            }

            graphicsPage.DrawString(content, font, XBrushes.Black, new XPoint(left + 4 + marginLeft, top + 12 +marginRight));
        }

        #endregion

    }
}
