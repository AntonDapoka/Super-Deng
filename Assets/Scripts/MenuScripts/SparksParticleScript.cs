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
        foreach (Transform child in sparksHolder.transform)
        {
            if (child.TryGetComponent<Spark>(out var spark))
            {
                if (spark.TryGetComponent<ParticleSystem>(out var ps))
                {
                    particleSystems.Add(ps);
                }
            }
        }



        StartRandomParticles();
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

        // Перемешиваем список и берем нужное количество элементов
        var selectedParticles = particleSystems.OrderBy(x => Random.value).Take(quantity);

        foreach (var ps in selectedParticles)
        {
            ps.Play();
        }
    }


}