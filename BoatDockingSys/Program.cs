using BoatDockingSys.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BoatDockingSys
{
    class Program
    {

        private static readonly Random random = new Random();
        private static Boat[][] harbor = new Boat[64][];
        private static int rejectedBoats = 0;
        private static bool hasReseted;
        
        static void Main(string[] args)
        {
            var hasLoaded = LoadHarborByFile();
            while (true)
            {
                if (!hasLoaded && !hasReseted)
                {
                    NewDay();
                }

                PrintHarbor();
                SaveHarborToFile();

                if (hasReseted)
                {
                    hasReseted = false;
                }

                var input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
                if (input.Key == ConsoleKey.R)
                {
                    hasReseted = ResetHarbor();
                }

                if (hasLoaded)
                {
                    hasLoaded = false;
                }
            }
        }

        static bool ResetHarbor()
        {
            harbor = new Boat[64][];
            rejectedBoats = 0;
            return true;
        }

        static void SaveHarborToFile()
        {
            var options = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };
            var jsonHarbor = JsonConvert.SerializeObject(harbor, options);
            var jsonRejectedBoats = JsonConvert.SerializeObject(rejectedBoats, options);
            File.WriteAllText("RejectedBoats.txt", jsonRejectedBoats);
            File.WriteAllText("SavedHarbor.txt", jsonHarbor);
        }

        static bool LoadHarborByFile()
        {
            if (File.Exists("SavedHarbor.txt"))
            {
                var options = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                };
                string jsonHarbor = File.ReadAllText("SavedHarbor.txt");
                Boat[][] loadedHarbor = JsonConvert.DeserializeObject<Boat[][]>(jsonHarbor, options);
                harbor = loadedHarbor;
                string jsonRejectedBoats = File.ReadAllText("RejectedBoats.txt");
                int loadedRejectedBoats = JsonConvert.DeserializeObject<int>(jsonRejectedBoats, options);
                rejectedBoats = loadedRejectedBoats;

                return true;
            }
            return false;
        }

        static void NewDay()
        {
            NextDay();
            DepartingBoats();

            for (int i = 0; i < 5; i++)
            {
                FindAvaibleSpot(CreateBoat());
            }
        }

        static Boat CreateBoat()
        {
            string boatId;
            int weight, maxSpeed, uniqueProperty;
            switch (random.Next(1, 4 + 1))
            {
                case 1:
                    uniqueProperty = random.Next(1, 6 + 1);
                    boatId = CreateBoatId("R-");
                    weight = random.Next(100, 300 + 1);
                    maxSpeed = random.Next(0, 3 + 1);
                    Boat rowBoat = new RowBoat(uniqueProperty, boatId, weight, maxSpeed);
                    return rowBoat;

                case 2:
                    uniqueProperty = random.Next(10, 1000 + 1);
                    boatId = CreateBoatId("M-");
                    weight = random.Next(200, 3000 + 1);
                    maxSpeed = random.Next(0, 60 + 1);
                    Boat motorBoat = new MotorBoat(uniqueProperty, boatId, weight, maxSpeed);
                    return motorBoat;

                case 3:
                    uniqueProperty = random.Next(10, 60 + 1);
                    boatId = CreateBoatId("S-");
                    weight = random.Next(800, 6000 + 1);
                    maxSpeed = random.Next(0, 12 + 1);
                    Boat sailBoat = new SailBoat(uniqueProperty, boatId, weight, maxSpeed);
                    return sailBoat;

                case 4:
                    uniqueProperty = random.Next(0, 500 + 1);
                    boatId = CreateBoatId("C-");
                    weight = random.Next(3000, 20000 + 1);
                    maxSpeed = random.Next(0, 20 + 1);
                    Boat cargoBoat = new CargoBoat(uniqueProperty, boatId, weight, maxSpeed);
                    return cargoBoat;

                default:
                    return null;
            }
        }

        static string CreateBoatId(string prefix)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id;
            do
            {
                var a = alphabet[random.Next(0, alphabet.Length)];
                var b = alphabet[random.Next(0, alphabet.Length)];
                var c = alphabet[random.Next(0, alphabet.Length)];
                id = prefix + a + b + c;
            } while (DoesIdExist(id));

            return id;
        }

        static bool DoesIdExist(string id)
        {
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] != null)
                {
                    if (harbor[i][0].Id == id)
                    {
                        return true;
                    }
                    else if (harbor[i][1] != null)
                    {
                        if (harbor[i][1].Id == id)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static void FindAvaibleSpot(Boat boat)
        {
            bool spotFound = false;
            string[] avaibleIndexes = new string[boat.Size];

            if (avaibleIndexes.Length > 3)
            {
                for (int i = harbor.Length - 1; i >= 3; i--)
                {
                    for (int j = 0; j < avaibleIndexes.Length; j++)
                    {
                        if ((i - j) >= 0)
                        {
                            if (harbor[i - j] == null)
                            {
                                avaibleIndexes[j] = (i - j).ToString();

                                if (avaibleIndexes[avaibleIndexes.Length - 1] != null)
                                {
                                    for (int k = 0; k < avaibleIndexes.Length; k++)
                                    {
                                        harbor[int.Parse(avaibleIndexes[k])] = new Boat[2] { boat, null };
                                    }
                                    spotFound = true;
                                    break;
                                }
                            }
                            else
                            {
                                avaibleIndexes.ToList().Clear();
                                i -= harbor[i - j][0].Size - 1;
                                break;
                            }
                        }
                    }
                    if (spotFound)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < harbor.Length; i++)
                {
                    for (int j = 0; j < avaibleIndexes.Length; j++)
                    {
                        if ((i + j) < harbor.Length)
                        {
                            if (harbor[i + j] == null)
                            {
                                avaibleIndexes[j] = (i + j).ToString();

                                if (avaibleIndexes[avaibleIndexes.Length - 1] != null)
                                {
                                    for (int k = 0; k < avaibleIndexes.Length; k++)
                                    {
                                        harbor[int.Parse(avaibleIndexes[k])] = new Boat[2] { boat, null };
                                    }
                                    spotFound = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (harbor[i + j][0] is RowBoat && boat is RowBoat && harbor[i + j][1] == null)
                                {
                                    harbor[i + j][1] = boat;
                                    spotFound = true;
                                    break;
                                }
                                else
                                {
                                    avaibleIndexes.ToList().Clear();
                                    i += harbor[i + j][0].Size - 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (spotFound)
                    {
                        break;
                    }
                }
            }
            if (!spotFound)
            {
                rejectedBoats++;
            }
        }

        static void PrintHarbor()
        {
            Console.Clear();
            List<string> printLines = new List<string>();
            for (int i = 0; i < harbor.Length; i++)
            {
                Boat boat = null;
                Boat secondarySlot = null;
                if (harbor[i] != null)
                {
                    boat = harbor[i][0];
                    secondarySlot = harbor[i][1];
                }

                if (boat == null)
                {
                    printLines.Add($"{i + 1}\tEmpty");
                }
                else
                {
                    printLines.Add($"{(boat.Size > 1 ? $"{i + 1}-{i + (boat.Size)}" : $"{i + 1}")}\t" +
                        $"{(boat.GetType().Name.Length < 8 ? $"{boat.GetType().Name}\t\t" : $"{ boat.GetType().Name}\t")}" +
                        $"{boat.Id}\t{boat.Weight}\t{(KnotToKmh(boat.MaxSpeed) > 100 ? $"{KnotToKmh(boat.MaxSpeed)} km/h\t" : $"{KnotToKmh(boat.MaxSpeed)} km/h\t\t")}{boat.GetUniqueProp()}");

                    if (secondarySlot != null)
                    {
                        printLines.Add($"{i + 1}\t{secondarySlot.GetType().Name}\t\t{secondarySlot.Id}\t" +
                            $"{secondarySlot.Weight}\t{KnotToKmh(secondarySlot.MaxSpeed)} km/h\t\t{secondarySlot.GetUniqueProp()}");
                    }

                    i += boat.Size - 1;
                }
            }
            Console.WriteLine("Place\tType\t\tId\tWeigth\tTop speed\tMisc\n");
            foreach (string line in printLines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("\nEmpty\tRejected\tWeight\t\tAvg Speed\tRowBoats\tMotorBoats\tSailBoats\tCargoBoats");
            Console.WriteLine($"{CountEmptySpots()}\t{rejectedBoats}\t\t{CalcTotalWeight()}\t\t{AverageMaxSpeed()}\t\t" +
                $"{CountBoats<RowBoat>()}\t\t{CountBoats<MotorBoat>()}\t\t{CountBoats<SailBoat>()}\t\t{CountBoats<CargoBoat>()}");
        }

        static int CountBoats<T>()
        {
            int numBoats = 0;
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] != null && harbor[i][0] is T)
                {
                    numBoats++;
                    if (harbor[i][1] != null && harbor[i][1] is T)
                    {
                        numBoats++;
                    }
                    i += harbor[i][0].Size - 1;
                }
            }
            return numBoats;
        }

        static int AverageMaxSpeed()
        {
            int totalSpeed = 0, numBoats = 0;
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] != null)
                {
                    totalSpeed += harbor[i][0].MaxSpeed;
                    numBoats++;
                    if (harbor[i][1] != null)
                    {
                        totalSpeed += harbor[i][1].MaxSpeed;
                        numBoats++;
                    }
                    i += harbor[i][0].Size - 1;
                }
            }
            if (numBoats > 0)
            {
                return totalSpeed / numBoats;
            }
            else
            {
                return 0;
            }
            
        }

        static int CalcTotalWeight()
        {
            int totalWeight = 0;
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] != null)
                {
                    totalWeight += harbor[i][0].Weight;
                    if (harbor[i][1] != null)
                    {
                        totalWeight += harbor[i][1].Weight;
                    }
                    i += harbor[i][0].Size - 1;
                }
            }
            return totalWeight;
        }

        static int CountEmptySpots()
        {
            int emptySpots = 0;
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] == null)
                {
                    emptySpots++;
                }
            }
            return emptySpots;
        }

        static int KnotToKmh(int knot)
        {
            return (int)(knot * 1.85200);
        }

        static void NextDay()
        {
            for (int i = 0; i < harbor.Length; i++)
            {
                if (harbor[i] != null)
                {
                    harbor[i][0].DaysInPort--;
                    if (harbor[i][1] != null)
                    {
                        harbor[i][1].DaysInPort--;
                    }
                    i += harbor[i][0].Size - 1;
                }
            }
        }

        static void DepartingBoats()
        {
            for (int i = 0; i < harbor.Length; i++)
            {
                if ((harbor[i] != null && harbor[i][0].DaysInPort == 0 && harbor[i][1] == null) ||
                    (harbor[i] != null && harbor[i][0].DaysInPort == 0 && harbor[i][1] != null &&
                    harbor[i][1].DaysInPort == 0))
                {
                    harbor[i] = null;
                }
            }
        }
    }
}
