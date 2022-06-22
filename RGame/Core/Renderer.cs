using System.Numerics;
using Raylib_cs;
using RGame.Game;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core;

public static class Renderer
{
    public static void DrawBlock(Block block,
        bool drawFront = true,
        bool drawBack = true,
        bool drawTop = true,
        bool drawBottom = true,
        bool drawRight = true,
        bool drawLeft = true)
    {
        var position = block.Position;
        var textureID = block.Texture!.Value.id;
        var color = (Color)block.TintColor;
        var width = block.Scale;
        var height = block.Scale;
        var length = block.Scale;

        float x = position.X;
        float y = position.Y;
        float z = position.Z;

        int vertexCount = 0;
        vertexCount += drawFront ? 6 : 0;
        vertexCount += drawBack ? 6 : 0;
        vertexCount += drawTop ? 6 : 0;
        vertexCount += drawBottom ? 6 : 0;
        vertexCount += drawRight ? 6 : 0;
        vertexCount += drawLeft ? 6 : 0;

        rlCheckRenderBatchLimit(vertexCount);

        rlSetTexture(textureID);

        //rlPushMatrix();
        // NOTE: Transformation is applied in inverse order (scale -> rotate -> translate)
        //rlTranslatef(2.0f, 0.0f, 0.0f);
        //rlRotatef(45, 0, 1, 0);
        //rlScalef(2.0f, 2.0f, 2.0f);

        rlBegin(DrawMode.QUADS);
        {
            rlColor4ub(color.r, color.g, color.b, color.a);

            // Front Face
            if (drawFront)
            {
                rlNormal3f(0.0f, 0.0f, 1.0f); // Normal Pointing Towards Viewer
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x - width / 2, y - height / 2, z + length / 2); // Bottom Left Of The Texture and Quad
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x + width / 2, y - height / 2, z + length / 2); // Bottom Right Of The Texture and Quad
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x + width / 2, y + height / 2, z + length / 2); // Top Right Of The Texture and Quad
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x - width / 2, y + height / 2, z + length / 2); // Top Left Of The Texture and Quad
            }

            // Back Face
            if (drawBack)
            {
                rlNormal3f(0.0f, 0.0f, -1.0f); // Normal Pointing Away From Viewer
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x - width / 2, y - height / 2, z - length / 2); // Bottom Right Of The Texture and Quad
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x - width / 2, y + height / 2, z - length / 2); // Top Right Of The Texture and Quad
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x + width / 2, y + height / 2, z - length / 2); // Top Left Of The Texture and Quad
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x + width / 2, y - height / 2, z - length / 2); // Bottom Left Of The Texture and Quad
            }

            // Top Face
            if (drawTop)
            {
                rlNormal3f(0.0f, 1.0f, 0.0f); // Normal Pointing Up
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x - width / 2, y + height / 2, z - length / 2); // Top Left Of The Texture and Quad
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x - width / 2, y + height / 2, z + length / 2); // Bottom Left Of The Texture and Quad
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x + width / 2, y + height / 2, z + length / 2); // Bottom Right Of The Texture and Quad
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x + width / 2, y + height / 2, z - length / 2); // Top Right Of The Texture and Quad
            }
            
            // Bottom Face
            if (drawBottom)
            {
                rlNormal3f(0.0f, -1.0f, 0.0f); // Normal Pointing Down
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x - width / 2, y - height / 2, z - length / 2); // Top Right Of The Texture and Quad
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x + width / 2, y - height / 2, z - length / 2); // Top Left Of The Texture and Quad
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x + width / 2, y - height / 2, z + length / 2); // Bottom Left Of The Texture and Quad
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x - width / 2, y - height / 2, z + length / 2); // Bottom Right Of The Texture and Quad
            }

            // Right face
            if (drawRight)
            {
                rlNormal3f(1.0f, 0.0f, 0.0f); // Normal Pointing Right
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x + width / 2, y - height / 2, z - length / 2); // Bottom Right Of The Texture and Quad
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x + width / 2, y + height / 2, z - length / 2); // Top Right Of The Texture and Quad
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x + width / 2, y + height / 2, z + length / 2); // Top Left Of The Texture and Quad
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x + width / 2, y - height / 2, z + length / 2); // Bottom Left Of The Texture and Quad
            }

            // Left Face
            if (drawLeft)
            {
                rlNormal3f(-1.0f, 0.0f, 0.0f); // Normal Pointing Left
                rlTexCoord2f(0.0f, 0.0f);
                rlVertex3f(x - width / 2, y - height / 2, z - length / 2); // Bottom Left Of The Texture and Quad
                rlTexCoord2f(1.0f, 0.0f);
                rlVertex3f(x - width / 2, y - height / 2, z + length / 2); // Bottom Right Of The Texture and Quad
                rlTexCoord2f(1.0f, 1.0f);
                rlVertex3f(x - width / 2, y + height / 2, z + length / 2); // Top Right Of The Texture and Quad
                rlTexCoord2f(0.0f, 1.0f);
                rlVertex3f(x - width / 2, y + height / 2, z - length / 2); // Top Left Of The Texture and Quad
            }
        }
        rlEnd();

        rlSetTexture(0);
    }


}