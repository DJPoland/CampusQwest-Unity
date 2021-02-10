using UnityEngine;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;

public class CognitoConfig : MonoBehaviour
{
    public const string PoolID = "us-east-1_BaXu0YkaN";
    public const string AppClientID = "56vsgokcpa2cfog3alqvocgusb";
    public const string IdentityPoolID = "us-east-1:57ad2e69-3d2f-4964-8052-276e593a5ea9";
    public static RegionEndpoint Region = RegionEndpoint.USEast1;
    public static AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), CognitoConfig.Region);
    public static CognitoAWSCredentials credentials = new CognitoAWSCredentials(IdentityPoolID, Region);
}
