using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public static class SaveData
{

    private static string txtDocumentPath;

    public static void StartSaving()
    {
        txtDocumentPath = Application.dataPath + "/SavedWords" + ".txt";
        PlayerPrefs.SetString("SavePath", txtDocumentPath);

        if(!File.Exists(txtDocumentPath))
        {
            CreateTextFile();
            Debug.Log("File created at: " + txtDocumentPath);
        }

        SaveNewData();
        Debug.Log("Saved known words to: " + txtDocumentPath);

    }

    public static void CreateTextFile()
    {
        File.WriteAllText(txtDocumentPath, "zzz"); 
    }

    public static void SaveNewData()
    {
        List<string> allWords = new List<string>(File.ReadAllLines(txtDocumentPath));
        List<string> sortedKnownWords = new List<string>();

        //Separate known words from all words.
        for(int i = 0; i < SaveWords.knownOrNotList.Count; i++)
        {
            if (SaveWords.knownOrNotList[i])
            {
                sortedKnownWords.Add(SaveWords.wordList[i]);
            }
            
        }
        //Sort known words
        sortedKnownWords = sortedKnownWords.OrderBy(x => x).ToList();


        // Our saved text file is already sorted. Our known words are sorted.
        // We'll go straight through placing the words where they belong without backtracking.
        // This is will take O(n) time.
        var index = 0;
        while (sortedKnownWords.Count > 0)
        {
            if (sortedKnownWords[0].CompareTo(allWords[index]) < 0)
            {
                allWords.Insert(index, sortedKnownWords[0]);
                sortedKnownWords.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }

        //Erase contents of existing document and insert in new order:
        File.WriteAllText(txtDocumentPath, string.Empty);
        foreach(string word in allWords)
        {
            File.AppendAllText(txtDocumentPath, word + "\n");
        }

    }

    // Delete File method (just in case)
    public static void DeleteFile()
    {
        File.Delete(txtDocumentPath);
    }
    

}
