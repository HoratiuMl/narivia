﻿using System;
using System.Xml.Serialization;
using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.UI.Graphics;

namespace Narivia.UI.Graphics.ImageEffects
{
    /// <summary>
    /// Effect.
    /// </summary>
    public class Effect
    {
        protected Image Image;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Narivia.UI.Graphics.ImageEffects.Effect"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [XmlIgnore]
        public string Key { get { return Type.ToString().Split('.').Last(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.UI.Graphics.ImageEffects.Effect"/> class.
        /// </summary>
        public Effect()
        {
            Active = false;
            Type = GetType();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="image">Image.</param>
        public virtual void LoadContent(ref Image image)
        {
            Image = image;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}

