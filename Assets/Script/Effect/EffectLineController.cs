using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLineController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    [SerializeField]
    private List<ParticleSystem> particlesNeedChangeColor;

    [System.Obsolete]
    public void PlayEffect(Color color)
    {
        particleSystem.Stop();
        foreach(ParticleSystem particle in particlesNeedChangeColor)
        {
            var col = particle.colorOverLifetime;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

            col.color = grad;
        }
        particleSystem.Play();
    }
}
