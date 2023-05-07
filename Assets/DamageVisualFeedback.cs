using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualFeedback : MonoBehaviour
{
    [SerializeField] string _tagName; 
    [SerializeField] Renderer _renderer;
    [SerializeField] Color _feedbackColor;
    [SerializeField] float _damageFlashTime = .15f;
    private Color _originalColor;

    void Start()
    {
        if (_renderer != null)
            _originalColor = _renderer.material.color;
    }

    private IEnumerator DamageFeedback()
    {
        _renderer.material.color = _feedbackColor;
        yield return new WaitForSeconds(_damageFlashTime);
        _renderer.material.color = _originalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _tagName)
        {
            // Use it your own way
            StartCoroutine(DamageFeedback());
        }
    }

}
