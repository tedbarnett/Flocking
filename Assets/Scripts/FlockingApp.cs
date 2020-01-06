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
    public Slider birdCountSlider;
    public Slider containmentWeightSlider; // used to set FlockBox Behaviors
    public Slider alignmentWeightSlider;
    public Slider cohesionWeightSlider;
    public Slider separationWeightSlider;
    public GameObject companyLogo;
    public GameObject behaviorPanels;
    private bool firstFrame = true;
    public float flockBoxLength = 21.0f;
    public float cameraBaseZ = 200.0f;

    private CloudFine.FlockBox flockBox;
    public CloudFine.BehaviorSettings boidSettings;
    public int maxBirds = 500;

    void Start()
    {
        behaviorPanels.transform.gameObject.SetActive(false); // hide behavior panels at start

        flockBox = GetComponent<CloudFine.FlockBox>();
        cameraTransform = FindObjectOfType<Camera>().GetComponent<Transform>();

        lastOrientation = Input.deviceOrientation;
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            isPortraitMode = false;
        }
        else
        {
            isPortraitMode = true;
        }
        isPortraitMode = !isPortraitMode;
        ChangeOrientation(); // this will set up the FlockBox size, then flip the isPortraitMode back to the correct value
        statusText.text = "";

    }

    private void Update()
    {
        if (firstFrame)
        {
            UpdateBirds(Mathf.RoundToInt(birdCountSlider.value)); // set bird count to current position of birdCountSlider
            statusText.text = "screenX: " + Screen.width + ", screenY: " + Screen.height;
            statusText.text = statusText.text + "\n" + "boxX: " + flockBox.dimensions_x + ", screenY: " + flockBox.dimensions_y;

            firstFrame = false;
        }
        if (lastOrientation == Input.deviceOrientation) return;
        ChangeOrientation();
    }

    public void SliderChanged(string sliderID)
    {
        if(sliderID == "birdCount")
        {
            UpdateBirds(Mathf.RoundToInt(birdCountSlider.value));
            return;
        }

        // Iterate through the array of all boid behaviors...

        foreach(CloudFine.SteeringBehavior behavior in boidSettings.Behaviors)
        {

            if ((sliderID == "containment") && (behavior.GetType()==typeof(CloudFine.ContainmentBehavior)))
                {
                    behavior.weight = containmentWeightSlider.value;
                    statusText.text = sliderID + ": " + containmentWeightSlider.value.ToString();

            }
            if ((sliderID == "alignment") && (behavior.GetType() == typeof(CloudFine.AlignmentBehavior)))
            {
                behavior.weight = alignmentWeightSlider.value;
                statusText.text = sliderID + ": " + alignmentWeightSlider.value.ToString();

            }
            if ((sliderID == "cohesion") && (behavior.GetType() == typeof(CloudFine.CohesionBehavior)))
            {
                behavior.weight = cohesionWeightSlider.value;
                statusText.text = sliderID + ": " + cohesionWeightSlider.value.ToString();

            }
            if ((sliderID == "separation") && (behavior.GetType() == typeof(CloudFine.SeparationBehavior)))
            {
                behavior.weight = separationWeightSlider.value;
                statusText.text = sliderID + ": " + separationWeightSlider.value.ToString();

            }
        }

    }

    private void ChangeOrientation()
    {
        isPortraitMode = !isPortraitMode;
        float cameraZ = 0;

        RectTransform birdCountRectTransform = birdCountSlider.GetComponent<RectTransform>();

        // TODO: Replace the hard-wired dimensions (and cam position) below with a formula based on current screen resolution

        if (!isPortraitMode || Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            // Device is in landscape mode
            //cameraPosition = new Vector3(94.0f, 58.0f, -102f);
            //cameraTransform.position = cameraPosition;
            flockBox.dimensions_x = Mathf.RoundToInt(flockBoxLength);
            flockBox.dimensions_y = Mathf.RoundToInt(flockBoxLength * ((float)Screen.height/(float)Screen.width));
            cameraZ = -cameraBaseZ * ((float)Screen.height / (float)Screen.width);

            //companyLogo.transform.gameObject.SetActive(false); // hide logo in landscape mode (too crowded)
        }
        else
        {
            // Device is in portrait mode
            //cameraPosition = new Vector3(55.0f, 112.0f, -202.0f);
            //cameraTransform.position = cameraPosition;
            flockBox.dimensions_x = Mathf.RoundToInt(flockBoxLength * ((float)Screen.width / (float)Screen.height));
            flockBox.dimensions_y = Mathf.RoundToInt(flockBoxLength);
            cameraZ = -cameraBaseZ;

            //companyLogo.transform.gameObject.SetActive(true); // hide logo in landscape mode (too crowded)

        }
        statusText.text = "screenX: " + Screen.width + ", screenY: " + Screen.height;
        statusText.text = statusText.text + "\n" + "boxX: " + flockBox.dimensions_x + ", screenY: " + flockBox.dimensions_y;

        cameraPosition = new Vector3((flockBox.dimensions_x * flockBox.cellSize)/2, (flockBox.dimensions_y * flockBox.cellSize) / 2, cameraZ);
        cameraTransform.position = cameraPosition;


        lastOrientation = Input.deviceOrientation;
    }

    public void UpdateBirds(int birdCount)
    {
        int i = 0;
        Transform flockParent = this.gameObject.transform;
        foreach (Transform child in flockParent)
        {
            if (child != flockParent)
            {
                if(i < birdCount)
                {
                    child.transform.gameObject.SetActive(true); // show all birds up to "birdCount"
                } else
                {
                    child.transform.gameObject.SetActive(false); // hide all birds after "birdCount" is reached
                }
            }
            i++;
        }
    }


} // class
