using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlataformaCai : MonoBehaviour
{

     float delay = 1.0f;
    [SerializeField] private Sprite[] _sprite;
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    private Rigidbody rb;
    private bool isFalling = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            isFalling = true; 
            StartCoroutine(ChangeSpritesBeforeFall());
        }
    }

    IEnumerator ChangeSpritesBeforeFall()
    {
        float timePerSprite = delay / _sprite.Length;

        for (int i = 0; i < _sprite.Length; i++)
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.sprite = _sprite[i];
            }
            yield return new WaitForSeconds(timePerSprite);
        }

        DerrubarPlataforma();
    }

    void DerrubarPlataforma()
    {
        rb.isKinematic = false;
        AudioController.audioController.PlaySoundEffectAtIndex(33);
    }
}