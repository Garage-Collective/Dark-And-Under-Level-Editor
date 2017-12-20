using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkUnderLevelEditor {

    class Level {

        public String line1;
        public String line2;

        public int startPosX = -1;
        public int startPosY = -1;
        public int direction;

        public int levelDimensionX;
        public int levelDimensionY;

        public Byte[,] tileData;
        public TreeNode node;

        public List<LevelEnemy> enemies = new List<LevelEnemy>();
        public List<LevelItem> items = new List<LevelItem>();
        public List<LevelDoor> doors = new List<LevelDoor>();

    }

}
