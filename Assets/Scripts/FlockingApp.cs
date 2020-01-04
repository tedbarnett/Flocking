using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class FlockingApp : MonoBehaviour
{
    private Vector2 resolution; // Current Resolution
    private Transform cameraTransform;
    private Vector3 cameraPosition;
    public Text statusText;
    private DeviceOrientation lastOrientation;
    private CloudFine.FlockBox flockBox;
    //private CloudFine.BehaviorSettings flockBehavior;
    public Button modeSwitch;
    private bool isPortraitMode;
    private Vector2 fingerDelta;

    public float containmentBehaviorWeight;
    //public CloudFine.AlignmentBehavior alignmentBehavior;
    //public CloudFine.CohesionBehavior cohesionBehavior;
    //public CloudFine.SeparationBehavior separationBehavior;

    void Start()
    {
        //DeviceChange.OnOrientationChange += MyOrientationChangeCode; // detect screen rotation
        flockBox = GetComponent<CloudFine.FlockBox>();
        cameraTransform = FindObjectOfType<Camera>().GetComponent<Transform>();
        lastOrientation = Input.deviceOrientation;
        statusText.text = "";
        modeSwitch.GetComponent<Button>().onClick.AddListener(ChangeOrientation);
        isPortraitMode = true;
        LeanTouch.OnGesture += HandleGesture;

    }

    private void Update()
    {
        if (lastOrientation == Input.deviceOrientation) return;
        ChangeOrientation();
    }

    private void HandleGesture(List<LeanFinger> fingers)
    {
        //Debug.Log("screen delta: " + LeanGesture.GetScreenDelta(fingers));
        fingerDelta = LeanGesture.GetScreenDelta(fingers);
        if (fingerDelta.x > 0.0 || fingerDelta.x < 0.0)
        {
            statusText.text = "x: " + fingerDelta.x;
            containmentBehaviorWeight = 2.0f;
            //alignmentBehavior.weight = 0.0f;
            //cohesionBehavior.weight = 0.0f;
            //separationBehavior.weight = 0.0f;
        }
        if (fingerDelta.y > 0.0 || fingerDelta.y < 0.0)
        {
            statusText.text = "y: " + fingerDelta.y;
            containmentBehaviorWeight = 3.0f;
            //alignmentBehavior.weight = 3.0f;
            //cohesionBehavior.weight = 3.0f;
            //separationBehavior.weight = 3.0f;

        }

    }

    private void ChangeOrientation()
    {
        isPortraitMode = !isPortraitMode;
        if (!isPortraitMode || Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            // Device is in landscape mode
            //statusText.text = "Landscape";
            cameraPosition = new Vector3(94.0f, 58.0f, -102f);
            cameraTransform.position = cameraPosition;
            flockBox.dimensions_x = 21;
            flockBox.dimensions_y = 10;
            flockBox.dimensions_z = 8;
        }
        else
        {
            // Device is in portrait mode
            //statusText.text = "Portrait";
            cameraPosition = new Vector3(55.0f, 112.0f, -202.0f);
            cameraTransform.position = cameraPosition;
            flockBox.dimensions_x = 10;
            flockBox.dimensions_y = 21;
            flockBox.dimensions_z = 8;

        }
        lastOrientation = Input.deviceOrientation;
    }

    void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            // Application came back to the fore; double-check authentication
            statusText.text = "Just opened app at " + System.DateTime.Now;
        }
    }
} // class
