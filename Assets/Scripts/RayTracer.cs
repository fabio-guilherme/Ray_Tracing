using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracer : MonoBehaviour
{
    public bool isPerspective = false;  // For switching views

    private Texture2D renderTexture;
    private int l = -1;
    private int r = 1;
    private int b = -1;
    private int t = 1;

    void Start()
    {
        renderTexture = new Texture2D(Screen.width, Screen.height);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
    }

    void RayTrace()
    {
        for (int x = 0; x < renderTexture.width; x++)
        {
            for (int y = 0; y < renderTexture.height; y++)
            {
                float u = l + ((r - l) * (x + 0.5f)) / Screen.width;
                float v = b + ((t - b) * (y + 0.5f)) / Screen.height;
                Ray ray = new Ray(new Vector3(u, v, 0), transform.forward);

                renderTexture.SetPixel(x, y, TraceRay(ray));
            }
        }
        renderTexture.Apply();
    }

    void RayTracePerspective()
    {
        for (int x = 0; x < renderTexture.width; x++)
        {
            for (int y = 0; y < renderTexture.height; y++)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y, 0));
                renderTexture.SetPixel(x, y, TraceRay(ray));
            }
        }
        renderTexture.Apply();
    }


    Color TraceRay(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);   // Shows the rays on the scene
        if (Physics.Raycast(ray, out hit))
        {
            Material mat = hit.collider.gameObject.GetComponent<Renderer>().material;
            return mat.color;
        }
        return Color.black;
    }

    void Update()
    {
        if (isPerspective)
        {
            RayTracePerspective();
        }
        else
        {
            RayTrace();
        }
    }

}