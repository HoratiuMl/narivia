﻿using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.Interface.Widgets
{
    public class Widget
    {
        /// <summary>
        /// Gets the position of this widget.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the size of this widget.
        /// </summary>
        /// <value>The size.</value>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Gets the screen area covered by this widget.
        /// </summary>
        /// <value>The screen area.</value>
        public Rectangle ScreenArea => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.Widget"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.Widget"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.Widget"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Destroyed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Widgets.Widget"/> class.
        /// </summary>
        public Widget()
        {
            Enabled = true;
            Visible = true;
            Opacity = 1.0f;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }
        }

        /// <summary>
        /// Destroys this widget.
        /// </summary>
        public virtual void Destroy()
        {
            UnloadContent();

            Destroyed = true;
        }
    }
}
