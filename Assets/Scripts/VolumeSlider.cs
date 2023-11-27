
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] TMP_Text masterVolDisplay;
    [SerializeField] TMP_Text SFXvolDisplay;
    [SerializeField] TMP_Text VOvolDisplay;

    [SerializeField] Scrollbar masterBar;
    [SerializeField] Scrollbar sfxBar;
    [SerializeField] Scrollbar voBar;

    private void OnEnable()
    {
        GameManager.Instance.GetAudioManager.GetVolumes(out float master, out float sfx, out float vos);
        
        masterBar.value = Convert(master);
        sfxBar.value = Convert(sfx);
        voBar.value = Convert(vos);

        Display(SFXvolDisplay, sfxBar.value);
        Display(masterVolDisplay, masterBar.value);
        Display(VOvolDisplay, voBar.value);
    }

    public void ONSFXChange(Scrollbar bar)
    {
        GameManager.Instance.GetAudioManager.SetSFXVolume(bar.value);
        Display(SFXvolDisplay, bar.value);
    }

    public void ONMasterChange(Scrollbar bar)
    {
        GameManager.Instance.GetAudioManager.SetMasterVolume(bar.value);

        Display(masterVolDisplay, bar.value);
    }

    public void ONVOChange(Scrollbar bar)
    {
        GameManager.Instance.GetAudioManager.SetVOVolume(bar.value);

        Display(VOvolDisplay, bar.value);
    }

  
    void Display(TMP_Text block, float value)
    {
        if(value <= 0) block.text = "Mute";
        else if (value >= 1.0f) block.text = "Max";
        else block.text = $"{(int)(value * 10.0f)}";
    }

    float Convert(float volume) => (20.0f + volume) / 20.0f;
}
