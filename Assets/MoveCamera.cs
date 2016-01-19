using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    //=== public Variables =====

    //=== private cache =====
    Camera mainCamera;


    //=== private Variables ====
    float postMousePosX;
    float postMousePosY;

    float cameraRotX;
    float cameraRotY;
    bool buttonDowned = false;

    //=== Serialized Variables ====
    [SerializeField] private float cameraRotSpeed = 1.0f;


    

     void Awake()
    {
        mainCamera = Camera.main;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
//            buttonDowned = true;
            postMousePosX = Input.mousePosition.x;
            postMousePosY = Input.mousePosition.y;
        }


        if (Input.GetMouseButton(0))
        {
            float mousePosX = Input.mousePosition.x;
            float mousePosY = Input.mousePosition.y;

            cameraRotX = (postMousePosX - mousePosX) * cameraRotSpeed;
            cameraRotY = (postMousePosY - mousePosY) * cameraRotSpeed;

            mainCamera.transform.Rotate(new Vector2(0, -cameraRotX), Space.World);
            mainCamera.transform.Rotate(new Vector2(cameraRotY, 0), Space.Self);
            postMousePosX = mousePosX;
            postMousePosY = mousePosY;
        }
	
	}
}
