using System;

namespace Inventory.Items.Flags
{
    [Flags]
    public enum ToolCapability
    {
        None   = 0,
        Hand   = 1 << 1,
        Till   = 1 << 2,
        Loosen = 1 << 3,
        Water  = 1 << 4,
        Chop   = 1 << 5
    }
}