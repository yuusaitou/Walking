using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreetViewUI : MonoBehaviour {
    //=== public ====
    //public Texture2D[] textures;
    private double heading = 0.0;
    private double pitch = 0.0;

    //=== Serialized ====
    [SerializeField] private float cameraRotSpeed = 0.3f;
    [SerializeField] private float reloadFps = 1.0f;
    [SerializeField]
    private int maxReload = 100;


    //=== private ====
    private int width = 640;
    private int height = 640;
    private double latitude = 35.708577;
    private double longitude = 139.7604026;
    private string key = "AIzaSyAfcaXGfwb4xJq707_VawEpcacP7fsBw_s";
    
    private bool startLoadImages = false;
    int loadCounter = 0;

    private float postMousePosX;
    private float postMousePosY;
    public double fov = 90;

    float sumDeltaTime = 0;


    // Use this for initialization
    void Start () {
        StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov));
	}

    // Update is called once per frame
    void Update() {
        sumDeltaTime += Time.deltaTime;


        if (startLoadImages && loadCounter < maxReload && sumDeltaTime > 1.0/reloadFps) {
            StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov));
            sumDeltaTime = 0;
            loadCounter++;
        }

        if(loadCounter >= maxReload) {
            startLoadImages = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            //            buttonDowned = true;
            postMousePosX = Input.mousePosition.x;
            postMousePosY = Input.mousePosition.y;
        }


        if (Input.GetMouseButton(0)) {
            float mousePosX = Input.mousePosition.x;
            float mousePosY = Input.mousePosition.y;

            heading =  Mathf.Repeat((-(postMousePosX - mousePosX) * cameraRotSpeed) + (float)heading, 360f);
            pitch = Mathf.Clamp((-(postMousePosY - mousePosY) * cameraRotSpeed +(float)pitch),-90f, 90f);

            postMousePosX = mousePosX;
            postMousePosY = mousePosY;
        }

    }

    void OnGUI(){
        GUI.Label(new Rect(10,10, 100, 100), "heading:"+heading+"\npitch:"+pitch+"\n");
        GUI.Label(new Rect(Screen.width - 160, 10, 100, 100), "startLoadImages:"+startLoadImages+"\nloadCounter:"+loadCounter+"\nsumDeltaTime:"+sumDeltaTime);

        if (GUI.Button(new Rect(10, Screen.height - 100, 100, 30), "start")) {
            startLoadImages = true;
            loadCounter = 0;
        }

        if (GUI.Button(new Rect(10, Screen.height - 50, 100, 30), "stop")) {
            startLoadImages = false;
        }



    }

    private IEnumerator GetStreetViewImage(double latitude, double longitude, double heading, double pitch, double fov)
    {
        string url = "https://maps.googleapis.com/maps/api/streetview?" + "size=" + width + "x" + height + "&location="
            + latitude + "," + longitude + "&heading=" + heading + "&pitch=" + pitch + "&fov=" + fov + "&sensor=false" + "&key=" + key;

        WWW www = new WWW(url);
        Debug.Log("loading...");
        yield return www;
        Debug.Log(url + ", roadCounter=" + loadCounter);
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = www.textureNonReadable;

    }
}
