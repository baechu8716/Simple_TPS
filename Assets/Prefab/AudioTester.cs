using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public AudioManager _audioManager;
    public AudioClip _sfxClip;

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SFXController sfx = _audioManager.GetSFX();
            sfx.transform.parent = transform;
            sfx.Play(_sfxClip);
        }
    }
}
