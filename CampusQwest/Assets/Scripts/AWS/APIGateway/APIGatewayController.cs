using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class APIGatewayController : MonoBehaviour
{
    private const string URL = "https://0kc0gke4s2.execute-api.us-east-1.amazonaws.com";
    private static APIGatewayController _instance;
    private User _userData;
    private List<Qwest> _qwests;
    private List<Leaderboard> _leaderboardData;

    void Start()
    {
        // Requests that are made at user startup
        try
        {
            StartCoroutine(GetUserData("/user"));
            StartCoroutine(GetQwests("/user/qwests/fetchQwests"));
        }
        catch (Exception e)
        {
            Debug.Log("Error with fetching data: " + e);
        }
    }
    
    void Awake() { 
        if (_instance == null) {
            DontDestroyOnLoad(gameObject);
            _instance = this;
		} else if (_instance != this)
        {
            Destroy(gameObject);
		}
    }

    private static IEnumerator GetQwests(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL + uri))
        {
            // Request with user information and wait for data.
            www.SetRequestHeader("Authorization", LoginScene.AccessToken);
            yield return www.SendWebRequest();

            string result = www.downloadHandler.text;
            Debug.Log(result);

            _instance._qwests = JsonConvert.DeserializeObject<List<Qwest>>(result);
        }
    }

    private static IEnumerator GetUserData(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL + uri))
        {
            // Request with user information and wait for data.
            www.SetRequestHeader("Authorization", LoginScene.AccessToken);
            yield return www.SendWebRequest();

            var result = www.downloadHandler.text;
            Debug.Log(result);
            _instance._userData = JsonConvert.DeserializeObject<User>(result);
        }
    }


    public static User GETUserData() {
        return _instance != null && _instance._userData != null ? _instance._userData : null;
    }

    public static List<Qwest> GETUserQwests() {
        return _instance != null && _instance._qwests != null ? _instance._qwests : null;
    }
    
    public static List<Leaderboard> GETLeaderboardData()
    {
        return _instance != null && _instance._leaderboardData != null ? _instance._leaderboardData : null;
    }
}
