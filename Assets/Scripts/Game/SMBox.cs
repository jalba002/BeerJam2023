using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SMBox : MonoBehaviour
{
    // When Enabled, it automatically adds to the list.
    public AreaGroup side;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        // Add itself to the available objectives.
        // Call director and add.
        // Make an instance of the director. But can be destroyed.
        GameDirector.Instance.AddBox(this);
        //this.gameObject.SetActive(true);
        meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        GameDirector.Instance.RemoveBox(this);
        meshRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        // Not needed as we could use the pooling system.
        GameDirector.Instance.RemoveBox(this);
    }
}