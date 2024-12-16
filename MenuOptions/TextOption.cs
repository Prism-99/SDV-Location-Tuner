using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley;

namespace LocationCustomization.MenuOptions
{
    internal class TextOption : OptionsElement
    {
        public CustomTextBox textBox;
        private Action<int, string> picked = null;
        private int option;
        private bool selected = false;
        public TextOption(string label, int whichOption,string currentText, Action<int, string> optionPicked, int x = -1, int y = -1) : base(label, x, y, (int)Game1.smallFont.MeasureString("Windowed Borderless Mode   ").X + 48, 44, whichOption)
        {
            option = whichOption;
            picked = optionPicked;
            textBox = new CustomTextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.dialogueFont, Game1.textColor);
            //textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
            //textBox.X = x;
            //textBox.Y = y;
            textBox.Width = (int)Game1.smallFont.MeasureString("Windowed Borderless Mode   ").X;
            textBox.Height = 44;
            textBox.OnEnterPressed += textBoxEnter;
            textBox.Text = currentText;
        }
        public void LostFocus()
        {
            if (selected)
            {
                textBoxEnter(textBox);
                textBox.Selected = false;
                selected = false;
            }
        }
        public void textBoxEnter(TextBox sender)
        {
            picked(option, sender.Text);
        }
        public override void draw(SpriteBatch b, int slotX, int slotY, IClickableMenu context = null)
        {
            textBox.X = slotX + bounds.Left - 8;
            textBox.Y = slotY + bounds.Top;
            textBox.Draw(b,false);
            base.draw(b, slotX, slotY, context);
        }
        public override void receiveLeftClick(int x, int y)
        {
            textBox.SelectMe();
            textBox.Update();
            selected = textBox.Selected;
            //base.receiveLeftClick(x, y);
        }
    }
}
