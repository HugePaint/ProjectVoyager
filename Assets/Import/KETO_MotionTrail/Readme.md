# CameraController Script

The CameraController script is used to control the position and rotation of a camera in a 3D space, with a given target to follow.

## Public Variables

- `target`: A Transform variable that determines the target that the camera will follow.
- `distance`: A float variable that determines the distance between the camera and the target.
- `height`: A float variable that determines the height of the camera above the target.
- `angle`: A float variable that determines the angle of the camera around the target.

## LateUpdate()

The LateUpdate() method is called every frame after all Update() methods have been called. This method is used to update the camera's position and rotation, based on the position and rotation of the target.

- `localRotation`: A Quaternion variable that represents the local rotation of the camera.
- `transform.rotation`: Sets the rotation of the camera to the `localRotation` quaternion.
- `negDistance`: A Vector3 variable that represents the negative distance between the camera and the target.
- `position`: A Vector3 variable that represents the position of the camera, calculated by multiplying `localRotation` by `negDistance` and adding the target position.
- `transform.position`: Sets the position of the camera to the `position` vector.


# PlayerMovement Script

This script controls the movement and rotation of the player character.

## Public Variables
- **moveSpeed**: The speed at which the player moves (default: 5)
- **turnSpeed**: The speed at which the player turns (default: 10)

## Private Variables
- **anim**: The Animator component attached to the player

## Methods
- **Start()**: Called once when the script is initialized. Gets the Animator component.
- **Update()**: Called every frame. Gets input from the player and moves the character accordingly.

## Details
- Uses `Input.GetAxis()` to get input from the player.
- Normalizes the movement direction vector to ensure consistent movement speed.
- Sets `isMoving` parameter of the Animator component to true or false based on whether the player is moving or not.
- Calculates the target rotation for the player based on the movement direction vector.
- Rotates the player towards the target rotation using `Quaternion.Slerp()`.


# Character Trail

Character Trail is a Unity script that creates a trail effect behind a character. It works by creating meshes from the character's skinned mesh renderers and then fading them out over time.

## Features

- Easy to use: Simply attach the script to the character and set the desired parameters in the inspector.
- Customizable: Adjust the duration, interval, and material of the trail effect to fit your needs.
- Particle system: Enable the particle system to create an additional visual effect.

## How to Use

1. Attach the `CharacterTrail` script to the character that you want to create a trail effect for.
2. Set the desired parameters in the inspector:
    - **Duration:** The total duration of the trail effect.
    - **Interval:** The time between each mesh being created.
    - **Destroy Delay:** The time delay before each mesh is destroyed.
    - **Spawn Transform:** The transform that the trail meshes will spawn at.
    - **Trail Material:** The material used for the trail meshes.
    - **Particles:** Whether or not to enable the particle system.
3. Press the space bar to activate the trail effect.

## Dependencies

- Unity 2019.4 or higher
- Universal Render Pipeline (URP)

**Note:** This project has a dependency on URP and will only work with URP installed and set as the current render pipeline.

## Limitations

- The script only works with skinned mesh renderers, so it may not work with other types of characters or objects.
- The script creates a lot of meshes, so it may not be suitable for use on low-end hardware.
