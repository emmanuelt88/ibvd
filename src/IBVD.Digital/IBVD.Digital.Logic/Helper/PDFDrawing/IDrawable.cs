using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace IBVD.Digital.Logic.Helper.PDFDrawing
{
    /// <summary>
    /// Interface which must implement all class that is drawable on PDFSharp document
    /// </summary>
    public interface IDrawablePDFSharp
    {
        void Draw(double left, double top);
    }
}
 
    