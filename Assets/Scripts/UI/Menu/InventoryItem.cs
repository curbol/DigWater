using UnityEngine;

public class InventoryItem
{
    private const string SpritePath = "Sprites/UI/Store/Items/";

    public int Id { get; set; }
    public int Group { get; set; }
    public int Value { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }

    public Sprite Sprite
    {
        get
        {
            return Resources.Load<Sprite>(SpritePath + Slug);
        }
    }
}