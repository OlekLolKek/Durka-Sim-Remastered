using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class InputController : IExecute
    {
        private readonly InputModel _inputModel;

        public InputController(InputModel inputModel)
        {
            _inputModel = inputModel;
        }

        public void Execute(float deltaTime)
        {
            _inputModel.Horizontal = Input.GetAxisRaw(AxisNames.HORIZONTAL);
            _inputModel.Vertical = Input.GetAxisRaw(AxisNames.VERTICAL);
            _inputModel.GetFireButtonDown = Input.GetKeyDown(AxisNames.FIRE);
            _inputModel.GetJumpButtonDown = Input.GetKeyDown(AxisNames.JUMP);
        }
    }
}