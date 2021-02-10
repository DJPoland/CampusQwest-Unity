using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.CognitoIdentityProvider.Model;

public class RegisterScene : MonoBehaviour
{
    public Button RegisterButton;
    public InputField UsernameField;
    public InputField PasswordField;
    public InputField ConfirmField;
    public InputField EmailField;

    private const string Login = "Login";

    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(() => _ = Register());
    }

    // TODO: Passwords should meet a length requirement
    // If the password and confirmPassword fields are not equivalent,
    // display a popup dialog
    // Same thing if the email is not valid
    private async Task Register()
    {
        Debug.Log("Signup method called");
        string username = UsernameField.text;
        string password = PasswordField.text;
        string confirmPassword = ConfirmField.text;
        string email = EmailField.text;

        SignUpRequest signUpRequest = CreateIdentityPoolRequest(username, password, email);
        try
        {
            ValidRegistration(password, confirmPassword, email);
            SignUpResponse request = await CognitoConfig.provider.SignUpAsync(signUpRequest);
            Debug.Log("Sign up was successful");
            SceneManager.LoadScene(Login);
        }
        catch (Exception e)
        {
            Debug.Log("Exception during registration" + e);
            return;
        }
    }

    private void ValidRegistration(string Password, string ConfirmPassword, string Email) 
    { 
        if (Password != ConfirmPassword)
        {
            Debug.LogError("Password and confirm password fields are invalid");
            throw new Exception("Invalid Registration");
        }
        else if (!Validator.EmailIsValid(Email)) {
            // TODO: Maybe we should throw a custom exception here.
            Debug.LogError("Email format is invalid");
            throw new Exception("Invalid Registration");
	    }
    }

    private SignUpRequest CreateIdentityPoolRequest(string username, string password, string email) 
    {
        SignUpRequest signUpRequest = new SignUpRequest()
        { 
	        ClientId = CognitoConfig.AppClientID, 
	        Username = username, 
	        Password = password 
	    };
        List<AttributeType> attributes = new List<AttributeType>()
        {
			new AttributeType() { Name = "email", Value = email }
        };
        signUpRequest.UserAttributes = attributes;
        return signUpRequest;
    }
}

