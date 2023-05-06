using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] KeyCode keyToPress = KeyCode.F; // the key to press to enter/exit the shop
        [SerializeField] GameObject showInteractTooltip; // the tooltip to display when the player can enter/exit the shop
        [SerializeField] GameObject spawnPoint;          // where the player will spawn at
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;
        [SerializeField] bool magicShop;  // allow for opening menu

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Show enter shop tooltip
                if (showInteractTooltip != null)
                {
                    showInteractTooltip.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Show enter shop tooltip
                if (showInteractTooltip != null)
                {
                    showInteractTooltip.SetActive(false);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Input.GetKeyDown(keyToPress))
                    StartCoroutine(Transition(other.gameObject));
            }
        }

        private IEnumerator Transition(GameObject player)
        {
            Fader fader = FindObjectOfType<Fader>();
            player.GetComponent<PlayerController>().enabled = false;
            yield return fader.FadeOut(fadeOutTime);

            UpdatePlayer(player);

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            player.GetComponent<PlayerController>().enabled = true;
        }

        private void UpdatePlayer(GameObject player)
        {
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
        }
    }
}
