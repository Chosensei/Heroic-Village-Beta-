using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tag Filter", menuName = "Abilities/Filters/Tag")]
public class TagFilter : FilteringStrategy
{
    [SerializeField] string tagToFilter = "";
    public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
    {
        // accept all gameobject and check for particular tags
        foreach (var gameObject in objectsToFilter)
        {
            if (gameObject.CompareTag(tagToFilter))
            {
                yield return gameObject;
            }
        }
    }

}
