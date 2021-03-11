using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgotPasswordScene : MonoBehaviour
{
    public Button sendEmailButton;
    public InputField emailField;
    public GameObject resetCodeField;
    public GameObject text;
    public GameObject resetPasswordButton;

    private InputField _resetCodeValue;
    private Button _resetEmailButton;

    void Start()
    {
        _resetCodeValue = resetCodeField.GetComponent<InputField>();
        _resetEmailButton = resetPasswordButton.GetComponent<Button>();
        
        _resetCodeValue.onValueChanged.AddListener(arg0 => CacheCode.Instance.confirmationCode = arg0 );
        _resetEmailButton.onClick.AddListener(ValidateCode);
        sendEmailButton.onClick.AddListener(() => _ = ForgotPassword());
    }

    private async Task ForgotPassword()
    {
        string email = emailField.text;
        
        var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), CognitoConfig.Region);
        var userPool = new CognitoUserPool(CognitoConfig.PoolID, CognitoConfig.AppClientID, provider);
        var user = new CognitoUser(email, CognitoConfig.AppClientID, userPool, provider);
        try
        {
            await user.ForgotPasswordAsync().ConfigureAwait(true);
            text.SetActive(true);
            resetCodeField.SetActive(true);
            resetPasswordButton.SetActive(true);
            CacheCode.Instance.email = email;
        }
        catch (Exception e)
        {
            Debug.Log("Exception occured during forgot password scene: " + e);
        }
    }

    private void ValidateCode()
    {
        if (_resetCodeValue.text.Length != 6)
        {
            // TODO: Show modal indicating code length is invalid.
            Debug.Log("Invalid code length. Code length should be 6 characters");
            return;
        }

        SceneManager.LoadScene("ResetPassword");
    }
}
