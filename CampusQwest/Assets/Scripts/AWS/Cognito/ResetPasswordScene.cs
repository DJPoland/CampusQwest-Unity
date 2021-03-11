using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetPasswordScene : MonoBehaviour
{
    public InputField newPasswordField;
    public InputField confirmPasswordField;
    public Button resetPasswordButton;
    // Start is called before the first frame update
    void Start()
    {
        resetPasswordButton.onClick.AddListener(() => _ = ChangePasswordForUser());
    }

    private async Task ChangePasswordForUser()
    {
        string password = newPasswordField.text;
            
            
        var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), CognitoConfig.Region);
        var userPool = new CognitoUserPool(CognitoConfig.PoolID, CognitoConfig.AppClientID, provider);
        var user = new CognitoUser(CacheCode.Instance.email, CognitoConfig.AppClientID, userPool, provider);
        try
        {
            ValidateInputFields(password);
            await user.ConfirmForgotPasswordAsync(CacheCode.Instance.confirmationCode, password).ConfigureAwait(true);
        }
        catch (InvalidLambdaResponseException e)
        {
            // idk why this gets thrown when the password change
            // is successful. I think this is a bug with the Cognito
            // service.
            SceneManager.LoadScene("Login");
        }
        catch (InvalidParameterException e)
        {
            // TODO: Show model indicating password is invalid
            // due to being too short/long or using invalid chars.
            Debug.LogError("New Password is too short/long or uses invalid characters" + e);
            SceneManager.LoadScene("ForgotPassword");
        }
        catch (Exception e)
        {
            Debug.LogError("Exception occured during reset password scene: " + e);
        }
    }

    private void ValidateInputFields(string password)
    {
        string confirmNewPassword = confirmPasswordField.text;
        if (password != confirmNewPassword)
        {
            Debug.LogError("Password and confirm password fields are invalid");
            // ErrorText.text = "Password and confirm password fields are invalid!";
            // ErrorModal.SetActive(true);
            throw new Exception("Invalid Registration");
        }
    }
    
}
