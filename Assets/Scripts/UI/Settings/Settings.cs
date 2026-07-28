using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private GameObject _settingsPanel;
    
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
 
    [Header("Sliders")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider natureSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider vfxSlider;
 
    [Header("Button")]
    [SerializeField] private Button exitButton;
    
    [Header("Defaults")]
    [SerializeField] private float defaultMouseSensitivity = 1f;
    [SerializeField, Range(0.0001f, 1f)] private float defaultVolume = 1f;
 
    // PlayerPrefs keys
    private const string KEY_MOUSE_SENS = "MouseSensitivity";
    private const string KEY_MASTER_VOL = "MasterVolume";
    private const string KEY_NATURE_VOL = "NatureVolume";
    private const string KEY_MUSIC_VOL = "MusicVolume";
    private const string KEY_VFX_VOL = "VFXVolume";


    private bool _isOpen = false;
    
    private void Start()
    {
        LoadSettings();
 
        // Hook up listeners after loading so we don't overwrite saved values on init
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        natureSlider.onValueChanged.AddListener(SetNatureVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        vfxSlider.onValueChanged.AddListener(SetVfxVolume);
    }

    void Update()
    {
        if (_isOpen != InputRouter.instance.EscapePressed)
        {
            _isOpen = InputRouter.instance.EscapePressed;
            _settingsPanel.SetActive(_isOpen);
        }
    }
 
    private void LoadSettings()
    {
        float mouseSens = PlayerPrefs.GetFloat(KEY_MOUSE_SENS, defaultMouseSensitivity);
        float master = PlayerPrefs.GetFloat(KEY_MASTER_VOL, defaultVolume);
        float nature = PlayerPrefs.GetFloat(KEY_NATURE_VOL, defaultVolume);
        float music = PlayerPrefs.GetFloat(KEY_MUSIC_VOL, defaultVolume);
        float vfx = PlayerPrefs.GetFloat(KEY_VFX_VOL, defaultVolume);
 
        mouseSensitivitySlider.value = mouseSens;
        masterSlider.value = master;
        natureSlider.value = nature;
        musicSlider.value = music;
        vfxSlider.value = vfx;
 
        ApplyMouseSensitivity(mouseSens);
        ApplyVolume("Master", master);
        ApplyVolume("Nature", nature);
        ApplyVolume("Music", music);
        ApplyVolume("VFX", vfx);
    }
 
    public void SetMouseSensitivity(float value)
    {
        ApplyMouseSensitivity(value);
        PlayerPrefs.SetFloat(KEY_MOUSE_SENS, value);
        PlayerPrefs.Save();
    }
 
    public void SetMasterVolume(float value)
    {
        ApplyVolume("Master", value);
        PlayerPrefs.SetFloat(KEY_MASTER_VOL, value);
        PlayerPrefs.Save();
    }
 
    public void SetNatureVolume(float value)
    {
        ApplyVolume("Nature", value);
        PlayerPrefs.SetFloat(KEY_NATURE_VOL, value);
        PlayerPrefs.Save();
    }
 
    public void SetMusicVolume(float value)
    {
        ApplyVolume("Music", value);
        PlayerPrefs.SetFloat(KEY_MUSIC_VOL, value);
        PlayerPrefs.Save();
    }
 
    public void SetVfxVolume(float value)
    {
        ApplyVolume("VFX", value);
        PlayerPrefs.SetFloat(KEY_VFX_VOL, value);
        PlayerPrefs.Save();
    }
 
    private void ApplyMouseSensitivity(float value)
    {
        
        GameManager.instance.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
    }
 
    private void ApplyVolume(string exposedParam, float sliderValue)
    {
        // Slider 0-1 (0 = mute, 1 = full volume).
        float dB = sliderValue <= 0.0001f ? -80f : Mathf.Log10(sliderValue) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }

    public void OnExitGamePressed()
    {
        Application.Quit();
    }
    
}
