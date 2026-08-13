using System;
using UnityEngine;

namespace Screens
{
    public class HitSlide : MonoBehaviour
    {
        [SerializeField]
        private Transform _startTarget;

        [SerializeField] 
        private Transform _endTarget;

        [SerializeField]
        private float _speed;

        private Transform _transform;
        private bool _isInput;

        public event Action EndMove;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            Move();
            
            if (_transform.position.x >= _endTarget.position.x && !_isInput)
            {
                _isInput = true;
                EndMove?.Invoke();
            }
        }

        public void StartMove()
        {
            _isInput = false;
            gameObject.SetActive(true);
            enabled = true;
            _transform.position = _startTarget.position;
        }

        public void OnEndMove()
        {
            enabled = false;
        }

        private void Move()
        {
            var currentPosition = _transform.position;
            currentPosition.x += _speed * Time.deltaTime;
            _transform.position = currentPosition;
        }
    }
}
