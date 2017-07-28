﻿using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Graphics.Extensions;
using Narivia.Gui.WorldMap;
using Narivia.Input;
using Narivia.Input.Events;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiWorldmap : GuiElement
    {
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public override Vector2 Size { get; set; }

        /// <summary>
        /// Gets the selected region identifier.
        /// </summary>
        /// <value>The selected region identifier.</value>
        [XmlIgnore]
        public string SelectedRegionId { get; private set; }

        IGameManager game;
        Camera camera;
        Map map;

        Sprite regionHighlight;
        Sprite selectedRegionHighlight;
        Sprite factionBorder;

        Vector2 mouseCoords;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            map = new Map { TileDimensions = Vector2.One * GameDefines.TILE_DIMENSIONS };

            regionHighlight = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle(GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS * 3, GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS),
                Tint = Color.White,
                Opacity = 1.0f
            };

            selectedRegionHighlight = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle(0, GameDefines.TILE_DIMENSIONS * 3, GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS),
                Tint = Color.White,
                Opacity = 1.0f
            };

            factionBorder = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle(0, GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS),
                Tint = Color.Blue,
                Opacity = 1.0f
            };

            camera.LoadContent();
            map.LoadContent(game.WorldId);

            regionHighlight.LoadContent();
            selectedRegionHighlight.LoadContent();
            factionBorder.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();
            map.UnloadContent();

            regionHighlight.UnloadContent();
            selectedRegionHighlight.UnloadContent();
            factionBorder.UnloadContent();

            base.UnloadContent();

            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);
            map.Update(gameTime);

            regionHighlight.Update(gameTime);
            selectedRegionHighlight.Update(gameTime);
            factionBorder.Update(gameTime);

            base.Update(gameTime);

            Vector2 mouseGameMapCoords = ScreenToMapCoordinates(mouseCoords);

            int x = (int)mouseGameMapCoords.X;
            int y = (int)mouseGameMapCoords.Y;

            if (x > 0 && x < game.WorldWidth &&
                y > 0 && y < game.WorldHeight)
            {
                // TODO: Handle the Id retrieval properly
                SelectedRegionId = game.GetWorldTile(x, y);

                // TODO: Also handle this properly
                if (game.FactionIdAtPosition(x, y) == "gaia")
                {
                    SelectedRegionId = null;
                }
            }
            else
            {
                SelectedRegionId = null;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch, camera);

            DrawRegionHighlight(spriteBatch);
            DrawFactionBorders(spriteBatch);

            base.Draw(spriteBatch);
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        /// <summary>
        /// Centres the camera on the position.
        /// </summary>
        public void CentreCameraOnPosition(int x, int y)
        {
            camera.CentreOnPosition(new Vector2(x * GameDefines.TILE_DIMENSIONS, y * GameDefines.TILE_DIMENSIONS));
        }

        void DrawRegionHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(SelectedRegionId))
            {
                return;
            }

            int cameraSizeX = (int)(camera.Size.X / map.TileDimensions.X + 2);
            int cameraSizeY = (int)(camera.Size.Y / map.TileDimensions.Y + 2);

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Vector2 screenCoords = new Vector2((int)(i * map.TileDimensions.X) - (int)(camera.Position.X % map.TileDimensions.X),
                                                       (int)(j * map.TileDimensions.Y) - (int)(camera.Position.Y % map.TileDimensions.Y));
                    Vector2 gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = (int)gameCoords.X;
                    int y = (int)gameCoords.Y;

                    if (x < 0 || x > game.WorldWidth ||
                        y < 0 || y > game.WorldHeight)
                    {
                        continue;
                    }

                    string regionId = game.GetWorldTile(x, y);
                    string factionId = game.FactionIdAtPosition(x, y);
                    Color factionColour = game.GetFaction(factionId).Colour.ToXnaColor();

                    if (factionId == "gaia")
                    {
                        continue;
                    }

                    if (SelectedRegionId == regionId)
                    {
                        selectedRegionHighlight.Position = screenCoords;
                        selectedRegionHighlight.Tint = factionColour;
                        selectedRegionHighlight.Draw(spriteBatch);
                    }
                    else
                    {
                        regionHighlight.Position = screenCoords;
                        regionHighlight.Tint = factionColour;
                        regionHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawFactionBorders(SpriteBatch spriteBatch)
        {
            int cameraSizeX = (int)(camera.Size.X / map.TileDimensions.X + 2);
            int cameraSizeY = (int)(camera.Size.Y / map.TileDimensions.Y + 2);

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Vector2 screenCoords = new Vector2((int)(i * map.TileDimensions.X) - (int)(camera.Position.X % map.TileDimensions.X),
                                                       (int)(j * map.TileDimensions.Y) - (int)(camera.Position.Y % map.TileDimensions.Y));
                    Vector2 gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = (int)gameCoords.X;
                    int y = (int)gameCoords.Y;

                    if (x < 0 || x > game.WorldWidth ||
                        y < 0 || y > game.WorldHeight)
                    {
                        continue;
                    }

                    string factionId = game.FactionIdAtPosition(x, y);
                    Color factionColour = game.GetFaction(factionId).Colour.ToXnaColor();

                    if (factionId == "gaia")
                    {
                        continue;
                    }

                    string factionIdN = game.FactionIdAtPosition(x, y - 1);
                    string factionIdW = game.FactionIdAtPosition(x - 1, y);
                    string factionIdS = game.FactionIdAtPosition(x, y + 1);
                    string factionIdE = game.FactionIdAtPosition(x + 1, y);

                    factionBorder.Position = screenCoords;
                    factionBorder.Tint = factionColour;

                    if (factionIdN != factionId &&
                        factionIdN != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameDefines.TILE_DIMENSIONS, 0,
                                                                      GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdW != factionId &&
                        factionIdW != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(0, GameDefines.TILE_DIMENSIONS,
                                                                      GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdS != factionId &&
                        factionIdS != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS * 2,
                                                                      GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdE != factionId &&
                        factionIdE != "gaia")
                    {
                        factionBorder.SourceRectangle = new Rectangle(GameDefines.TILE_DIMENSIONS * 2, GameDefines.TILE_DIMENSIONS,
                                                                      GameDefines.TILE_DIMENSIONS, GameDefines.TILE_DIMENSIONS);
                        factionBorder.Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map cpprdomates based on the specified screen coordinates.
        /// </summary>
        /// <returns>The map coordinates.</returns>
        /// <param name="screenCoords">Screen coordinates.</param>
        Vector2 ScreenToMapCoordinates(Vector2 screenCoords)
        {
            return new Vector2((int)((camera.Position.X + screenCoords.X) / map.TileDimensions.X),
                               (int)((camera.Position.Y + screenCoords.Y) / map.TileDimensions.Y));
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            mouseCoords = e.CurrentMousePosition;
        }
    }
}
