using UnityEngine;

public class MapSegment : MonoBehaviour
{
    public float overrideLenght = 0f;

    public float Length
    {
        get
        {
            if (overrideLenght > 0f) return overrideLenght;

            var renderers = GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return 10f;

            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                b.Encapsulate(renderers[i].bounds);
            }
            return b.size.x;
        }
    }
}