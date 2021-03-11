using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;

public class RegisterScene : MonoBehaviour
{
    public Button RegisterButton;
    public InputField UsernameField;
    public InputField PasswordField;
    public InputField ConfirmField;
    public InputField EmailField;
    public GameObject ErrorModal;
    public Text ErrorText;


    private const string Login = "Login";

    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(() => _ = Register());
    }

    private async Task Register()
    {
        Debug.Log("Signup method called");
        string username = UsernameField.text;
        string password = PasswordField.text;
        string confirmPassword = ConfirmField.text;
        string email = EmailField.text;

        try
        {
            ValidRegistration(password, confirmPassword, email);
            var signUpRequest = CreateIdentityPoolRequest(username, password, email);
            var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), CognitoConfig.Region);
            await provider.SignUpAsync(signUpRequest);
            Debug.Log("Sign up was successful");
            SceneManager.LoadScene(Login);
        }
        catch (Exception e)
        {
            Debug.Log("Exception during registration: " + e);
            ErrorText.text = "Something went wrong...";
            ErrorModal.SetActive(true);
            return;
        }
    }

    private void ValidRegistration(string password, string confirmPassword, string email)
    {
        if (password != confirmPassword)
        {
            Debug.LogError("Password and confirm password fields are invalid");
            ErrorText.text = "Password and confirm password fields are invalid!";
            ErrorModal.SetActive(true);
            throw new Exception("Invalid Registration");
        }

        if (!Validator.EmailIsValid(email)) {
            Debug.LogError("Email format is invalid");
            ErrorText.text = "Email format is invalid!";
            ErrorModal.SetActive(true);
            throw new Exception("Invalid Registration");
        }
    }

    private SignUpRequest CreateIdentityPoolRequest(string username, string password, string email) 
    {
        var signUpRequest = new SignUpRequest()
        { 
	        ClientId = CognitoConfig.AppClientID, 
	        Username = username, 
	        Password = password 
	    };
        var attributes = new List<AttributeType>()
        {
			new AttributeType() { Name = "email", Value = email }
        };
        signUpRequest.UserAttributes = attributes;
        return signUpRequest;
    }
}

