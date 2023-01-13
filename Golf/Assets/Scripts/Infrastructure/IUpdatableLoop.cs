using System;

namespace Infrastructure
{
    public interface IUpdatableLoop
    {
        public event Action OnUpdate;
        public void Update();
    }
}