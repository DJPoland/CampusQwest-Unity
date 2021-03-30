using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private const string URL = "https://0kc0gke4s2.execute-api.us-east-1.amazonaws.com";
    private int _itemNumber;
    private bool _isBanner;
    
    public GameObject PopupBox;
    public Text text;
    public Button equipButton;
    public string[] descriptions;

    void Start()
    {
        equipButton.onClick.AddListener(delegate
        {
            StartCoroutine(UpdateProfile("/user/updateProfile", _itemNumber, _isBanner));
        });
    }

    public void enablePopupBox(int itemNumber)
    {
        _itemNumber = itemNumber;
        if (itemNumber >= 10)
        {
            _isBanner = true;
        }
        PopupBox.SetActive(true);
        text.text = descriptions[itemNumber];
    }
    
    private static IEnumerator UpdateProfile(string uri, int selected, bool isBanner)
    {
        string jsonString = "";
        if (isBanner)
        {
            selected %= 10;
            jsonString = "{\"selectedAvatar\":" + -1 +", \"selectedBanner\":" + selected + "}";
        }
        else
        {
            jsonString = "{\"selectedAvatar\":" + selected +", \"selectedBanner\":" + -1 + "}";
        }
        
        using (UnityWebRequest www = UnityWebRequest.Post(URL + uri, jsonString))
        {
            // Request with user information and wait for data.
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            var result = www.downloadHandler.text;
            Debug.Log(result);
            
            // TODO: check status code 
        }
    }
}
