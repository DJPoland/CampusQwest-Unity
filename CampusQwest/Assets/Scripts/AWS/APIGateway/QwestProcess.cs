using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QwestProcess : MonoBehaviour
{
    private const string URL = "https://0kc0gke4s2.execute-api.us-east-1.amazonaws.com";
    private static QwestProcess _instance;
    private Qwest _qwest;
    private User _user;
    private string _locationImage;
    private long _index;
    public Text clueText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Create Qwest Instance
    void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static IEnumerator PostStartQwest(string uri, int qwestId)
    {
        var jsonString = "{\"id\":" + qwestId +"}";
        using (UnityWebRequest www = UnityWebRequest.Post(URL + uri, jsonString))
        {
            www.SetRequestHeader("Authorization", LoginScene.AccessToken);
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.responseCode == 201)
            {
                // Update user data somehow
            }
            else
            {
                // invalid response and an exception should be thrown
            }
        }
    }

    private static void ChangeLocation(long index)
    {
        _instance.clueText.text = _instance._qwest.Locations[index].Clue;
        
        _instance._locationImage = _instance._qwest.Locations[_instance._index].AssetImage;
    }

    public static string GETLocationImage()
    {
        return _instance != null && _instance._qwest != null ? _instance._locationImage : null;
    }
}
