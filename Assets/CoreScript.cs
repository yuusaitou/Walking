using UnityEngine;
using System.Collections;

public class CoreScript : MonoBehaviour
{
    //=== public ====
    public Texture2D[] textures;
    public double heading = 0.0;
    public double pitch = 0.0;


    //=== private ====
    private int width = 640;
    private int height = 640;
    private double latitude = 35.708577;
    private double longitude = 139.7604026;
    private string key = "AIzaSyAfcaXGfwb4xJq707_VawEpcacP7fsBw_s";

    public double fov;
    private int skyNum;

    void Awake()
    {
        fov = 90;
    }

    void Start()
    {
        UpdateSkybox();
    }
    
    // メイン部分
    void UpdateSkybox()
    {
        textures = new Texture2D[6];
        // 前後左右上下をそれぞれ取得
        for (skyNum = 0; skyNum < 6; skyNum++)
        {
            switch (skyNum)
            {
                case 0:
                    //front
                    heading = 0;
                    pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                case 1: // 
                        //back
                    heading = 180;
                    pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                case 2: // left
                    heading = 90;
                    pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                case 3: // right
                    heading = 270;
                    pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                case 4: // up
                    heading = 0;
                    pitch = 90;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                case 5: // down 
                    heading = 0;
                    pitch = -90;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
            }
        }
    }

    // Google Map Street Viewの画像を取ってくる 
    private IEnumerator GetStreetViewImage(double latitude, double longitude, double heading, double pitch, double fov, int skyNum)
    {
        string url = "https://maps.googleapis.com/maps/api/streetview?" + "size=" + width + "x" + height + "&location="
            + latitude + "," + longitude + "&heading=" + heading + "&pitch=" + pitch + "&fov=" + fov + "&sensor=false" + "&key=" + key;
        Debug.Log(skyNum + " : " + url);
        WWW www = new WWW(url);
        yield return www;
        // texturesに取得してテクスチャをセット 
        textures[skyNum] = www.texture;
        // ちゃんとテクスチャを取得できたらskyboxを作る 
        CreateSkyBox();
    }


    void CreateSkyBox()
    {
        // Manifestを作成 
        SkyboxManifest manifest = new SkyboxManifest(textures[0], textures[1], textures[2], textures[3], textures[4], textures[5]);
        // 作成したManifestからマテリアルを作成 
        Material material = CreateSkyboxMaterial(manifest);
        SetSkybox(material);
        enabled = false;
    }

    public static Material CreateSkyboxMaterial(SkyboxManifest manifest)
    {
        // Skybox形式のマテリアルを作成してテクスチャをセット 
        Material result = new Material(Shader.Find("RenderFX/Skybox"));
        result.SetTexture("_FrontTex", manifest.textures[0]);
        result.SetTexture("_BackTex", manifest.textures[1]);
        result.SetTexture("_LeftTex", manifest.textures[2]);
        result.SetTexture("_RightTex", manifest.textures[3]);
        result.SetTexture("_UpTex", manifest.textures[4]);
        result.SetTexture("_DownTex", manifest.textures[5]);
        return result;
    }


    void SetSkybox(Material material)
    {
        // メインカメラを取得して 
        GameObject camera = Camera.main.gameObject;

        // Skyboxを取得 
        Skybox skybox = camera.GetComponent<Skybox>();
        // Skyboxがなければ作成
        if (skybox == null)
            skybox = camera.AddComponent<Skybox>();
        // Skyboxにマテリアルをセット
        skybox.material = material;

    }
}



// Skyboxマニフェストの構造体
public struct SkyboxManifest
    {
        public Texture2D[] textures;
        public SkyboxManifest(Texture2D front, Texture2D back, Texture2D left, Texture2D right, Texture2D up, Texture2D down)
        {
            textures = new Texture2D[6]
            {
                front,
                back,
                left,
                right,
                up,
                down
            };
        }
    }
