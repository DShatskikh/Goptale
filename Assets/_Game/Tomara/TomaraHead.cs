using System;
using UnityEngine;

public sealed class TomaraHead : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator  = GetComponent<Animator>();
    }

    private void Update()
    {
        if (DialogueWindow.Instance && DialogueWindow.Instance.IsAnimated)
        {
            _animator.SetBool("IsSpeak", true);
        }
        else
        {
            _animator.SetBool("IsSpeak", false);
        }
    }
}
