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

        //[Header("Camera Offset")]
        //[SerializeField] private CinemachinePositionComposer _camera;
        //[SerializeField, Range(0, 10)] private float _offset = 2;

        private Core _player;
        PlayerStun _stun;
        private int _input;

        private void Awake() 
        { 
            _player = GetComponent<Core>(); 
            _stun = GetComponent<PlayerStun>();

            if (_stun != null)
                _stun.OnStunFinished += RecuperarControl;
        }
        private void FixedUpdate()
        {
            if (_stun != null && _stun.IsStunned)
            {
                var vel = _player.Body.linearVelocity;
                vel.x = 0f;
                _player.Body.linearVelocity = vel;
                _player.Animator.SetInteger("Speed", 0);

                //if (_camera != null)
                //    _camera.TargetOffset.x = 0f;

                return;
            }

            float targetSpeed = _input * _speed;
            float speedDif = targetSpeed - _player.Body.linearVelocityX;
            float movement = speedDif * _accelRate;

            if (movement != 0)
                _player.AddForceX(movement, ForceMode2D.Force);

            if (_input != 0)
                _player.SetDirection(_input);
        }

        internal void OnMove(InputValue value)
        {
            if (_stun != null && _stun.IsStunned)
            {
                _input = 0;
                _player.Animator.SetInteger("Speed", 0);

                //if (_camera != null)
                //    _camera.TargetOffset.x = 0f;

                return;
            }

            float input = value.Get<Vector2>().x;
            _input = Mathf.RoundToInt(input);

            _player.Animator.SetInteger("Speed", _input);

            //if (_camera != null)
            //    _camera.TargetOffset.x = _input * _offset;
        }

        private void RecuperarControl()
        {
            _input = 0; 
            _player.Animator.SetInteger("Speed", 0);
            //if (_camera != null)
            //    _camera.TargetOffset.x = 0f;
        }
    }
}