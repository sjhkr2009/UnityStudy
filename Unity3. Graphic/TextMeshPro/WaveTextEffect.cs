using System.Collections;
using UnityEngine;
using TMPro;

public class WaveTextEffect : MonoBehaviour {
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private int startIndex = 0;
    [SerializeField] private int endIndex = int.MaxValue;
    [SerializeField] private float bounceHeight = 5f;
    [SerializeField] private float bounceSpeed = 10f;
    [SerializeField] private float bounceDelay = 1f;
    [SerializeField] private float charInterval = 0.05f;

    private void Start() {
        StartCoroutine(BounceAnimation());
    }

    IEnumerator BounceAnimation() {
        yield return new WaitForSeconds(0.1f);

        var textInfo = targetText.textInfo;

        while (true) {
            for (int i = 0; i < textInfo.characterCount; ++i) {
                var charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;
                if (i < startIndex || i > endIndex) continue; 

                yield return new WaitForSeconds(charInterval);

                StartCoroutine(BounceChar(i, bounceHeight, bounceSpeed));
            }
            
            yield return new WaitForSeconds(bounceDelay);
        }
    }

    IEnumerator BounceChar(int charIndex, float height, float speed) {
        var textInfo = targetText.textInfo;
        var charInfo = textInfo.characterInfo[charIndex];

        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;
        var vertices = textInfo.meshInfo[materialIndex].vertices;

        float timer = 0.0f;
        while (timer < Mathf.PI) {
            timer += Time.deltaTime * speed;

            float offset = (Mathf.Sin(timer) * height);

            var waveOffset = new Vector3(0, offset, 0);
            
            for (int i = 0; i < 4; i++) {
                vertices[vertexIndex + i] += waveOffset;
            }

            targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            yield return null;
            
            for (int i = 0; i < 4; i++) {
                vertices[vertexIndex + i] -= waveOffset;
            }
        }
    }
}
