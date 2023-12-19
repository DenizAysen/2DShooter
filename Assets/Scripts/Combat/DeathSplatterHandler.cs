using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UnSubscribeEvents()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }
    private void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, transform.rotation);
        SpriteRenderer deathSplatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if (colorChanger)
        {
        Color splatterColor = colorChanger.DefaultColor;
        deathSplatterSpriteRenderer.color = splatterColor;
        }
        newSplatterPrefab.transform.SetParent(this.transform);
    }
    private void SpawnDeathVFX(Health sender)
    {
        GameObject _deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, transform.rotation);
        ParticleSystem.MainModule ps = _deathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if (colorChanger)
        {
        Color currentColor = colorChanger.DefaultColor;
        ps.startColor = currentColor;
        }
        _deathVFX.transform.SetParent(this.transform);
    }
}
