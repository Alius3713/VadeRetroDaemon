using _Project.Scripts.Core.Data.Tools;

namespace _Project.Scripts.UI.Preparation
{
    public class ToolDragPayload
    {
        public static ToolDefinition DraggedTool { get; private set; }
        public static int SourceSlotIndex { get; private set; }
        public static bool WasDroppedOnSlot { get; private set; }

        public static void Set(ToolDefinition tool, int sourceSlotIndex)
        {
            DraggedTool = tool;
            SourceSlotIndex = sourceSlotIndex;
            WasDroppedOnSlot = false;
        }

        public static void MarkDroppedOnSlot()
        {
            WasDroppedOnSlot = true;
        }
        
        public static void Clear()
        {
            DraggedTool = null;
            SourceSlotIndex = -1;
            WasDroppedOnSlot = false;
        }
    }
}
