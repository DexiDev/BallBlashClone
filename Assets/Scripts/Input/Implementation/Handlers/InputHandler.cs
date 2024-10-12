using System;
using DexiDev.Game.Core;
using DexiDev.Game.Data.Fields;
using DexiDev.Game.Gameplay;
using DexiDev.Game.Input;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Characters.Handlers.Player
{
    public class InputHandler : IHandler<PlayerController>
    {
        [SerializeField] private float _stopAcceleration = 5f;
        [SerializeField] private bool _enableXDirection;
        [SerializeField] private bool _enableYDirection;

        private DirectionField _directionField;
        private Vector3 _targetDirection;

        private InputManager _inputManager;

        [Inject]
        private void Install(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        private void Awake()
        {
            _directionField = _targetData.GetDataField<DirectionField>(true);
        }

        public void Update()
        {
            if (_inputManager.Direction != Vector2.zero)
            {
                _targetDirection = new Vector3(_enableXDirection ? _inputManager.Direction.x : _enableYDirection ? _inputManager.Direction.y : 0f, 0f);
            }

            bool isMoving = _targetDirection != Vector3.zero;
            
            if (isMoving)
            {
                _targetData.Move(_targetDirection * Time.deltaTime);
            }

            if (_targetDirection != Vector3.zero)
            {
                _targetDirection = Vector3.Lerp(_targetDirection, Vector3.zero, Time.deltaTime * _stopAcceleration);
            }

            _directionField?.SetValue(_targetDirection);
        }
    }
}