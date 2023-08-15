using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blocktype
{
    BLACK,
    WHITE,
}

namespace CherryTeaGames.Generators
{
    public class MapGen : MonoBehaviour
    {
        public List<Texture2D> maps; // All maps assume same width and height as first.
        public GameObject ground;
        public GameObject blockW;
        public GameObject blockB;
        public GameObject enemy;
        public float distanceBetweenCells = 1.05f;

        public Transform groundHold;
        public Transform blockWHold;
        public Transform blockBHold;
        public Transform enemyHold;

#if (UNITY_EDITOR)
        [ContextMenu("Generate Map From Texture")]
        public void GenerateMap()
        {
            ResetBlocks();
            this.transform.localScale = new Vector3(1, 1, 1); // Set to 1 before layout so everything has a scale of 1.
                                                              // enemyHold.transform.localScale = new Vector3(1,1,1);
            CreateGround();
            CreateMap();

            // Adjust Maps levels
            blockBHold.transform.localPosition = new Vector3(0, .66f, 0); // emperically determined to be above ground.
            blockWHold.transform.localPosition = new Vector3(0, -.66f, 0); // emperically determined to be below ground.
        }

        public void CreateGround()
        {
            int mapwidth = maps[0].width + Mathf.FloorToInt((distanceBetweenCells - blockB.transform.lossyScale.x) * maps[0].width);
            int mapheight = maps[0].height + Mathf.FloorToInt((distanceBetweenCells - blockB.transform.lossyScale.z) * maps[0].height);
            GameObject _ground = Instantiate(ground, new Vector3(1, 0, 1), Quaternion.identity);
            _ground.transform.localScale = new Vector3(mapwidth, _ground.transform.localScale.y, mapheight);
            _ground.transform.parent = groundHold;
        }

        [ContextMenu("GenerateTestGround")]
        public void CreateDummyGround()
        {
            int mapwidth = 32;
            int mapheight = 32;
            GameObject _ground = Instantiate(ground, new Vector3(1, 0, 1), Quaternion.identity);
            _ground.transform.localScale = new Vector3(mapwidth, _ground.transform.localScale.y, mapheight);
            _ground.transform.parent = groundHold;
        }

        public void CreateMap()
        {
            foreach (Texture2D map in maps)
            {
                int mapwidth = map.width;
                int mapheight = map.height;
                int xoffset = -(map.width) / 2;
                int zoffset = -(map.height) / 2;
                Color[] pixels = map.GetPixels();
                float px, pz;
                for (int i = 0; i < pixels.Length; i++)
                {
                    px = i % mapwidth * distanceBetweenCells + xoffset;
                    pz = Mathf.Floor(i / mapwidth) * distanceBetweenCells + zoffset;
                    if (pixels[i].a == 0)
                        continue;
                    if (pixels[i] == Color.white)
                    {
                        CreateBlock(blocktype.WHITE, px, pz);
                    }
                    if (pixels[i] == Color.black)
                    {
                        CreateBlock(blocktype.BLACK, px, pz);
                    }
                    if (pixels[i].r > 0 && pixels[i].b == 0)
                    {
                        createEnemy(px, pz);
                    }
                }
            }
        }

        private void createEnemy(float x_pos, float z_pos)
        {
            GameObject _enemy = Instantiate(enemy, new Vector3(x_pos, .5f, z_pos), Quaternion.identity);
            _enemy.transform.parent = enemyHold;
        }

        public void CreateBlock(blocktype tp, float x_pos, float z_pos)
        {
            Transform container = blockWHold;
            GameObject block = blockW;
            if (tp == blocktype.BLACK)
            {
                container = blockBHold;
                block = blockB;
            }
            GameObject _block = Instantiate(block, new Vector3(x_pos, 0, z_pos), Quaternion.identity);
            _block.transform.SetParent(container, false);
        }

        [ContextMenu("Remove Generated Items")]
        public void ResetBlocks()
        {
            while (blockWHold.childCount != 0)
            {
                GameObject.DestroyImmediate(blockWHold.GetChild(0).gameObject);
            }
            while (blockBHold.childCount != 0)
            {
                GameObject.DestroyImmediate(blockBHold.GetChild(0).gameObject);
            }
            ResetGround();
            ResetEnemies();
        }

        public void ResetGround()
        {
            while (groundHold.childCount != 0)
            {
                GameObject.DestroyImmediate(groundHold.GetChild(0).gameObject);
            }
        }

        public void ResetEnemies()
        {
            while (enemyHold.childCount != 0)
            {
                GameObject.DestroyImmediate(enemyHold.GetChild(0).gameObject);
            }
        }
#endif
    }
}
