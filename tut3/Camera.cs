using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tut3
{
    /// <summary>
    /// A basic camera using Euler angles
    /// </summary>
    class Camera
    {
        private float r = 10f;
        public Vector3 Position = new Vector3(2f, 2f, 2f);
        public Vector3 Direction = new Vector3((float)Math.PI, 0f, 0f);
        public Vector3 Target = new Vector3(0f, 0f, 0f);
        private Vector3 Right = new Vector3(1f, 0f, 0f);
        private Vector3 Up = new Vector3(0f, 1f, 0f);

        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;
        
        float horizontalAngle = 0f;
        float verticalAngle = 0.0f;

        public Camera()
        {
            var px = r * (float)(Math.Cos(verticalAngle) * Math.Sin(horizontalAngle));
            var py = r * (float)(Math.Sin(verticalAngle));
            var pz = r * (float)(Math.Cos(verticalAngle) * Math.Cos(horizontalAngle));
            Position = new Vector3(px, py, pz);

            var rx = (float)(Math.Sin(horizontalAngle - Math.PI / 2.0f));
            var ry = (float)(0);
            var rz = (float)(Math.Cos(horizontalAngle - Math.PI / 2.0f));
            Right = new Vector3(rx, ry, rz);

            Direction = Target - Position;
            Up = Vector3.Cross(Direction, Right);
        }

        /// <summary>
        /// Calculate a view matrix for this camera
        /// </summary>
        /// <returns>A view matrix from this camera</returns>
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Target, Up);
        }

        /// <summary>
        /// Moves the camera in local space
        /// </summary>
        /// <param name="x">Distance to move along the screen's x axis</param>
        /// <param name="y">Distance to move along the axis of the camera</param>
        /// <param name="z">Distance to move along the screen's y axis</param>
        public void Move(float x, float y, float z)
        {
            /** When the camera moves, we don't want it to move relative to the world coordinates 
             * (like the XYZ space its position is in), but instead relative to the camera's view. 
             * Like the view angle, this requires a bit of trigonometry. */

            Vector3 offset = new Vector3();

            Vector3 forward = Up;
            Vector3 right = Right;

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
            Target += offset;
        }
        
        private void show()
        {
            Console.WriteLine("Position: " + Position.ToString());
            Console.WriteLine("Direction: " + Direction.ToString());
            Console.WriteLine("Up: " + Up.ToString());
        }
        
        public void AddRotation(float xpos, float ypos)
        {
            // Compute new orientation
            horizontalAngle -= MouseSensitivity * xpos;
            verticalAngle += MouseSensitivity * ypos;

            var px = r*(float)(Math.Cos(verticalAngle) * Math.Sin(horizontalAngle));
            var py = r*(float)(Math.Sin(verticalAngle));
            var pz = r*(float)(Math.Cos(verticalAngle) * Math.Cos(horizontalAngle));
            Position = new Vector3(px, py, pz);

            var rx = (float)(Math.Sin(horizontalAngle - Math.PI / 2.0f));
            var ry = (float)(Math.Sin(verticalAngle));
            var rz = (float)(Math.Cos(horizontalAngle - Math.PI / 2.0f));
            Right = new Vector3(rx, ry, rz);

            Direction = Target - Position;
            Up = Vector3.Cross(Direction, Right);

            show();
        }

        public void Radius(float deltaPrecise)
        {
            r += deltaPrecise*MouseSensitivity;
            AddRotation(0, 0);
            Console.WriteLine("Radius: " + r);
        }
    }
}
