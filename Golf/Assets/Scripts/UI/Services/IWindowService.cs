using Infrastructure.Services;

namespace UI.Services
{
    public interface IWindowService : IService
    {
        public void Open(WindowID windowID);
    }
}