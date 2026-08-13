using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Level0Dialogue", menuName = "Dialogue/Dialogue", order = 52)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private List<Node> _nodes;

        public List<Node> Nodes => _nodes;
    }
}
