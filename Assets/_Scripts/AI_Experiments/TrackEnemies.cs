using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemies : MonoBehaviour
{
    /// <summary>
    /// Source: ItsJustOneGuy 
    /// Title: How to find find closest enemy in a direction
    /// </summary>
    /// 
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject closestEnemyInDirection;
    public Transform body;  // the body that player is using to find


    public void FindClosestGameObject()
    {
        GameObject closest = null;
        float distance = 30f;
        Vector3 position = body.transform.position;
        foreach (GameObject go in enemies)
        {
            Vector3 difference = go.transform.position - position;
            float currentDist = difference.sqrMagnitude;
            if (currentDist < distance)
            {
                closest = go;
                distance = currentDist;
            }
        }
        closestEnemyInDirection = closest;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cage Wall")
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (other.gameObject == enemies[i])
                {
                    return; 
                }
            }
            enemies.Add(other.gameObject);
            FindClosestGameObject();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cage Wall")
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (other.gameObject == enemies[i])
                {
                    if (enemies[i] == closestEnemyInDirection)
                    {
                        closestEnemyInDirection = null; 
                    }
                    enemies.RemoveAt(i);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(body.transform.position, closestEnemyInDirection.transform.position);
    }
}
