using Lab;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bullet;

    public List<AudioClip> fireAudios;

    public AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void FireDown(Vector2 dir)
    {
        audioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
        audioSource.Play();

        GameObject bulletObj = Instantiate(bullet);
        bulletObj.transform.position = bullet.transform.position;
        Bullet playerBullet = bulletObj.GetComponent<Bullet>();
        playerBullet.Init(dir, "enemy");
    }

    public void FireHold(Vector2 dir)
    {
       
    }

    public void FireUp(Vector2 dir)
    {
       
    }
}
