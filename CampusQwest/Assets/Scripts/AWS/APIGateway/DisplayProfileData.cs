using UnityEngine;
using UnityEngine.UI;

public class DisplayProfileData : MonoBehaviour
{
    private User _userData;
    private bool _updatedUserData = false;

    public Slider slider;
    public Text username;
    public Text rank;
    public Image[] trophies;
    public Image[] medals;
    public Image avatar;

    void Update()
    {
        if (_userData == null) {
            _userData = APIGatewayController.GETUserData();
		} else if (!_updatedUserData) { 
			Debug.Log("Experience " + _userData.Exp);
            slider.value = _userData.Exp;        
		    Debug.Log("Username " + _userData.Username);
            username.text = _userData.Username;
            Debug.Log("Rank " + _userData.Rank);
            rank.text = _userData.Rank;
            ShowUnlockedTrophies();
            ShowUnlockedMedals();

            var selectedSprite = Resources.Load<Sprite>("Images/Avatars/avatar" + _userData.SelectedAvatar);
            avatar.sprite = selectedSprite;

            _updatedUserData = true;
		}

        // TODO: get profile image for user.
    }

    private void ShowUnlockedTrophies()
    {
        if (trophies.Length != _userData.unlockedTrophies.Length)
        {
            Debug.Log("The backend is not communicating properly.");
            // TODO: Could throw an error to be more specific.
            return;
        }

        int i = 0;
        foreach (bool unlocked in _userData.unlockedTrophies)
        {
            if (unlocked)
            {
                trophies[i].color = Color.white;
            }
            i++;
        }
    }

    private void ShowUnlockedMedals()
    {
        if (medals.Length != _userData.unlockedMedals.Length)
        {
            Debug.Log("The backend is not communicating properly.");
            return;
        }
        
        int i = 0;
        foreach (bool unlocked in _userData.unlockedMedals)
        {
            if (unlocked)
            {
                medals[i].color = Color.white;
            }
            i++;
        }
        
    }
}