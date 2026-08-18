using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        ChangeWindowTitle.SetTitle("Пресс ту ф4 то фулл скрин");
        Application.targetFrameRate = 120;

#if !UNITY_EDITOR
        Stats.Instance = SaveSystem.Load();
        Meta.Instance = SaveSystem.MetaLoad();

        if (SaveSystem.IsSave())
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        else
        {
            Stats.Instance = Stats.GetDefault();
            Meta.Instance = SaveSystem.MetaLoad();
            SceneManager.LoadScene("ScreenSaver", LoadSceneMode.Single);
        }
        
        return;
#endif
        
        Stats.Instance = Stats.GetDefault();
        Meta.Instance = Meta.GetDefault();
        Fedya.IsLoad = true;
        
        // SceneManager.LoadScene("ScreenSaver", LoadSceneMode.Single);

        // Meta.Instance.IsCompleteDemo = true;
        // Stats.Instance.IsGenocide = true;
        // SceneManager.LoadScene("End Demo", LoadSceneMode.Single);

        // Stats.Instance = SaveSystem.Load();
        // Stats.Instance.RUB = 9999;
        // SceneManager.LoadScene("Menu", LoadSceneMode.Single);

        // Stats.Instance = Stats.GetDefault();

        // SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        // SceneManager.LoadSceneAsync("ScreenSaver",  LoadSceneMode.Additive);

        // Meta.Instance.IsCompleteTutorial = true;

        Stats.Instance.LevelName = "Level 20";
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        SceneManager.LoadScene(Stats.Instance.LevelName, LoadSceneMode.Additive);

        Stats.Instance.TomaraCutscene = 4;
        // Stats.Instance.RUB = 200;
        Stats.Instance.LV = 3;
        Stats.Instance.Kills = 12;
        Stats.Instance.IsGenocide = true;

        // Stats.Instance.TryAddItem(Constants.ANTIPOHMELIN);
        // Stats.Instance.TryAddItem(Constants.JAGUAR);
        // Stats.Instance.TryAddItem(Constants.K_ADIDAS);
        // Stats.Instance.TryAddItem(Constants.K_NIKE);
        // Stats.Instance.TryAddItem(Constants.K_PAL);
        // Stats.Instance.TryAddItem(Constants.MASHA_JAM);
        // Stats.Instance.TryAddItem(Constants.MASHA_PIES);
        // Stats.Instance.TryAddItem(Constants.ROZOCHKA);

        // Stats.Instance.LevelName = "Level 1";
        // var majorZvetkov = Instantiate(Resources.Load<GameObject>("Major Zvetcov"));
        // SceneManager.MoveGameObjectToScene(majorZvetkov, SceneManager.GetSceneByName("Battle"));
    }
    
    private void Start()
    {
#if !UNITY_EDITOR
        return;
#endif
        
        // CoroutineRunner.Instance.StartCoroutine(AwaitBattleZvetkov());
        // CoroutineRunner.Instance.StartCoroutine(AwaitBattleGopnik());
        // CoroutineRunner.Instance.StartCoroutine(AwaitBattleGopnik_Three());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleTrain());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleDJNikolay());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleAlkash());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleNarik());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleGopnik_Narik());
        // CoroutineRunner.Instance.StartCoroutine(AwaitBattleEmpty());
        // CoroutineRunner.Instance.StartCoroutine(AwaitStartBattleTomara());
        // CoroutineRunner.Instance.StartCoroutine(AwaitBattleText());
    }

    private IEnumerator AwaitBattleText()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var text = Instantiate(Resources.Load<GameObject>("Enemy Test"));
        SceneManager.MoveGameObjectToScene(text, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitBattleZvetkov()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var majorZvetkov = Instantiate(Resources.Load<GameObject>("Major Zvetcov"));
        SceneManager.MoveGameObjectToScene(majorZvetkov, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleTrain()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var train = Instantiate(Resources.Load<GameObject>("Train"));
        SceneManager.MoveGameObjectToScene(train, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleAlkash()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var alkash = Instantiate(Resources.Load<GameObject>("Alkash"));
        SceneManager.MoveGameObjectToScene(alkash, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleNarik()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var narik = Instantiate(Resources.Load<GameObject>("Narik"));
        SceneManager.MoveGameObjectToScene(narik, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleGopnik_Narik()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        
        var gopnik = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik, SceneManager.GetSceneByName("Battle"));
        gopnik.transform.position = new Vector3(-2.8f, gopnik.transform.position.y);
            
        var narik = Instantiate(Resources.Load<GameObject>("Narik"));
        SceneManager.MoveGameObjectToScene(narik, SceneManager.GetSceneByName("Battle"));
        narik.transform.position = new Vector3(2.8f, narik.transform.position.y);
            
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitBattleGopnik()
    {
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        
        var gopnik_1 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_1, SceneManager.GetSceneByName("Battle"));
        gopnik_1.transform.position = new Vector3(0f, gopnik_1.transform.position.y);
        
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitBattleGopnik_Three()
    {
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        
        var gopnik_1 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_1, SceneManager.GetSceneByName("Battle"));
        gopnik_1.transform.position = new Vector3(-5f-1.11f, gopnik_1.transform.position.y);
        
        var gopnik_2 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_2, SceneManager.GetSceneByName("Battle"));
        gopnik_2.transform.position = new Vector3(-1.11f, gopnik_2.transform.position.y);
        
        var gopnik_3 = Instantiate(Resources.Load<GameObject>("Gopnik"));
        SceneManager.MoveGameObjectToScene(gopnik_3, SceneManager.GetSceneByName("Battle"));
        gopnik_3.transform.position = new Vector3(5f-1.11f, gopnik_3.transform.position.y);
        
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleDJNikolay()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var train = Instantiate(Resources.Load<GameObject>("DJ Nikolay"));
        SceneManager.MoveGameObjectToScene(train, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Boss Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitStartBattleTomara()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var tomara = Instantiate(Resources.Load<GameObject>("Tomara"));
        SceneManager.MoveGameObjectToScene(tomara, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Boss Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
    
    private IEnumerator AwaitBattleEmpty()
    {
        yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
        var empty = Instantiate(Resources.Load<GameObject>("Empty Enemy"));
        SceneManager.MoveGameObjectToScene(empty, SceneManager.GetSceneByName("Battle"));
    }
}
