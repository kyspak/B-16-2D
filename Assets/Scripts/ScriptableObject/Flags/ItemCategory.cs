using System;

[Flags]
    public enum ItemCategory
    {
        None       = 0,
        Weapon     = 1 << 0,
        Armor      = 1 << 1,
        Consumable = 1 << 2,
        Buildable  = 1 << 3,
        Tool       = 1 << 4
    }
