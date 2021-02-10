using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.Extensions.CognitoAuthentication;

public class LoginScene : MonoBehaviour
{
    public Button LoginButton;
    public Button RegisterButton;
    public Button ForgotPasswordTextButton;
    public InputField UsernameField;
    public InputField PasswordField;

    public static string AccessToken { get; private set; }

    private const string RegisterString = "Register";
    private const string HomeString = "Home";


    void Start()
    {
        LoginButton.onClick.AddListener(() => _ = Login());
        //ForgotPasswordTextButton.onClick.AddListener(() => _ = ForgotPassword());
        RegisterButton.onClick.AddListener(() => SceneManager.LoadScene(RegisterString));
    }

    private async Task Login()
    {
        Debug.Log("Login button clicked");
        string username = UsernameField.text;
        string password = PasswordField.text;

        // Sets up the unsigned request to AWS's Cognito service for user
        CognitoUserPool userPool = new CognitoUserPool(CognitoConfig.PoolID, CognitoConfig.AppClientID, CognitoConfig.provider);
        CognitoUser user = new CognitoUser(username, CognitoConfig.AppClientID, userPool, CognitoConfig.provider);
        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest(){ Password = password };

        try
        {
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(true);
            AccessToken = authResponse.AuthenticationResult.AccessToken;
            Debug.Log("User Access Token: " + AccessToken);
            SceneManager.LoadScene(HomeString);
	    }
        catch (Exception e)
        {
            Debug.Log("Exception occured during login: " + e);
            return;
	    }

    }

    //private async Task ForgotPassword() 
    //{
    //    // TODO: Bring user to screen which prompts for user's username/password.
    //    Debug.Log("Forgot Password button clicked");
    //}
}
