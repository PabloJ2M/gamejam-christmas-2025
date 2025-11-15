using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Player.Controller
{
    [RequireComponent(typeof(Core))]
    public class Movement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _accelRate = 25;

        [Header("Camera")]
        [SerializeField] private CinemachinePositionComposer _camera;
        [SerializeField, Range(0, 10)] private float _offset = 2;

        private Core _player;
        private int _input;

        private void Awake() => _player = GetComponent<Core>();
        private void FixedUpdate()
        {
            float targetSpeed = _input * _speed;
            float speedDif = targetSpeed - _player.Body.linearVelocityX;
            float movement = speedDif * _accelRate;

            if (movement != 0) _player.AddForceX(movement, ForceMode2D.Force);
            if (_input != 0) _player.SetDirection(_input);
        }

        private void OnMove(InputValue value)
        {
            float input = value.Get<Vector2>().x;

            _input = Mathf.RoundToInt(input);
            _player.Animator.SetInteger("Speed", _input);
            _camera.TargetOffset.x = _input * _offset;
        }
    }
}