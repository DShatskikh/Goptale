using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameOver : MonoBehaviour
{
    public static string Message;
    public static string Script;
    public static Vector3 Position;

    [SerializeField]
    private Transform _determination;

    [SerializeField]
    private TMP_Text _gameOverLabel;
    
    [SerializeField]
    private TMP_Text _messageLabel;

    [SerializeField]
    private AudioSource _sfxText;
    
    [SerializeField]
    private Rigidbody2D[] _shards;
    
    private void Start()
    {
        CoroutineRunner.Instance.StartCoroutine(Await());
    }

    private IEnumerator Await()
    {
        _determination.position = Position;

        if (Message == null)
        {
            Message = $"{Stats.Instance.Name} сохраняй четкость";
        }
        
        _messageLabel.text = Message;
        
        _gameOverLabel.gameObject.SetActive(false);
        _messageLabel.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1);
        
        _determination.gameObject.SetActive(false);

        foreach (var shard in _shards)
        {
            shard.gameObject.SetActive(true);
        }
        
        var FORCE = 5;
        _shards[0].AddForce(new Vector2(1, 0) * FORCE, ForceMode2D.Impulse);
        _shards[1].AddForce(new Vector2(-1, 0) * FORCE, ForceMode2D.Impulse);
        _shards[2].AddForce(new Vector2(0, 1) * FORCE, ForceMode2D.Impulse);
        _shards[3].AddForce(new Vector2(1, 1) * FORCE, ForceMode2D.Impulse);
        _shards[4].AddForce(new Vector2(-1, 1) * FORCE, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(1);
        
        _gameOverLabel.gameObject.SetActive(true);
        
        var color = _gameOverLabel.color;
        color.a = 0f;
        _gameOverLabel.color = color;
        
        while (_gameOverLabel.color.a < 1)
        {
            color = _gameOverLabel.color;
            color.a += Time.deltaTime;
            _gameOverLabel.color = color;
            yield return null;
        }
        
        _messageLabel.gameObject.SetActive(true);
        _messageLabel.text = string.Empty;

        foreach (var symbol in Message)
        {
            yield return new WaitForSeconds(0.05f);
            _messageLabel.text += symbol;
            _sfxText.Play();
        }
        
        Message =  $"{Stats.Instance.Name} не теряй четкость!";
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

        if (Script == "Train")
        {
            yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Single);
            var train = Instantiate(Resources.Load<GameObject>("Train"));
            SceneManager.MoveGameObjectToScene(train, SceneManager.GetSceneByName("Battle"));
            var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
            SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
        }
        else
        {
            OverworldCamera.Instance.IsUpLimit = false;
            OverworldCamera.Instance.IsDownLimit = false;
            OverworldCamera.Instance.IsRightLimit = false;
            OverworldCamera.Instance.IsLeftLimit = false;
            SceneManager.LoadScene("Overworld", LoadSceneMode.Additive);
            yield return SceneManager.UnloadSceneAsync("Game Over", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            //Fedya.Instance.enabled = false;
            Stats.Instance = SaveSystem.Load();
            SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);
            Fedya.Instance.transform.position = Stats.Instance.Position;
            //Fedya.Instance.enabled = true;
        }
        
        Script = string.Empty;
    }
}
