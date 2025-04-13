using UnityEngine;

public class PointLight : MonoBehaviour {
    [SerializeField] private Light m_pointLight;

    private Material m_material;
    private static readonly int LightPos = Shader.PropertyToID("_LightPos");

    void Start() {
        m_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pointLight && m_material) {
            var lightPos = m_pointLight.transform.position;
            m_material.SetVector(LightPos, lightPos);
        }
    }
}
