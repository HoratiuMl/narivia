﻿using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Interface.Widgets
{
    public class InfoBar : Widget
    {
        Image background;

        Image regionsIcon, regionsText;
        Image holdingsIcon, holdingsText;
        Image wealthIcon, wealthText;
        Image troopsIcon, troopsText;

        [XmlIgnore]
        public int Regions { get; set; }

        [XmlIgnore]
        public int Holdings { get; set; }

        [XmlIgnore]
        public int Wealth { get; set; }

        [XmlIgnore]
        public int Troops { get; set; }

        public Color BackgroundColour { get; set; }

        public Color TextColour { get; set; }

        public float Spacing { get; set; }

        public InfoBar()
        {
            BackgroundColour = Color.Black;
            TextColour = Color.Gold;

            Spacing = 6.0f;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Position = Position,
                Scale = Size,
                Tint = BackgroundColour
            };

            regionsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            };
            holdingsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(48, 0, 16, 16)
            };
            wealthIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            troopsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(32, 0, 16, 16)
            };

            regionsText = new Image
            {
                Text = Regions.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            holdingsText = new Image
            {
                Text = Holdings.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            wealthText = new Image
            {
                Text = Wealth.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            troopsText = new Image
            {
                Text = Troops.ToString(),
                FontName = "InfoBarFont",
                Tint = TextColour
            };

            background.LoadContent();

            regionsIcon.LoadContent();
            holdingsIcon.LoadContent();
            wealthIcon.LoadContent();
            troopsIcon.LoadContent();

            regionsText.LoadContent();
            holdingsText.LoadContent();
            wealthText.LoadContent();
            troopsText.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();

            regionsIcon.UnloadContent();
            holdingsIcon.UnloadContent();
            wealthIcon.UnloadContent();
            troopsIcon.UnloadContent();

            regionsText.UnloadContent();
            holdingsText.UnloadContent();
            wealthText.UnloadContent();
            troopsText.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            regionsText.Text = Regions.ToString();
            holdingsText.Text = Holdings.ToString();
            wealthText.Text = Wealth.ToString();
            troopsText.Text = Troops.ToString();

            regionsIcon.Position = new Vector2(Position.X + Spacing, Position.Y + (Size.Y - regionsIcon.ScreenArea.Height) / 2);
            regionsText.Position = new Vector2(regionsIcon.ScreenArea.Right + Spacing, Position.Y + (Size.Y - regionsText.ScreenArea.Height) / 2);
            holdingsIcon.Position = new Vector2(regionsText.ScreenArea.Right + Spacing, regionsIcon.Position.Y);
            holdingsText.Position = new Vector2(holdingsIcon.ScreenArea.Right + Spacing, regionsText.Position.Y);
            wealthIcon.Position = new Vector2(holdingsText.ScreenArea.Right + Spacing, holdingsIcon.Position.Y);
            wealthText.Position = new Vector2(wealthIcon.ScreenArea.Right + Spacing, holdingsText.Position.Y);
            troopsIcon.Position = new Vector2(wealthText.ScreenArea.Right + Spacing, wealthIcon.Position.Y);
            troopsText.Position = new Vector2(troopsIcon.ScreenArea.Right + Spacing, wealthText.Position.Y);

            background.Update(gameTime);

            regionsIcon.Update(gameTime);
            holdingsIcon.Update(gameTime);
            wealthIcon.Update(gameTime);
            troopsIcon.Update(gameTime);

            regionsText.Update(gameTime);
            holdingsText.Update(gameTime);
            wealthText.Update(gameTime);
            troopsText.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);

            regionsIcon.Draw(spriteBatch);
            holdingsIcon.Draw(spriteBatch);
            wealthIcon.Draw(spriteBatch);
            troopsIcon.Draw(spriteBatch);

            regionsText.Draw(spriteBatch);
            holdingsText.Draw(spriteBatch);
            wealthText.Draw(spriteBatch);
            troopsText.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
