using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    //=== public Variables =====

    //=== private cache =====
    Camera mainCamera;
    CoreScript coreScript;
    Vector2 latitudeAndLongitude;


    //=== private Variables ====
    float m_postMousePosX;
    float m_postMousePosY;

    float m_cameraRotX;
    float m_cameraRotY;
    bool buttonDowned = false;

    Vector2 m_fieldPoint;

    //=== Serialized Variables ====
    [SerializeField] private float cameraRotSpeed = 1.0f;
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float fieldToGeocode = 1.0f;
    [SerializeField]
    private float reloadInterval = 10.0f;


    GUIStyle guiStyle ;
   public GUIStyleState styleState;

    void Awake()
    {
        mainCamera = Camera.main;
        coreScript = GetComponent<CoreScript>();
        m_fieldPoint = Vector2.zero;
    }

	// Use this for initialization
	void Start () {

        guiStyle = new GUIStyle();
        styleState = new GUIStyleState();
        styleState.background = Texture2D.blackTexture;
        guiStyle.normal = styleState;
        latitudeAndLongitude = coreScript.getLatitudeAndLogitude();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
//            buttonDowned = true;
            m_postMousePosX = Input.mousePosition.x;
            m_postMousePosY = Input.mousePosition.y;
        }


        if (Input.GetMouseButton(0))
        {
            float mousePosX = Input.mousePosition.x;
            float mousePosY = Input.mousePosition.y;

            m_cameraRotX = (m_postMousePosX - mousePosX) * cameraRotSpeed;
            m_cameraRotY = (m_postMousePosY - mousePosY) * cameraRotSpeed;

            mainCamera.transform.Rotate(new Vector2(0, -m_cameraRotX), Space.World);
            mainCamera.transform.Rotate(new Vector2(m_cameraRotY, 0), Space.Self);
            m_postMousePosX = mousePosX;
            m_postMousePosY = mousePosY;
        }

        if (Input.GetButton("Vertical")){
            detectPosChange();
        }
	}

    void FixedUpdate() {


    }

    void OnGUI() {

        

        GUI.Label(new Rect(10, 10, 300, 100), "fieldPoint = (" + m_fieldPoint.x + "," + m_fieldPoint.y + ")"
            + "\nfieldPoint.magnitude = " + m_fieldPoint.magnitude
            + "\nLatitude:" + latitudeAndLongitude.x
            + "\nLongitude:" + latitudeAndLongitude.y, guiStyle);
        

    }
 

    void detectPosChange() {
        Vector3 cameraDirVec3;
        cameraDirVec3 = mainCamera.transform.rotation * Vector3.forward;
        Vector2 cameraDirVec2 = new Vector2(cameraDirVec3.z, cameraDirVec3.x).normalized;
        m_fieldPoint += cameraDirVec2 * moveSpeed;

        if (m_fieldPoint.magnitude > reloadInterval) {
            coreScript.moveWorld(m_fieldPoint.x * fieldToGeocode, m_fieldPoint.y * fieldToGeocode);
            m_fieldPoint = Vector2.zero;
            latitudeAndLongitude = coreScript.getLatitudeAndLogitude();
        }
        

    }

}
