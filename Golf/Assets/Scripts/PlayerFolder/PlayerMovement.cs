using System;
using System.Collections;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Services.Animation;
using Infrastructure.Services.Particle;
using UnityEngine;

namespace PlayerFolder
{
    public class PlayerMovement : IDisposable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPlayerAnimation _playerAnimation;
        private readonly IPlayerParticle _playerParticle;

        private float _speed;
        private float _movementMagnitude;

        private readonly float _initialSpeed;
        private readonly float _reduceSpeedForTick;
        private readonly float _reduceSpeedAfterCollision;
        private readonly float _minMovement;

        private Player _player;

        private Transform _playerTransform;

        private Vector3 _direction;
        private Vector3 _posBeforeMoving;
        
        private Rigidbody _playerRB;

        public PlayerMovement(IGameFactory gameFactory, ICoroutineRunner coroutineRunner, 
            PlayerStaticData playerStaticData, IPlayerAnimation playerAnimation, IPlayerParticle playerParticle)
        {
            _coroutineRunner = coroutineRunner;
            _playerAnimation = playerAnimation;
            _playerParticle = playerParticle;

            #region SetPlayerStaticData

            _minMovement = playerStaticData.MinMovement;
            _speed = playerStaticData.Speed;
            _initialSpeed = playerStaticData.Speed;
            _reduceSpeedForTick = playerStaticData.ReduceSpeedForTick;
            _reduceSpeedAfterCollision = playerStaticData.ReduceSpeedAfterCollision;

            #endregion

            gameFactory.OnPlayerCreated += SetPlayerData;
        }

        public void Move(Vector3 direction)
        {
            _posBeforeMoving = _playerTransform.position;
            _coroutineRunner.StartCoroutine(MovePlayer(direction));
        }

        private IEnumerator MovePlayer(Vector3 movement)
        {
            float distance = movement.magnitude;
            
            _direction = movement.normalized;
            _speed = _initialSpeed;
            
            while (_speed > 0 && _player != null)
            {
                movement = distance * _direction *  _speed * Time.deltaTime;
                _movementMagnitude = movement.magnitude;
                
                _playerRB.velocity = movement;
                _speed -= _reduceSpeedForTick;
                
                if (movement.magnitude > _minMovement) 
                    _playerAnimation.Spin(_direction, distance, _speed);
                
                _player.Target.localPosition = Vector3.zero;

                yield return null;
            }
           
            if (_playerRB != null)
                _playerRB.velocity = Vector3.zero;
        }

        private void OnWallCollisionEnterAction(Collision collision)
        {
            var collContact = collision.contacts[0];
            var collContactNormal = collContact.normal;

            //just `cause ball might hit not in set direction due to width
            var direction = Vector3.Normalize(collContact.point - _posBeforeMoving);

            float angleBetweenBallAndWall = Mathf.Abs(Vector3.Dot(direction, collContactNormal));
            _direction = Vector3.Reflect(direction, collContactNormal);

            float bounceForce = _movementMagnitude * angleBetweenBallAndWall;
            _playerAnimation.Bounce(collContact.point, bounceForce);
            _playerParticle.SmokeBurst(collContact.point, collContactNormal, bounceForce);

            _speed *= _reduceSpeedAfterCollision;
        }

        private void SetPlayerData(Player player)
        {
            _player = player;
            _playerTransform = player.transform;
            _playerRB = player.GetComponent<Rigidbody>();
            
            player.OnWallCollisionEnterAction += OnWallCollisionEnterAction;
            player.OnPlayerDestroy += Dispose;
        }

        public void Dispose()
        {
            _player.OnWallCollisionEnterAction -= OnWallCollisionEnterAction;
            _player.OnPlayerDestroy -= Dispose;
        }
    }
}