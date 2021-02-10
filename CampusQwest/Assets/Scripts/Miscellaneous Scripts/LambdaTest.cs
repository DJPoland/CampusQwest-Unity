using UnityEngine;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;

public class LambdaTest : MonoBehaviour
{
    class AddNumData
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }

        public AddNumData(int n1, int n2)
        {
            this.Num1 = n1;
            this.Num2 = n2;
	    }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TestLambda() 
    {
        AmazonLambdaClient LambdaClient = new AmazonLambdaClient(CognitoConfig.credentials, CognitoConfig.Region);

        AddNumData addData = new AddNumData(2, 3);
        string payload = JsonConvert.SerializeObject(addData);
        InvokeRequest InvokeRequest = new InvokeRequest
        {
            FunctionName = "addNums",
            InvocationType = InvocationType.RequestResponse,
            Payload = payload
        };

        InvokeResponse LambdaResponse = LambdaClient.Invoke(InvokeRequest);
    }
}
