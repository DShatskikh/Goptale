using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class Node
    {
        public Node(string text)
        {
            Text = text;
        }
        
        public string Text;
        public GameObject Head;
        public int NumberEmotion;
        public float SpeedRecruiting = 0.05f;
        public AudioClip soundPrint;
    }
}