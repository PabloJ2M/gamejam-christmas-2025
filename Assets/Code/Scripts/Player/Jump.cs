using UnityEngine;

namespace Player.Controller
{
    [RequireComponent(typeof(Core))]
    public class Jump : MonoBehaviour
    {
        [SerializeField] private float _jumpForce, _coyoteTime = 0.2f;

        [Header("Detection")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector2 _areaSize = new(0.5f, 0.2f);
        [SerializeField] private float _distance = 1f;

        public bool IsGrounded { get; private set; }

        private Core _player;
        private float _airTime;

        private void Awake() => _player = GetComponent<Core>();
        private void Update() => _airTime = IsGrounded ? 0 : _airTime += Time.deltaTime;
        private void FixedUpdate() => _player.Animator.SetFloat("Gravity", _player.Body.linearVelocityY);

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!_player.CompareLayer(collision.collider, _groundMask)) return;
            if (IsOverFloor()) return;
            SetGroundState(false);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_player.CompareLayer(collision.collider, _groundMask)) return;
            if (!IsOverFloor()) return;
            SetGroundState(true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new(0f, 1f, 0f, 0.5f);
            Gizmos.DrawCube(HitPoint(), _areaSize);
        }

        private void OnJump()
        {
            if (!IsGrounded && _airTime > _coyoteTime) return;

            _player.Animator.SetTrigger("Jump");
            _player.VelocityY(_jumpForce);
            _airTime = _coyoteTime;
            SetGroundState(false);
        }

        private Vector2 HitPoint() => _distance * Vector2.down + (Vector2)transform.position;
        private bool IsOverFloor() => Physics2D.OverlapBox(HitPoint(), _areaSize, 0, _groundMask);
        private void SetGroundState(bool value)
        {
            _player.Animator.SetBool("IsGrounded", value);
            IsGrounded = value;
        }
    }
}