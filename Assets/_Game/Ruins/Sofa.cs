using System.Collections;
using UnityEngine;

public sealed class Sofa : Usable
{
    [SerializeField]
    private GameObject _armchair1, _armchair2, _belash;

    [SerializeField]
    private SpriteRenderer _background;
    
    [SerializeField]
    private AudioSource _sfx;
    
    [SerializeField]
    private ParticleSystem _particles;
    
    private void Start()
    {
        if (Stats.Instance.TomaraCutscene > 11)
        {
            _belash.SetActive(true);
        }

        if (Stats.Instance.TomaraCutscene == 12)
        {
            _armchair1.SetActive(false);
            _armchair2.SetActive(true);
        }
    }

    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }
    
    private IEnumerator AwaitUse()
    {
        if (Stats.Instance.TomaraCutscene != 11)
        {
            yield return DialogueWindow.StartDialogue(new [] {
                "Старый задрипанный диван.",
            });
            
            Fedya.Instance.enabled = true;
            yield break;
        }
        
        var position = Fedya.Instance.transform.position;
     
        Fedya.Instance.GetComponent<Collider2D>().enabled = false;
        Fedya.Instance.transform.position = new Vector3(-3.57f, 7.19f);
        Fedya.Instance.transform.eulerAngles = new Vector3(0, 0, -90);
        Fedya.Instance.SetDirection(new Vector2(0, -1));
        _background.gameObject.SetActive(true);
        _sfx.Play();
        _particles.Play();
        
        var color = _background.color;
        color.a = 0;
        _background.color = color;
        
        while (_background.color.a < 175 / 255f)
        {
            color = _background.color;
            color.a += Time.deltaTime;
            _background.color = color;
            
            yield return null;
        }
        
        yield return new WaitForSeconds(3);
        _armchair1.SetActive(false);
        _armchair2.SetActive(true);
        _belash.SetActive(true);
        
        while (_background.color.a > 0)
        {
            color = _background.color;
            color.a -= Time.deltaTime;
            _background.color = color;
            
            yield return null;
        }
        
        _particles.Stop();
        _background.gameObject.SetActive(false);
        Fedya.Instance.transform.position = position;
        Fedya.Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        Fedya.Instance.GetComponent<Collider2D>().enabled = true;
        Stats.Instance.TomaraCutscene = 12;
        Fedya.Instance.enabled = true;
    }
}
