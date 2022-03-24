﻿using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Terrain data emtity.
    /// </summary>
    public class TerrainEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public string Spritesheet { get; set; }

        /// <summary>
        /// Gets or sets the colour in hexadecimal.
        /// </summary>
        /// <value>The colour's hexadecimal value.</value>
        public string ColourHexadecimal { get; set; }

        public int ZIndex { get; set; }
    }
}
