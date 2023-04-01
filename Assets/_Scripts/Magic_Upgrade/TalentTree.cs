using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : MonoBehaviour
{
    [Tooltip("Magic spells that can be unlocked")]
    [SerializeField] private Talents[] talents;
    [Tooltip("Starting magic spells that are already unlocked by default")]
    [SerializeField] private Talents[] unlockedByDefault;

    void Start()
    {
        ResetTalents();
    }

    // Reset talent when restart game
    private void ResetTalents()
    {
        foreach (Talents t in talents)
        {
            t.Lock();
        }

        foreach (Talents t in unlockedByDefault)
        {
            t.Unlock();
        }
    }
}
