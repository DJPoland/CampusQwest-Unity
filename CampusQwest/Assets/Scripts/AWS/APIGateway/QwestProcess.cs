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
    private Location _location;
    private long _index;

    public Text clueText;
    public Slider thermo;

    void Update()
    {
        if (_instance._location != null && Input.location.status == LocationServiceStatus.Running)
        {
            QwestProcess.CheckDistance();
        }
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

    public static void SetQwestInfo()
    {
        Debug.Log("Setting Qwest Info");
        // This includes a check from Post Start to see if user if available from the web update.
        _instance._user = null;
        while (_instance._user == null)
        {
            _instance._user = APIGatewayController.GETUserData();
        }
        while (_instance._qwest == null)
        {
            _instance._qwest = APIGatewayController.GETQwestById(_instance._user.CurrentQwest.QwestId);
        }

        _instance._index = _instance._user.CurrentQwest.LocationIndex;
        QwestProcess.SetLocation(_instance._index);
    }



    public static IEnumerator PostStartQwest(string uri, int qwestId)
    {
        Debug.Log("Entered Starting a qwest with id " + qwestId);
        WWWForm form = new WWWForm();
        form.AddField("id", qwestId);
        using (UnityWebRequest www = UnityWebRequest.Post(URL + uri, form))
        {
            www.SetRequestHeader("Authorization", LoginScene.AccessToken);
            yield return www.SendWebRequest();

            if (www.responseCode == 201)
            {
                APIGatewayController.ResetAPIControllerInstance();
                QwestProcess.SetQwestInfo();
            }
            else
            {
                Debug.Log("Could not start Scripting process. " + www.responseCode +" " + www.downloadHandler.text);
            }
        }

    }

    private static void SetLocation(long index)
    {
        _instance._location = _instance._qwest.Locations[index];
        _instance._locationImage = _instance._location.AssetImage;
        _instance.clueText.text = _instance._location.Clue;

        if (Input.location.status != LocationServiceStatus.Running)
        {
            QwestProcess.LocationService();
        }
    }

    private static IEnumerator LocationService()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
    }

    public static void CheckDistance()
    {
        float latitude = Input.location.lastData.latitude + _instance._location.Latitude;
        float longitude = Input.location.lastData.longitude + _instance._location.Longitude;
        float distance = Mathf.Sqrt((latitude * latitude) + (longitude * longitude));
        float target = 100 - distance;

        _instance.thermo.value = target > 0 ? target : 0;
    }

    public static string GETLocationImage()
    {
        return _instance != null && _instance._qwest != null ? _instance._locationImage : null;
    }

/*    public static IEnumerator NextLocation(string uri)
    {
        Input.location.Stop();
        using (UnityWebRequest www = UnityWebRequest.Post(URL + uri))
        {
            www.SetRequestHeader("Authorization", LoginScene.AccessToken);
            yield return www.SendWebRequest();

            if (www.responseCode == 201)
            {
                var result = www.downloadHandler.text;

                if
                QwestProcess.SetQwestInfo();
            }
            else
            {
                Debug.Log("Could not start Scripting process. " + www.responseCode);
            }
        }
    }*/
}
