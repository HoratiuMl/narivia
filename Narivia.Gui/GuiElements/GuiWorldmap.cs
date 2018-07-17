using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.Common.Extensions;
using Narivia.GameLogic.GameManagers;
using Narivia.Gui.SpriteEffects;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiWorldmap : GuiElement
    {
        /// <summary>
        /// Gets the selected province identifier.
        /// </summary>
        /// <value>The selected province identifier.</value>
        public string SelectedProvinceId { get; private set; }

        IGameManager gameManager;
        IWorldManager worldManager;

        Camera camera;
        World world;

        TextureSprite provinceHighlight;
        TextureSprite factionBorder;
        TextureSprite provinceBorder;

        Dictionary<string, Terrain> terrains;
        Dictionary<string, TextureSprite> terrainSprites;

        TerrainSpriteSheetEffect terrainEffect;
        ProvinceBorderEffect provinceBorderEffect;
        FactionBorderEffect factionBorderEffect;

        Point2D mouseCoords;

        public GuiWorldmap(
            IGameManager gameManager,
            IWorldManager worldManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            world = gameManager.GetWorld();

            terrains = worldManager.GetTerrains().ToDictionary(x => x.Id, x => x);
            terrainSprites = new Dictionary<string, TextureSprite>();

            terrainEffect = new TerrainSpriteSheetEffect(worldManager);
            provinceBorderEffect = new ProvinceBorderEffect(worldManager);
            factionBorderEffect = new FactionBorderEffect(worldManager);

            foreach (Terrain terrain in terrains.Values)
            {
                TextureSprite terrainSprite = new TextureSprite
                {
                    ContentFile = $"World/Terrain/{terrain.Spritesheet}",
                    SourceRectangle = new Rectangle2D(
                        GameDefines.MapTileSize, GameDefines.MapTileSize * 3,
                        GameDefines.MapTileSize, GameDefines.MapTileSize),
                    SpriteSheetEffect = terrainEffect,
                    Active = true
                };

                terrainSprite.LoadContent();
                terrainSprites.AddOrUpdate(terrain.Spritesheet, terrainSprite);
            }

            provinceHighlight = new TextureSprite
            {
                ContentFile = "Interface/Worldmap/highlight",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.White
            };

            provinceBorder = new TextureSprite
            {
                ContentFile = "Interface/Worldmap/province-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                Tint = Colour.Black,
                Opacity = 0.25f,
                SpriteSheetEffect = provinceBorderEffect,
                Active = true
            };
            factionBorder = new TextureSprite
            {
                ContentFile = "Interface/Worldmap/faction-border",
                SourceRectangle = new Rectangle2D(Point2D.Empty, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = factionBorderEffect,
                Active = true
            };

            camera.LoadContent();

            provinceHighlight.LoadContent();
            factionBorder.LoadContent();
            provinceBorder.LoadContent();

            terrainEffect.Activate();
            provinceBorderEffect.Activate();
            factionBorderEffect.Activate();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();

            provinceHighlight.UnloadContent();
            factionBorder.UnloadContent();
            provinceBorder.UnloadContent();
            factionBorder.UnloadContent();

            terrainSprites.Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);

            provinceHighlight.Update(gameTime);
            factionBorder.Update(gameTime);
            provinceBorder.Update(gameTime);

            base.Update(gameTime);

            Point2D mouseGameMapCoords = ScreenToMapCoordinates(mouseCoords);

            int x = mouseGameMapCoords.X;
            int y = mouseGameMapCoords.Y;

            if (x > 0 && x < world.Width &&
                y > 0 && y < world.Height)
            {
                // TODO: Handle the Id retrieval properly
                SelectedProvinceId = world.Tiles[x, y].ProvinceId;

                // TODO: Also handle this properly
                if (worldManager.GetFaction(x, y).Type == FactionType.Gaia)
                {
                    SelectedProvinceId = null;
                }
            }
            else
            {
                SelectedProvinceId = null;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Point camCoordsBegin = new Point(
                camera.Location.X / GameDefines.MapTileSize,
                camera.Location.Y / GameDefines.MapTileSize);

            Point camCoordsEnd = new Point(
                camCoordsBegin.X + camera.Size.Width / GameDefines.MapTileSize + 2,
                camCoordsBegin.Y + camera.Size.Height / GameDefines.MapTileSize + 1);

            foreach (Terrain terrain in terrains.Values.OrderBy(x => x.ZIndex))
            {
                for (int y = camCoordsBegin.Y; y < camCoordsEnd.Y; y++)
                {
                    for (int x = camCoordsBegin.X; x < camCoordsEnd.X; x++)
                    {
                        if (world.Tiles[x, y].TerrainId == terrain.Id)
                        {
                            DrawTile(spriteBatch, x, y);
                        }
                    }
                }
            }

            DrawProvinceHighlight(spriteBatch);
            DrawBorders(spriteBatch);

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Centres the camera on the specified location.
        /// </summary>
        public void CentreCameraOnLocation(Point2D worldLocation)
        {
            camera.CentreOnLocation(worldLocation * GameDefines.MapTileSize);
        }

        void DrawTile(SpriteBatch spriteBatch, int x, int y)
        {
            WorldTile tile = world.Tiles[x, y];

            Terrain terrain = terrains[tile.TerrainId]; // TODO: Optimise this. Don't call this every time

            // TODO: Don't do all this, and definetely don't do it here
            terrainEffect.TileLocation = new Point2D(x, y);
            terrainEffect.TerrainId = terrain.Id;
            terrainEffect.TilesWith = new List<string> { terrain.Id };
            terrainEffect.Update(null);

            DrawTerrainSprite(spriteBatch, x, y, terrain.Spritesheet, 1, 3);
        }

        void DrawTerrainSprite(SpriteBatch spriteBatch, int tileX, int tileY, string spritesheet, int spritesheetX, int spritesheetY)
        {
            TextureSprite terrainSprite = terrainSprites[spritesheet];

            terrainSprite.Location = new Point2D(
                tileX * GameDefines.MapTileSize - camera.Location.X,
                tileY * GameDefines.MapTileSize - camera.Location.Y);
            terrainSprite.SourceRectangle = new Rectangle2D(
                GameDefines.MapTileSize * spritesheetX, GameDefines.MapTileSize * spritesheetY,
                GameDefines.MapTileSize, GameDefines.MapTileSize);

            terrainSprite.Draw(spriteBatch);
        }

        void DrawProvinceHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(SelectedProvinceId))
            {
                return;
            }

            int cameraSizeX = camera.Size.Width / GameDefines.MapTileSize + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MapTileSize + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D((i * GameDefines.MapTileSize) - camera.Location.X % GameDefines.MapTileSize,
                                                     (j * GameDefines.MapTileSize) - camera.Location.Y % GameDefines.MapTileSize);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > world.Width ||
                        y < 0 || y > world.Height)
                    {
                        continue;
                    }

                    string provinceId = world.Tiles[x, y].ProvinceId;
                    Faction faction = worldManager.GetFaction(x, y);

                    if (faction.Type == FactionType.Gaia)
                    {
                        continue;
                    }

                    if (SelectedProvinceId == provinceId)
                    {
                        provinceHighlight.Location = screenCoords;
                        provinceHighlight.Tint = faction.Colour.ToColour();
                        provinceHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawBorders(SpriteBatch spriteBatch)
        {
            int cameraSizeX = camera.Size.Width / GameDefines.MapTileSize + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MapTileSize + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D(
                        (i * GameDefines.MapTileSize) - camera.Location.X % GameDefines.MapTileSize,
                        (j * GameDefines.MapTileSize) - camera.Location.Y % GameDefines.MapTileSize);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > world.Width ||
                        y < 0 || y > world.Height)
                    {
                        continue;
                    }

                    Faction faction = worldManager.GetFaction(x, y);

                    if (faction.Type == FactionType.Gaia)
                    {
                        continue;
                    }

                    Colour tintColour = faction.Colour.ToColour();

                    provinceBorder.Location = screenCoords;
                    factionBorder.Location = screenCoords;
                    factionBorder.Tint = tintColour;

                    provinceBorderEffect.TileLocation = gameCoords;
                    provinceBorderEffect.UpdateFrame(null);

                    factionBorderEffect.TileLocation = gameCoords;
                    factionBorderEffect.UpdateFrame(null);

                    provinceBorder.Draw(spriteBatch);
                    factionBorder.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Gets the map cpprdomates based on the specified screen coordinates.
        /// </summary>
        /// <returns>The map coordinates.</returns>
        /// <param name="screenCoords">Screen coordinates.</param>
        Point2D ScreenToMapCoordinates(Point2D screenCoords)
        {
            return new Point2D(
                (camera.Location.X + screenCoords.X) / GameDefines.MapTileSize,
                (camera.Location.Y + screenCoords.Y) / GameDefines.MapTileSize);
        }

        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            mouseCoords = e.Location;
        }
    }
}
