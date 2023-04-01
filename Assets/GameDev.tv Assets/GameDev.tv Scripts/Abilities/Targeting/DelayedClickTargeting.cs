using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click")]
public class DelayedClickTargeting : TargetingStrategy
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 cursorHotspot;
    [SerializeField] private LayerMask layerMask;   // This is set to ignore terrain layer
    [SerializeField] private float areaAffectRadius;
    [SerializeField] Transform targetingPrefab; 

    Transform targetingPrefabInstance = null;
    public override void StartTargeting(AbilityData data, Action finished)
    {
        PlayerController pc = data.GetUser().GetComponent<PlayerController>();
        pc.StartCoroutine(Targeting(data, pc, finished)); 
    }

    private IEnumerator Targeting(AbilityData data, PlayerController pc, Action finished)
    {
        pc.enabled = false; 
        if (targetingPrefabInstance == null)
        {
            targetingPrefabInstance = Instantiate(targetingPrefab); 
        }
        else
        {
            targetingPrefabInstance.gameObject.SetActive(true);
        }
        // Scale up the size of summon circle
        targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);
        while (!data.IsCancelled())
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
            RaycastHit raycastHit;
            // Acquiring Targets inside an area of effect radius
            if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
            {
                targetingPrefabInstance.position = raycastHit.point; 
                if (Input.GetMouseButtonDown(0))
                {
                    // Keep checking every frame if mouse button been released 
                    while (Input.GetMouseButton(0))
                    {
                        yield return null;
                    }
                    // Or you can also do it like this
                    //yield return new WaitWhile(() => Input.GetMouseButton(0));

                    // Set a target point for prefab to spawn at
                    data.SetTargetedPoint(raycastHit.point); 
                    data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                    break;
                }
            }
            yield return null;
        }
        // Hide targeting if canceled
        targetingPrefabInstance.gameObject.SetActive(false);
        //if mouse button been released, renable player controls
        pc.enabled = true;
        finished();
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
    {
       
        RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
        foreach (var hit in hits)
        {
            yield return hit.collider.gameObject; 
        }
    }
}
