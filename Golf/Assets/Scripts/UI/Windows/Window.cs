using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class Window : MonoBehaviour
    {
        public List<Button> ExitButtons;

        private void Awake()
        {
            OnAwake();
        }
        
        protected virtual void OnAwake()
        {
            foreach (var button in ExitButtons)
            {
                button.onClick.AddListener(() => Destroy(gameObject));
            } 
        }
    }
}