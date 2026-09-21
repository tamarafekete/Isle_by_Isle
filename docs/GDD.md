# [Working Title] - Game Design Document

**Status:** pre-production, v0.1 (compiled from planning Sessions 0.1 to 0.7)
**Engine:** Godot 4.7.2 (.NET build), C#, Rider
**Platform:** PC first
**Team:** solo developer, about 4 to 5 hours per week

---

## 1. Vision

**Pitch.** A cozy isometric farming game where you grow a tiny floating island into a personal archipelago, one bridge at a time.

**Target feeling.** Motivated, curious, and calm.

- _Motivated:_ you grow your skills and upgrade your tools and gear.
- _Curious:_ every bridge might lead somewhere new.
- _Calm:_ you are never punished.

**Target player.** Someone who plays Stardew Valley or Animal Crossing to wind down and likes decorating and tending things. Combat and optimization exist for players who want them, but nobody is required to touch them.

**Not in the game.** Time-limited failure, permadeath, item loss on defeat, survival needs (hunger, heat and cold are optional buffs only).

## 2. Design pillars

Every feature must pass these four rules.

1. **Tend, don't grind.** If a feature creates pressure or repetition without charm, we cut it.
2. **Your island, your layout.** Placement choices matter mechanically or visually.
3. **Expansion is spatial.** Growth comes from physically building outward, not from menus.
4. **Never punish.** No death, no lost inventory, no failure clock. If defeated, you teleport home with everything.

## 3. The hook

The player designs the bridges, and the bridge layout decides which islands can be reached and what each one offers.

- The home island has a fixed biome (Meadow).
- Every other island is generated (randomized) based on its **level**.
- **Island level** equals the number of bridge segments from home along the shortest connected route.
- Low-level biomes sit closer to home. Higher-level islands hold rarer materials.

## 4. Core loops

**30 seconds.** Use a tool on something (chop, mine, till, water, harvest), get a satisfying result, and collect a resource or crop. Better tools make this faster, bigger or more rewarding.

**30 minutes (one island day).** Tend crops, gather materials, craft or upgrade something, place or decorate. Then spend the rest of the day at home or build a bridge and explore.

**Long term (the archipelago).**

1. Home island resources are enough for the first tools and the first bridges.
2. Bridges lead to islands with better or rarer resources, tied to level and biome.
3. Rarer resources plus higher skill unlock better tool tiers.
4. Better tools reach higher-level islands and make gathering at home more efficient.
5. Repeat, with the island and archipelago becoming more personal each cycle.

## 5. Progression

### Tool tiers

| Tier     | Material                       | Source                   | Status    |
| -------- | ------------------------------ | ------------------------ | --------- |
| Wood     | Wood                           | Home island              | Version 1 |
| Stone    | Stone                          | Home island              | Version 1 |
| Copper   | Copper ore (smelted into bars) | Rocky Highlands, level 1 | Version 1 |
| Iron     | Iron ore                       | Level 2 islands          | Later     |
| Titanium | Titanium ore                   | Level 3 islands          | Later     |

### Skills

Skills are levelled by use and unlock tool tiers. There are no skill trees.

| Skill                                       | Tool                 | Version |
| ------------------------------------------- | -------------------- | ------- |
| Gardening                                   | Hoe, watering can    | 1       |
| Mining                                      | Pickaxe              | 1       |
| Gathering (woodcutting and foraging plants) | Axe, hands or basket | 1       |
| Cooking, combat, furniture crafting         | Various              | Later   |

### Upgrade rule

Upgrading a tool requires the tier's material plus the matching skill level. Recipes are always visible, and locked tiers show what is missing. Skill levels run 1 to 5 in version 1. Combat gear later uses the same tier system.

Progression is never forced. Players who only farm and decorate can ignore higher tiers.

## 6. Content

### 6.1 Biomes

