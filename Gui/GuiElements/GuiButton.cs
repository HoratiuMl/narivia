﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.Controls.Enumerations;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiButton : GuiControl, IGuiControl
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public string ContentFile { get; set; }

        GuiImage image;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiButton"/> class.
        /// </summary>
        public GuiButton()
        {
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            image = new GuiImage
            {
                Id = $"{Id}_{nameof(image)}",
                ContentFile = ContentFile
            };
            text = new GuiText()
            {
                Id = $"{Id}_{nameof(text)}"
            };
            
            RegisterChildren(image, text);
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void RegisterEvents()
        {
            Clicked += OnClicked;
            MouseEntered += OnMouseEntered;
        }

        void UnregisterEvents()
        {
            Clicked -= OnClicked;
            MouseEntered -= OnMouseEntered;
        }

        void SetChildrenProperties()
        {
            if (!IsHovered)
            {
                image.SourceRectangle = new Rectangle2D(0, 0, Size.Width, Size.Height);
            }
            else
            {
                image.SourceRectangle = new Rectangle2D(Size.Width, 0, Size.Width, Size.Height);
            }

            text.Text = Text;
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != MouseButton.Left)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
