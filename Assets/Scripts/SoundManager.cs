using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private SO_SFXList sfxList;

    public static SoundManager Instance;

    private void Awake()
    {
        if( Instance != null )
        {
            Destroy(gameObject);
            // Actually, this is helpful behaviour, so I can put them in any scene and only one will persist
            // so I'm silencing this error as it's not harmful.
//            Debug.LogError("Attempted to reinstatiate SoundManager, but it exists, so bailing.");
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX_PlayerShot()
    {
        if (GameManager.Instance.MuteSFX)
            return;

        audioSource.PlayOneShot(sfxList.playershot);
    }

    public void PlaySFX_Explosion()
    {
        if (GameManager.Instance.MuteSFX)
            return;

        audioSource.PlayOneShot(sfxList.explosion);
    }


}
