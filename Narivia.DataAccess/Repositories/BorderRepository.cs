﻿using System;
using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class BorderRepository : IBorderRepository
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<Tuple<string, string>, BorderEntity> borderEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.BorderRepository"/> class.
        /// </summary>
        public BorderRepository()
        {
            borderEntitiesStore = new Dictionary<Tuple<string, string>, BorderEntity>();
        }

        /// <summary>
        /// Adds the specified border.
        /// </summary>
        /// <param name="border">Border.</param>
        public void Add(BorderEntity border)
        {
            Tuple<string, string> key = new Tuple<string, string>(border.Region1Id, border.Region2Id);
            borderEntitiesStore.Add(key, border);
        }

        /// <summary>
        /// Gets the border with the specified faction and unit identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public BorderEntity Get(string region1Id, string region2Id)
        {
            Tuple<string, string> key = new Tuple<string, string>(region1Id, region2Id);
            return borderEntitiesStore[key];
        }

        /// <summary>
        /// Gets all the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        public IEnumerable<BorderEntity> GetAll()
        {
            return borderEntitiesStore.Values;
        }

        /// <summary>
        /// Removes the border with the specified faction and unit identifiers.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public void Remove(string region1Id, string region2Id)
        {
            Tuple<string, string> key = new Tuple<string, string>(region1Id, region2Id);
            borderEntitiesStore.Remove(key);
        }
    }
}
