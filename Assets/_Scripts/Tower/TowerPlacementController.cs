using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    public bool occupied = false;
    private BaseTowerController tower;

    private Renderer tpc_renderer;
    private Coroutine pulseRoutine;
    private Color originalColor;
    private Color blueColor;
    private float pulseDuration = 1.5f;
    private float pulseIntensity = 0.02f;

    public BaseTowerController Tower { get => tower; set => tower = value; }

    private void Awake()
    {
        tpc_renderer = GetComponent<Renderer>();
        originalColor = tpc_renderer.material.color;
        blueColor = new Color(0f, 255f, 255f, 1f);
    }

    private void Start()
    {
        // Don't start the pulse routine here
    }

    private void OnMouseDown()
    {
        if (!occupied && GMDebug.Instance.isBuilding)
        {
            UIManager.Instance.ShowBuildTowerMenuUI(this);
        }
        // Also maybe want to pause the game here perhaps? 
    }

    private void Update()
    {
        if (GMDebug.Instance.isBuilding)
        {
            if (pulseRoutine == null)
            {
                pulseRoutine = StartCoroutine(Pulse());
            }
        }
        else
        {
            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
                pulseRoutine = null;
                tpc_renderer.material.SetFloat("_Surface", 0f); // set surface to transparent
                tpc_renderer.material.color = originalColor;
            }
        }
    }

    private IEnumerator Pulse()
    {
        while (true)
        {
            if (!occupied)
            {
                float t = 0f;
                while (t < pulseDuration)
                {
                    t += Time.deltaTime;
                    float a = Mathf.Lerp(pulseIntensity, 0f, t / pulseDuration);
                    tpc_renderer.material.color = Color.Lerp(originalColor, blueColor, a);
                    tpc_renderer.material.SetFloat("_Surface", 1f); // set surface to opaque
                    yield return null;
                }

                t = 0f;
                while (t < pulseDuration)
                {
                    t += Time.deltaTime;
                    float a = Mathf.Lerp(0f, pulseIntensity, t / pulseDuration);
                    tpc_renderer.material.color = Color.Lerp(originalColor, blueColor, a);
                    tpc_renderer.material.SetFloat("_Surface", 1f); // set surface to opaque
                    yield return null;
                }
            }
            else
            {
                tpc_renderer.material.SetFloat("_Surface", 0f); // set surface to transparent
                tpc_renderer.material.color = originalColor;
                yield return null;
            }
        }
    }

    public void TowerPlaced(BaseTowerController tower)
    {
        this.Tower = tower;
        occupied = true;

        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
            tpc_renderer.material.SetFloat("_Surface", 0f); // set surface to transparent
            tpc_renderer.material.color = originalColor;
        }
    }

    public void TowerRemoved()
    {
        Tower = null;
        occupied = false;

        if (pulseRoutine == null && GMDebug.Instance.isBuilding)
        {
            pulseRoutine = StartCoroutine(Pulse());
        }
    }
}