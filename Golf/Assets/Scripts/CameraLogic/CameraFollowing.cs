using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Factories;
using UnityEngine;

namespace CameraLogic
{
    public class CameraFollowing
    {
        private float _rotationAngleX;
        private float _rotationAngleY;
        private float _distance;
        private float _offsetY;

        private Transform _camera;
        private Transform _player;

        public CameraFollowing(IGameFactory gameFactory, PlayerStaticData playerStaticData, ILateUpdatableLoop lateUpdatableLoop)
        {
            SetPlayerStaticData(playerStaticData);
            
            gameFactory.OnPlayerCreated += SetPlayerDynamicData;
            lateUpdatableLoop.OnLateUpdate += LateUpdate;
        }

        private void LateUpdate()
        {
            if (_player == null)
                return;

            Quaternion rotation = Quaternion.Euler(_rotationAngleX, _rotationAngleY, 0);
            Vector3 position = rotation * new Vector3(0, 0, -_distance) + FollowingPosition();

            _camera.rotation = rotation;
            _camera.position = position;
        }

        private Vector3 FollowingPosition()
        {
            Vector3 followingPosition = _player.position;
            followingPosition.y += _offsetY;

            return followingPosition;
        }

        private void SetPlayerDynamicData(PlayerFolder.Player player)
        {
            _player = player.transform;
            _camera = player.CameraFollower.transform;
        }

        private void SetPlayerStaticData(PlayerStaticData playerStaticData)
        {
            _rotationAngleX = playerStaticData.RotationAngleX;
            _rotationAngleY = playerStaticData.RotationAngleY;
            _distance = playerStaticData.Distance;
            _offsetY = playerStaticData.OffsetY;
        }
    }
}