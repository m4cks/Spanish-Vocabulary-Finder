using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ConvertPDF : MonoBehaviour
{
    private static string tempSavePath;
    public static string filePath;

    public static void ExtractTextFromPdf()
    {
        using (PdfReader reader = new PdfReader(filePath))
        {
            StringBuilder text = new StringBuilder();
            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {

                string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                string[] theLines = thePage.Split('\n');
                foreach(var theLine in theLines)
                {
                    text.Append(theLine + " ");
                }
            }
            SaveWords.inputText = text.ToString().Substring(0, text.Length - 1);
        }

    }


    public void SelectFile()
    {
            #if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Title", "", "pdf");
            #endif
        tempSavePath = Application.dataPath + "/TempDoc.txt";
        File.WriteAllText(tempSavePath, string.Empty);

        Debug.Log("File Selected, file path saved: " + filePath);
    }
}
