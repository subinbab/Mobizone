
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelectPdf;
namespace UILayer.Data
{
    public class ConvertPdf 
    {
        public string GenerateReportPDF(string content, string file, string reportname)
        {
            string fileName = "\\" + file + ".pdf";
            string folderName = "\\" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            string relativePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = relativePath + "\\ReportsPDF" + "\\" + reportname + folderName;
            string filePath = folderPath + fileName;
            PdfPageSize pageSize = PdfPageSize.Letter;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape;
            int webPageWidth = 1024;
            int webPageHeight = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;

            //create new folder inside PdfProcessing
            System.IO.Directory.CreateDirectory(folderPath);

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(content.ToString());

            //doc.Security.CanEditContent = false;
            //doc.Security.CanFillFormFields = false;

            // save pdf document
            byte[] pdfBytes = doc.Save();

            // close pdf document
            doc.Close();
            FileStream fs = new FileStream(filePath, FileMode.Create);
            fs.Write(pdfBytes, 0, pdfBytes.Length);
            fs.Close();

            return filePath;
        }
    }
}
