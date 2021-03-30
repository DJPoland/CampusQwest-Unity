using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DisplayLeaderboard : MonoBehaviour
{
    private const string URL = "https://0kc0gke4s2.execute-api.us-east-1.amazonaws.com";
    private static List<Leaderboard> _leaderboardData;
    private bool _updatedLeaderboard;
    private readonly List<GameObject> _gameObjectCache = new List<GameObject>();

    public GameObject playerObjectSchema;
    public Dropdown qwestDropdown;

    // Update is called once per frame

    void Start()
    {
        qwestDropdown.value = 0;
        StartCoroutine(GetLeaderboardsForQwests("/leaderboard", 1));
        qwestDropdown.onValueChanged.AddListener(delegate
        {
            UpdateLeaderboard(qwestDropdown);
        });
    }

    void Update()
    {
        
        if (!_updatedLeaderboard && _leaderboardData != null)
        {
            Debug.Log("Leaderboard being created");
           
            CreateUsersForQwest();
        
            _updatedLeaderboard = true;
        }
        
    }

    private void UpdateLeaderboard(Dropdown change)
    {
        try
        {
            for (int i = _gameObjectCache.Count() - 1; i >= 0; i--)
            {
                var obj = _gameObjectCache[i];
                _gameObjectCache.RemoveAt(i);
                Destroy(obj);
            }

            Debug.Log(change.value + 1);

            _leaderboardData = null;
            _updatedLeaderboard = false;
            
            StartCoroutine(GetLeaderboardsForQwests("/leaderboard", change.value + 1));
        }
        catch (InvalidOperationException e)
        {
            Debug.LogError("Qwest ID selected does not exist: " + e);
        }
        catch (Exception e)
        {
            Debug.LogError("Error with updating leaderboard: " + e);
        }
    }

    private void CreateUsersForQwest()
    {
        var selectedLeaderboard = _leaderboardData;
        float positionY = playerObjectSchema.transform.position.y;
        foreach (var userMetadata in selectedLeaderboard)
        {
            var user = userMetadata.Username;
            var totalTime = userMetadata.TotalTime;
            var selectedAvatar = userMetadata.SelectedAvatar;
            
            var userPanelObj = Instantiate(playerObjectSchema, transform, false);
            
            var usernameText = userPanelObj.transform.Find("Name").GetComponent<Text>();
            var scoreText = userPanelObj.transform.Find("Score").GetComponent<Text>();
            var profileImageComponent = userPanelObj.transform.Find("Image").GetComponent<Image>();
            var selectedSprite = Resources.Load<Sprite>("Images/Avatars/avatar" + selectedAvatar);
            
            usernameText.text = user;
            scoreText.text = totalTime;
            profileImageComponent.sprite = selectedSprite;
            
            var positionX = userPanelObj.transform.position.x;
            userPanelObj.transform.position = new Vector3(positionX, positionY);
            
            positionY -= 207; 
            
            userPanelObj.SetActive(true);
            _gameObjectCache.Add(userPanelObj);
        }
    }
    
    private static IEnumerator GetLeaderboardsForQwests(string uri, int qwestId)
    {
        var jsonString = "{\"qwestId\":" + qwestId +"}";
        using (UnityWebRequest www = UnityWebRequest.Post(URL + uri, jsonString))
        {
            // Request with user information and wait for data.
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            var result = www.downloadHandler.text;
            Debug.Log(result);

            _leaderboardData = JsonConvert.DeserializeObject<List<Leaderboard>>(result);
        }
    }
}
