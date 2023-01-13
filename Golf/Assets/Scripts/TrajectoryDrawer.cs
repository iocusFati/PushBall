using Infrastructure;
using Infrastructure.Factories;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class TrajectoryDrawer
{
    private LineRenderer _lineRenderer;
    
    private Vector3 _playerPosition => _player.transform.position;
    private Vector3 _targetPosition => _player.Target.position;
    private PlayerFolder.Player _player;

    public TrajectoryDrawer(IUpdatableLoop updater, IGameFactory gameFactory)
    {
        gameFactory.OnPlayerCreated += UpdateData;
        updater.OnUpdate += Update;
    }

    private void Update()
    {
        if (_player == null) return;
        
        _lineRenderer.SetPosition(0, _playerPosition);
        _lineRenderer.SetPosition(1, _targetPosition);
    }

    private void UpdateData(PlayerFolder.Player player)
    {
        _player = player;
        
        _lineRenderer = Object.FindObjectOfType(typeof(LineRenderer)).GetComponent<LineRenderer>();
    }
}
