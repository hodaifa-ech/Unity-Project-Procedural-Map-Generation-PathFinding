using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudios : AudioPlayer
{
    [SerializeField]
    private AudioClip shootBulletClip = null, outOfBulletsClip = null;

    public void PlayShootSound()
    {
        PlayClip(shootBulletClip);
    }

    public void PlayNoBulletsSound()
    {
        PlayClip(outOfBulletsClip);
    }
}