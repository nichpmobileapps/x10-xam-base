
using System;
using System.Collections.Generic;
using System.Text;
using X10Card.Models;

namespace X10Card
{
    public interface IPdfViewer
    {
        //void OpenPdfFile(string filePath);
        //void OpenPdfFile(byte[] pdfData);
        bool SaveAndOpenPDF(string fileName, byte[] data);

    }
    
}
