using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.AssetProviderFolder
{
    public interface IAssets : IService
    {
        public TCreatable Instantiate<TCreatable>(string path, Transform parent) where TCreatable : Object;
        public TCreatable Instantiate<TCreatable>(string path, Vector3 at) where TCreatable : Object;
        public TCreatable Instantiate<TCreatable>(string path) where TCreatable : Object;
    }
}