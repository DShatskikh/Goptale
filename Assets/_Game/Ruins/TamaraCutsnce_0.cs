using System.Collections;
using UnityEngine;

public sealed class TamaraCutsnce_0 : MonoBehaviour
{
    [SerializeField]
    private GameObject _shards;
    
    [SerializeField]
    private ParticleSystem _shardsParticles;
    
    public void Hit()
    {
        GetComponent<AudioSource>().Play();
        _shardsParticles.Play();
        StartCoroutine(AwaitHit());
    }

    private IEnumerator AwaitHit()
    {
        yield return new WaitForSeconds(0.5f);
        _shards.SetActive(true);   
    }
}
