﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Flag mapping extensions for converting between entities and domain models.
    /// </summary>
    static class FlagMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="flagEntity">Flag entity.</param>
        internal static Flag ToDomainModel(this FlagEntity flagEntity)
        {
            Flag flag = new Flag
            {
                Id = flagEntity.Id,
                Background = flagEntity.Background,
                Emblem = flagEntity.Emblem,
                Skin = flagEntity.Skin,
                BackgroundPrimaryColour = ColorTranslator.FromHtml(flagEntity.BackgroundPrimaryColourHexadecimal),
                BackgroundSecondaryColour = ColorTranslator.FromHtml(flagEntity.BackgroundSecondaryColourHexadecimal),
                EmblemColour = ColorTranslator.FromHtml(flagEntity.EmblemColourHexadecimal)
            };

            return flag;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="flag">Flag.</param>
        internal static FlagEntity ToEntity(this Flag flag)
        {
            FlagEntity flagEntity = new FlagEntity
            {
                Id = flag.Id,
                Background = flag.Background,
                Emblem = flag.Emblem,
                Skin = flag.Skin,
                BackgroundPrimaryColourHexadecimal = ColorTranslator.ToHtml(flag.BackgroundPrimaryColour),
                BackgroundSecondaryColourHexadecimal = ColorTranslator.ToHtml(flag.BackgroundSecondaryColour),
                EmblemColourHexadecimal = ColorTranslator.ToHtml(flag.EmblemColour)
            };

            return flagEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="flagEntities">Flag entities.</param>
        internal static IEnumerable<Flag> ToDomainModels(this IEnumerable<FlagEntity> flagEntities)
        {
            IEnumerable<Flag> flags = flagEntities.Select(flagEntity => flagEntity.ToDomainModel());

            return flags;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="flags">Flags.</param>
        internal static IEnumerable<FlagEntity> ToEntities(this IEnumerable<Flag> flags)
        {
            IEnumerable<FlagEntity> flagEntities = flags.Select(flag => flag.ToEntity());

            return flagEntities;
        }
    }
}
