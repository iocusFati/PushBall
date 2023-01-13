using System;
using Infrastructure.Services;
using PlayerFolder;
using UnityEngine;

namespace Infrastructure.Factories
{
    public interface IGameFactory : IService
    {
        event Action<Player> OnPlayerCreated;
        Player CreatePlayer(Vector3 at);
    }
}