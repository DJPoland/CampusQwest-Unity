using UnityEngine;
using UnityEngine.UI;

public class DisplayProfileData : MonoBehaviour
{
    private User userData = null;
    private bool updatedUserData = false;

    public Slider slider;
    public Image profileImage;
    public Text username;

    // Start is called before the first frame update
    void Update()
    {
        if (userData == null) {
            userData = APIGatewayController.GETUserData();
		} else if (!updatedUserData) { 
			Debug.Log("Experience " + userData.Exp);
            slider.value = userData.Exp;        
		    Debug.Log("Username " + userData.Username);
            username.text = userData.Username;
            Debug.Log("Rank " + userData.Rank);

            updatedUserData = true;
		}

        // TODO: get profile image for user.
    }
}