| Biome                    | Feel                    | Materials                                               | Plants and food                                 | Special                 |
| ------------------------ | ----------------------- | ------------------------------------------------------- | ----------------------------------------------- | ----------------------- |
| **Meadow** (home, fixed) | Gentle, green, safe     | Wood, stone                                             | Oak, berries, wild wheat, potato, carrot, fiber | Clay (riverbank), flint |
| **Forest**               | Lush, shady, mysterious | Hardwood (birch, pine), dark stone, early coal outcrops | Mushrooms, herbs, moss                          | Resin                   |
| **Rocky Highlands**      | Craggy, bold, windy     | Copper, then iron, gems (quartz, ruby), slate           | Hardy mountain plants, lichen, wild rose        | Flint                   |
| **Desert**               | Hot, sparse, barren     | Iron, then titanium (deep), sandstone, rare ores        | Cactus, desert grasses, dates                   | Fine sand               |
| **Snowy**                | Quiet, frosty, austere  | Titanium veins, frozen stone                            | Frostberries, winter moss, ice flower           | Clear ice               |

### 6.2 Resources by island level

Each biome has a resource table, and each resource has a minimum island level. Biome decides which resources can appear. Level decides how far into that table you get.

| Biome           | Level 1                    | Level 2         | Level 3+            |
| --------------- | -------------------------- | --------------- | ------------------- |
| Forest          | Hardwood, resin, mushrooms | More rare herbs | Same                |
| Rocky Highlands | Copper, flint              | + Iron, gems    | + Richer veins      |
| Desert          | Rare                       | Iron, sand      | + Titanium          |
| Snowy           | Does not spawn             | Rare            | Titanium, clear ice |

**Spawn rule.** Low levels mostly roll Forest and Rocky Highlands. Desert and Snowy become likely only farther out.

### 6.3 Crops

| Crop         | Grow time | Seed source                    | Use                                        |
| ------------ | --------- | ------------------------------ | ------------------------------------------ |
| Wheat        | 3 days    | Meadow (wild wheat)            | Staple food, flour, bread, animal feed     |
| Potato       | 4 days    | Meadow (wild berry areas)      | Baked food, optional hunger-restore buff   |
| Carrot       | 3 days    | Meadow (wild greens)           | Simple food, bait, animal taming           |
| Prickly pear | 5 days    | Desert (harvested from cactus) | Hydration, optional heat-resistance buff   |
| Frostberry   | 6 days    | Snowy (frosty bushes)          | Tea or brew, optional cold-resistance buff |

Version 1 ships with wheat, potato and carrot.

### 6.4 Wild plants and trees (gathering skill)

- **Meadow:** oak (basic building wood), wild rose (gathering XP, early bandage material).
- **Forest:** birch (light wood for furniture and better tool handles), brown mushroom (cooking).
- **Rocky Highlands:** hardy shrub or wild rose (gathering XP, fiber).
- **Desert:** cactus (gives cactus trunks, a special wood substitute).
- **Snowy:** winter moss (insulation for clothes and beds).

### 6.5 Materials

- **Common:** wood, stone, fiber, copper ore (smelted into copper bars in a furnace).
- **Biome-specific:** clay (Meadow), resin (Forest), flint (Rocky Highlands), fine sand (Desert, smelts into glass), clear ice (Snowy, for cooling chests or water).

### 6.6 Placeables

| Category          | Items                                                                                                  |
| ----------------- | ------------------------------------------------------------------------------------------------------ |
| Fences            | Wooden fence, stone fence                                                                              |
| Paths             | Trampled dirt path, stone-slab paving                                                                  |
| Storage           | Wooden chest (15 slots), reinforced chest with copper or iron (30 slots)                               |
| Decorations       | Bench (can sit), clay flower pot (holds any wildflower), outdoor torch or campfire                     |
| Bridges           | Wooden bridge segment, 3 units wide. One item used both between islands and to cross gaps on an island |
| Crafting stations | Workbench (tools, chests, furniture), furnace (smelts ore into bars, sand into glass)                  |

## 7. Systems

### 7.1 Time and weather

- One in-game day lasts about 10 to 12 real minutes. Time pauses in menus.
- Sleeping in a bed skips to the next morning. Night is for mood and light only.
- Version 1 weather: sunny or rainy. Rain waters crops automatically.
- No seasons in version 1. Nothing has a real-time deadline.

### 7.2 Farming

- Till soil with the hoe, plant a seed, water daily, harvest when grown.
- Growth advances one stage on each day the crop was watered. Unwatered crops pause. **Crops never die or rot.**
- Tool tier changes the area: wood watering can 1 tile, stone 3, copper 5. Hoe tiers work the same way.
- Seeds come from wild plants. Harvesting returns the crop plus a chance of extra seeds.

