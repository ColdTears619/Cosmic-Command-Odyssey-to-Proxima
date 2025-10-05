// Variables of game
int chanceOfSuccessScavenge = 50;
int gameDifficulty = 1;
int gameTurn = 10;
int minProbabilityEventToWin = 55;
int maxHealth = 100, maxStamina = 100, maxSupply = 200, maxMorale = 100;

decimal rateOfIncreaseCrewHealth = 15;
decimal rateOfIncreaseShipHealth = 15;
decimal rateOfIncreaseCrewStamina = 20;
decimal rateOfIncreaseCrewMorale = 5;

bool gameOver = false;

Random rand = new Random();

// Arrays of mini game
//* First Column: Crew Role, Second Column: Health, Third Column: Stamina
string[,] crewsInformation = { { "Pilot", "100", "100" }, { "Scientist", "100", "100" }, { "Engineer", "100", "100" }, { "Medic", "100", "100" }, { "Security Officer", "100", "100" } };

//* First Column: Event name, Second Column: Success Message, Third Column: Fail Message
string[,] cosmicEvents =
{
    {
        "A radiant storm of meteors fills the sky! Some carry rare minerals… but others are deadly. Will you try to collect them?",
        "You navigate through the fiery rain and collect a glowing shard of starlight! Your courage is rewarded.",
        "A meteor grazes your ship — your shields flicker, and you barely survive the blaze."
    },
    {
        "You enter a beautiful nebula where strange whispers echo through the void. They offer guidance… or deceit. Will you listen?",
        "The whispers reveal a hidden path through space — your intuition saves you from danger.",
        "The whispers twist into screams — your mind reels as you lose direction in the fog."
    },
    {
        "A nearby black hole pulls everything with its immense gravity. You must adjust your engines fast to avoid being trapped!",
        "You fire the boosters at just the right moment and slingshot around the black hole — gaining a burst of speed!",
        "Too slow! The gravitational force crushes your ship’s hull as you spiral into the abyss."
    },
    {
        "An ancient energy awakens during the eclipse. The cosmos invites you to align your ship’s systems in perfect harmony.",
        "The ritual completes — your ship glows with cosmic light, restoring energy and granting newfound focus.",
        "You misalign the sequence — the cosmic energy backfires, draining your power cells."
    },
    {
        "A mysterious cosmic virus spreads among your crew after passing through a strange energy field. As the ship’s Medic, you must act fast to contain it!",
        "Your swift diagnosis and creative treatment stabilize the crew. The cosmic illness fades, and morale soars — you’re the ship’s hero.",
        "Your antidote misfires — several crew members fall into stasis, and the ship’s medical bay becomes a quarantine zone."
    },
    {
        "Sensors detect an unidentified lifeform aboard the ship — invisible to scanners, but leaving traces of dark matter behind. Will you confront it?",
        "You track the intruder through the shadows, isolate it in a containment field, and restore order to the ship.",
        "The creature vanishes before you can react — systems flicker, and a haunting presence lingers in the corridors."
    }
};

string[,] playerStatus = { { "Space Ship Health", "100" }, { "Crew Morale", "50" }, { "Supply", "150" } };

//* first Column: Event index, Second Column: Crew, Third Column: Chance To Success
int[,] relatedMatrix =
{
    {0, 2, 50},
    {1, 0, 50},
    {2, 0, 50},
    {3, 1, 50},
    {4, 3, 50},
    {5, 4, 50},
};
string[] chooseRole =
{
    "1. Pilot",
    "2. Scientist",
    "3. Engineer",
    "4. Medic",
    "5. Security Officer"
};

// TODO: Replace increase health, morale param with variable
string[] actionsMenu =
{
    $"1. Performing emergency repairs (+{rateOfIncreaseShipHealth} ship health, -10 Supply)",
    $"2. Medical Handling & Rest (+{rateOfIncreaseCrewMorale} Morale, +{rateOfIncreaseCrewMorale} Crew Health, +{rateOfIncreaseCrewStamina} Crew Stamina, -15 Supplies)",
    $"3. Search for resources ({chanceOfSuccessScavenge} chance of success)"
};

string[] gameDifficultyMenu = { "1. Easy", "2. Normal", "3. Hard" };
string[] mainMenuOptions = {
    "1. Start Game",
    "2. Choose Difficulty",
    "3. Exit"
};

//-----------------------------------------

