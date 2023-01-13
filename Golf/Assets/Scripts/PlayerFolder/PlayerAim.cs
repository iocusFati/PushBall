using System.Collections;
using Infrastructure;
using Infrastructure.Factories;
using Infrastructure.Services.Input;
using UnityEngine;

namespace PlayerFolder
{
    public class PlayerAim
    {
        private readonly IInputService _inputService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly PlayerMovement _playerMovement;

        private Player _player;

        private bool _isMoving;

        public PlayerAim(IInputService inputService, IGameFactory gameFactory, IUpdatableLoop updatableLoop, 
            ICoroutineRunner coroutineRunner ,PlayerMovement playerMovement)
        {
            _inputService = inputService;
            _coroutineRunner = coroutineRunner;
            _playerMovement = playerMovement;
            
            updatableLoop.OnUpdate += Update;
            gameFactory.OnPlayerCreated += SetPlayer;
        }

        private void Update()
        {
            if (_player == null)
                return;

            Vector2 direction = _inputService.GetMovement(OnMouseButtonUp) * 0.015f;
            Debug.Log(direction);
            if (direction != Vector2.zero)
            {
                StopSetTargetToBall();
                _player.Target.localPosition = new Vector3(direction.y, 0.5f, -direction.x);
            }

            void OnMouseButtonUp()
            {
                Vector3 targetPosition = _player.Target.localPosition;
                Vector3 moveDirection = new Vector3(targetPosition.x, 0, targetPosition.z);
                
                _playerMovement.Move(moveDirection);

                SetTargetToBall();
            }
        }

        private void SetTargetToBall()
        {
            _isMoving = true;
            _coroutineRunner.StartCoroutine(SetTargetToBallCoroutine());
        }

        private void StopSetTargetToBall()
        {
            _isMoving = false;
            _coroutineRunner.StopCoroutine(SetTargetToBallCoroutine());
        }

        private IEnumerator SetTargetToBallCoroutine()
        {
            while (_isMoving && _player != null)
            {
                _player.Target.localPosition = Vector3.zero;
                yield return null;
            }
        }

        private void SetPlayer(Player player)
        {
            _player = player;
            _player.OnPlayerDestroy += () =>
            {
                _player = null;
                Debug.Log("Destroy");
            };
        }
    }
}