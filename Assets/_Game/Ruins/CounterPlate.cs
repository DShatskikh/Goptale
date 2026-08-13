using TMPro;
using UnityEngine;

public sealed class CounterPlate : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    [SerializeField]
    private AudioSource _sfx;

    [SerializeField]
    private Sprite _activate, _deactivate;
    
    public int Counter;
    public bool IsActive = true;

    public void Upgrade()
    {
        _label.text = Counter.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        _sfx.Play();
        GetComponent<SpriteRenderer>().sprite = _activate; // -0.145
        _label.transform.localPosition = new Vector3(_label.transform.localPosition.x, -0.145f);
        
        if (!IsActive)
            return;
        
        Counter++;

        if (Counter > 9)
            Counter = 0;
        
        Upgrade();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<Fedya>())
            return;
        
        GetComponent<SpriteRenderer>().sprite = _deactivate; // -0.0176
        _label.transform.localPosition = new Vector3(_label.transform.localPosition.x, -0.0176f);
    }
}
