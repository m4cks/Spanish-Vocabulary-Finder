using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExportScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpObject;

    public void Start()
    {

        string textToExport = "";
        for(int i = 0; i < SaveWords.wordList.Count; i++)
        {
            if (!SaveWords.knownOrNotList[i])
            {
                textToExport += SaveWords.wordList[i] + "\t" + SaveWords.translatedWordList[i];
                textToExport += "\n";

                //Debug.Log(textToExport);
            }
        }
        tmpObject.text = textToExport;

    }

    public void CopyToClipboardMethod()
    {
        GUIUtility.systemCopyBuffer = tmpObject.text;
        Debug.Log($"Copied to Clipboard:\n{tmpObject.text}");
    }
}
