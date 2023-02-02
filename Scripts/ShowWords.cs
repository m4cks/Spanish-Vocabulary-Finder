using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowWords : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject TheWord;
    [SerializeField] Transform ContentView;
    [SerializeField] GameObject AllReady;
    public static List<GameObject> allWords;

    void Start()
    {
        // Debug.Log("Input text in script ShowWords: " + SaveWords.wordList[0]);
        allWords = new List<GameObject>();
        for(var i = 0; i < SaveWords.wordList.Count; i++)
        {
            GameObject tw = Instantiate(TheWord) as GameObject;

            //ContentView.SetParent(tw.transform);

            tw.transform.parent = ContentView.transform;
            var spanish = tw.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var translation = tw.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            spanish.text = SaveWords.wordList[i];
            translation.text = SaveWords.translatedWordList[i];

            allWords.Add(tw);
        }

        AllReady.transform.SetAsLastSibling();

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ReviseLists();
    }

    public void ReviseLists()
    {
        Debug.Log("Removing and Identifying words in lists");

        RemoveWords();

        IdentifyKnownWords();
        SaveData.StartSaving();
    }

    public void RemoveWords()
    {
        var i = 0;
        while (i < allWords.Count)
        {
            if (allWords[i].transform.GetChild(3).GetComponent<Toggle>().isOn)
            {
                SaveWords.knownOrNotList.RemoveAt(i);
                SaveWords.wordList.RemoveAt(i);
                SaveWords.translatedWordList.RemoveAt(i);
                allWords.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    public void IdentifyKnownWords()
    {
        for (int i = 0; i < allWords.Count; i++)
        {
            if (allWords[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                SaveWords.knownOrNotList[i] = true;
            }
        }
    }

}
