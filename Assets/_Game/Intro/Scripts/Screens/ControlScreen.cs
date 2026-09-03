using UnityEngine;

namespace Screens
{
    public class ControlScreen : Screen
    {
        [SerializeField] private ChoiceNameScreen _choiceNameScreen;
        
        private void Update()
        {
            if (InputManager.Instance.IsSubmitDown && _canvasGroup.alpha == 1)
            {
                _choiceNameScreen.Show();
                Close();
            }
        }
    }
}
