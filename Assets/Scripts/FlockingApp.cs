using System.Collections;
using System.Collections.Generic;
//using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class FlockingApp : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 cameraPosition;
    public Text statusText;
    private DeviceOrientation lastOrientation;
    private bool isPortraitMode;
    //private Vector2 fingerDelta;
    public Slider separationWeightSlider; // used to set FlockBox Separation Behavior

    private CloudFine.FlockBox flockBox;

    //TODO: How do I access (and change) FlockBox Behavior values?
    //public float containmentBehaviorWeight;
    //public CloudFine.AlignmentBehavior alignmentBehavior;
    //public CloudFine.CohesionBehavior cohesionBehavior;
    public CloudFine.SeparationBehavior separationBehavior; // does not seem to work!

    void Start()
    {
        flockBox = GetComponent<CloudFine.FlockBox>();
        cameraTransform = FindObjectOfType<Camera>().GetComponent<Transform>();
        //separationWeightSlider = GetComponent<Slider>();
        separationWeightSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SliderChanged(); });

        lastOrientation = Input.deviceOrientation;
        statusText.text = "";
        isPortraitMode = true;
        //LeanTouch.OnGesture += HandleGesture;
    }

    private void Update()
    {
        if (lastOrientation == Input.deviceOrientation) return;
        ChangeOrientation();
    }

    public void SliderChanged() // TODO: Make FlockBox Behaviors change here!
    {
        statusText.text = "Separation: " + separationWeightSlider.value.ToString();
        separationBehavior.weight = separationWeightSlider.value; //ERROR: not set to an instance of an object?

    }

    // TODO: Replace sliders with LeanTouch finger gestures (swipes to change Flock Behavior values)
    //private void HandleGesture(List<LeanFinger> fingers)
    //{
    //    //Debug.Log("screen delta: " + LeanGesture.GetScreenDelta(fingers));
    //    fingerDelta = LeanGesture.GetScreenDelta(fingers);
    //    if (fingerDelta.x > 0.0 || fingerDelta.x < 0.0)
    //    {
    //        statusText.text = "x: " + fingerDelta.x;
    //        containmentBehaviorWeight = 2.0f;
    //        alignmentBehavior.weight = 0.0f;
    //        cohesionBehavior.weight = 0.0f;
    //        separationBehavior.weight = 0.0f;
    //    }
    //    if (fingerDelta.y > 0.0 || fingerDelta.y < 0.0)
    //    {
    //        statusText.text = "y: " + fingerDelta.y;
    //        containmentBehaviorWeight = 3.0f;
    //        alignmentBehavior.weight = 3.0f;
    //        cohesionBehavior.weight = 3.0f;
    //        separationBehavior.weight = 3.0f;

    //    }
    //}

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
            statusText.text = "Just opened app at " + System.DateTime.Now; //TODO: Testing this for Photo Reviewer app
        }
    }
} // class
