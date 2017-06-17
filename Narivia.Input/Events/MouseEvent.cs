﻿using Microsoft.Xna.Framework;

namespace Narivia.Input.Events
{
    /// <summary>
    /// Mouse event handler.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Mouse event arguments.
    /// </summary>
    public class MouseEventArgs
    {
        /// <summary>
        /// Gets current position of the mouse.
        /// </summary>
        /// <value>The current mouse position.</value>
        public Vector2 CurrentMousePosition { get; private set; }

        /// <summary>
        /// Gets previous position of the mouse.
        /// </summary>
        /// <value>The previous mouse position.</value>
        public Vector2 PreviousMousePosition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="currentMousePosition">Mouse position.</param>
        public MouseEventArgs(Vector2 currentMousePosition, Vector2 previousMousePosition)
        {
            CurrentMousePosition = currentMousePosition;
            PreviousMousePosition = previousMousePosition;
        }
    }
}
