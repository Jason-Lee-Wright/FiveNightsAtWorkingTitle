using TMPro;
using UnityEngine;

public class CameraUI : MonoBehaviour
{
    [SerializeField] IntEvent camIndex;
    [SerializeField] TextMeshProUGUI camText;

    private void SetCamText(int index)
    {
        camText.text = $"CAM {index + 1}";
    }

    private void OnEnable()
    {
        camIndex.onEvent += SetCamText;
    }

    private void OnDisable()
    {
        camIndex.onEvent += SetCamText;
    }
}
