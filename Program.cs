
namespace ProjectProposal1_PetAdoption
{
    // -------------------- STAFF CLASS --------------------
    public class Staff
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Birthday { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }

        public Staff(string fullName, string username, string birthday, string contactNumber, string password)
        {
            FullName = fullName;
            Username = username;
            Birthday = birthday;
            ContactNumber = contactNumber;
            Password = password;
        }

        public string ToFileLine() => $"{FullName}|{Username}|{Birthday}|{ContactNumber}|{Password}";

        public static Staff FromFileLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length == 5)
                return new Staff(parts[0], parts[1], parts[2], parts[3], parts[4]);
            return null;
        }
    }

    // -------------------- FILE MANAGER CLASS --------------------
    public class FileManager
    {
        protected static string mainFolder = @"C:\Project Proposal\PET ADOPTION RECORD SYSTEM FOLDERS";
        protected static string recordFolder = @"C:\Project Proposal\PET ADOPTION RECORD SYSTEM FOLDERS\Records";
        protected static string logFolder = @"C:\Project Proposal\PET ADOPTION RECORD SYSTEM FOLDERS\Log Records";
        protected static string staffFile = @"C:\Project Proposal\PET ADOPTION RECORD SYSTEM FOLDERS\StaffList.txt";

        protected FileManager()
        {
            try
            {
                Directory.CreateDirectory(mainFolder);
                Directory.CreateDirectory(recordFolder);
                Directory.CreateDirectory(logFolder);
            }
            catch (Exception ex)
            {
                // Initialization failure should be visible to operator
                Console.WriteLine($"Error initializing application folders: {ex.Message}");
            }
        }

        protected string MakeRecordFileName(string adopter, string pet)
        {
            string safeAdopter = adopter.Replace(" ", "_");
            string safePet = pet.Replace(" ", "_");
            return Path.Combine(recordFolder, safeAdopter + "_" + safePet + ".txt");
        }

        protected void LogAction(string username, string message)
        {
            string logFileName = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string logFilePath = Path.Combine(logFolder, logFileName);
            string logEntry = $"[{DateTime.Now}] ({username}) {message}";
            try
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // If logging fails, do not crash the app; show minimal info
                Console.WriteLine($"Warning: failed to write log entry: {ex.Message}");
            }
        }

        // -------------------- Staff persistence (shared) --------------------
        // Load staff list from disk. Returns empty list if file missing or on error.
        protected List<Staff> LoadStaffList()
        {
            var list = new List<Staff>();
            try
            {
                if (!File.Exists(staffFile)) return list;
                foreach (var line in File.ReadAllLines(staffFile))
                {
                    var s = Staff.FromFileLine(line);
                    if (s != null) list.Add(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading staff list: {ex.Message}");
            }
            return list;
        }

        // Save provided staff list to disk.
        protected void SaveStaffList(List<Staff> list)
        {
            try
            {
                var lines = list.Select(s => s.ToFileLine()).ToArray();
                File.WriteAllLines(staffFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving staff list: {ex.Message}");
            }
        }


        // -------------------- Helper methods for tabular output --------------------
        protected static void PrintRecordsTable(List<(string Adopter, string Pet, string Type, string Date)> rows,
                                                int capAdopter = 30, int capPet = 20, int capType = 15, int capDate = 25)
        {
            int wAdopter = Math.Min(capAdopter, Math.Max("Adopter".Length, MaxLength(rows, r => r.Adopter)));
            int wPet     = Math.Min(capPet,     Math.Max("Pet".Length,     MaxLength(rows, r => r.Pet)));
            int wType    = Math.Min(capType,    Math.Max("Type".Length,    MaxLength(rows, r => r.Type)));
            int wDate    = Math.Min(capDate,    Math.Max("Date Adopted".Length, MaxLength(rows, r => r.Date)));

            string format = $"| {{0,-{wAdopter}}} | {'{1,-{wPet}}} | {{2,-{wType}}} | {{3,-{wDate}}} |";
            int totalWidth = wAdopter + wPet + wType + wDate + (3 * 3) + 6;

            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(format, "Adopter", "Pet", "Type", "Date Adopted");
            Console.WriteLine(new string('-', totalWidth));

            foreach (var r in rows)
            {
                Console.WriteLine(format,
                    Truncate(r.Adopter, wAdopter),
                    Truncate(r.Pet, wPet),
                    Truncate(r.Type, wType),
                    Truncate(FormatDate(r.Date), wDate));
            }

            Console.WriteLine(new string('-', totalWidth));
        }

        protected static int MaxLength(List<(string Adopter, string Pet, string Type, string Date)> rows, Func<(string Adopter, string Pet, string Type, string Date), string> selector)
        {
            int max = 0;
            foreach (var item in rows)
            {
                var s = selector(item) ?? "";
                if (s.Length > max) max = s.Length;
            }
            return max;
        }

        protected static string Truncate(string s, int max)
        {
            if (s == null) return "";
            if (s.Length <= max) return s;
            if (max <= 3) return s.Substring(0, max);
            return s.Substring(0, max - 3) + "...";
        }

        // Simplified, culture-neutral date formatting
        protected static string FormatDate(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            if (DateTime.TryParse(raw, out DateTime dt))
                return dt.ToString("MM/dd/yyyy h:mm tt");
            return raw;
        }

        // Simple title-case helper (culture-insensitive) to replace TextInfo.ToTitleCase usage
        protected static string ToTitleCaseSimple(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input ?? "";
            var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                var p = parts[i];
                if (p.Length == 0) continue;
                parts[i] = char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1).ToLowerInvariant() : "");
            }
            return string.Join(" ", parts);
        }
    }

    // -------------------- PET RECORD CLASS --------------------
    public class PetRecord
    {
        public string AdopterName { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public DateTime AdoptionDate { get; private set; }

        public PetRecord(string adopter, string pet, string type)
        {
            AdopterName = adopter;
            PetName = pet;
            PetType = type;
            AdoptionDate = DateTime.Now;
        }

        public string ToRecordText() =>
            $"Adopter: {AdopterName}\nPet: {PetName}\nType: {PetType}\nDate Adopted: {AdoptionDate}";
    }

    // -------------------- MAIN PET ADOPTION SYSTEM --------------------
    public class PetAdoptionSystem : FileManager
    {
        private List<Staff> staffList = new List<Staff>();
        private const string MANAGER_USERNAME = "manager";
        private const string MANAGER_PASSWORD = "admin123";

        public PetAdoptionSystem() : base() { staffList = LoadStaffList(); }

        // -------------------- LOGIN METHOD --------------------
        public void Login()
        {
            bool wrongAttempt = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║     PET ADOPTION SYSTEM LOGIN    ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.Write("Username: ");
                string user = Console.ReadLine()?.Trim();
                Console.Write("Password: ");
                string pass = Console.ReadLine()?.Trim();


                if (user == MANAGER_USERNAME && pass == MANAGER_PASSWORD)
                {
                    ManagerMenu();
                    return;
                }

                var staff = staffList.Find(s => s.Username == user && s.Password == pass);
                if (staff != null)
                {
                    StaffMenu(staff);
                    return;
                }

                // Wrong credentials
                Console.WriteLine("\nInvalid username or password!");
                if (wrongAttempt)
                {
                    Console.WriteLine("Type 'forgot' to reset password or press Enter to retry:");
                    string choice = Console.ReadLine()?.Trim().ToLower();
                    if (choice == "forgot")
                    {
                        ResetStaffPassword();
                        continue;
                    }
                }
                else
                {
                    wrongAttempt = true;
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey(true);
                }
            }
        }

        // -------------------- MANAGER MENU --------------------
        private void ManagerMenu()
        {
            ManagerActions manager = new ManagerActions(staffList);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════╗");
                Console.WriteLine("║          MANAGER MENU             ║");
                Console.WriteLine("╚═══════════════════════════════════╝");
                Console.WriteLine("║ 1. View All Adoption Records      ║");
                Console.WriteLine("║ 2. View Summary                   ║");
                Console.WriteLine("║ 3. View Logs                      ║");
                Console.WriteLine("║ 4. Manage Staff Accounts          ║");
                Console.WriteLine("║ 5. Logout                         ║  ");
                Console.WriteLine("╚═══════════════════════════════════╝\n");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        manager.ViewRecords(); // Already exists
                        Console.Clear();
                        break;
                    case "2":
                        ViewSummary(); // New method for summary
                        Console.Clear();
                        break;
                    case "3":
                        manager.ViewLogs(); // New method to show logs
                        break;
                    case "4":
                        manager.ManageStaffAccounts();
                        break;
                    case "5":
                        Console.Clear(); Console.WriteLine("Logging out...... THANK YOU FOR USING THE SYSYEM!");
                        return; // Exit manager menu
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey(true);

                        break;
                }
            }
        }


        // -------------------- STAFF MENU --------------------
        private void StaffMenu(Staff staff)
        {
            var staffActions = new StaffActions(staff);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════════════╗");
                Console.WriteLine($"        STAFF MENU ({staff.FullName})         ");
                Console.WriteLine("╚═══════════════════════════════════════════════╝");
                Console.WriteLine("║ 1. Add Adoption Record                        ║");
                Console.WriteLine("║ 2. View All Records                           ║");
                Console.WriteLine("║ 3. Search Record                              ║");
                Console.WriteLine("║ 4. Modify Record                              ║");
                Console.WriteLine("║ 5. Delete Record                              ║");
                Console.WriteLine("║ 6. View Summary                               ║");
                Console.WriteLine("║ 7. Logout                                     ║");
                Console.WriteLine("╚═══════════════════════════════════════════════╝\n");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": staffActions.AddRecord(); break;
                    case "2": staffActions.ViewRecords(); Console.Clear(); break;
                    case "3": staffActions.SearchRecord(); break;
                    case "4": staffActions.ModifyRecord(); break;
                    case "5": staffActions.DeleteRecord(); break;
                    case "6": ViewSummary(); Console.Clear() ; break;
                    case "7": Console.Clear(); Console.WriteLine("Logging out...... THANK YOU FOR USING THE SYSYEM!"); return;
                    default: Console.WriteLine("Invalid choice. Press any key..."); Console.ReadKey(true); break;
                }
            }
        }

        private void ViewSummary()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════╗");
            Console.WriteLine("║        ADOPTION SUMMARY       ║");
            Console.WriteLine("╚═══════════════════════════════╝");

            try
            {
                string[] files = Directory.GetFiles(recordFolder, "*.txt");
                Console.WriteLine($"Total adoption records: {files.Length}\n");

                var petTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (var file in files)
                {
                    string[] lines = File.ReadAllLines(file);
                    string type = "";
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Type:", StringComparison.OrdinalIgnoreCase))
                        {
                            type = line.Substring("Type:".Length).Trim();
                            break;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(type))
                        type = "Unknown";

                    if (petTypes.ContainsKey(type))
                        petTypes[type]++;
                    else
                        petTypes[type] = 1;
                }

                int wType = Math.Min(30, Math.Max("Pet Type".Length, MaxLenKey(petTypes)));
                int wCount = Math.Max("Count".Length, 5);

                string fmt = $"| {{0,-{wType}}} | {{1,{wCount}}} |";
                int totalWidth = wType + wCount + 7;

                Console.WriteLine(new string('-', totalWidth));
                Console.WriteLine(fmt, "Pet Type", "Count");
                Console.WriteLine(new string('-', totalWidth));

                foreach (var kvp in petTypes)
                {
                    string displayType = string.IsNullOrWhiteSpace(kvp.Key) ? "Unknown" : ToTitleCaseSimple(kvp.Key);
                    Console.WriteLine(fmt, Truncate(displayType, wType), kvp.Value);
                }

                Console.WriteLine(new string('-', totalWidth));
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
            finally
            {
                Console.Clear();
            }

            static int MaxLenKey(Dictionary<string, int> dict)
            {
                int max = 0;
                foreach (var k in dict.Keys)
                {
                    var s = k ?? "";
                    if (s.Length > max) max = s.Length;
                }
                return max;
            }

            static string Truncate(string s, int max)
            {
                if (s == null) return "";
                if (s.Length <= max) return s;
                if (max <= 3) return s.Substring(0, max);
                return s.Substring(0, max - 3) + "...";
            }
        }               

        // -------------------- RESET STAFF PASSWORD (FORGOT PASSWORD) --------------------
        private void ResetStaffPassword()
        {
            Console.Clear();
            Console.WriteLine("=== FORGOT PASSWORD ===");
            Console.Write("Enter your username: ");
            string username = Console.ReadLine()?.Trim();

            var staff = staffList.Find(s => s.Username == username);
            if (staff == null)
            {
                Console.WriteLine("Username not found! Press any key...");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Enter your birthday (MM/dd/yyyy): ");
            string birthday = Console.ReadLine()?.Trim();

            if (staff.Birthday != birthday)
            {
                Console.WriteLine("Incorrect birthday! Press any key...");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Enter your new password: ");
            string newPassword = Console.ReadLine()?.Trim();
            staff.Password = newPassword;
            SaveStaffList(staffList);

            Console.WriteLine("Password successfully reset! Press any key...");
            Console.ReadKey(true);
        }
    }

    // -------------------- PROGRAM START --------------------
    public class Program
    {
        public static void Main()
        {
            PetAdoptionSystem system = new PetAdoptionSystem();
            system.Login();
        }
    }
}
