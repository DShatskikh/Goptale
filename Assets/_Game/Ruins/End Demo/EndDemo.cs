using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public sealed class EndDemo : MonoBehaviour
{
    [SerializeField]
    private GameObject _slide1, _slide2;
    
    [SerializeField]
    private VideoPlayer _videoPlayer, _videoPlayer2, _videoPlayerGenocide;
    
    [SerializeField]
    private AudioSource _audioSource;
    
    [SerializeField]
    private AudioClip _music;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(4);
        
        _slide1.SetActive(false);
        _slide2.SetActive(true);
        
        yield return new WaitForSeconds(5);
        
        _slide2.SetActive(false);
        yield return new WaitForSeconds(1);
        
        _audioSource.Play();
        
        yield return DialogueWindow.StartDialogue(new[] {"Звонок..."});
        MusicManager.Instance.Play(_music);

        if (!Meta.Instance.IsCompleteDemo)
        {
            yield return DialogueWindow.StartDialogue(new[] {
                "Привет игрок.",
                "Боже как я рад что я смог выпустить эту поделку.",
                "Она должна была выйти в далёком 2019 году...",
                "...когда был рассвет Подзёмкино и фан игр по Undertale %(по моему мнению).",
                "Но у меня тогда не хватило навыков программирования и я забил на проект.",
                "И тебя наверное мучает вопрос %а что произошло дальше?",
                $"Смог ли {Stats.Instance.Name} пройти свой путь и не спится?!",
                "ХЗ %я пока не доделал ту часть.",
                "(Конец звонка)*(Конец DEMO)",
            });

            var time = MusicManager.Instance.GetTime;
            MusicManager.Instance.Stop();
            yield return new WaitForSeconds(4);
            
            MusicManager.Instance.Play(_music);
            MusicManager.Instance.SetTime(time);
            _audioSource.Play();
            yield return DialogueWindow.StartDialogue(new[] {"Звонок...", "Ладно-ладно я шучу.%*Вот что было дальше..."}); 
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new[] {
                "Привет игрок.*Я тронут что ты решил пройти игру еще раз.",
                "Тебя наверное всё еще мучает вопрос, %а что произошло дальше?",
                "Вот что было дальше...",
            }); 
        }
        
        MusicManager.Instance.Stop();
        
        var videoPlayer = _videoPlayer;

        if (Stats.Instance.IsGenocide)
        {
            videoPlayer = _videoPlayerGenocide;
        }
        else if (Meta.Instance.IsCompleteDemo)
        {
            videoPlayer = _videoPlayer2;
        }
        
        videoPlayer.Play();

        Meta.Instance.IsCompleteDemo = true;
        SaveSystem.MetaSave();
        
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitForSeconds((float)videoPlayer.length);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        
        Application.Quit();
    }
}
