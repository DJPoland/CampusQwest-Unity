using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLeaderboard : MonoBehaviour
{
    private List<Leaderboard> _leaderboardData;
    private bool _updatedLeaderboard;
    private List<GameObject> _gameObjectCache = new List<GameObject>();

    public GameObject playerObjectSchema;
    public Dropdown qwestDropdown;

    // Update is called once per frame

    void Start()
    {
        qwestDropdown.onValueChanged.AddListener(delegate
        {
            UpdateLeaderboard(qwestDropdown);
        });
    }

    void Update()
    {
        
        if (_leaderboardData == null)
        {
            
            _leaderboardData = APIGatewayController.GETLeaderboardData();
            
        } 
        else if (!_updatedLeaderboard)
        {
            Debug.Log("Leaderboard being created");
           
            CreateUsersForQwest(1);
        
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

            CreateUsersForQwest(change.value + 1);
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

    private void CreateUsersForQwest(int qwestId)
    {
        var selectedLeaderboard = _leaderboardData.First(leaderboard => leaderboard.QwestId == qwestId);
        float positionY = playerObjectSchema.transform.position.y;
        foreach (var userMetadata in selectedLeaderboard.MetadataUsers)
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
}
