using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Registry for sprites, allowing easy retrieval based on GhostType.
/// </summary>
[CreateAssetMenu(fileName = "GhostTypeSpriteRegistry", menuName = "Scriptable Objects/GhostTypeSpriteRegistry")]
public class GhostTypeSpriteRegistry : ScriptableObject
{
    [System.Serializable]
    public class GhostTypeSpriteMapping
    {
        public GhostType type;
        public Sprite sprite;
        public Color color = Color.white; // Optional: default white, can be used for tinting or other effects
    }

    public List<GhostTypeSpriteMapping> mappings;

    // Caches for quick access
    private Dictionary<GhostType, Sprite> _spriteLookup;
    private Dictionary<GhostType, Color> _colorLookup;

    void InitializeLookup()
    {
        if (_spriteLookup == null || _colorLookup == null)
        {
            _spriteLookup = new Dictionary<GhostType, Sprite>();
            _colorLookup = new Dictionary<GhostType, Color>();

            foreach (var mapping in mappings)
            {
                _spriteLookup[mapping.type] = mapping.sprite;
                _colorLookup[mapping.type] = mapping.color;
            }
        }
    }

    public Sprite GetSprite(GhostType type)
    {
        InitializeLookup();

        return _spriteLookup.TryGetValue(type, out var sprite) ? sprite : null;
    }

    public Color GetColor(GhostType type)
    {
        InitializeLookup();

        return _colorLookup.TryGetValue(type, out var color) ? color : Color.white;
    }
}


