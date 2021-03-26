using UnityEngine;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    public ImageTargetBehaviour ImageTargetTemplate;
    public static bool whenOpen = false;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }


    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);

        }
    }

    void Update()
    {
        if(whenOpen){
            mCloudRecoBehaviour.CloudRecoEnabled = true;
            whenOpen = false;
        }
            
    }


    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {

        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;
        // do something with the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;

        // Save this statement for whenever we get database data and insert(where google-logo is) the current image that the user is trying to detect.
        // if(cloudRecoSearchResult.TargetName != "google-logo")
        //     return;


        // stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;

        Debug.Log("++++++++" + cloudRecoSearchResult.TargetName);

        // Hides the larger chests from being seen in the main camera view
        if (mTargetMetadata == "TreasureChest1meter")
        {
            GameObject.Find("TreasureChest1meter").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject.Find("TreasureChest1meter").transform.localScale = new Vector3(0, 0, 0);
        }

        // Hides the larger chests from being seen in the main camera view
        if (mTargetMetadata == "TreasureChest01MeterWall")
        {
            GameObject.Find("TreasureChest01MeterWall").transform.localScale = new Vector3((float)0.2, (float)0.2, (float)0.2);
        }
        else
        {
            GameObject.Find("TreasureChest01MeterWall").transform.localScale = new Vector3(0, 0, 0);
        }

        // Hides the larger chests from being seen in the main camera view
        if (mTargetMetadata == "TreasureChest01meter")
        {
            GameObject.Find("TreasureChest01meter").transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
        }
        else
        {
            GameObject.Find("TreasureChest01meter").transform.localScale = new Vector3(0, 0, 0);
        }


        // Build augmentation based on target
        if (ImageTargetTemplate)
        {
            // enable the new result with the same ImageTargetBehaviour: 
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);

        }

    }

    // void OnGUI()
    // {
    //     // Display current 'scanning' status
    //     GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
    //     // Display metadata of latest detected cloud-target
    //     GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
    //     // If not scanning, show button
    //     // so that user can restart cloud scanning
    //     if (!mIsScanning)
    //     {
    //         if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
    //         {
    //             // Restart TargetFinder
    //             mCloudRecoBehaviour.CloudRecoEnabled = true;
    //         }
    //     }
    // }


}