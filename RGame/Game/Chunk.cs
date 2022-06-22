using System.Numerics;
using Raylib_cs;
using RGame.Core;

namespace RGame.Game;

public class Chunk
{
    private const int ChunkSize = 16;
    private const int ChunkHeight = 16;

    private Block[,,] m_Blocks;

    public Chunk()
    {
        m_Blocks = new Block[ChunkSize, ChunkHeight, ChunkSize];
        BuildChunk();
    }

    private void BuildChunk()
    {
        for (int x = 0; x < m_Blocks.GetLength(0); x++)
        {
            for (int y = 0; y < m_Blocks.GetLength(1); y++)
            {
                for (int z = 0; z < m_Blocks.GetLength(2); z++)
                {
                    m_Blocks[x, y, z] = new Block(BlockType.Dirt)
                    {
                        Position = new Vector3(x, y, z),
                        Active = true
                    };
                }
            }
        }
    }

    private Block? GetBlockOrNull(uint x, uint y, uint z)
    {
        if (x >= ChunkSize ||
            y >= ChunkHeight ||
            z >= ChunkSize)
            return null;
        var block = m_Blocks[x, y, z];
        return block;
    }

    public void Draw(bool debug = false)
    {
        foreach (var block in m_Blocks)
        {
            var position = block.Position;

            block.DrawTop = GetBlockOrNull((uint)position.X, (uint)position.Y + 1, (uint)position.Z) == null;
            block.DrawBottom = GetBlockOrNull((uint)position.X, (uint)position.Y - 1, (uint)position.Z) == null;
            block.DrawLeft = GetBlockOrNull((uint)position.X - 1, (uint)position.Y, (uint)position.Z) == null;

            block.DrawRight = GetBlockOrNull((uint)position.X + 1, (uint)position.Y, (uint)position.Z) == null;
            block.DrawFront = GetBlockOrNull((uint)position.X, (uint)position.Y, (uint)position.Z + 1) == null;
            block.DrawBack = GetBlockOrNull((uint)position.X, (uint)position.Y, (uint)position.Z - 1) == null;

            block.Draw(debug);
        }
    }
}