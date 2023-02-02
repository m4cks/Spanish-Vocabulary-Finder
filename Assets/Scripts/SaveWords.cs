using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static WebGrab;

public class SaveWords : MonoBehaviour
{
    [SerializeField] public Slider _progressBar;
    [SerializeField] public TextMeshProUGUI _progressDescription;

    public static string inputText;
    private static int startNumber = 11; //Omit excess text from the web scrape

    public static List<string> wordList;
    public static List<string> translatedWordList;
    public static List<bool> knownOrNotList;
    
    IEnumerator Start()
    {

        _progressBar.value = 0;


        //Splits string of text into a List
        Debug.Log("Starting InputTextToList()");
        _progressDescription.text = "Selecting unique words...";
        yield return StartCoroutine(InputTextToList());
        _progressBar.value = 10;


        // Convert words to their base form, keeping only unique words using a hash set.
        Debug.Log("Starting GetBaseForms()");
        _progressDescription.text = "Getting base form of words...";
        yield return StartCoroutine(GetBaseForms());
        _progressBar.value = 40;


        // Eliminate words that the user already knows
        Debug.Log("Starting TrimWordList()");
        _progressDescription.text = "Trimming your list...";
        yield return StartCoroutine(TrimWordList());
        _progressBar.value = 70;


        // Grab translation of remaining words
        Debug.Log("Starting TranslateNewWords()");
        _progressDescription.text = "Translating words...";
        yield return StartCoroutine(TranslateNewWords());
        _progressBar.value = 100;


        SceneManager.LoadScene(3);
    }

    public IEnumerator InputTextToList()
    {
        Debug.Log("The input text: " + inputText);

        inputText = inputText.ToLower();
        var inputTextNoPunctuation = "";
        for (int i = 0; i < inputText.Length; i++)
        {
            if (Char.IsLetter(inputText[i]) || inputText[i] == ' ')
            {
                inputTextNoPunctuation += inputText[i];
            }
            _progressBar.value += 10.0f / inputText.Length;
            yield return null;
        }

        //Hash Set for unique words only, and then put in order!
        wordList = new HashSet<string>(inputTextNoPunctuation.Split(' ')).OrderBy(x => x).ToList();        
    }



    public IEnumerator GetBaseForms()
    {
        HashSet<string> newWordList = new HashSet<string>();
        foreach (string word in wordList)
        {
            Debug.Log("Spanish call on: " + word);
            string data = SpanishCall(word);
            if(data != null)
            {
                int cutOff = data.IndexOf("?") - startNumber;
                data = data.Substring(startNumber, cutOff);
                Debug.Log($"Adding {data} without percentages!");
                newWordList.Add(RemovePercentages(data));
            }

            _progressBar.value += 30.0f / wordList.Count;
            yield return null;

            //    Debug.Log($"Word is: {data} and cutOff is set to: {cutOff}");

        }
        wordList = newWordList.ToList();
    }

    public IEnumerator TranslateNewWords()
    {
        List<string> newTranslations = new List<string>();
        List<bool> allFalse = new List<bool>();

        int i = 0;
        while(i < wordList.Count)
        {
            string data = EnglishCall(wordList[i]);
            if (data != null)
            {
                var cutOff = data.IndexOf("?") - startNumber;
                data = data.Substring(startNumber, cutOff);

                newTranslations.Add(RemovePercentages(data));
                allFalse.Add(false);

                _progressBar.value += 30.0f / wordList.Count;

                yield return null;
                i++;
            }
            else
            {
                wordList.RemoveAt(i);

            }
        }
        translatedWordList = newTranslations;
        knownOrNotList = allFalse;

        //Debug.Log("Done with SaveWords Script in last method TranslateWords()");
    }

    private IEnumerator TrimWordList()
    {
        string txtDocumentPath = PlayerPrefs.GetString("SavePath");
        if(!File.Exists(txtDocumentPath)) { yield break; }
        string[] savedWords = File.ReadAllLines(txtDocumentPath);

        // Keep in mind, savedWords is already sorted because we save the words that way.
        // Also, our allWords list is already sort. We can go straight through determining if the word already exists with
        // O(n) time complexity

        var indexSaved = 0;
        var indexCur = 0;
        //Debug.Log("Length of wordList = " + wordList.Count + " ---- Length of saved words = " + savedWords.Length);
        while(savedWords.Length > indexSaved && wordList.Count > indexCur)
        {
            switch (wordList[indexCur].CompareTo(savedWords[indexSaved]))
            {
                case 0:
                    wordList.RemoveAt(indexCur);
                    indexSaved++;
                    break;
                case 1:
                    indexSaved++;
                    break;
                case -1:
                    indexCur++;
                    break;
            }
            _progressBar.value +=  30.0f / (savedWords.Length + wordList.Count);
            yield return null;

        }

    }

    private static string RemovePercentages(string data)
    {
        var debugPercentages = 0;
        while (data.Contains("%") || data.Contains("&"))
        {
            //Debug.Log(data);
            
            if (data.Contains("%20"))    { data = data.Replace("%20", " ");    }
            if (data.Contains("%C3%B1")) { data = data.Replace("%C3%B1", "ñ"); }
            if (data.Contains("%C3%A1")) { data = data.Replace("%C3%A1", "á"); }
            if (data.Contains("%C3%A9")) { data = data.Replace("%C3%A9", "é"); }
            if (data.Contains("%C3%AD")) { data = data.Replace("%C3%AD", "í"); }
            if (data.Contains("%C3%B3")) { data = data.Replace("%C3%B3", "ó"); }
            if (data.Contains("%C3%BA")) { data = data.Replace("%C3%BA", "ú"); }
            if (data.Contains("%C2%A1")) { data = data.Replace("%C2%A1", "¡"); }
            if (data.Contains("%C2%BF")) { data = data.Replace("%C2%BF", "¿"); }
            if (data.Contains("&#x27;")) { data = data.Replace("&#x27;", "'"); }

            debugPercentages++;
            if (debugPercentages > 7)
            {
                Debug.Log($"Problem removing % or & in RemovePercentages() in SaveWords. word = {data}");
                break;
            }
        }

        return data;
    }



}