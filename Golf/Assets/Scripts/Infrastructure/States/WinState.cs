using Infrastructure.Services.Input;
using Infrastructure.States.Interfaces;
using UI.Services;

namespace Infrastructure.States
{
    public class WinState : IState
    {
        private readonly IWindowService _windowService;
        private readonly IInputService _inputService;

        public WinState(IWindowService windowService, IInputService inputService)
        {
            _windowService = windowService;
            _inputService = inputService;
        }
        public void Enter()
        {
            _windowService.Open(WindowID.Win);
            _inputService.BlockInput();
        }
        
        public void Exit()
        {
            
        }
    }
}