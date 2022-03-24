using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public class WorldTile : IEquatable<WorldTile>
    {
        /// <summary>
        /// Gets or sets the province identifier.
        /// </summary>
        /// <value>The province identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ProvinceId { get; set; }

        /// <summary>
        /// Gets the terrain identifier.
        /// </summary>
        /// <value>The terrain identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string TerrainId { get; set; }

        public List<string> TerrainIds { get; set; }

        [Range(0, byte.MaxValue)]
        public byte Altitude { get; set; }

        public bool HasRiver { get; set; }

        public bool HasWater { get; set; }

        public WorldTile()
        {
            TerrainIds = new List<string>();
        }

        /// <summary>
        /// Determines whether the specified <see cref="WorldTile"/> is equal to the current <see cref="WorldTile"/>.
        /// </summary>
        /// <param name="other">The <see cref="WorldTile"/> to compare with the current <see cref="WorldTile"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="WorldTile"/> is equal to the current
        /// <see cref="WorldTile"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(WorldTile other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(ProvinceId, other.ProvinceId) &&
                   string.Equals(TerrainId, other.TerrainId);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="WorldTile"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="WorldTile"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="WorldTile"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((WorldTile)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="WorldTile"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((ProvinceId != null ? ProvinceId.GetHashCode() : 0) * 397) ^
                       (TerrainId != null ? TerrainId.GetHashCode() : 0);
            }
        }
    }
}
