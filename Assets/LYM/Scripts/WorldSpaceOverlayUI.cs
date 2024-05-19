using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldSpaceOverlayUI : MonoBehaviour
{
    private const string shaderTestMode = "unitt_GUI_ZTestMode";
    [SerializeField] UnityEngine.Rendering.CompareFunction desiredUIComparsion = UnityEngine.Rendering.CompareFunction.Always;
    [SerializeField] Graphic[] uiElementToApplyEffectTo;

    private Dictionary<Material, Material> materialsMappings = new Dictionary<Material, Material>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var graphic in uiElementToApplyEffectTo)
        {
            Material material = graphic.materialForRendering;
            if (material == null)
            {
                Debug.Log("Target Material doesn't have rendering component.");
                continue;
            }

            if (materialsMappings.TryGetValue(material, out Material materialCopy) == false)
            {
                materialCopy = new Material(material);
                materialsMappings.Add(material, materialCopy);
            }

            materialCopy.SetInt(shaderTestMode, (int)desiredUIComparsion);
            graphic.material = materialCopy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
