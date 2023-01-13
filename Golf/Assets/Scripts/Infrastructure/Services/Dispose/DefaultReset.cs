using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Dispose
{
    public interface IDefaultResetService : IService
    {
        public void AddListener(IDefault disposable);
        public void ToDefault();
    }

    public class DefaultResetService : IDefaultResetService
    {
        private readonly List<IDefault> _toDefaults = new();

        public void AddListener(IDefault disposable) =>
            _toDefaults.Add(disposable);

        public void ToDefault()
        {
            foreach (var item in _toDefaults)
            {
                item.ToDefault();
            }
        }
    }
}