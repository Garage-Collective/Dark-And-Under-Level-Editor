using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkUnderLevelEditor {

    public enum Images {
        Enemy,
        Item,
        Door,
        EnemyRoot,
        ItemRoot,
        DoorRoot,
        Level,
        Error,
        File
    };

    public enum MapElement {
        Floor,
        Wall,
        LockedGate = 100,
        LockedDoor,
        UnlockedDoor,
    };

    public enum EnemyType {
        Occular,
        Skeleton,
        Sparkat,
        Wraith,
        Dragon,
        Rat,
        Slime
    };

    public enum ItemType {
        None,
        Key,
        Potion,
        Scroll,
        Shield,
        Sword,
        LockedGate = 100,
        LockedDoor,
        UnlockedDoor,
        SelfLockingDoor
    };

    static class Utils {

        public static string ToString(bool value)
        {
            return value ? "true" : "false";
        }

        private static readonly Dictionary<ItemType, string> itemTypeDictionary = CreateEnumDictionary<ItemType>();
        private static readonly Dictionary<EnemyType, string> enemyTypeDictionary = CreateEnumDictionary<EnemyType>();
        
        private static Dictionary<T, string> CreateEnumDictionary<T>() {
            return GetEnumValues<T>().ToDictionary(type => type, type => Enum.GetName(typeof(T), type));
        }
        
        // Handy Type-safe wrapper
        private static IEnumerable<T> GetEnumValues<T>() {
            var values = Enum.GetValues(typeof(T));
            // If value isn't a T this will throw an exception
            foreach(T value in values) {
                yield return value;
            }
        }

        public static String getDoorTypeDescription(ItemType itemType) {

            if((int)itemType < (int)ItemType.LockedGate) {
                throw new ArgumentOutOfRangeException("itemType was not a door");
            }
            return itemTypeDictionary[itemType];

        }

        public static String getItemTypeDescription(ItemType itemType) {


            if ((int)itemType >= (int)ItemType.LockedGate) {
                throw new ArgumentOutOfRangeException("itemType was not an item");
            }
            return itemTypeDictionary[itemType];

        }

        public static String getEnemyTypeDescription(EnemyType enemyType) {

            return enemyTypeDictionary[enemyType];

        }

        public static List<LevelError> validateLevel(Level level, List<Tile> tiles) {

            List<LevelError> errors = new List<LevelError>();
            Byte[,] levelData = new Byte[255, 255];


            Boolean doorInError = true;

            if (level.levelDimensionX > 0 && level.levelDimensionY > 0) {

                for (int x = 0; x < level.levelDimensionX; x++) {

                    for (int y = 0; y < level.levelDimensionY; y++) {

                        int tileNumber = level.tileData[y, x];
                        Tile tile = tiles[tileNumber];

                        for (Byte tileY = 0; tileY < 15; tileY++) {

                            for (Byte tileX = 0; tileX < 15; tileX++) {

                                int index = tileX + ((Byte)(tileY / 8) * 15);

                                if ((tile.Data[((tileY / 8) * 15) + tileX] & (1 << (tileY % 8))) > 0) {

                                    levelData[(y * 15) + tileY, (x * 15) + tileX] = 1;

                                }

                            }

                        }

                    }

                }


                foreach (LevelEnemy levelEnemy in level.enemies) {

                    if (levelData[levelEnemy.startPosY, levelEnemy.startPosX] == 1) {

                        levelEnemy.inError = true;
                        levelEnemy.node.SelectedImageIndex = (int)Images.Error;
                        levelEnemy.node.ImageIndex = (int)Images.Error;
                        levelEnemy.node.EnsureVisible();

                        LevelError levelError = new LevelError();
                        levelError.level = level;
                        levelError.node = levelEnemy.node;
                        levelError.error = Utils.getEnemyTypeDescription(levelEnemy.enemyType) + " is located in the same place as a wall.";
                        errors.Add(levelError);

                    }
                    else {

                        levelEnemy.node.SelectedImageIndex = (int)Images.Enemy;
                        levelEnemy.node.ImageIndex = (int)Images.Enemy;
                        levelEnemy.inError = false;

                    }

                }

                foreach (LevelItem levelItem in level.items) {

                    if (levelData[levelItem.startPosY, levelItem.startPosX] == 1) {

                        levelItem.inError = true;
                        levelItem.node.SelectedImageIndex = (int)Images.Error;
                        levelItem.node.ImageIndex = (int)Images.Error;
                        levelItem.node.EnsureVisible();

                        LevelError levelError = new LevelError();
                        levelError.level = level;
                        levelError.node = levelItem.node;
                        levelError.error = Utils.getItemTypeDescription(levelItem.itemType) + " is located in the same place as a wall.";
                        errors.Add(levelError);

                    }
                    else {

                        levelItem.inError = false;
                        levelItem.node.SelectedImageIndex = (int)Images.Item;
                        levelItem.node.ImageIndex = (int)Images.Item;

                    }

                }

                if (level.startPosX == -1 || level.startPosY == -1) {

                    LevelError levelError = new LevelError();
                    levelError.level = level;
                    levelError.node = level.node;
                    levelError.error = "The player's starrting position must be selected.";
                    errors.Add(levelError);

                }
                else {

                    if (levelData[level.startPosY, level.startPosX] == 1) {

                        LevelError levelError = new LevelError();
                        levelError.level = level;
                        levelError.node = level.node;
                        levelError.error = "The player is located in the same place as a wall.";
                        errors.Add(levelError);

                    }

                }

                foreach (LevelDoor levelDoor in level.doors) {

                    if (levelDoor.doorType == ItemType.LockedDoor) {
                        doorInError = false;
                    }

                }

                if (doorInError) {

                    LevelError levelError = new LevelError();
                    levelError.level = level;
                    levelError.node = level.node;
                    levelError.error = "At least one door must be added to the level.";
                    errors.Add(levelError);

                }

            }

            return errors;

        }


        public static TreeNode getRootNode(TreeNode node) {

            while (node.Parent != null) {
                node = node.Parent;
            }

            return node;

        }

    }

}
