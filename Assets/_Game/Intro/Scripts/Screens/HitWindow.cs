using System;
using System.Collections;
using UnityEngine;

namespace Screens
{
    public class HitWindow : Screen
    {
        [SerializeField]
         private HitSlide _hitSlide;

         [SerializeField]
         private float _speedOpen = 1;

         private Transform _transform;

         private void Awake()
         {
             _transform = transform;
         }

         public override void Show()
         {
             gameObject.SetActive(true);
             StartCoroutine(WaitOpen());
             _hitSlide.EndMove += Close;
         }

         public override void Close()
         {
             StartCoroutine(WaitClose());
         }

         private IEnumerator WaitOpen()
         {
             var scale = 0f;

             while (scale < 1)
             {
                 scale += _speedOpen * Time.deltaTime;
                 _transform.localScale = new Vector3(scale, 1, 1);
                 yield return null;
             }
             
             _hitSlide.StartMove();
         }
         
         private IEnumerator WaitClose()
         {
             var scale = 1f;

             while (scale > 0)
             {
                 scale -= _speedOpen * Time.deltaTime;
                 _transform.localScale = new Vector3(scale, 1, 1);
                 yield return null;
             }
             
             gameObject.SetActive(false);
             _hitSlide.EndMove -= Close;
         }
    }
}
