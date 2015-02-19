using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using IBVD.Digital.Logic.Helper.PDFDrawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.Logic.Helper
{
    public static class PDFGenerator
    {
        public static PdfSharp.Pdf.PdfDocument GenerarCanciones(IList<Cancion> canciones, bool small)
        {
            int maxPerPage = 40;

            if (small)
                maxPerPage = 19;

            Grid grilla = new Grid(16, 16, maxPerPage, "Listado de Canciones", "Iglesia Bautista de Villa Domínico", 0, 15);
            grilla.HeaderAlignCenter = true;
            grilla.CreateColumn("Titulo", 325, false);
            grilla.CreateColumn("Tono", 40, true);
            grilla.CreateColumn("Compás", 50, true);
            
            foreach (Cancion cancion in canciones)
            {
                Dictionary<string, Grid.Column> row = new Dictionary<string, Grid.Column>();

                row.Add("Titulo", new Grid.Column(cancion.Titulo.PadRight(50,'\0'), false, 0, 0));
                row.Add("Tono", new Grid.Column(cancion.Tono.PadRight(3, '\0'), false, 8, 0));
                row.Add("Compás", new Grid.Column(cancion.Compas, false, 5, 0));
                grilla.CreateRow(row, 6,0);
            }

            int top = 40;

            if (small)
                top = 20;
            grilla.Draw(90, top);
            
            return grilla.GetDocument();

        }

        public static PdfSharp.Pdf.PdfDocument GenerarItems(IList<ItemReunion> items, bool small)
        {
            int maxPerPage = 40;

            if (small)
                maxPerPage = 19;

            Grid grilla = new Grid(16, 16, maxPerPage, "Listado de items de la reunión", "Iglesia Bautista de Villa Domínico", 0, 15);
            grilla.HeaderAlignCenter = true;
            grilla.CreateColumn("Titulo", 325, false);
            //grilla.CreateColumn("Tono", 40, true);
            //grilla.CreateColumn("Compás", 50, true);

            foreach (ItemReunion item in items)
            {
                Dictionary<string, Grid.Column> row = new Dictionary<string, Grid.Column>();

                row.Add("Titulo", new Grid.Column(item.GetText().PadRight(50, '\0'), false, 0, 0));
                grilla.CreateRow(row, 6, 0);
            }

            int top = 40;

            if (small)
                top = 20;
            grilla.Draw(90, top);

            return grilla.GetDocument();

        }

        public static PdfSharp.Pdf.PdfDocument GenerarReunion(Reunion reunion)
        {

            Grid grilla = new Grid(16, 16, 40, "Listado de Canciones", "Iglesia Bautista de Villa Domínico");
            //grilla.HeaderAlignCenter = true;
            //grilla.CreateColumn("Titulo", 325, false);
            //grilla.CreateColumn("Tono", 40, true);
            //grilla.CreateColumn("Compás", 50, true);

            //foreach (Cancion cancion in reunion.GetCancionesOrdenadas())
            //{
            //    Dictionary<string, Grid.Column> row = new Dictionary<string, Grid.Column>();

            //    row.Add("Titulo", new Grid.Column(cancion.Titulo.PadRight(50, '\0'), false, 0, 0));
            //    row.Add("Tono", new Grid.Column(cancion.Tono.PadRight(3, '\0'), false, 8, 0));
            //    row.Add("Compás", new Grid.Column(cancion.Compas, false, 5, 0));
            //    grilla.CreateRow(row, 6, 0);
            //}

            grilla.DrawDatosReunion(reunion, 90, 40, 0, 20);
            //grilla.Draw(90, 40);

            return grilla.GetDocument();
        }
    }
}
