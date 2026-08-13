using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Level0DialogueOneMessage", menuName = "Dialogue/DialogueOneMessage", order = 53)]
    public class DialogueOneMessage : ScriptableObject
    {
        [SerializeField]
        private Node _node;

        public Node Node => _node;
    }
}