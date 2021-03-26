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
    public Button registerButton;
    public InputField usernameField;
    public InputField passwordField;
    public InputField confirmField;
    public InputField emailField;
    public GameObject errorModal;
    public Text errorText;
    public Dropdown dropdownCampus;

    private const string Login = "Login";

    // Start is called before the first frame update
    void Start()
    {
        registerButton.onClick.AddListener(() => _ = Register());
    }

    private async Task Register()
    {
        Debug.Log("Signup method called");
        string username = usernameField.text;
        string password = passwordField.text;
        string confirmPassword = confirmField.text;
        string email = emailField.text;

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
            errorText.text = "Something went wrong...";
            errorModal.SetActive(true);
            return;
        }
    }

    private void ValidRegistration(string password, string confirmPassword, string email)
    {
        if (password != confirmPassword)
        {
            Debug.LogError("Password and confirm password fields are invalid");
            errorText.text = "Password and confirm password fields are invalid!";
            errorModal.SetActive(true);
            throw new Exception("Invalid Registration");
        }

        if (!Validator.EmailIsValid(email)) {
            Debug.LogError("Email format is invalid");
            errorText.text = "Email format is invalid!";
            errorModal.SetActive(true);
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
        string campusValue = dropdownCampus.options[dropdownCampus.value].text;
        var attributes = new List<AttributeType>()
        {
			new AttributeType() { Name = "email", Value = email },
            new AttributeType() { Name = "custom:campus", Value = campusValue }
        };
        signUpRequest.UserAttributes = attributes;
        return signUpRequest;
    }
}

