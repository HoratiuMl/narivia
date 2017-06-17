﻿using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Menu item widget that cycles through the values of a list of strings
    /// </summary>
    public class MenuListSelector : MenuItem
    {
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public List<string> Values { get; set; }

        /// <summary>
        /// Gets the index of the selected value.
        /// </summary>
        /// <value>The index of the selected value.</value>
        [XmlIgnore]
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        [XmlIgnore]
        public string SelectedValue
        {
            get
            {
                if (Values.Count > 0 && Values.Count > SelectedIndex)
                {
                    return Values[SelectedIndex];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText
        {
            get
            {
                string displayText = Text;

                if (!string.IsNullOrEmpty(SelectedValue))
                {
                    displayText += $" : {SelectedValue}";
                }

                return displayText;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuListSelector"/> class.
        /// </summary>
        public MenuListSelector()
        {
            Values = new List<string>();
            SelectedIndex = -1;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (Values.Count > 0)
            {
                SelectedIndex = 0;
            }

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.KeyboardKeyPressed += InputManager_OnKeyboardKeyPressed;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.KeyboardKeyPressed -= InputManager_OnKeyboardKeyPressed;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Values.Count == 0)
            {
                return;
            }

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            if (SelectedIndex > Values.Count - 1)
            {
                SelectedIndex = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Values.Count - 1;
            }

            TextImage.Text = DisplayText;
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!ScreenArea.Contains(e.MousePosition))
            {
                return;
            }

            if (e.Button == MouseButton.LeftButton)
            {
                SelectedIndex += 1;
            }
            else if (e.Button == MouseButton.RightButton)
            {
                SelectedIndex -= 1;
            }
        }

        /// <summary>
        /// Fires when a keyboard key was pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void InputManager_OnKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Right || e.Key == Keys.D)
            {
                SelectedIndex += 1;
            }
            else if (e.Key == Keys.Left || e.Key == Keys.A)
            {
                SelectedIndex -= 1;
            }
        }
    }
}
