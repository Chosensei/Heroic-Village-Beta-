using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material daySkybox;
    public Material nightSkybox;
    public float dayAmbientIntensity;
    public float nightAmbientIntensity;
    public Light sunLight;
    public Light [] villageLights; 

    private void Start()
    {
        RenderSettings.skybox = daySkybox;
    }
    private void Update()
    {
        // For Debug purposes 
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ToggleSkybox();
        //}
    }
    public void ToggleSkybox()
    {
        if (RenderSettings.skybox == daySkybox)
        {
            RenderSettings.skybox = nightSkybox;
            RenderSettings.ambientIntensity = nightAmbientIntensity;
            sunLight.enabled = false;
            LightSwitchOn(true); 
        }
        else
        {
            RenderSettings.skybox = daySkybox;
            RenderSettings.ambientIntensity = dayAmbientIntensity;
            sunLight.enabled = true;
            LightSwitchOn(false);
        }
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
    private void LightSwitchOn(bool flag)
    {
        if (flag == true)
        {
            for (int i = 0; i < villageLights.Length; i++)
            {
                villageLights[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < villageLights.Length; i++)
            {
                villageLights[i].enabled = false;
            }
        }

    }
}
