
using System.Collections;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public WebCamTexture cameraTexture;
    public string cameraName = "C505e HD Webcam";

    private string isUser;

    private MeshRenderer renderer;

    void Start()
    {
        renderer = this.GetComponent<MeshRenderer>();

        StartCoroutine(Test1());
    }

    IEnumerator Test1()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        bool isUser = Application.HasUserAuthorization(UserAuthorization.WebCam);
        isUser = false;

        if (!isUser)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            cameraName = devices[0].name;
            cameraTexture = new WebCamTexture(cameraName, 1024, 768, 30);
            cameraTexture.Play();

            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 24);
            renderer.material.mainTexture = cameraTexture;
        }
    }
}
