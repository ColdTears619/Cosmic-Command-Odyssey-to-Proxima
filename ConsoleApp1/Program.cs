//--------------------------------Variables of game--------------------------------
using System.ComponentModel;
using System.Linq.Expressions;

int chanceOfSuccessScavenge = 50;
int gameDifficulty = 1;
int totalTurn = 10;
int minProbabilityEventToWin = 55;
int maxHealth = 100, maxStamina = 100, maxSupply = 200, maxMorale = 100;
int minMoral = 25, turnToLeaveCrew = 3;
string userChoice;

int rateOfIncreaseCrewHealth = 10;
int rateOfIncreaseShipHealth = 10;
int rateOfIncreaseCrewStamina = 15;
int rateOfIncreaseCrewMorale = 5;
int rateOfIncreaseSupply = 25;

int rateOfDecreaseSupply = 20;
int rateOfDecreaseHealth = 5;
int rateOfDecreaseStamina = 20;


bool gameOver = false;

Random rand = new Random();

//--------------------------------Arrays of mini game---------------------------------
//* First Column: Crew Role, Second Column: Health, Third Column: Stamina
string[,] crewsInformation = { { "Pilot", "100", "100" }, { "Scientist", "100", "100" }, { "Engineer", "100", "50" }, { "Medic", "100", "100" }, { "Security Officer", "100", "100" } };

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
    {0, 2, 35},
    {1, 0, 35},
    {2, 0, 35},
    {3, 1, 35},
    {4, 3, 35},
    {5, 4, 35},
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
    $"1. Performing emergency repairs (+{rateOfIncreaseShipHealth} ship health, -{rateOfDecreaseSupply + 5} Supply)",
    $"2. Medical Handling & Rest (+{rateOfIncreaseCrewMorale} Morale, +{rateOfIncreaseCrewHealth} Crew Health, +{rateOfIncreaseCrewStamina} Crew Stamina, -{rateOfDecreaseSupply} Supplies)",
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

