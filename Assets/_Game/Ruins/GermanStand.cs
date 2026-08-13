using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GermanStand : Usable
{
    public override void Use()
    {
        Fedya.Instance.enabled = false;
        CoroutineRunner.Instance.StartCoroutine(AwaitUse());
    }

    private IEnumerator AwaitUse()
    {
        if (Stats.Instance.TomaraCutscene >= 2)
        {
            if (Stats.Instance.GermanState != 3)
            {
                yield return DialogueWindow.StartDialogue("Герман невозмутимо стоит.");
            }
            else
            {
                yield return DialogueWindow.StartDialogue("...");
            }
            
            Fedya.Instance.enabled = true;
            yield break;
        }

        var startBattleScreen = Instantiate(Resources.Load<StartBattleScreen>("StartBattleScreen"), 
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);
        yield return new WaitUntil(() => startBattleScreen.IsEnd);
        yield return  SceneManager.UnloadSceneAsync(Stats.Instance.LevelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        yield return  SceneManager.LoadSceneAsync("Battle",  LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));
        yield return  SceneManager.UnloadSceneAsync("Overworld", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        var german = Instantiate(Resources.Load<GameObject>("German"));
        SceneManager.MoveGameObjectToScene(german, SceneManager.GetSceneByName("Battle"));
        var background = Instantiate(Resources.Load<GameObject>("Enemy Background"));
        SceneManager.MoveGameObjectToScene(background, SceneManager.GetSceneByName("Battle"));
    }
}
