using UnityEngine;
using UnityEngine.UI;

public class FlockingApp : MonoBehaviour
{
    public Slider birdCountSlider;
    public Slider containmentWeightSlider; // used to set FlockBox Behaviors
    public Slider alignmentWeightSlider;
    public Slider cohesionWeightSlider;
    public Slider separationWeightSlider;
    public GameObject companyLogo;
    public GameObject behaviorPanels;
    public float flockBoxLength = 21.0f; // long edge of flockBox
    public Text statusText; // used for on-screen debug messaging
    private CloudFine.FlockBox flockBox;
    public CloudFine.BehaviorSettings boidSettings;
    public int maxBirds = 500;

    private bool firstFrame = true;
    private bool isPortraitMode;
    private bool wasPortraitMode;
    private Transform cameraTransform;
    private Vector3 cameraPosition;
    private float newWidth;
    private float newHeight;
    private float cameraZ;
    private int lastHeight;

    void Start()
    {
        behaviorPanels.transform.gameObject.SetActive(false); // hide behavior panels at start

        flockBox = GetComponent<CloudFine.FlockBox>();
        cameraTransform = FindObjectOfType<Camera>().GetComponent<Transform>();

        isPortraitMode = (Screen.width < Screen.height);
        wasPortraitMode = isPortraitMode; // used to watch for screen orientation changes
        lastHeight = Screen.height;

        ChangeOrientation(); // this will set up the correct FlockBox size and camera angle
        statusText.text = "";

    }

    private void Update()
    {
        if (firstFrame)
        {
            UpdateBirds(Mathf.RoundToInt(birdCountSlider.value)); // set bird count to current position of birdCountSlider
            // set up Tracked Bird (2x size and Red in color)
                Transform trackedBird = this.gameObject.transform.GetChild(0).GetChild(0);
                trackedBird.localScale = new Vector3(2.0f * trackedBird.localScale.x, 2.0f * trackedBird.localScale.y, 2.0f * trackedBird.localScale.z);
                Material trackedBirdMaterial = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
                trackedBirdMaterial.color = Color.red;
            firstFrame = false;
        }
        isPortraitMode = (Screen.width < Screen.height);
        if ((lastHeight != Screen.height) || (isPortraitMode != wasPortraitMode)) ChangeOrientation();
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
        isPortraitMode = (Screen.width < Screen.height);

        float cameraFOV = Camera.main.fieldOfView;
        float cameraFOVRadians = cameraFOV * Mathf.Deg2Rad;
        float triangleTangent = Mathf.Tan(cameraFOVRadians/2.0f);  // e.g. 60 degree camera angle implies z = (height/2)/(tan(30))
        float cellSize = (float)flockBox.cellSize;
        float fudge = 1.10f; // tweak the camera position out 10% further than computed

        RectTransform birdCountRectTransform = birdCountSlider.GetComponent<RectTransform>();

        // TODO: Replace the hard-wired dimensions (and cam position) below with a formula based on current screen resolution

        if (!isPortraitMode)
        {
            // Device is in landscape mode
            newWidth = flockBoxLength; // width is the "long edge" in portrait mode
            newHeight = newWidth * ((float)Screen.height/(float)Screen.width);
        }
        else
        {
            // Device is in portrait mode
            newHeight = flockBoxLength; // height is the "long edge" in portrait mode
            newWidth = newHeight * ((float)Screen.width / (float)Screen.height);
        }
        flockBox.dimensions_x = Mathf.RoundToInt(newWidth);
        flockBox.dimensions_y = Mathf.RoundToInt(newHeight);
        cameraZ = -(fudge) * ((newHeight * cellSize) / 2.0f) / triangleTangent; // 60 degree camera angle implies z = (height/2)/(tan(30))

        statusText.text = "screenX: " + Screen.width + ", screenY: " + Screen.height;
        statusText.text = statusText.text + "\n" + "boxX: " + flockBox.dimensions_x + ", screenY: " + flockBox.dimensions_y;
        statusText.text = statusText.text + "\n" + "cameraZ: " + cameraZ;

        cameraPosition = new Vector3(5.0f + (newWidth * cellSize) /2, (newHeight * cellSize) / 2, cameraZ);
        cameraTransform.position = cameraPosition;

        companyLogo.transform.gameObject.SetActive(isPortraitMode); // hide logo in landscape mode (too crowded)
        wasPortraitMode = isPortraitMode; // reset comparison bool
        lastHeight = Screen.height;
    }

    public void UpdateBirds(int birdCount)
    {
        int i = 0;
        Transform flockParent = this.gameObject.transform;

        foreach (Transform child in flockParent)
        {
            if (child != flockParent)
            {
                if (i < birdCount)
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
