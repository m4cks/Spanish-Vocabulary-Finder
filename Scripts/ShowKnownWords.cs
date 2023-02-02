using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ShowKnownWords : MonoBehaviour
{
    [SerializeField] GameObject TheWord;
    [SerializeField] Transform ContentView;
    [SerializeField] TextMeshProUGUI NoWords;

    void Start()
    {
        string txtDocumentPath = PlayerPrefs.GetString("SavePath");
        
        if (File.Exists(txtDocumentPath))
        {
            NoWords.enabled = false;

            string[] savedWords = File.ReadAllLines(txtDocumentPath);
            DisplayText(savedWords);
        }

    }


    private void DisplayText(string[] savedWords)
    {
        for(int i = 0; i < savedWords.Length-1; i++)
        {
            GameObject tw = Instantiate(TheWord) as GameObject;

            tw.transform.parent = ContentView.transform;
            var word = tw.transform.GetComponent<TextMeshProUGUI>();
            word.text = "• " + savedWords[i];

        }
    }

}
