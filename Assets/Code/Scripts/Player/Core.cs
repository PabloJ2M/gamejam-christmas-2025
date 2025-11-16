using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    [RequireComponent(typeof(PlayerInput), typeof(Animator), typeof(Rigidbody2D))]
    public class Core : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public Rigidbody2D Body { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Body = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetDirection => transform.right;
        public void SetDirection(int value) => transform.eulerAngles = new(0, value > 0 ? 0f : 180f, 0f);

        public void AddForceX(float force, ForceMode2D mode = ForceMode2D.Impulse) => Body.AddForceX(force, mode);
        public void AddForceY(float force, ForceMode2D mode = ForceMode2D.Impulse) => Body.AddForceY(force, mode);

        public void VelocityDirection(Vector2 direcion) => Body.linearVelocity = direcion;
        public void VelocityY(float value) => Body.linearVelocityY = value;
        public void VelocityX(float value) => Body.linearVelocityX = value;

        public bool CompareLayer(Collider2D obj, LayerMask mask) => ((1 << obj.gameObject.layer) & mask) != 0;
        public RaycastHit2D RayDetection(Vector2 direction, Vector2 offset, float distance, LayerMask mask)
        {
            Ray2D ray = new(Body.position + offset, direction);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distance, mask);
            Debug.DrawRay(ray.origin, ray.direction * distance, hit.collider ? Color.green : Color.red);
            return hit;
        }
    }
}