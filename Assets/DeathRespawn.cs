using UnityEngine;
using System.Collections;
using RPG.Attributes;
using RPG.SceneManagement;

public class DeathRespawn : MonoBehaviour
{
    public Transform respawnLocation;
    public float initialRespawnDelay = 2.0f;
    public float respawnDelayIncrease = 2.0f;
    // Fader
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    //[SerializeField] float fadeWaitTime = 0.5f;
    private float currentRespawnDelay;
    private bool isRespawning = false;

    void Start()
    {
        print("Not dead yet");
        currentRespawnDelay = initialRespawnDelay;
    }

    void Update()
    {
        if (isRespawning)
        {
            currentRespawnDelay += respawnDelayIncrease * Time.deltaTime;
            if (currentRespawnDelay >= initialRespawnDelay)
            {
                StartCoroutine(Transition(gameObject));
            }
        }
    }

    public void RespawnPlayer()
    {
        if (!isRespawning)
        {
            currentRespawnDelay = initialRespawnDelay;
            isRespawning = true;
        }
    }

    private void Respawn(GameObject player)
    {
        isRespawning = false;
        player.transform.position = respawnLocation.position;
        player.transform.rotation = respawnLocation.rotation;
        GetComponent<Health>().RestoreFullHealth();
    }
    private IEnumerator Transition(GameObject player)
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(currentRespawnDelay);

        Respawn(player); 
        yield return new WaitForSeconds(fadeOutTime);

        fader.FadeIn(fadeInTime);
    }
}
