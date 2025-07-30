using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public GhostTypeSpriteRegistry spriteRegistry;
    public Image spriteReference;

    [Header("Soul References")]
    public TMP_Text soulCounterText;
    public Slider soulRegenBar;

    // unique per portal. assumes number of portals won't go past int.MAX_VALUE
    public int id { get; private set; }
    public GhostType type { get; private set; }

    private SoulManager _soulManager;
    private PortalManager _portalManager;

    public void Setup(PortalManager portalManager, int id, GhostType type)
    {
        this.id = id;
        this.type = type;

        // set sprite based on type
        Sprite sprite = spriteRegistry.GetSprite(type);
        if (sprite == null)
        {
            Debug.LogWarning($"No sprite found for ghost type {type}");
        }
        else
        {
            spriteReference.sprite = sprite;
            spriteReference.color = spriteRegistry.GetColor(type);

            GameObject slider = soulRegenBar.fillRect.gameObject;
            slider.GetComponent<Image>().color = spriteRegistry.GetColor(type);
        }

        _soulManager = GameManager.Instance.soulManager;
        _portalManager = portalManager;
    }

    public void UpdateSoulUI()
    {
        if (_soulManager == null)
        {
            Debug.LogWarning("SoulManager is not set. Cannot update soul UI.");
            return;
        }

        // Update soul counter text
        soulCounterText.text = _soulManager.currentSoulCount.ToString();

        // Update soul regeneration bar
        if (_soulManager.maxSoulCount > 0)
        {
            float regenProgress = (float)_soulManager.currentRegenTime / (float)_soulManager.regenerationDuration;
            soulRegenBar.value = regenProgress;
        }
        else
        {
            soulRegenBar.value = 0f; // No souls to show
        }
    }

    // Event Trigger on BasePortal button
    public void OnClick()
    {
        if (_soulManager.CanConsumeSoul())
        {
            _soulManager.ConsumeSoul();
            UpdateSoulUI();

            // Notify portal manager to spawn a ghost at this portal
            _portalManager.SpawnGhostCallback(type);
        }
        else
        {
            Debug.LogWarning("No souls available to consume.");
        }
    }


}