### 7.3 Gathering and skills

- Axe on trees, pickaxe on rocks and ores, hands or basket on wild plants.
- Each action gives XP to its skill. Skills gate tool tiers and give small passive perks, such as a chance for a bonus drop.
- Trees and rocks respawn after a few days.

### 7.4 Tool upgrades

- Done at the workbench: required material plus required skill level.
- Higher tiers mean fewer hits to gather, larger areas for the hoe and can, and access to harder materials.

### 7.5 Crafting

- Hand crafting for basics (fence, torch), workbench for tools, chests and furniture, furnace for smelting.
- Crafting is instant or a short animation. No waiting timers.

### 7.6 Building and placement

- All placeables snap to a tile grid, with a placement preview, rotation and a green or red validity indicator.
- Picking up or demolishing refunds 100%.
- Decorations are mostly visual, with small effects (a torch lights the night, a bench lets you sit).

### 7.7 Bridges and islands

- The archipelago is a **grid of cells**. The home island sits in the center cell, and neighboring cells hold undiscovered islands.
- The player places bridge segments from a dock point at the island edge toward a neighboring cell. When a bridge reaches a cell, its island is **generated from a seed** and revealed.
- Island level equals bridge distance from home. Level and the spawn rule decide biome and resources.
- Bridges can branch, and islands can connect to several others.
- Each segment costs wood (stronger materials later), so distance is an investment.
- Generated islands are saved and stay the same when revisited.

### 7.8 Inventory

- 20 backpack slots plus a hotbar of 8, with stacks up to 99.
- Tools sit in their own slots. Chests hold 15 or 30 slots.
- A full inventory blocks pickup with a friendly message. Items are never destroyed.

### 7.9 Optional combat (design now, build later)

- A few enemy types on higher-level islands, one or two weapons using the same tier system, simple health.
- Defeat means teleport home with everything kept.

### 7.10 Economy

- Version 1 has **no currency and no shop**. Progress comes from gathering and crafting.

### 7.11 Survival-style effects

- Food and drinks give optional buffs (hunger restore, heat or cold resistance). These are never needs and never cause penalties.

## 8. Art, audio and feel

### Art direction

Storybook and painterly, achieved through lighting, a strict palette per biome, a toon and paper shader, and one cohesive free stylized asset pack. No custom Blender or painting work for now. Free sources to evaluate: Kenney, Quaternius, Poly Pizza (check every license).

**Consistency rules (what keeps it from looking cheap):** one asset source per category, one scale, one palette per biome, one shader, one lighting setup.

**Style test before committing:** one ground tile, one tree, one rock and one fence, lit and shaded in a scene. If it looks right, the whole game follows that look.

### Palette by biome

Meadow: fresh greens and warm yellows. Forest: deep greens and teal shadows. Rocky Highlands: warm grays and copper orange. Desert: sand and terracotta. Snowy: icy blues and white. UI: warm cream and wood-brown.

### Camera and UI

- Fixed isometric orthogonal camera with optional 90-degree rotation. Objects that block the player fade.
- Minimal UI: hotbar at the bottom, small clock and day counter, menus that look like paper or wood.

### Audio

Soft acoustic guitar, piano and light percussion, unhurried, with separate calm day and night tracks. Warm, rounded sound effects. Ambient sound per biome.

### Juice

Harvest pop with a small sparkle, squash-and-stretch when a tree falls, dust puffs when tilling, a soft glow when a bridge segment connects, visible crop growth stages, smooth camera follow.

## 9. Scope and roadmap

### 9.1 Feature priorities

| Priority       | Features                                                                                                                                                                                                                                                                                                   |
| -------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Must**       | Isometric camera and player, interaction, gathering, inventory and hotbar, farming loop with 3 crops, day/night and sleep, workbench and furnace crafting, tool tiers (wood, stone, copper), core placeables, bridges plus 2 generated islands (Meadow and Rocky Highlands), save/load, the painterly look |
| **Should**     | Forest biome, skill levels with small perks, rain, music and sound, juice effects                                                                                                                                                                                                                          |
| **Could**      | Bench and flower pot, basic combat, extra crops                                                                                                                                                                                                                                                            |
| **Won't (v1)** | Iron and titanium, Desert and Snowy, NPCs and story, currency, cooking, furniture crafting, seasons                                                                                                                                                                                                        |

