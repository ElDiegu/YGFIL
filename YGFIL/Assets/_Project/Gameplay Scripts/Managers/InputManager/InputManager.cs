using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YGFIL.Managers
{
    public class InputManager : StaticInstance<InputManager>
    {
        [SerializeField] private PlayerInput playerInput;
        public InputStruct InputStruct { get; private set; }
        
        protected override void Awake() 
        {
            base.Awake();
            if (!playerInput) playerInput = GetComponent<PlayerInput>();
            InputStruct = new InputStruct(playerInput);
        }
        
        private void Update() 
        {
            GatherInput();
        }

        private void GatherInput()
        {
            InputStruct = new InputStruct(playerInput);
        }
        
        public InputStruct GetInput() 
        {
            return InputStruct;
        }
    }
    
    public struct InputStruct
    {
        public PlayerInput playerInput;
        public bool LeftClick { get; private set;}
        public bool RightClick { get; private set; }        
        
        public InputStruct(PlayerInput playerInput) 
        {
            this.playerInput = playerInput;
            this.LeftClick = playerInput.actions["LeftClick"].triggered;
            this.RightClick = playerInput.actions["RightClick"].triggered;
        }
    }
}
