using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Android;

public class SceneSwitcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int nextScene;
    [SerializeField] private GameObject button;
    [SerializeField] private int right;
    [SerializeField] private int down;
        
    [SerializeField] private GameObject inputField;

    // Able to add sounds to button clicks on scene switch
    // [SerializeField] private AudioClip _compressClip, _uncompressClip;
    // [SerializeField] private AudioSource _source


    public void OnPointerDown(PointerEventData eventData)
    {
        button.transform.Translate(Vector2.right * right);
        button.transform.Translate(-Vector2.up * down);
        //  _source.PlayOneShot(_compressClip);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        button.transform.Translate(-Vector2.right * right);
        button.transform.Translate(Vector2.up * down);
        //  _source.PlayOneShot(_uncompressClip);

        if (nextScene == 5)
        {
            if(inputField.GetComponent<TMP_InputField>().text != "")
            {
                SaveWords.inputText = inputField.GetComponent<TMP_InputField>().text;
            } 
            else if(ConvertPDF.filePath != null) 
            {
                ConvertPDF.ExtractTextFromPdf();
            }
            Debug.Log("Text set in SaveWords script");
        }

        SwitchScene(nextScene);
    }

    public static void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

}
