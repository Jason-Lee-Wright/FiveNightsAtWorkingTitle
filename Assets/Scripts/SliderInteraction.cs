using UnityEngine;
using TMPro;
public class SliderInteraction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] string volumeLabel;
    public void SetVolumeText(float volume)
    {
        volumeText.text = $"{volumeLabel} {volume:F1}";
    }
}
