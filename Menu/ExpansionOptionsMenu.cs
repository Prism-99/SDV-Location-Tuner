using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using LocationCustomization.MenuOptions;
using StardewValley.TokenizableStrings;
using SDV_Realty_Core;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using StardewValley;

namespace LocationCustomization.Menu
{
    internal class ExpansionOptionsMenu : IClickableMenu
    {
        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;
        private ClickableTextureComponent scrollBar;
        private OptionsButton okButton;
        private OptionsButton cancelButton;
        private OptionsButton clearButton;
        private Rectangle scrollBarRunner;
        private bool scrolling;
        public List<OptionsElement> options = new List<OptionsElement>();
        public List<ClickableComponent> optionSlots = new List<ClickableComponent>();
        public int currentItemIndex = 0;
        private int optionsSlotHeld = -1;
        private GameLocationCustomizations customizations;
        private const int numberOfSlots = 6;
        private Texture2D menuTexture;
        private string displayName;
        private Action onClose = null;
        private string hoverText = "";
        private Dictionary<int, string> optionHoverText = new();
        private Point clearButtonPos;
        private Point okButtonPos;
        private Point cancelButtonPos;
        private bool showAdvanced;
        public ExpansionOptionsMenu(int x, int y, int width, int height, bool showCloseButton, GameLocationCustomizations customizations, Action onClose, bool showAdvanced) : base(x, y, width, height, showCloseButton)
        {
            this.showAdvanced = showAdvanced;
            this.onClose = onClose;
            menuTexture = Game1.content.Load<Texture2D>("Maps\\MenuTiles");
            this.customizations = customizations;
            displayName = customizations.LocationName;
            if (Game1.locationData.TryGetValue(customizations.LocationName, out var locData))
            {
                displayName = TokenParser.ParseText(locData.DisplayName);
            }

            upArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + 16, yPositionOnScreen + 64, 44, 48), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), 4f);
            downArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + 16, yPositionOnScreen + height - 64, 44, 48), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), 4f);
            scrollBar = new ClickableTextureComponent(new Rectangle(upArrow.bounds.X + 12, upArrow.bounds.Y + upArrow.bounds.Height + 4, 24, 40), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), 4f);
            scrollBarRunner = new Rectangle(scrollBar.bounds.X, upArrow.bounds.Y + upArrow.bounds.Height + 4, scrollBar.bounds.Width, height - 128 - upArrow.bounds.Height - 8);

            okButton = new OptionsButton(I18n.LC_Button_Save(), SaveCustomizations);
            cancelButton = new OptionsButton(I18n.LC_Button_Cancel(), ExitMenu);
            clearButton = new OptionsButton("Clear", ClearCustomization);

            okButtonPos = new Point(xPositionOnScreen + width - 230, yPositionOnScreen + height - 64 - borderWidth);
            cancelButtonPos = new Point(xPositionOnScreen + width - 270 - okButton.bounds.Width, yPositionOnScreen + height - 64 - borderWidth);
            clearButtonPos = new Point(xPositionOnScreen + width - 280 - okButton.bounds.Width - clearButton.bounds.Width, yPositionOnScreen + height - 64 - borderWidth);

            int ccHeight = (height - 160) / (numberOfSlots + 1);
            int top = yPositionOnScreen + 84;// + 80 + 4;

            for (int i = 0; i < numberOfSlots; i++)
            {
                optionSlots.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 16, top + i * ccHeight + 8, width - 32, ccHeight + 4), i.ToString() ?? "")
                {
                    myID = i,
                    downNeighborID = ((i < 6) ? (i + 1) : (-7777)),
                    upNeighborID = ((i > 0) ? (i - 1) : (-7777)),
                    fullyImmutable = true
                });
            }

            //options.Add(new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11234"), 0));
            options.Add(new DropDownOption(I18n.LC_SeasonOverride(), GetOptionsList(1), customizations.SeasonOverride, 1, OptionPicked));
            optionHoverText.Add(0, I18n.LC_SeasonOverride_TT());
            options.Add(new DropDownOption(I18n.LC_GreenSpawn(), GetOptionsList(2), GetYesNoDefaultValue(customizations.CanHaveGreenRainSpawns), 2, OptionPicked));
            optionHoverText.Add(1, I18n.LC_GreenSpawn_TT());
            options.Add(new DropDownOption(I18n.LC_GiantCrops(), GetOptionsList(2), GetYesNoDefaultValue(customizations.AllowGiantCrops), 4, OptionPicked));
            optionHoverText.Add(3, I18n.LC_GiantCrops_TT());
            options.Add(new DropDownOption(I18n.LC_TreePlanting(), GetOptionsList(2), GetYesNoDefaultValue(customizations.AllowTreePlanting), 8, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_CanBuild(), GetOptionsList(2), GetYesNoDefaultValue(customizations.CanBuildHere), 17, OptionPicked));
            optionHoverText.Add(16, I18n.LC_CanBuild_TT());
            options.Add(new DropDownOption(I18n.LC_CanPlant(), GetOptionsList(2), GetYesNoDefaultValue(customizations.CanPlantHere), 3, OptionPicked));
            optionHoverText.Add(2, I18n.LC_CanPlant_TT());

            options.Add(new DropDownOption(I18n.LC_SeedSeason(), GetOptionsList(2), GetYesNoDefaultValue(customizations.SeedsIgnoreSeason), 30, OptionPicked));
            //optionHoverText.Add(29, I18n.LC_SeedSeason_TT());

            options.Add(new DropDownOption(I18n.LC_WinterGrassSurvive(), GetOptionsList(2), GetYesNoDefaultValue(customizations.AllowGrassSurviveInWinter), 5, OptionPicked));
            optionHoverText.Add(4, I18n.LC_WinterGrassSurvive_TT());
            options.Add(new DropDownOption(I18n.LC_GrassSpread(), GetOptionsList(2), GetYesNoDefaultValue(customizations.EnableGrassSpread), 6, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_GrassSpreadWinter(), GetOptionsList(2), GetYesNoDefaultValue(customizations.AllowGrassGrowInWinter), 7, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_SkipWeed(), GetOptionsList(2), GetYesNoDefaultValue(customizations.SkipWeedGrowth), 9, OptionPicked));

            options.Add(new DropDownOption(I18n.LC_No_Birds(), GetOptionsList(4), GetYesNoValue(customizations.NoBirdies), 19, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Butterflies(), GetOptionsList(4), GetYesNoValue(customizations.NoButterflies), 20, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Bunnies(), GetOptionsList(4), GetYesNoValue(customizations.NoBunnies), 21, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Crows(), GetOptionsList(4), GetYesNoValue(customizations.NoCrows), 26, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Frogs(), GetOptionsList(4), GetYesNoValue(customizations.NoFrogs), 27, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Opossums(), GetOptionsList(4), GetYesNoValue(customizations.NoOpossums), 25, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Owls(), GetOptionsList(4), GetYesNoValue(customizations.NoOwls), 24, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Squirrels(), GetOptionsList(4), GetYesNoValue(customizations.NoSquirrels), 22, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Woodpeckers(), GetOptionsList(4), GetYesNoValue(customizations.NoWoodpeckers), 23, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Clouds(), GetOptionsList(4), GetYesNoValue(customizations.NoClouds), 18, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Debris(), GetOptionsList(4), GetYesNoValue(customizations.NoDebris), 29, OptionPicked));
            options.Add(new DropDownOption(I18n.LC_No_Lightning(), GetOptionsList(4), GetYesNoValue(customizations.NoLightning), 28, OptionPicked));

            options.Add(new TextOption(I18n.LC_FirstDay(), 10, customizations.FirstDayWeedMultiplier?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(9, I18n.LC_FirstDay_TT());
            options.Add(new TextOption(I18n.LC_MinWeeds(), 11, customizations.MinDailyWeeds?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(10, I18n.LC_MinWeeds_TT());
            options.Add(new TextOption(I18n.LC_MaxWeeds(), 12, customizations.MaxDailyWeeds?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(11, I18n.LC_MaxWeeds_TT());

            options.Add(new TextOption(I18n.LC_MinForage(), 13, customizations.MinDailyForageSpawn?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(12, I18n.LC_MinForage_TT());
            options.Add(new TextOption(I18n.LC_MaxForage(), 14, customizations.MaxDailyForageSpawn?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(13, I18n.LC_MaxForage_TT());

            options.Add(new TextOption("Max Spawned Forage", 32, customizations.MaxSpawnedForageAtOnce?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(31, "");

            options.Add(new TextOption(I18n.LC_DirtDecay(), 15, customizations.DirtDecayChance?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(14, I18n.LC_DirtDecay_TT());
            options.Add(new TextOption(I18n.LC_Clay(), 16, customizations.ChanceForClay?.ToString() ?? "-1", OptionPicked));
            optionHoverText.Add(15, I18n.LC_Clay_TT());

            if (showAdvanced)
            {
                options.Add(new DropDownOption("Location Context", GetOptionsList(3), customizations.ContextId, 31, OptionPicked));
                optionHoverText.Add(30,"Location Context");
            }
        }
        private void ClearCustomization()
        {
            customizations.ClearCustomizations();
            ResetDisplayValues();
        }
        private void ResetDisplayValues()
        {
            foreach (var option in options)
            {
                switch (option)
                {
                    case DropDownOption dropDownOption:
                        if (dropDownOption.dropDownOptions.Count == 2)
                            dropDownOption.selectedOption = 1;
                        else
                            dropDownOption.selectedOption = 0;
                        break;
                    case TextOption textOption:
                        textOption.textBox.Text = "-1";
                        break;
                }
            }
        }
        private void ExitMenu()
        {
            if (this == Game1.activeClickableMenu)
            {
                if (onClose != null)
                    onClose();

                Game1.exitActiveMenu();
            }
        }
        private void SaveCustomizations()
        {
            customizations.SaveCustomizations();
            Game1.playSound("shwip");
            ExitMenu();
        }
        /// <summary>
        /// Process value changes 
        /// </summary>
        /// <param name="option">Which parameter is being set</param>
        /// <param name="value">Value from CC</param>
        private void OptionPicked(int option, string value)
        {
            switch (option)
            {
                case 1:
                    customizations.SeasonOverride = value;
                    break;
                case 2:
                    customizations.CanHaveGreenRainSpawns = GetYesNoDefaultValue(value);
                    break;
                case 3:
                    customizations.CanPlantHere = GetYesNoDefaultValue(value);
                    break;
                case 4:
                    customizations.AllowGiantCrops = GetYesNoDefaultValue(value);
                    break;
                case 5:
                    customizations.AllowGrassSurviveInWinter = GetYesNoDefaultValue(value);
                    break;
                case 6:
                    customizations.EnableGrassSpread = GetYesNoDefaultValue(value);
                    break;
                case 7:
                    customizations.AllowGrassGrowInWinter = GetYesNoDefaultValue(value);
                    break;
                case 8:
                    customizations.AllowTreePlanting = GetYesNoDefaultValue(value);
                    break;
                case 9:
                    customizations.SkipWeedGrowth = GetYesNoDefaultValue(value);
                    break;
                case 10:
                    customizations.FirstDayWeedMultiplier = GetIntValue(value);
                    break;
                case 11:
                    customizations.MinDailyWeeds = GetIntValue(value);
                    break;
                case 12:
                    customizations.MaxDailyWeeds = GetIntValue(value);
                    break;
                case 13:
                    customizations.MinDailyForageSpawn = GetIntValue(value);
                    break;
                case 14:
                    customizations.MaxDailyForageSpawn = GetIntValue(value);
                    break;
                case 15:
                    customizations.DirtDecayChance = GetFloatValue(value);
                    break;
                case 16:
                    customizations.ChanceForClay = GetFloatValue(value);
                    break;
                case 17:
                    customizations.CanBuildHere = GetYesNoDefaultValue(value);
                    break;
                case 18:
                    customizations.NoClouds = value == "yes";
                    break;
                case 19:
                    customizations.NoBirdies = value == "yes";
                    break;
                case 20:
                    customizations.NoButterflies = value == "yes";
                    break;
                case 21:
                    customizations.NoBunnies = value == "yes";
                    break;
                case 22:
                    customizations.NoSquirrels = value == "yes";
                    break;
                case 23:
                    customizations.NoWoodpeckers = value == "yes";
                    break;
                case 24:
                    customizations.NoOwls = value == "yes";
                    break;
                case 25:
                    customizations.NoOpossums = value == "yes";
                    break;
                case 26:
                    customizations.NoCrows = value == "yes";
                    break;
                case 27:
                    customizations.NoFrogs = value == "yes";
                    break;
                case 28:
                    customizations.NoLightning = value == "yes";
                    break;
                case 29:
                    customizations.NoDebris = value == "yes";
                    break;
                case 30:
                    customizations.SeedsIgnoreSeason = GetYesNoDefaultValue(value);
                    break;
                case 31:
                    customizations.ContextId = value;
                    break;
                case 32:
                    customizations.MaxSpawnedForageAtOnce = GetIntValue(value);
                    break;
            }
        }
        /// <summary>
        /// Contvert string into nullable float
        /// -1 = null
        /// </summary>
        /// <param name="value">String to be converted</param>
        /// <returns></returns>
        private float? GetFloatValue(string value)
        {
            if (value == null) return null;
            if (float.TryParse(value, out float floatValue))
            {
                if (floatValue == -1)
                    return null;
                else
                    return floatValue;
            }
            return null;
        }
        /// <summary>
        /// Convert string into nullable int
        /// -1 = null
        /// </summary>
        /// <param name="value">String to be converted</param>
        /// <returns></returns>
        private int? GetIntValue(string value)
        {
            if (value == null) return null;
            if (int.TryParse(value, out int intValue))
            {
                if (intValue == -1)
                    return null;
                else
                    return intValue;
            }
            return null;
        }
        private Dictionary<string, string> GetOptionsList(int optionId)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            switch (optionId)
            {
                case 1:
                    // Seasons
                    results.Add("none", I18n.LC_Season_None());
                    results.Add("Spring", I18n.LC_Season_Spring());
                    results.Add("Summer", I18n.LC_Season_Summer());
                    results.Add("Fall", I18n.LC_Season_Fall());
                    results.Add("Winter", I18n.LC_Season_Winter());
                    break;
                case 2:
                    MakeYesNoDefaultList(results);
                    break;
                case 3:
                    foreach (var context in Game1.locationContextData)
                    {
                        results.Add(context.Key, context.Key);
                    }
                    break;
                case 4:
                    results.Add("yes", I18n.LC_Drop_Yes());
                    results.Add("no", I18n.LC_Drop_No());
                    break;
            };

            return results;
        }
        /// <summary>
        /// Convert string to nullable boolean
        /// 'default' = null
        /// 'yes'= true
        /// 'no' = false
        /// </summary>
        /// <param name="value">String to be converted</param>
        /// <returns></returns>
        private bool? GetYesNoDefaultValue(string value)
        {
            switch (value)
            {
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }
        private string GetYesNoValue(bool value)
        {
            return value ? "yes" : "no";
        }
        private string GetYesNoDefaultValue(bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? "yes" : "no";
            }
            return "default";
        }
        private void MakeYesNoDefaultList(Dictionary<string, string> options)
        {
            options.Add("default", I18n.LC_Drop_Default());
            options.Add("yes", I18n.LC_Drop_Yes());
            options.Add("no", I18n.LC_Drop_No());
        }
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (downArrow.containsPoint(x, y) && currentItemIndex < Math.Max(0, options.Count - numberOfSlots))
            {
                optionsSlotHeld = -1;
                downArrowPressed();
                Game1.playSound("shwip");
            }
            else if (upArrow.containsPoint(x, y) && currentItemIndex > 0)
            {
                optionsSlotHeld = -1;
                upArrowPressed();
                Game1.playSound("shwip");
            }
            else if (scrollBar.containsPoint(x, y))
            {
                scrolling = true;
            }
            //, 
            else if (okButton.bounds.Contains(x - okButtonPos.X, y - okButtonPos.Y))
            {
                SaveActiveTextBox(-1);
                okButton.receiveLeftClick(x - okButtonPos.X, y - okButtonPos.Y);
            }
            else if (cancelButton.bounds.Contains(x - cancelButtonPos.X, y - cancelButtonPos.Y))
            {
                cancelButton.receiveLeftClick(x - cancelButtonPos.X, y - cancelButtonPos.Y);
            }
            else if (clearButton.bounds.Contains(x - clearButtonPos.X, y - clearButtonPos.Y))
            {
                clearButton.receiveLeftClick(x - clearButtonPos.X, y - clearButtonPos.Y);
            }
            else if (!downArrow.containsPoint(x, y) && x > xPositionOnScreen + width && x < xPositionOnScreen + width + 128 && y > yPositionOnScreen && y < yPositionOnScreen + height)
            {
                scrolling = true;
                leftClickHeld(x, y);
                releaseLeftClick(x, y);
            }

            currentItemIndex = Math.Max(0, Math.Min(options.Count - numberOfSlots, currentItemIndex));
            UnsubscribeFromSelectedTextbox();
            bool handled = false;
            for (int i = 0; i < optionSlots.Count; i++)
            {
                if (optionSlots[i].bounds.Contains(x, y) && currentItemIndex + i < options.Count && options[currentItemIndex + i].bounds.Contains(x - optionSlots[i].bounds.X, y - optionSlots[i].bounds.Y))
                {
                    SaveActiveTextBox(i);
                    options[currentItemIndex + i].receiveLeftClick(x - optionSlots[i].bounds.X, y - optionSlots[i].bounds.Y);
                    handled = true;
                    optionsSlotHeld = i;
                    break;
                }
            }
            if (!handled)
                base.receiveLeftClick(x, y, playSound);
        }
        private void SaveActiveTextBox(int selectedIndex)
        {
            if (optionsSlotHeld != -1 && optionsSlotHeld != selectedIndex && options[currentItemIndex + optionsSlotHeld] is TextOption textOption)
            {
                textOption.LostFocus();
            }
        }
        public override void snapCursorToCurrentSnappedComponent()
        {
            if (currentlySnappedComponent != null && currentlySnappedComponent.myID < options.Count)
            {
                OptionsElement optionsElement = options[currentlySnappedComponent.myID + currentItemIndex];
                if (!(optionsElement is OptionsDropDown dropdown))
                {
                    if (!(optionsElement is OptionsPlusMinusButton))
                    {
                        if (optionsElement is OptionsInputListener)
                        {
                            Game1.setMousePosition(currentlySnappedComponent.bounds.Right - 48, currentlySnappedComponent.bounds.Center.Y - 12);
                        }
                        else
                        {
                            Game1.setMousePosition(currentlySnappedComponent.bounds.Left + 48, currentlySnappedComponent.bounds.Center.Y - 12);
                        }
                    }
                    else
                    {
                        Game1.setMousePosition(currentlySnappedComponent.bounds.Left + 64, currentlySnappedComponent.bounds.Center.Y + 4);
                    }
                }
                else
                {
                    Game1.setMousePosition(currentlySnappedComponent.bounds.Left + dropdown.bounds.Right - 32, currentlySnappedComponent.bounds.Center.Y - 4);
                }
            }
            else if (currentlySnappedComponent != null)
            {
                base.snapCursorToCurrentSnappedComponent();
            }
        }
        public override void receiveKeyPress(Keys key)
        {
            if ((optionsSlotHeld != -1 && optionsSlotHeld + currentItemIndex < options.Count) || (Game1.options.snappyMenus && Game1.options.gamepadControls))
            {
                if (currentlySnappedComponent != null && Game1.options.snappyMenus && Game1.options.gamepadControls && options.Count > currentItemIndex + currentlySnappedComponent.myID && currentItemIndex + currentlySnappedComponent.myID >= 0)
                {
                    options[currentItemIndex + currentlySnappedComponent.myID].receiveKeyPress(key);
                }
                else if (options.Count > currentItemIndex + optionsSlotHeld && currentItemIndex + optionsSlotHeld >= 0)
                {
                    options[currentItemIndex + optionsSlotHeld].receiveKeyPress(key);
                }
            }
            base.receiveKeyPress(key);
        }
        public override void setCurrentlySnappedComponentTo(int id)
        {
            currentlySnappedComponent = getComponentWithID(id);
            snapCursorToCurrentSnappedComponent();
        }
        protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
        {
            base.customSnapBehavior(direction, oldRegion, oldID);
            if (oldID == 6 && direction == 2 && currentItemIndex < Math.Max(0, options.Count - 7))
            {
                downArrowPressed();
                Game1.playSound("shiny4");
            }
            else
            {
                if (oldID != 0 || direction != 0)
                {
                    return;
                }
                if (currentItemIndex > 0)
                {
                    upArrowPressed();
                    Game1.playSound("shiny4");
                    return;
                }
                currentlySnappedComponent = getComponentWithID(12348);
                if (currentlySnappedComponent != null)
                {
                    currentlySnappedComponent.downNeighborID = 0;
                }
                snapCursorToCurrentSnappedComponent();
            }
        }

        public override void applyMovementKey(int direction)
        {
            if (!IsDropdownActive())
            {
                base.applyMovementKey(direction);
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (!GameMenu.forcePreventClose && !IsDropdownActive())
            {
                base.receiveScrollWheelAction(direction);
                if (direction > 0 && currentItemIndex > 0)
                {
                    upArrowPressed();
                    Game1.playSound("shiny4");
                }
                else if (direction < 0 && currentItemIndex < Math.Max(0, options.Count - numberOfSlots))
                {
                    downArrowPressed();
                    Game1.playSound("shiny4");
                }
                if (Game1.options.SnappyMenus)
                {
                    snapCursorToCurrentSnappedComponent();
                }
            }
        }
        public override void releaseLeftClick(int x, int y)
        {
            if (!GameMenu.forcePreventClose)
            {
                base.releaseLeftClick(x, y);
                if (optionsSlotHeld != -1 && optionsSlotHeld + currentItemIndex < options.Count)
                {
                    options[currentItemIndex + optionsSlotHeld].leftClickReleased(x - optionSlots[optionsSlotHeld].bounds.X, y - optionSlots[optionsSlotHeld].bounds.Y);
                }
                //optionsSlotHeld = -1;
                scrolling = false;
            }
        }
        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
            {
                return;
            }
            base.leftClickHeld(x, y);
            if (scrolling)
            {
                int y2 = scrollBar.bounds.Y;
                scrollBar.bounds.Y = Math.Min(yPositionOnScreen + height - 64 - 12 - scrollBar.bounds.Height, Math.Max(y, yPositionOnScreen + upArrow.bounds.Height + 20));
                float percentage = (float)(y - scrollBarRunner.Y) / (float)scrollBarRunner.Height;
                currentItemIndex = Math.Min(options.Count - numberOfSlots, Math.Max(0, (int)((float)options.Count * percentage)));
                setScrollBarToCurrentIndex();
                if (y2 != scrollBar.bounds.Y)
                {
                    Game1.playSound("shiny4");
                }
            }
            else if (optionsSlotHeld != -1 && optionsSlotHeld + currentItemIndex < options.Count)
            {
                options[currentItemIndex + optionsSlotHeld].leftClickHeld(x - optionSlots[optionsSlotHeld].bounds.X, y - optionSlots[optionsSlotHeld].bounds.Y);
            }
        }
        private void downArrowPressed()
        {
            if (!IsDropdownActive())
            {
                UnsubscribeFromSelectedTextbox();
                downArrow.scale = downArrow.baseScale;
                currentItemIndex++;
                setScrollBarToCurrentIndex();
            }
        }
        private void upArrowPressed()
        {
            if (!IsDropdownActive())
            {
                UnsubscribeFromSelectedTextbox();
                upArrow.scale = upArrow.baseScale;
                currentItemIndex--;
                setScrollBarToCurrentIndex();
            }
        }
        public bool IsDropdownActive()
        {
            if (optionsSlotHeld != -1 && optionsSlotHeld + currentItemIndex < options.Count && options[currentItemIndex + optionsSlotHeld] is DropDownOption)
            {
                return true;
            }
            return false;
        }
        private void setScrollBarToCurrentIndex()
        {
            if (options.Count > 0)
            {
                scrollBar.bounds.Y = scrollBarRunner.Height / Math.Max(1, options.Count - numberOfSlots) * currentItemIndex + upArrow.bounds.Bottom + 4;
                if (scrollBar.bounds.Y > downArrow.bounds.Y - scrollBar.bounds.Height - 4)
                {
                    scrollBar.bounds.Y = downArrow.bounds.Y - scrollBar.bounds.Height - 4;
                }
            }
        }
        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, speaker: false, drawOnlyBox: true);
            Rectangle sourceRect = new Rectangle(64, 128, 64, 64);
            int destSize = 40;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + destSize - 10, destSize - 10, width - 10 - destSize, destSize + 10), sourceRect, Color.White);
            sourceRect.Y = 0;
            sourceRect.X = 0;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + 10, 10, destSize, destSize), sourceRect, Color.White);
            sourceRect.X = 192;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + width - destSize, 10, destSize, destSize), sourceRect, Color.White);
            sourceRect.Y = 192;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + width - destSize, 10 + destSize + 10, destSize, destSize), sourceRect, Color.White);
            sourceRect.X = 0;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + 10, 10 + destSize + 10, destSize, destSize), sourceRect, Color.White);
            sourceRect.X = 128;
            sourceRect.Y = 0;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + 10 + destSize, 10, width - destSize * 2 - 10, destSize), sourceRect, Color.White);
            sourceRect.Y = 192;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + 10 + destSize, 20 + destSize, width - destSize * 2 - 10, destSize), sourceRect, Color.White);
            sourceRect.Y = 128;
            sourceRect.X = 0;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + 10, destSize, destSize, destSize), sourceRect, Color.White);
            sourceRect.X = 192;
            b.Draw(menuTexture, new Rectangle(xPositionOnScreen + width - destSize, destSize, destSize, destSize), sourceRect, Color.White);

            int textcentre = (int)(Game1.dialogueFont.MeasureString(displayName).X / 2.0);
            b.DrawString(Game1.dialogueFont, displayName, new Vector2(xPositionOnScreen + width / 2 - textcentre, yPositionOnScreen + 15), Color.Black);

            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            okButton.draw(b, okButtonPos.X, okButtonPos.Y);
            cancelButton.draw(b, cancelButtonPos.X, cancelButtonPos.Y);
            clearButton.draw(b, clearButtonPos.X, clearButtonPos.Y);

            for (int i = 0; i < optionSlots.Count; i++)
            {
                if (currentItemIndex >= 0 && currentItemIndex + i < options.Count)
                {
                    options[currentItemIndex + i].draw(b, optionSlots[i].bounds.X, optionSlots[i].bounds.Y, this);
                }
            }
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            if (!GameMenu.forcePreventClose)
            {
                upArrow.draw(b);
                downArrow.draw(b);
                if (options.Count > numberOfSlots)
                {
                    drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), scrollBarRunner.X, scrollBarRunner.Y, scrollBarRunner.Width, scrollBarRunner.Height, Color.White, 4f, drawShadow: false);
                    scrollBar.draw(b);
                }
            }

            base.draw(b);
            drawMouse(b, ignore_transparency: true);
            if (!hoverText.Equals(""))
            {
                drawHoverText(b, hoverText, Game1.smallFont);
            }
        }
        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            hoverText = "";
            int offset = 350;
            for (int i = 0; i < optionSlots.Count; i++)
            {
                if (optionSlots[i].bounds.Contains(x - offset, y) && currentItemIndex + i < options.Count && options[currentItemIndex + i].bounds.Contains(x - offset - optionSlots[i].bounds.X, y - optionSlots[i].bounds.Y))
                {
                    if (optionHoverText.TryGetValue(options[currentItemIndex + i].whichOption - 1, out string text))
                        hoverText = text;

                    break;
                }
            }
        }
        public virtual void UnsubscribeFromSelectedTextbox()
        {
            if (Game1.keyboardDispatcher.Subscriber == null)
            {
                return;
            }
            foreach (OptionsElement option in options)
            {
                if (option is TextOption entry && Game1.keyboardDispatcher.Subscriber == entry.textBox)
                {
                    Game1.keyboardDispatcher.Subscriber = null;
                    break;
                }
            }
        }
    }
}
