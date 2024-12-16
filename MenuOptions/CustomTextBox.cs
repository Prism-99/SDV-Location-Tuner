using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using StardewValley;


namespace LocationCustomization.MenuOptions
{
    internal class CustomTextBox:TextBox
    {
        public CustomTextBox(Texture2D textBoxTexture, Texture2D caretTexture, SpriteFont font, Color textColor):base(textBoxTexture,caretTexture,font,textColor)
        {

        }
        public override void Draw(SpriteBatch spriteBatch, bool drawShadow = true)
        {
            bool caretVisible = Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1000.0 >= 500.0;
            string toDraw = Text;
            if (PasswordBox)
            {
                toDraw = "";
                for (int i = 0; i < Text.Length; i++)
                {
                    toDraw += "•";
                }
            }
            if (_textBoxTexture != null)
            {
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X, Y, 16, Height), new Rectangle(0, 0, 16, Height), Color.White);
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X + 16, Y, Width - 32, Height), new Rectangle(16, 0, 4, Height), Color.White);
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X + Width - 16, Y, 16, Height), new Rectangle(_textBoxTexture.Bounds.Width - 16, 0, 16, Height), Color.White);
            }
            else
            {
                Game1.drawDialogueBox(X - 32, Y - 112 + 10, Width + 80, Height, speaker: false, drawOnlyBox: true);
            }
            Vector2 size = _font.MeasureString(toDraw);
            while (size.X > (float)Width)
            {
                toDraw = toDraw.Substring(1);
                size = _font.MeasureString(toDraw);
            }
            if (caretVisible && Selected)
            {
                spriteBatch.Draw(Game1.staminaRect, new Rectangle(X + 16 + (int)size.X + 2, Y + 8, 4, 32),new Rectangle(0,0,4,32), _textColor,0f,Vector2.Zero,SpriteEffects.None,0.99f);
            }
            if (drawShadow)
            {
                Utility.drawTextWithShadow(spriteBatch, toDraw, _font, new Vector2(X + 16, Y + ((_textBoxTexture != null) ? 12 : 8)), _textColor);
            }
            else
            {
                spriteBatch.DrawString(_font, toDraw, new Vector2(X + 16, Y + ((_textBoxTexture != null) ? 2 : 8)), _textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }
        }
    }
}
