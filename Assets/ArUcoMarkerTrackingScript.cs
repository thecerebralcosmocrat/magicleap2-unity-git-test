using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ArUcoMarkerTrackingScript : MonoBehaviour
{
    public float qrCodeMarkerSize; // size of the expected QR code marker in meters
    public float arucoMarkerSize; // size of the expected ArCco marker in meters
    public MLMarkerTracker.MarkerType markerType; // is the detected marker a QR? ArUco? EAN_13? what type of marker?
    public MLMarkerTracker.ArucoDictionaryName arucoDict; // for ArUco markers, which "dictionary" or type of ArUco markers?

    // this is the object that will be instantiated on the detected marker
    public GameObject TrackerObjectPrefab;

    private void Start()
    {
        // create a tracker settings object with variables defined above
        MLMarkerTracker.TrackerSettings trackerSettings = MLMarkerTracker.TrackerSettings.Create(true, markerType, qrCodeMarkerSize, arucoDict, arucoMarkerSize, MLMarkerTracker.Profile.Default);

        // start marker tracking with tracker settings object and callback function
        _ = MLMarkerTracker.SetSettingsAsync(trackerSettings);
    }

    // subscribe to the event that detected markers
    private void OnEnable()
    {
        MLMarkerTracker.OnMLMarkerTrackerResultsFound += OnTrackerResultsFound;
    }

    //  when the marker is successfully detected this function is called
    private void OnTrackerResultsFound(MLMarkerTracker.MarkerData data)
    {
        // instantiate the tracker prefab object to align with worldspace up
        GameObject obj = Instantiate(TrackerObjectPrefab, data.Pose.position, data.Pose.rotation);
        obj.transform.up = Vector3.up;

        // stop scanning after object has been instantiased
        _ = MLMarkerTracker.StopScanningAsync();
    }
}
