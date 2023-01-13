using System.Collections;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Services.Animation;
using UnityEngine;

namespace PlayerFolder
{
    public class PlayerAnimation : IPlayerAnimation
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private Transform _playerMeshRotation;
        private Transform _playerMeshScale;
        private Transform _playerTransform;
        private Transform _playerPivotAdjuster;
        private MeshRenderer _playerMeshRenderer;

        private Color _initialColor;
        private readonly Color _onHitColor;
        private readonly Vector3 _scaleTo;
        private readonly float _radius;
        private readonly float _duration;
        private readonly float _minMovement;
        private readonly float _durationKoeficient;

        private bool _bouncing;

        public PlayerAnimation(IGameFactory playerFactory, PlayerStaticData playerStaticData, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _radius = playerStaticData.Radius;
            _duration = playerStaticData.Duration;
            _scaleTo = playerStaticData.ForwardScale;
            _onHitColor = playerStaticData.OnHitColor;
            _minMovement = playerStaticData.MinMovement;
            _durationKoeficient = playerStaticData.DurationKoeficient;

            playerFactory.OnPlayerCreated += SetPlayerDynamicData;
        }
        
        public void Spin(Vector3 direction, float distance, float speed)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.down, direction);
            float angle = distance * -speed * (180f / Mathf.PI) * Time.deltaTime / _radius;
            
            _playerMeshRotation.localRotation = Quaternion.AngleAxis(angle, rotationAxis) * _playerMeshRotation.localRotation;
        }

        public void Bounce(Vector3 hitPos, float movementMagnitude)
        {
            Vector3 initialRotation = _playerPivotAdjuster.localRotation.eulerAngles;
            _playerPivotAdjuster.LookAt(hitPos);
            Vector3 currentRotation = _playerPivotAdjuster.localRotation.eulerAngles;
            Vector3 angleToRotate = currentRotation - initialRotation;
            Debug.Log(angleToRotate);
            // _playerMesh.Rotate(-angleToRotate);

            if (movementMagnitude > _minMovement) 
                _playerMeshRenderer.material.color = _onHitColor;

            if (_bouncing) return;
            
            _coroutineRunner.StartCoroutine(ScaleUpAndDown(_playerPivotAdjuster, _scaleTo, _duration, movementMagnitude));
        }

        private IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration,
            float movementMagnitude)
        {
            _bouncing = true;
            Vector3 initialScale = transform.localScale;
 
            for (float time = 0 ; time < duration * 2 ; time += Time.deltaTime)
            {
                float progress = Mathf.PingPong(time, duration) / duration;
                progress *= movementMagnitude * _durationKoeficient;
                transform.localScale = Vector3.Lerp(initialScale, upScale, progress - 0.1f);
                
                yield return null;
            }
            
            transform.localScale = initialScale;
            // _playerMesh.SetParent(_playerTransform, true);
            _playerMeshRenderer.material.color = _initialColor;
            _bouncing = false;
        }

        private void SetPlayerDynamicData(Player player)
        {
            _playerMeshRotation = player.PlayerMeshRotation;
            _playerMeshScale = _playerMeshScale;
            _playerTransform = player.transform;
            _playerMeshRenderer = player.MeshRenderer;
            _playerPivotAdjuster = player.PivotAdjuster;
            _initialColor = _playerMeshRenderer.material.color;
        }
    }
}