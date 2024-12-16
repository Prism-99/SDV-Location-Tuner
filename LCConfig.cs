using StardewModdingAPI.Utilities;
using StardewModdingAPI;

namespace LocationCustomization
{ 
    public class LCConfig
    {
       
        public bool AdvancedEnabled { get; set; } = true;
        public KeybindList Keybinding {  get; set; } = new KeybindList(new Keybind(SButton.F2, SButton.LeftControl));
    }
}
