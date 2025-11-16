using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
    public class TextMeshPro_Scrolling : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _padding;

        private readonly LinkedList<RectTransform> _texts = new();
        private float _preferredWidth;
        private bool _inverted;

        private void Start()
        {
            _inverted = _speed < 0;
            _texts.AddFirst(transform.GetChild(0) as RectTransform);
            _preferredWidth = _texts.First.Value.GetComponent<TextMeshProUGUI>().preferredWidth + _padding;

            for (int i = 1; i < 3; i++)
            {
                RectTransform clone = Instantiate(_texts.First.Value, transform);
                SetBlockAtLastPosition(clone);
                _texts.AddLast(clone);
            }
        }
        private void Update()
        {
            float delta = _speed * 50 * Time.deltaTime;
            foreach (var text in _texts) DisplaceText(text, delta);

            RectTransform rect = _texts.First.Value;
            if ((!_inverted && rect.localPosition.x + _preferredWidth > 0) || (_inverted && rect.localPosition.x - _preferredWidth < _preferredWidth)) return;

            SetBlockAtLastPosition(rect);
            _texts.RemoveFirst();
            _texts.AddLast(rect);
        }

        private void SetBlockAtLastPosition(RectTransform rect)
        {
            Vector2 lastPosition = _texts.Last.Value.localPosition;
            rect.localPosition = new(lastPosition.x + (_inverted ? -_preferredWidth : _preferredWidth), lastPosition.y);
        }
        private void DisplaceText(RectTransform rect, float delta)
        {
            Vector2 pos = rect.localPosition;
            pos.x -= delta;
            rect.localPosition = pos;
        }
    }
}