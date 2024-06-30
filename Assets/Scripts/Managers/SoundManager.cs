using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Dictionary<BGM_list, AudioClip> BGM_audioclips = new Dictionary<BGM_list, AudioClip>();
    [SerializeField] Dictionary<SFX_list, AudioClip> SFX_audioclips = new Dictionary<SFX_list, AudioClip>();

    [SerializeField] public float volume_BGM = 0.5f;
    [SerializeField] public float volume_SFX = 1f;

    [SerializeField] public List<BGM_Datas> BGM_datas = new List<BGM_Datas>();
    [SerializeField] public List<SFX_Datas> SFX_datas = new List<SFX_Datas>();

    [SerializeField] private GameObject BGM_Object;
    [SerializeField] private GameObject SFX_Object;

    [SerializeField] public GameObject SoundUI;

    private enum SoundType {
        BGM,
        SFX,
    }

    [System.Serializable]
    [SerializeField]
    public struct SFX_Datas
    {
        public SFX_list sfx_name;
        public AudioClip audio;
    }

    [System.Serializable]
    [SerializeField]
    public struct BGM_Datas
    {
        public BGM_list bgm_name;
        public AudioClip audio;
    }

    // ȿ���� ���
    public enum SFX_list
    { 
        FLAP_1, FLAP_2, FLAP_3, FLAP_4, WHEEP,
        PUT_ON_1,
        MATCH, NON_MATCH,
        VICTORY, BUTTON_CLICK,
        TIMEOUT,
        
    }

    // ����� ���
    public enum BGM_list
    {
        Home_BGM1, Home_BGM2, Home_BGM3, Home_BGM4,
    }

    // �̱���
    static public SoundManager Instance; 
    private void Awake()
    {
        if (Instance == null)   // ���� ����
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else  
            Destroy(this.gameObject);
    }

    void Start()
    {
        // ����Ʈ�� ���� SFX audioClip �� ��� dictionary�� ����
        for (int i = 0; i < SFX_datas.Count; i++)
        {
            if (SFX_datas[i].audio == null) continue; // ȿ���� ������ ����X
            SFX_audioclips.Add(SFX_datas[i].sfx_name, SFX_datas[i].audio);
        }
        // ����Ʈ�� ���� BGM audioClip �� ��� dictionary�� ����
        for (int i = 0; i < BGM_datas.Count; i++)
        {
            if (BGM_datas[i].audio == null) continue; // ����� ������ ����X
            BGM_audioclips.Add(BGM_datas[i].bgm_name, BGM_datas[i].audio);
        }

        // �� �ε�� �̺�Ʈ
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }


    // ���� ��� - ���
    public void PlayBGM(BGM_list _type)
    {
        // ���� �̸�
        BGM_list playSoundName = _type;

        // ���� ��ü
        GameObject soundObject = BGM_Object;
        AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
        audioSource.clip = BGM_audioclips[playSoundName]; // ���� �ҷ�����
        audioSource.volume = volume_BGM; // ��������
        audioSource.Play(); // ���� ���
    }

    // ���� ��� - ȿ����
    public void PlaySFX(SFX_list _type)
    {
        // ���� �̸�
        SFX_list playSoundName = _type;

        // ���� ��ü
        GameObject soundObject = SFX_Object;
        AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
        audioSource.volume = volume_SFX; // ��������
        audioSource.PlayOneShot(SFX_audioclips[playSoundName]); // ���� ���
    }

    // �������� ���� �ٲܶ� ���
    public void ChangeVolume_BGM(float _vol)
    {
        volume_BGM = _vol;
        AudioSource audioSource = BGM_Object.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
        audioSource.volume = volume_BGM; // ��������
    }

    // �������� ���� �ٲܶ� ���
    public void ChangeVolume_SFX(float _vol)
    {
        volume_SFX = _vol;
        AudioSource audioSource = SFX_Object.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
        audioSource.volume = volume_SFX; // ��������
    }

    public void Play_ButtonClickSound()
    {
        PlaySFX(SFX_list.BUTTON_CLICK);
    }

    public void PlayBGM()
    {
        PlayBGM(BGM_list.Home_BGM1);
    }

    public void StopBGM()
    {
        BGM_Object.GetComponent<AudioSource>().Stop();
    }


    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main") SoundUI.SetActive(true);
        else SoundUI.SetActive(false);
    }
}