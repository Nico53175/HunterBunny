# HunterBunny

TBD: 

Item system including monster and loot drops, crafting, and experience systems. Approach:

1. Defining Items
- Item Data Structure:  Class which defines an item.
- Properties: ID, name, type, category, rarity, craftingID, stackAmount, Material, Prefab any other relevant attributes
-> ScriptableObjects for Item Definitions
2. Inventory System
Inventory Data Structure: System to manage player inventory: Array[itemID][Count] of item objects.
- Adding/Removing Items
- Checking for available space
UI Representation: Create UI elements to represent the inventory in the game, allowing players to view and interact with their items.
4. Monster and Loot Drops
- Drop Table: defines what items can drop from which monsters, including drop rates.
- Randomized Drops: Use the drop table to determine what dropped.
- Randomization mechanism 
5. Loot Collection
- Collect dropped items, adding them to their inventory.
6. Crafting System
- Crafting Recipes: Define crafting recipes, including required items and quantities.
- Crafting Interface: Create a UI where players can select items to craft and view required materials.
- Crafting Process: Implement the logic to check if the player has the required items and to create the new item if they do.
7. Experience System
- Experience Points: Define how players earn experience points (e.g., defeating monsters, crafting items).
- Leveling Up: Implement a system for leveling up, including experience thresholds and rewards for reaching new levels.
- Tying Experience to Items: Consider if and how experience points and levels impact the item system (e.g., items that can only be used or crafted at certain levels).
8. Saving and Loading
- Persistent Data: Implement a way to save and load inventory and item data, so that player progress is maintained between play sessions.
9. General Considerations
- Balancing: Regularly balance the item system, adjusting drop rates, crafting requirements, and item attributes to ensure a fair and enjoyable game experience.
- Expandability: Design the system in a way that allows you to easily add new items, monsters, and crafting recipes in the future.

Enemy State Machine 
- Add new States
- Make existing States work

New Animations

- Player:
-> Death
-> Free fall
-> Jump in place
-> Running backwards
-> Jumping Backwards
-> Walking Backwards

- Enemy:
-> Attacking 
-> Walking 
-> Chase
-> Crawl

Implementing URP for better Graphics and Ambient Lighting
