using UnityEngine;

public class DoorSign : MonoBehaviour
{
    public string signText = "Door";
    public float textSize = 0.1f;
    public Color textColor = Color.black;
    public Color backgroundColor = new Color(0.8f, 0.7f, 0.5f);
    public Vector3 signSize = new Vector3(1f, 0.5f, 0.05f);
    
    private TextMesh textMesh;
    
    void Start()
    {
        CreateSign();
    }
    
    void CreateSign()
    {
        // Create the sign backing
        GameObject signBackingObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        signBackingObj.transform.parent = transform;
        signBackingObj.transform.localPosition = Vector3.zero;
        signBackingObj.transform.localScale = signSize;
        
        // Set the backing material
        Renderer backingRenderer = signBackingObj.GetComponent<Renderer>();
        Material backingMaterial = new Material(Shader.Find("Standard"));
        backingMaterial.color = backgroundColor;
        backingRenderer.material = backingMaterial;
        
        // Create the text
        GameObject textObj = new GameObject("SignText");
        textObj.transform.parent = transform;
        textObj.transform.localPosition = new Vector3(0, 0, -signSize.z/2 - 0.01f); // Slightly in front of sign
        textObj.transform.localRotation = Quaternion.identity;
        
        // Add TextMesh component
        textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = signText;
        textMesh.fontSize = 50;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.color = textColor;
        textMesh.characterSize = textSize;
    }
    
    // Method to update the sign text at runtime if needed
    public void UpdateText(string newText)
    {
        signText = newText;
        if (textMesh != null)
        {
            textMesh.text = newText;
        }
    }
}