//---------------------Main Loop------------------------

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
        gameOver = CheckGameStatus();
        if (gameOver)
            break;

        Console.Clear();
        Console.WriteLine($"==================================================\nYear {turn} out of {totalTurn} | Destination: Proxima Considers\n==================================================\n");

        DisplayShipStatus();
        DisplayCrewStatus();

        int chosenEvent = DisplayAndChooseEvent();
        int chosenCrew = DisplayAndChooseCrewForEvent();

        Console.WriteLine($"chosen event is: {chosenEvent}\nchosen crew is: {chosenCrew}");
        ResultOfDecisionForEvent(chosenEvent, chosenCrew);

        // TODO: Display action menu and choose for action
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

    if (gameOver)
    {
        Console.WriteLine("\n\t\tGame Over — The cosmos claims another soul, yet the universe awaits your next attempt\n\nPlease Press Any Key To Back To Main Menu");
        Console.ReadLine();
    }
    else
    {
        Console.Clear();
        Console.WriteLine("\n\t\tVictory — The cosmos bends to your will, and the stars honor your triumph. Now rest, Commander… new challenges await beyond the horizon.\n\nPlease Press Any Key To Back To Main Menu");
        Console.ReadLine();
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

//---------------Display Crews And Choose Crew For Event--------------------
int DisplayAndChooseCrewForEvent()
{
    Console.WriteLine("Who Should Lead This Mission?");
    for (int role = 0; role < crewsInformation.GetLength(0); role++)
    {
        if (Convert.ToInt32(crewsInformation[role, 1]) > 0)
        {
            Console.WriteLine($"\t{role + 1}) {crewsInformation[role, 0]}");
        }
    }

    int userInput;
    //* Health And Stamina Of Crew
    bool[] crewCondition = [false, false];
    do
    {
        Console.Write("Please Choose Role (number): ");
        bool validInput = int.TryParse(Console.ReadLine(), out userInput);

        if (!validInput)
            continue;

        if (userInput > 0 && userInput < 6)
        {
            CheckCrewStatus(userInput - 1, ref crewCondition);
            if (crewCondition[0] == false) Console.WriteLine("Your chosen crew member has fallen. Their light fades among the stars...");
            if (crewCondition[1] == false) Console.WriteLine("Your crew is exhausted. Their movements slow, their focus drifts — they need rest.");
        }
    } while ((userInput <= 0 || userInput >= 5) && (crewCondition[0] == false || crewCondition[1] == false));

    return userInput - 1;
}

//---------------Check Crew Parameters--------------------
void CheckCrewStatus(int userInput, ref bool[] crewCondition)
{
    //* Check Health and Stamina
    if (Convert.ToInt32(crewsInformation[userInput, 1]) >= 10) crewCondition[0] = true;
    if (Convert.ToInt32(crewsInformation[userInput, 2]) >= 10) crewCondition[1] = true;
}

//---------------Calculate Success Of Event--------------------
void ResultOfDecisionForEvent(int chosenEvent, int chosenCrew)
{
    int chance = rand.Next(1, 60);
    Console.WriteLine($"Dice Role: {chance}. Chance to success: {minProbabilityEventToWin}.");

    int correctChosenRole = 10;
    if (relatedMatrix[chosenEvent, 1] == chosenCrew)
        correctChosenRole = relatedMatrix[chosenEvent, 2];

    if (correctChosenRole > 10)
        Console.WriteLine("You choose correct role for lead this event!\n");


    //? Think about reward and penalty methods
    if ((chance + correctChosenRole) >= minProbabilityEventToWin)
    {
        Console.WriteLine(cosmicEvents[chosenEvent, 1]);
        Console.WriteLine("\nEvent Passed: Crew morale and supplies have increased. Keep up the momentum, Commander.");
        Penalty(2, chosenCrew);
        reward();
    }
    else
    {
        Console.WriteLine(cosmicEvents[chosenEvent, 2]);
        Console.WriteLine("\nEvent Failed: Crew health and stamina reduced, morale drops, ship sustains damage, and supplies are lost. Recover quickly, Commander.");

        Penalty();
    }
}

//---------------Check Win OR Loose--------------------
bool CheckGameStatus()
{
    if (shipStatus[0, 1] == "0" || shipStatus[1, 1] == "0" || shipStatus[2, 1] == "0")
        return true;
    else
    {
        int deadCrew = 0;
        for (int crew = 0; crew < crewsInformation.GetLength(0); crew++)
        {
            if (crewsInformation[crew, 1] == "0")
                deadCrew++;
        }
        return deadCrew == crewsInformation.GetLength(0) ? true : false;
    }
}

//---------------Give Reward--------------------
//* 1: Increase crew health, stamina and morale but decrease supply
//* 2: Increase morale and supply after event successfully passed
//* 3: Increase supply after scavenge
//* 4: Increase ship health and reduce supply
void reward(int actionChoose = 2)
{
    ChangeRateParamsByRoles();
    switch (actionChoose)
    {
        case 1:
            //* Increase health and stamina and decrease supply

            if (Convert.ToInt32(shipStatus[2, 1]) < 19)
            {
                Console.WriteLine("\nSupplies Critical: Crew health and stamina cannot recover — provisions exhausted. Scavenge for supplies, Commander.");
                break;
            }
            for (int crew = 0; crew < crewsInformation.GetLength(0); crew++)
            {
                if (crewsInformation[crew, 1] == "0")
                    continue;

                IncreaseCrewParams(crew);
            }

            ReduceSupply(rateOfDecreaseSupply);
            IncreaseMorale();
            break;
        case 2:
            //* Increase supply and morale
            IncreaseMorale();
            IncreaseSupplyShip();
            break;
        case 3:
            //* Increase supply
            IncreaseSupplyShip();
            break;
        case 4:
            //* Increase ship health and reduce supply
            if (Convert.ToInt32(shipStatus[2, 1]) < 24)
            {
                Console.WriteLine("\nSupplies Critical: Ship repairs impossible — resources too low. Recommend scavenging for supplies next turn, Commander.");
                break;
            }

            ReduceSupply(rateOfDecreaseSupply - 5);
            IncreaseShipHealth();
            break;
        default:
            break;
    }
}

//---------------Increase Crew Params Method--------------------
void IncreaseCrewParams(int crewNumber)
{
    decimal crewHealth = Convert.ToInt32(crewsInformation[crewNumber, 1]) + rateOfIncreaseCrewHealth;
    decimal crewStamina = Convert.ToInt32(crewsInformation[crewNumber, 2]) + rateOfIncreaseCrewStamina;
    crewsInformation[crewNumber, 1] = crewHealth > maxHealth ? "100" : Convert.ToString(crewHealth);
    crewsInformation[crewNumber, 2] = crewStamina > maxStamina ? "100" : Convert.ToString(crewStamina);
}

//---------------Increase Crew Params Method--------------------
//* Effect roles on rate parameters
//TODO: Remove messages and transfer to new methods which that check health and display messages for recovery
void ChangeRateParamsByRoles()
{
    if (crewsInformation[3, 1] != "0")
    {
        Console.WriteLine("\nMedic Assigned: Crew health and stamina recovery are increased by 50%. Stay steady, Commander.");
        rateOfIncreaseCrewStamina += 5;
        rateOfIncreaseCrewHealth += 5;
    }
    else
    {
        Console.WriteLine("\nNo Medic Assigned: Crew health and stamina recovery are slower than normal. Stay vigilant, Commander.");
        rateOfIncreaseCrewStamina = 15;
        rateOfIncreaseCrewHealth = 10;
    }
    if (crewsInformation[4, 1] != "0")
    {
        Console.WriteLine("\nSecurity Officer Assigned: Crew morale increased by 50%. Discipline and focus hold steady, Commander.");
        rateOfIncreaseCrewMorale += 5;
    }
    else
    {
        Console.WriteLine("\nNo Medic Assigned: Crew health and stamina recovery are slower than normal. Stay vigilant, Commander.");
        rateOfIncreaseCrewMorale = 5;
    }
    if (crewsInformation[2, 1] != "0")
    {
        Console.WriteLine("\nEngineer Assigned: Ship health recovery and system repairs are increased by 50%. Keep the engines running, Commander.");
        rateOfIncreaseShipHealth += 5;
    }
    else
    {
        Console.WriteLine("\nNo Engineer Assigned: Ship repairs are slower, and system recovery is reduced. Monitor the hull carefully, Commander.");
        rateOfIncreaseShipHealth = 10;
    }
}
//---------------Increase Ship Params Method--------------------
void IncreaseSupplyShip()
{
    decimal supply = Convert.ToInt32(shipStatus[2, 1]) + rateOfIncreaseSupply;
    shipStatus[2, 1] = supply > maxSupply ? "200" : Convert.ToString(supply);
}

void IncreaseMorale()
{
    decimal morale = Convert.ToInt32(shipStatus[1, 1]) + rateOfIncreaseCrewMorale;
    shipStatus[1, 1] = morale > maxMorale ? "100" : Convert.ToString(morale);
}

void IncreaseShipHealth()
{
    decimal shipHealth = Convert.ToInt32(shipStatus[0, 1]) + rateOfIncreaseShipHealth;
    shipStatus[0, 1] = shipHealth > maxHealth ? "100" : Convert.ToString(shipHealth);
}

//---------------Give Penalty--------------------
//* 1: Penalty after event failed
//* 2: Reduce Specific crew
void Penalty(int reduceMode = 1, int crewNumber = -1)
{
    //TODO: Add switch case for reduce all parameters
    switch (reduceMode)
    {
        case 1:
            ReduceShipHealth(10);
            ReduceSupply(rand.Next(10, 30));
            ReduceCrewMorale(rand.Next(10, 15));
            ReduceCrewParams(crewNumber);
            break;
        case 2:
            ReduceSupply(crewNumber);
            break;
        default:
            break;
    }
}

//---------------Decrease Ship Supply Method--------------------
void ReduceSupply(int decreaseSupplyParam = 0)
{
    int decreaseSupply = decreaseSupplyParam != 0 ? decreaseSupplyParam : rateOfDecreaseSupply;
    decimal supply = Convert.ToInt32(shipStatus[2, 1]) - decreaseSupply;
    shipStatus[2, 1] = Convert.ToString(supply);
}

//---------------Decrease Ship Health Method--------------------
void ReduceShipHealth(int decreaseHealthParam = 0)
{
    decimal health = Convert.ToInt32(shipStatus[0, 1]) - decreaseHealthParam;
    shipStatus[0, 1] = Convert.ToString(health);
}

//---------------Decrease Morale Method--------------------
void ReduceCrewMorale(int decreaseMoraleParam = 0)
{
    decimal morale = Convert.ToInt32(shipStatus[1, 1]) - decreaseMoraleParam;
    shipStatus[1, 1] = Convert.ToString(morale);
}

//---------------Decrease Crew Params Method--------------------
void ReduceCrewParams(int crewNumber)
{
    if (crewNumber != -1)
    {
        decimal crewHealth = Convert.ToInt32(crewsInformation[crewNumber, 1]) - rateOfDecreaseHealth;
        decimal crewStamina = Convert.ToInt32(crewsInformation[crewNumber, 2]) - rateOfDecreaseStamina;
        crewsInformation[crewNumber, 1] = crewHealth <= 0 ? "0" : Convert.ToString(crewHealth);
        crewsInformation[crewNumber, 2] = crewStamina <= 0 ? "0" : Convert.ToString(crewStamina);
    }
    else
    {
        for (int crew = 0; crew < crewsInformation.GetLength(0); crew++)
        {
            decimal crewHealth = Convert.ToInt32(crewsInformation[crew, 1]) - (rateOfDecreaseHealth + rand.Next(1, 10));
            decimal crewStamina = Convert.ToInt32(crewsInformation[crew, 2]) - (rateOfDecreaseStamina + rand.Next(1, 10));
            crewsInformation[crew, 1] = crewHealth <= 0 ? "0" : Convert.ToString(crewHealth);
            crewsInformation[crew, 2] = crewStamina <= 0 ? "0" : Convert.ToString(crewStamina);
        }
    }
}