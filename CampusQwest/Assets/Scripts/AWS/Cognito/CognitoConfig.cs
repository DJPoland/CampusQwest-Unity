using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;

public class CognitoConfig : MonoBehaviour
{
    public const string PoolID = "us-east-1_qRJ2N0GOu";
    public const string UserPoolProvider = "cognito-idp.us-east-1.amazonaws.com/us-east-1_qRJ2N0GOu";
    public const string AppClientID = "5dujj59nunfh7qkb9lce7jl8os";
    public const string IdentityPoolID = "us-east-1:7022d3f1-978e-4219-8827-8a9fc186f1c7";

    public static RegionEndpoint Region = RegionEndpoint.USEast1;
    public static CognitoAWSCredentials credentials = new CognitoAWSCredentials(IdentityPoolID, Region);
}
