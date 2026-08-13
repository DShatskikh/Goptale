using UnityEngine;

public sealed class Parallax : MonoBehaviour
{
    [SerializeField]
    private float _multiply = 1f;

    [SerializeField]
    private float _addX;
    
    private void Update()
    {
        transform.position = new Vector3(_addX + Camera.main.transform.position.x * _multiply, transform.position.y);
    }
}
