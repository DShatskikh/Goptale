using System.Collections;
using UnityEngine;

public sealed class DJNikolay_Attack_1 : MonoBehaviour
{
    [SerializeField]
    private float _strength = 0.02f;

    private Vector3 originalPosition;
    private float shakeTimer;
    
    private IEnumerator Start()
    {
        originalPosition = transform.position;
        yield return new WaitForSeconds(7f);
        Destroy(gameObject);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position = originalPosition + (Vector3)Random.insideUnitCircle * _strength;
    }
}