### 9.2 Milestones

| #   | Milestone                                                          | Rough time              |
| --- | ------------------------------------------------------------------ | ----------------------- |
| 0   | Finish planning and setup                                          | Done with this document |
| 1   | Foundation: camera, player, island, style test                     | 2 to 3 weeks            |
| 2   | Interaction and gathering                                          | 3 weeks                 |
| 3   | Items and inventory (data-driven)                                  | 3 weeks                 |
| 3.5 | **Hook spike:** grey-box bridge placement and second island reveal | 2 weeks                 |
| 4   | Farming and time                                                   | 4 weeks                 |
| 5   | Crafting, tool tiers and skills                                    | 4 weeks                 |
| 6   | Building and placement                                             | 3 weeks                 |
| 7   | Real island generation and biomes                                  | 4 to 6 weeks            |
| 8   | Save/load                                                          | 3 weeks                 |
| 9   | Polish and art pass                                                | 4 to 6 weeks            |

Total: roughly 8 to 10 months at 4 to 5 hours per week. Estimates are rough and assume programming experience but no prior Godot experience.

### 9.3 Risks and mitigations

- **Grid placement and bridges:** solve early with the hook spike, and use one grid system for everything.
- **Procedural islands:** start with simple noise-based shapes and hand-authored biome rules.
- **Saving generated worlds:** save only each island's seed plus the player's changes.
- **Art consistency:** follow the section 8 rules from day one.
- **Scope creep:** anything not on the Must list waits until version 1 is complete.

## 10. Technical setup

- **Engine:** latest stable Godot 4.x, .NET build. Stay on one version for the whole project and take only patch updates.
- **Language and editor:** C# with JetBrains Rider. Renderer: Forward+.
- **Version control:** Git with a private GitHub repo. Commit at the end of each work session and tag each milestone.

### Folder structure

```
res://
├── assets/          (models, textures, audio, fonts)
├── data/            (item, crop, recipe and biome resource files)
├── shaders/
├── autoload/        (global systems)
└── features/
    ├── player/
    ├── world/
    ├── islands/
    ├── farming/
    ├── inventory/
    ├── crafting/
    ├── building/
    └── ui/
```

### .gitignore

```
.godot/
*.translation
bin/
obj/
.vs/
.idea/
```

### C# conventions

- Scenes and assets: `snake_case` (`oak_tree.tscn`). Scripts: `PascalCase` matching the class (`Player.cs`).
- Classes, methods and properties: `PascalCase`. Private fields: `_camelCase`.
- Signals: `[Signal] delegate void ItemCollectedEventHandler(...)`, named in the past tense.
- Godot script classes must be `partial`. Custom data files are `Resource` classes marked `[GlobalClass]`.

### Architecture rules

1. Data-driven content: items, crops, recipes and biomes are resource files.
2. One grid system as the single source of truth for tile positions.
3. Signals and a few autoloads (game clock, event bus, save manager) instead of nodes reaching into each other.
4. Save the minimum: island seeds plus player changes.

### Work rhythm

Two or three sessions per week, each with one small finishable goal. End every session with the game running and a commit. Keep a short TODO list in the repo.

## 11. Post-v1 ideas

Design notes for later versions. None of these is required for the first playable version.

### Later-version roadmap

1. Forest biome, skill levels with small perks, rain, music and sound, juice effects.
2. Bench and flower pot, basic combat, extra crops.
3. Iron and titanium, Desert and Snowy, NPCs and story, currency, cooking, furniture crafting, seasons.

### Ideas to develop

- **Wandering tinkerer NPC:** shows up after the first bridge and offers tool upgrade blueprints.
- **Explorer NPCs on generated islands:** for example a hermit, a lost traveler, a merchant camp.
- **Friendship instead of XP** as the way NPC relationships grow.
- **Island personality:** points of interest such as an abandoned camp, a ruin or a shrine.
- **Collections journal or museum** for gathered items.
- **Expanded weather and time of day.**
- **Companion pet.**

## 12. Open decisions

- The game's name.
- Exact Godot version to pin.
- Which free asset pack to use as the main pack.
- Skill level thresholds per tool tier.
- How islands are generated in detail (noise shape, decoration density).
