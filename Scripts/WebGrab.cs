using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine.Networking;

public class WebGrab //: MonoBehaviour 
{

    //Works in editor, not in Android!
    public static string SpanishCall(string palabra)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc = web.Load("https://www.spanishdict.com/translate/" + palabra);

        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            HtmlAttribute att = link.Attributes["href"];
            if (att.Value.Contains("langFrom=es"))
            {
                Debug.Log($"Spanish value found: {att.Value}");
                return att.Value;
            }
        }

        Debug.Log("Spanish base word not found: " + palabra);
        return null;
    }

    public static string EnglishCall(string word)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc = web.Load("https://www.spanishdict.com/translate/" + word);

        for(var i = doc.DocumentNode.SelectNodes("//a[@href]").Count-1; i >= 0 ; i--)
        {
            HtmlAttribute att = doc.DocumentNode.SelectNodes("//a[@href]")[i].Attributes["href"];
            if (att.Value.Contains("langFrom=en"))
            {
                Debug.Log($"English value found: {att.Value}");
                return att.Value;
            }
        }
        Debug.Log("Didn't find translation for word: " + word);
        return null;
    }


    /*

    public static IEnumerator GetData (string url)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.OptionReadEncoding = false;
        Debug.Log("Failing at webrequest?");
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.Send();

            if (request.isError) // Error
            {
                Debug.Log(request.error);
            }
            else // Success
            {

                htmlDoc.LoadHtml(request.downloadHandler.text);
                
            }
        }
    }


 
   public static string SpanishCall(string palabra)
   {
       var pageUrl = "https://www.spanishdict.com/translate/" + palabra;


      

        yield return StartCoroutine(GetData(pageUrl));
       //var request = (HttpWebRequest)WebRequest.Create(pageUrl);
       //request.Method = "GET";

       Debug.Log("Good after webrequest. Failing at GetResponse()");

        

       // using (var response = (HttpWebResponse)request.GetResponse())
       //{
       //    Debug.Log("Nope all good, failing at GetResponseStream()");
       //    using (var stream = response.GetResponseStream())
       //    {
       //        Debug.Log("Error here?");
       //        htmlDoc.Load(stream, Encoding.UTF8);
       //        Debug.Log("yay!");
       //    }
       //}



       //string HTML;
       //using (var wc = new WebClient())
       //{
       //    Debug.Log("going to download string from: " + pageUrl);
       //    HTML = wc.DownloadString(pageUrl);
       //    Debug.Log("DownloadString(pageUrl): " + HTML);
       //}
       //var doc = new HtmlAgilityPack.HtmlDocument();
       //Debug.Log("doc made: loading HTML");
       //doc.LoadHtml(HTML);



       //get the div by id and then get the inner text 
       Debug.Log("Selecting nodes '//a'");
       string testDivSelector = "//a";
       var htmlBody = htmlDoc.DocumentNode.SelectNodes(testDivSelector);
       if(htmlBody == null)
       {
           Debug.Log("No nodes found!!!");
       }
       Debug.Log("Going into foreach loop");
       foreach (HtmlNode link in htmlBody)
       {
           HtmlAttribute att = link.Attributes["href"];
           if (att.Value.Contains("langFrom=es"))
           {
               //Value found.
               //   Debug.Log(att.Value);
               return att.Value;
           }
       }

       Debug.Log("Spanish base word not found: " + palabra);
       return null;
   }
 */
   
}
