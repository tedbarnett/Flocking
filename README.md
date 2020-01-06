# Flocking
Emulating bird-flocking behavior on an iOS device (Unity and C#).  This project was inspired by the ["Flock"](https://www.exploratorium.edu/exhibits/flock) exhibit at the San Francisco Exploratorium.  

To build this Unity project:
- Clone this repository
- Open the cloned repository folder in Unity (2019.3 or above)
- Purchase and install the Unity Asset FlockBox ([link here](https://assetstore.unity.com/packages/tools/ai/flock-box-155028))
- Set build target to iOS
- Build for your iPhone or iPad!  Should run in Unity Editor as well.

Tap the screen to bring up the slider controls.  Adjust any one of these variables to change the size and behavior of the flock:
- **Number of birds**: adjust flock size from 1 to 500 birds (note that bird #1 is red and larger than other birds)
- **Containment** behavior: Staying inside the defined flock box
- **Alignment** behavior: Steer towards the average forward direction of nearby birds
- **Cohesion** behavior: Steer towards the midpoint of nearby birds.
- **Separation** behavior: Steer *away* from nearby birds. Weighted by distance so that the closer other birds get, the
greater the separation force becomes.

Screenshot from iPad below:

![Screenshot of Flocking](https://github.com/tedbarnett/Flocking/blob/master/flocking-screenshot.jpg)


This app was built using the excellent Unity Store Asset "FlockBox" ([link here](https://assetstore.unity.com/packages/tools/ai/flock-box-155028)).

https://assetstore.unity.com/packages/tools/ai/flock-box-155028
