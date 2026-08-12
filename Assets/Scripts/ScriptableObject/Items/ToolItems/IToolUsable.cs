using Inventory.Items.Flags;

namespace Inventory.Items.ToolItem
{
    public interface IToolUsable
    {
        ToolCapability Capability { get; }
        int Volume { get; }
    }
}