using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

public class LoginScene : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisterButton;
    public Button ForgotPasswordTextButton;
    public InputField UsernameField;
    public InputField PasswordField;
    public GameObject ErrorModal;
    public Text ErrorText;
    public static string AccessToken { get; private set; }
    private static string IdToken { get; set; }
    
    private const string RegisterString = "Register";
    private const string HomeString = "Home";


    void Start()
    {
        LoginButton.onClick.AddListener(() => _ = Login());
        RegisterButton.onClick.AddListener(() => SceneManager.LoadScene(RegisterString));
    }

    private async Task Login()
    {
        string username = UsernameField.text;
        string password = PasswordField.text;

        // Sets up the unsigned request to AWS's Cognito service for user
		var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), CognitoConfig.Region);
        var userPool = new CognitoUserPool(CognitoConfig.PoolID, CognitoConfig.AppClientID, provider);
        var user = new CognitoUser(username, CognitoConfig.AppClientID, userPool, provider);
        var authRequest = new InitiateSrpAuthRequest() { Password = password };

        try
        {
            var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(true);
            IdToken = authResponse.AuthenticationResult.IdToken;
            AccessToken = authResponse.AuthenticationResult.AccessToken;
            Debug.Log("User Access Token: " + IdToken);
            CognitoConfig.credentials.AddLogin(CognitoConfig.UserPoolProvider, IdToken);
            SceneManager.LoadScene(HomeString);
        }
        catch (UserNotFoundException e)
        {
            ErrorText.text = "Username or password is invalid!";
            ErrorModal.SetActive(true);
            
            Debug.Log("UserNotFoundException occured during login: " + e);
            Debug.Log("Here!");
        }
        catch (Exception e)
        {
            Debug.Log("Exception occured during login: " + e);
            ErrorText.text = "Something went wrong: " + e;
            ErrorModal.SetActive(true);
        }

    }

    // Probably should be in CognitoConfig file... I'll keep it here for now
    //public static async Task<Dictionary<string, string>> GetUserId()
    //{
    //    Debug.Log("Getting user's id...");

    //    Task<GetUserResponse> responseTask = provider.GetUserAsync(new GetUserRequest { AccessToken = AccessToken });
    //    GetUserResponse responseObject = await responseTask;

    //    string subId = "";
    //    foreach (var attribute in responseObject.UserAttributes)
    //    {
    //        if (attribute.Name == "sub")
    //        {
    //            subId = attribute.Value;
    //            break;
    //        }
    //    }
    //    return new Dictionary<string, string> { { "username", subId } };
    //}
}
