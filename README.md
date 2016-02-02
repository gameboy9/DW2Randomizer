# Dragon Warrior 2 Randomizer
## Features
- An opportunity to adjust some enemy attributes to DW2 Remix values. **
- An opportunity to half the gold and experience requirements to reduce grinding.
- An opportunity to increase XP by 50% for all monsters EXCEPT metal slimes and metal babbles.  (due to the ease of killing them)
- Four Levels Of Randomization (with an option to not randomize at all)
- An opportunity to adjust the seed from 1 to 2^32 (2 billion plus) for racing possibilities.
- Super speedy battles!  (instead of waiting 2/3 of a second between battle statements, it is almost instantaneous)
- Prologue automatically skipped.  The game now starts, albeit a bit awkwardly, with the soldier limping into Midenhall's Throne Room.

** If this is done, Army Ant's XP/GP will increase to 6XP/7GP so they are more in line with other enemies in its' class.  Also, Metal Babbles and Metal Slime's attributes will change so they have a very high chance of running, in line with other Dragon Warrior games.

### For the first three levels (Slight, Moderate, Heavy)
- Treasures scrambled inside a particular "zone" in moderate difficulty, scrambled all over the world in heavy difficulty.  (but key items will stay in places before they are required)
- Weapon and item stores are scrambled similarly.
- Monster zones are adjusted by two, four, or eight "levels" in either direction.
- Boss fights are adjusted similarly.  There is an opportunity for other enemies to join in boss fights that normally wouldn't join.
- Starting statistics can be adjusted by up to 3/6/12 HP/MP and 1/3/6 Strength/Agility.
- Each level up can be adjusted by up to 1/3/6 points for all statistics.
- Spells are adjusted by 1/3/6 levels, but are then rearranged so they are learned in order. (I currently don't know how to scramble the spells around without introducing UI confusion)

### Insane difficulty
- All monsters are completely randomized, except for their HP, Defense, Attack Power, XP, and their gold. (in attack power's case, it is randomized from half to twice their original attack power)
- Monsters resistances are randomized as well, but higher level monsters have a higher chance of higher resistances.
- Monster zones are completely random, but they will be easier until the Moonbrooke shrine, and will get progressively harder starting at the Sea Cave.
- Boss fights are completely randomized as well, but the ship fight is easier.
- Weapon and item stores are completely randomized.  You theoretically could buy items such as Water Flying Clothes, Thunder Swords, Leaves of The World Tree, Golden Keys, Cloaks Of Wind, and so forth.  Crests and the Mirror Of Ra, as examples, still have to be found.
- Weapon and armor power are completely randomized from 1 to 100 for weapons, 70 for armor, 40 for shields, and 30 for helmets.
- Weapon and armor prices are adjusted according to the power randomized.  (power ^ 2.3, 2.4, 2.8, 3.0 for weapons, armor, shields, and helmets respectively)
- Who equips each weapon, armor, shield, and helmet is also randomized.
- The only exceptions are the bamboo stick and clothes (maximum power 10 for each), because all members will start with those items, and they all can equip those items.
- Spells are learned randomly, but progressively.  (see above for explanation)
- Treasures are completely randomized, but you will find all key items before they are required either in the treasures to be found, or possibly weapon and item stores.
- Starting gold is randomized from 0-255.  It is possible to start with 0 gold, especially if you start the game with a crest.
- The house of healing's revive cost is randomized from 1-20GP/level.
- The inns are randomized as well from 1-20GP/person.
- All stats are completely randomized, but overflows will be prevented.

## To Do/Wishlist
- Randomize spell learning better in insane randomness.
- Figure out a way to revive all party members with full HP and MP on a party wipe(AKA ColdAsACod), regardless of save location. (this only takes effect at the Rhone Shrine)
- Figure out a way to adjust the interface so magic can be randomized effectively without UI confusion.

## Special Thanks
- Mcgrew, for his Dragon Warrior 1 Randomizer(https://github.com/mcgrew/dwrandomizer), giving me the inspiration to create this randomizer.  Twitch:  http://www.twitch.tv/mcgrew
- joe73ffdq, for his help in finding ROM addresses to make this possible.
- Highspirits, for being the first to give this a try... and the free advertising.  :)  Twitch:  http://www.twitch.tv/highspirits
- Evilash25, for also testing out the randomizer and finding some bugs that I needed to squash.  Twitch:  http://www.twitch.tv/evilash25
