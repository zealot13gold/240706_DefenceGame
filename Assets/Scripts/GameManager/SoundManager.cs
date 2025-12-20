using UnityEngine;
using System;

// 게임에 필요한 각종 BGM, SFX 저장
// 게임 사운드 상태 저장 및 관리(변경은 옵션창에서 수행)

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance=null;

    // 게임 BGM 등록
    [Header("BGM 등록")]
    [Tooltip("로비 BGM")] public AudioClip lobbyBGMClip;

    // 게임 SFX 등록
    [Header("SFX 등록")]
    [Header("플레이어 유닛 대사")]
    [Tooltip("유닛 생산")] public AudioClip playerProducedClip;
    [Tooltip("유닛 선택")] public AudioClip playeChosenClip;
    [Tooltip("유닛 이동")] public AudioClip playerMoveClip;
    [Tooltip("유닛 공격")] public AudioClip playerAttackClip;
    [Tooltip("유닛 피격")] public AudioClip playerHitClip;
    [Tooltip("유닛 사망")] public AudioClip playerDeadClip;

    [Header("플레이어 공격 효과음")]
    [Tooltip("무기 사용")] public AudioClip fireArmClip;

    [Header("적 유닛 대사")]
    [Tooltip("유닛 이동")] public AudioClip enemyMoveClip;
    [Tooltip("유닛 공격")] public AudioClip enemyAttackClip;
    [Tooltip("유닛 사망")] public AudioClip enemyDeadClip;

    [Header("적 공격 효과음")]
    [Tooltip("적 공격")] public AudioClip hitClip;

    // 이벤트 등록
    public event Action<float> bgmVolumeChanged;
    public event Action<float> sfxVolumeChanged;

    // 음량비 저장
    [HideInInspector] public float bgmVolume;
    [HideInInspector] public float sfxVolume;

    private void Awake()
    {
        if(instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BGMVolume(float volume)
    {
        // SoundManager에 슬라이더로부터 전달된 음량비 저장
        bgmVolume = volume;

        // SoundManager를 구독하는 모든 오브젝트에 해당 값 전달
        bgmVolumeChanged?.Invoke(volume);
    }

    public void SFXVolume(float volume)
    {
        // SoundManager에 슬라이더로부터 전달된 음량비 저장
        sfxVolume = volume;

        // SoundManager를 구독하는 모든 오브젝트에 해당 값 전달
        sfxVolumeChanged?.Invoke(volume);
    }
}
