using DirectX11;
using UnityEngine;

namespace Assets.ProceduralGeneration.LineDrawing
{
    /// <summary>
    /// I think this does a hole-less (no diagonals) linecast. Ideal for grid raytraces
    /// </summary>
    public struct GridRaycaster2D
    {

        public static GridRaycaster2D InitCast(Ray2D ray, float maxLength, int gridWidth, int gridHeight)
        {
            var ret = new GridRaycaster2D();
            float gridCellSize = 1;

            ret.ray = ray;
            getHelpers(gridCellSize, ray.origin.x, ray.direction.x, out ret.tileX, out ret.dtileX, out ret.dtX, out ret.ddtX);
            getHelpers(gridCellSize, ray.origin.y, ray.direction.y, out ret.tileY, out ret.dtileY, out ret.dtY, out ret.ddtY);
            ret.done = false;
            ret.t = 0;
            ret.maxLength = maxLength;
            ret.pixWidth = gridWidth;
            ret.pixHeight = gridHeight;
            return ret;
        }


        // Width and height of the texture in pixels.
        public int pixWidth;
        public int pixHeight;

        /// The code

        int tileX, dtileX;
        int tileY, dtileY;
        float dtX, ddtX;
        float dtY, ddtY;
        private float t;
        private Ray2D ray;
        private float maxLength;
        private bool done;

        private int numSteps; // For infinite loop protection

        public bool MoveNext(out Point2 outStep)
        {
            if (numSteps > 10000)
            {
                outStep = new Point2();
                // Infinite loop protectino
                done = true;
                Debug.LogError("too long grid 2d raycast");
                return false;
            }

            if (done)
            {
                outStep = new Point2();
                return false;
            }

            numSteps++;

            if (ray.direction.x * ray.direction.x + ray.direction.y * ray.direction.y > 0
                )
            {
                //while (tileX >= 0 && tileX < pixWidth && tileY >= 0 && tileY < pixHeight)
                if (tileX >= 0 && tileX <= pixWidth && tileY >= 0 && tileY <= pixHeight
                    && t < maxLength)
                {
                    //grid[tileY][tileX] = true;
                    outStep = new Point2(tileX - 1, tileY - 1);
                    //                    pix[tileY * pixHeight + tileX] = Color.white;

                    //mark(ray.startX + ray.dirX * t, ray.startY + ray.dirY * t);

                    if (dtX < dtY)
                    {
                        tileX = tileX + dtileX;
                        var dt = dtX;
                        t = t + dt;
                        dtX = dtX + ddtX - dt;
                        dtY = dtY - dt;
                    }
                    else
                    {
                        tileY = tileY + dtileY;
                        var dt = dtY;
                        t = t + dt;
                        dtX = dtX - dt;
                        dtY = dtY + ddtY - dt;
                    }

                    return true;
                }
            }
            else
            {
                //grid[tileY][tileX] = true;
                outStep = new Point2(tileX - 1, tileY - 1);
                done = true;
                return true;

                //                Draw.Debug.CircleXZ(new Vector3(tileX + 0.5f-1, 0, tileY + 0.5f-1), 0.5f, Color.yellow);
                //                pix[tileY * pixHeight + tileX] = Color.white;
            }
            outStep = new Point2();

            return false;

        }


        static void getHelpers(float cellSize, float pos, float dir, out int tile, out int dTile, out float dt, out float ddt)
        {
            tile = Mathf.FloorToInt(pos / cellSize) + 1;
            if (dir > 0.0001f)
            {
                dTile = 1;
                dt = ((tile + 0) * cellSize - pos) / dir;
            }
            else if (dir < -0.0001f)
            {
                dTile = -1;
                dt = ((tile - 1) * cellSize - pos) / dir;
            }
            else
            {
                // zero case
                dTile = 0;
                dt = float.MaxValue;
                ddt = 0;
                return;

            }

            ddt = dTile * cellSize / dir;
            //return tile, dTile, dt, dTile* cellSize / dir
        }
    }
}