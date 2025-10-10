// Variables of game
using System.ComponentModel;
using System.Linq.Expressions;

int chanceOfSuccessScavenge = 50;
int gameDifficulty = 1;
int totalTurn = 10;
int minProbabilityEventToWin = 55;
int maxHealth = 100, maxStamina = 100, maxSupply = 200, maxMorale = 100;
int minMoral = 25, turnToLeaveCrew = 3;
string userChoice;

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

string[,] shipStatus = { { "Space Ship Health", "100" }, { "Crew Morale", "100" }, { "Supply", "200" } };

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

string[] actionsMenu =
{
    $"1. Performing emergency repairs (+{rateOfIncreaseShipHealth} ship health, -10 Supply)",
    $"2. Medical Handling & Rest (+{rateOfIncreaseCrewMorale} Morale, +{rateOfIncreaseCrewHealth} Crew Health, +{rateOfIncreaseCrewStamina} Crew Stamina, -15 Supplies)",
    $"3. Search for resources ({chanceOfSuccessScavenge} chance of success)"
};

string[] firstMessages =
{
    "Initializing systems...",
    "Scanning star maps...",
    "Crew members awakening from cryosleep...",
    "Welcome aboard the starship *Eclipse*.",
    @"
      Mission Briefing:
      You are the commander of an exploration crew searching for habitable worlds.
      Manage your crew, survive cosmic events, and uncover ancient secrets.

      Tip: Each decision affects your fate.
    ",
    "The stars stretch endlessly before you.",
    "A new journey begins..."
};

string[] gameDifficultyMenu = { "1. Easy", "2. Normal", "3. Hard" };
string[] mainMenuOptions = {
    "1. Start Game",
    "2. Choose Difficulty",
    "3. Exit"
};

//---------------Main Loop---------------------

do
{
    Console.Clear();
    foreach (string option in mainMenuOptions)
    {
        Console.WriteLine(option);
    }
    Console.Write("Please Select (number): ");
    userChoice = Console.ReadLine();

    switch (userChoice)
    {
        case "1":
            StartGame();
            break;
        case "2":
            ChooseDifficulty();
            break;
        default:
            break;

    }

} while (userChoice != "3" && userChoice != null);

//---------------Start Game---------------------
void StartGame()
{
    GameInitialize();
    // DisplayFirstMessages();

    for (int turn = 1; turn <= totalTurn; turn++)
    {
        Console.Clear();
        Console.WriteLine($"==================================================\nYear {turn} out of {totalTurn} | Destination: Proxima Considers\n==================================================\n");

        DisplayShipStatus();
        DisplayCrewStatus();

        int eventNumber = DisplayAndChooseEvent();
        DisplayAndChooseCrewForEvent();
        // ResultOfDecisionForEvent(eventNumber, crewNumber);

        // CheckShipParameters();
        // CheckCrewParameters();

        // DisplayActionMenu();
        // ChooseAction();
        // DisplayResultOfAction();

        Console.WriteLine("> Press Any Key For Next Turn (Escape Key For Exit)");
        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
            Console.Clear();
            Console.WriteLine("You Loose :(");
            Console.WriteLine("> Press Any Key To Back To The Main Menu");
            Console.ReadLine();
            break;
        }
    }
}

//---------------Choice Difficulty---------------------
void ChooseDifficulty()
{
    int userInput;
    bool validInput;
    do
    {
        Console.Clear();
        foreach (string difficulty in gameDifficultyMenu)
            Console.WriteLine(difficulty);

        Console.Write("Please choice difficulty (number): ");
        validInput = int.TryParse(Console.ReadLine(), out userInput);

        if (!validInput)
            continue;


        if (userInput > 0 && userInput < 4)
            gameDifficulty = userInput;

    } while (userInput <= 0 || userInput >= 4);

}

//---------------Initializing Game---------------------
void GameInitialize()
{
    if (gameDifficulty == 2)
    {
        shipStatus[1, 1] = "50";
        shipStatus[2, 1] = "100";
    }
    else if (gameDifficulty == 3)
    {
        shipStatus[0, 1] = "50";
        shipStatus[1, 1] = "30";
        shipStatus[2, 1] = "50";
        for (int crew = 0; crew < crewsInformation.GetLength(0); crew++)
        {
            crewsInformation[crew, 1] = "50";
            crewsInformation[crew, 2] = "50";
        }
    }
}

//---------------Display First Messages---------------------
void DisplayFirstMessages()
{
    foreach (string message in firstMessages)
    {
        foreach (char ch in message)
        {
            Console.Write(ch);
            Thread.Sleep(50);
        }
        Console.WriteLine();
    }
    Console.WriteLine("> Press Any Key to begin your mission.");
    Console.ReadLine();
}

//---------------Display Ship Status---------------------
void DisplayShipStatus()
{
    Console.WriteLine("Ship Status: ");
    for (int status = 0; status < shipStatus.GetLength(0); status++)
    {
        Console.WriteLine($"- {shipStatus[status, 0]}: {shipStatus[status, 1]}");
    }
    Console.WriteLine();
}

//---------------Display Crew Status--------------------
void DisplayCrewStatus()
{
    Console.WriteLine("Crew Status: ");
    for (int status = 0; status < crewsInformation.GetLength(0); status++)
    {
        Console.WriteLine($"- {crewsInformation[status, 0]}:\n\tHealth: {crewsInformation[status, 1]}\n\tStamin: {crewsInformation[status, 2]}\n");
    }
}

//---------------Display And Choose Event--------------------
int DisplayAndChooseEvent()
{
    int cosmicEventNumber = rand.Next(0, cosmicEvents.GetLength(0) - 1);
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"Cosmic Event:\n\t- {cosmicEvents[cosmicEventNumber, 0]}\n");
    return cosmicEventNumber;
}

//---------------Display Event--------------------
void DisplayAndChooseCrewForEvent()
{
    Console.WriteLine("Who Should Lead This Mission?");
    for (int role = 0; role < crewsInformation.GetLength(0); role++)
    {
        Console.WriteLine($"\t{role + 1}) {crewsInformation[role, 0]}");
    }

    Console.WriteLine("Please Choose: ");

    //TODO: Choose Exist Crew To Lead Mission
    //TODO: Check Health And Stamina
}