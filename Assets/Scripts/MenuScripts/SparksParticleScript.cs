using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SparksParticleScript : MonoBehaviour
{
    [SerializeField] private GameObject sparksHolder;
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Start()
    {
        particleSystems = sparksHolder.GetComponentsInChildren<Spark>()
            .Select(s => s.GetComponent<ParticleSystem>())
            .Where(ps => ps != null)
            .ToList();
    }

    public void StartRandomParticles()
    {
        int count = Random.Range(0, 100);
        int quantity = count switch
        {
            >= 20 and < 40 => 1,
            >= 40 and < 75 => 2,
            >= 75 and < 95 => 3,
            >= 95 and < 100 => 4,
            _ => 0
        };

        PlayRandomParticles(quantity);
    }

    private void PlayRandomParticles(int quantity)
    {
        if (particleSystems.Count == 0 || quantity <= 0) return;
        var selectedParticles = particleSystems.OrderBy(x => Random.value).Take(quantity);
        foreach (var ps in selectedParticles) ps.Play();
    }
}