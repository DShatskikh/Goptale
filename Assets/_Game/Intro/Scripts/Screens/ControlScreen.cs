using UnityEngine;

namespace Screens
{
    public class ControlScreen : Screen
    {
        private readonly string Submit = "Submit";

        [SerializeField] private ChoiceNameScreen _choiceNameScreen;
        
        private void Update()
        {
            if (Input.GetButtonDown(Submit) && _canvasGroup.alpha == 1)
            {
                _choiceNameScreen.Show();
                Close();
            }
        }
    }
}
