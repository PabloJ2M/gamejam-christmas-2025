using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textoUI;
    [SerializeField] private float _startDelay;
    [SerializeField, Range(0f, 0.1f)] private float _charTime = 0.02f;

    [SerializeField, TextArea] private string[] _messages;

    private WaitForSeconds _charDelay;
    private int _index;

    private void Awake() => _charDelay = new(_charTime);
    private void OnEnable() => _textoUI.SetText(string.Empty);

    public void Play() => StartCoroutine(WriteTextAnimation());

    private IEnumerator WriteTextAnimation()
    {
        _textoUI.SetText(_messages[_index]);
        _textoUI.ForceMeshUpdate();

        TMP_TextInfo info = _textoUI.textInfo;
        Color32[] newVertexColors = info.meshInfo[0].colors32;

        for (int i = 0; i < info.characterCount; i++)
            SetCharColor(info, i, ref newVertexColors, 0);

        yield return new WaitForSeconds(_startDelay);

        for (int i = 0; i < info.characterCount; i++)
        {
            if (!info.characterInfo[i].isVisible) continue;
            yield return _charDelay;
            SetCharColor(info, i, ref newVertexColors, 255);
        }

        _index++;
        _index %= _messages.Length;
    }
    private void SetCharColor(TMP_TextInfo info, int index, ref Color32[] newVertexColors, byte alpha)
    {
        int vertexIndex = info.characterInfo[index].vertexIndex;
        Color32 c = newVertexColors[vertexIndex];

        for (int j = 0; j < 4; j++) newVertexColors[vertexIndex + j] = new Color32(c.r, c.g, c.b, alpha);
        _textoUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}