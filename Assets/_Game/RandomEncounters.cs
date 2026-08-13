using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public sealed class RandomEncounters : MonoBehaviour
{
    public static List<string> DefeatedEnemiesID = new();
    
    private float _timer;
    
    private IEnumerator Start()
    {
        foreach (var id in DefeatedEnemiesID)
        {
            if (id == "Гопник")
            {
                var gopnik = Instantiate(Resources.Load<GameObject>("Gopnik_Dead"));
                gopnik.transform.position = Fedya.Instance.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                
                if (Random.Range(0, 2) == 1)
                    gopnik.GetComponent<UsableDialogue>().SetDialogues(new []{"Блять"});
                
                SceneManager.MoveGameObjectToScene(gopnik, SceneManager.GetSceneByName(Stats.Instance.LevelName));
            }
            else if (id == "Алкаш")
            {
                var alkash = Instantiate(Resources.Load<GameObject>("Alkash_Dead"));
                alkash.transform.position = Fedya.Instance.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                
                if (Random.Range(0, 2) == 1)
                    alkash.GetComponent<UsableDialogue>().SetDialogues(new []{"Блять"});
                
                SceneManager.MoveGameObjectToScene(alkash, SceneManager.GetSceneByName(Stats.Instance.LevelName));
            }
            else if (id == "Нарик")
            {
                var narik = Instantiate(Resources.Load<GameObject>("Narik_Dead"));
                narik.transform.position = Fedya.Instance.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                
                if (Random.Range(0, 2) == 1)
                    narik.GetComponent<UsableDialogue>().SetDialogues(new []{"Блять"});
                
                SceneManager.MoveGameObjectToScene(narik, SceneManager.GetSceneByName(Stats.Instance.LevelName));
            }
        }
        
        DefeatedEnemiesID = new List<string>();
        
        _timer = 80 + Random.Range(0, 41);
        
        while (_timer > 0)
        {
            // Руинах нужно убить 20 монстров
            yield return new WaitForSeconds(0.1f);

            if (Fedya.Instance.IsMove)
            {
                _timer -= 1;
            }

            if (_timer <= 0)
            {
                Debug.Log("Начали битву");
                CoroutineRunner.Instance.StartCoroutine(AwaitStartBattle());
            }
        }
    }

    private IEnumerator AwaitStartBattle()
    {
        Fedya.Instance.enabled = false;
        Fedya.Instance.Danger.gameObject.SetActive(true);
                
        yield return new WaitForSeconds(1f);
                
        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x,  Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        var randomIndex = Random.Range(0, 5);

        if (Stats.Instance.Kills == 11)
        {
            randomIndex = Random.Range(0, 3);
        }
        else if (Stats.Instance.Kills >= 12)
        {
            randomIndex = -1;
        }
        
        if (randomIndex == 0)
        {
            var gopnik_1 = Instantiate(Resources.Load<GameObject>("Gopnik"));
            SceneManager.MoveGameObjectToScene(gopnik_1, SceneManager.GetSceneByName("Battle"));
            gopnik_1.transform.position = new Vector3(0f, gopnik_1.transform.position.y);
        }
        else if (randomIndex == 1)
        {
            var alkash = Instantiate(Resources.Load<GameObject>("Alkash"));
            SceneManager.MoveGameObjectToScene(alkash, SceneManager.GetSceneByName("Battle"));
            alkash.transform.position = new Vector3(0f, alkash.transform.position.y);
        }
        else if (randomIndex == 2)
        {
            var narik = Instantiate(Resources.Load<GameObject>("Narik"));
            SceneManager.MoveGameObjectToScene(narik, SceneManager.GetSceneByName("Battle"));
            narik.transform.position = new Vector3(0f, narik.transform.position.y);
        }
        else if (randomIndex == 3)
        {
            var gopnik_1 = Instantiate(Resources.Load<GameObject>("Gopnik"));
            SceneManager.MoveGameObjectToScene(gopnik_1, SceneManager.GetSceneByName("Battle"));
            gopnik_1.transform.position = new Vector3(-2.8f, gopnik_1.transform.position.y);
            
            var gopnik_2 = Instantiate(Resources.Load<GameObject>("Gopnik"));
            SceneManager.MoveGameObjectToScene(gopnik_2, SceneManager.GetSceneByName("Battle"));
            gopnik_2.transform.position = new Vector3(2.8f, gopnik_2.transform.position.y);
        }
        else if (randomIndex == 4)
        {
            var gopnik = Instantiate(Resources.Load<GameObject>("Gopnik"));
            SceneManager.MoveGameObjectToScene(gopnik, SceneManager.GetSceneByName("Battle"));
            gopnik.transform.position = new Vector3(-2.8f, gopnik.transform.position.y);
            
            var narik = Instantiate(Resources.Load<GameObject>("Narik"));
            SceneManager.MoveGameObjectToScene(narik, SceneManager.GetSceneByName("Battle"));
            narik.transform.position = new Vector3(2.8f, narik.transform.position.y);
        }
        else if (randomIndex == -1)
        {
            var emptyEnemy = Instantiate(Resources.Load<GameObject>("Empty Enemy"));
            SceneManager.MoveGameObjectToScene(emptyEnemy, SceneManager.GetSceneByName("Battle"));
        }

        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
}
