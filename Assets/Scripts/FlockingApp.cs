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
    public Slider containmentWeightSlider; // used to set FlockBox Behaviors
    public Slider alignmentWeightSlider;
    public Slider cohesionWeightSlider;
    public Slider separationWeightSlider;
    //public enum behaviorType { Containment, Alignment, Cohesion, Separation };

    private CloudFine.FlockBox flockBox;
    public CloudFine.BehaviorSettings boidSettings;
    public List<Slider> AllSliders = new List<Slider>();


    void Start()
    {
        AllSliders.Add(containmentWeightSlider);
        AllSliders.Add(alignmentWeightSlider);
        AllSliders.Add(cohesionWeightSlider);
        AllSliders.Add(separationWeightSlider);

        flockBox = GetComponent<CloudFine.FlockBox>();
        cameraTransform = FindObjectOfType<Camera>().GetComponent<Transform>();
        //separationWeightSlider = GetComponent<Slider>();
        //separationWeightSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SliderChanged(behaviorType behaviorIs); });

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

    public void SliderChanged(string sliderID) // TODO: Make FlockBox Behaviors change here!
    {

        //separationBehavior.weight = separationWeightSlider.value; //ERROR: not set to an instance of an object?

        // Iterate through the array of all boids 

        foreach(CloudFine.SteeringBehavior behavior in boidSettings.Behaviors)
        {

            if ((sliderID == "containment") && (behavior.GetType()==typeof(CloudFine.ContainmentBehavior)))
                {
                    behavior.weight = containmentWeightSlider.value;
                    statusText.text = sliderID + ": " + containmentWeightSlider.value.ToString();
                    FadeOthers(containmentWeightSlider);

            }
            if ((sliderID == "alignment") && (behavior.GetType() == typeof(CloudFine.AlignmentBehavior)))
            {
                behavior.weight = alignmentWeightSlider.value;
                statusText.text = sliderID + ": " + alignmentWeightSlider.value.ToString();
                FadeOthers(alignmentWeightSlider);

            }
            if ((sliderID == "cohesion") && (behavior.GetType() == typeof(CloudFine.CohesionBehavior)))
            {
                behavior.weight = cohesionWeightSlider.value;
                statusText.text = sliderID + ": " + cohesionWeightSlider.value.ToString();
                FadeOthers(cohesionWeightSlider);

            }
            if ((sliderID == "separation") && (behavior.GetType() == typeof(CloudFine.SeparationBehavior)))
            {
                behavior.weight = separationWeightSlider.value;
                statusText.text = sliderID + ": " + separationWeightSlider.value.ToString();
                FadeOthers(separationWeightSlider);

            }


        }

    }

    public void SliderTouched(string sliderID)
    {
        if (sliderID == "containment")
        {
            FadeOthers(containmentWeightSlider);
        }
        if (sliderID == "alignment")
        {
            FadeOthers(alignmentWeightSlider);
        }
        if (sliderID == "cohesion")
        {
            FadeOthers(cohesionWeightSlider);
        }
        if (sliderID == "separation")
        {
            FadeOthers(separationWeightSlider);
        }
    }

    private void FadeOthers(Slider sliderTouched)
    {
        Color fadedColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        Color visibleColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color inVisibleColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        foreach (Slider thisSlider in AllSliders)
        {
            var allImages = thisSlider.GetComponentsInChildren<Image>();
            foreach (Image image in allImages)
            {
                image.color = fadedColor;
            }
            var allText = thisSlider.GetComponentInChildren<Text>();
            allText.color = fadedColor;
            Image buttonImage = thisSlider.GetComponentInChildren<Button>().GetComponentInChildren<Image>();
            buttonImage.color = inVisibleColor;
        }

        // De-fade the one chosen
        var thisImages = sliderTouched.GetComponentsInChildren<Image>();
        foreach (Image image in thisImages)
        {
            image.color = visibleColor;
        }
        var thisText = sliderTouched.GetComponentInChildren<Text>();
        thisText.color = visibleColor;
        Image thisButtonImage = sliderTouched.GetComponentInChildren<Button>().GetComponentInChildren<Image>();
        thisButtonImage.color = inVisibleColor;
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

} // class
