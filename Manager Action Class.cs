
namespace ProjectProposal1_PetAdoption
{
    public class ManagerActions : FileManager
    {
        private List<Staff> staffList;

        public ManagerActions(List<Staff> staffListRef)
        {
            staffList = staffListRef ?? new List<Staff>();
        }

        // -------------------- VIEW LOGS --------------------
        public void ViewLogs()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║          VIEW LOGS               ║");
            Console.WriteLine("╚══════════════════════════════════╝\n");

            string[] logFiles;
            try
            {
                logFiles = Directory.GetFiles(logFolder, "*.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing log folder: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                return;
            }

            if (logFiles.Length == 0)
            {
                Console.WriteLine("No log files found. Press any key...");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Available Log Files:\n");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"{"No.",-5} {"Date",-12} {"File Name",-20}");
            Console.WriteLine("------------------------------------------------");

            Dictionary<int, string> logDict = new Dictionary<int, string>();
            int index = 1;

            foreach (var file in logFiles)
            {
                try
                {
                    string fileName = Path.GetFileName(file);         // e.g., log_20251113.txt
                    if (fileName.Length < 12)
                        throw new FormatException("Log file name too short to contain date part.");

                    string datePart = fileName.Substring(4, 8);      // e.g., 20251113
                    DateTime logDate = DateTime.ParseExact(datePart, "yyyyMMdd", null);
                    string formattedDate = logDate.ToString("MM/dd/yyyy");

                    Console.WriteLine($"{index,-5} {formattedDate,-12} {fileName,-20}");
                    logDict.Add(index, file);
                    index++;
                }
                catch
                {
                    // Skip files that don't match expected pattern
                    continue;
                }
            }

            Console.WriteLine("------------------------------------------------\n");
            Console.WriteLine("You can either:");
            Console.WriteLine("1. Enter the number of the log to view it");
            Console.WriteLine("2. Enter a date (MM/dd/yyyy) to view that day's log\n");
            Console.Write("Your choice (number or date): ");

            string input = Console.ReadLine()?.Trim();

            string selectedFile = null;

            // Check if input is a number
            if (int.TryParse(input, out int choice) && logDict.ContainsKey(choice))
            {
                selectedFile = logDict[choice];
            }
            else
            {
                // Check if input is a valid date
                if (DateTime.TryParseExact(input, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime inputDate))
                {
                    string targetFile = $"log_{inputDate:yyyyMMdd}.txt";
                    selectedFile = Path.Combine(logFolder, targetFile);

                    if (!File.Exists(selectedFile))
                        selectedFile = null;
                }
            }

            if (selectedFile != null)
            {
                Console.Clear();
                Console.WriteLine($"--- Log: {Path.GetFileName(selectedFile)} ---\n");
                try
                {
                    Console.WriteLine(File.ReadAllText(selectedFile));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading log file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice or log for that date not found.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        // -------------------- VIEW RECORDS --------------------
        public void ViewRecords()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════╗");
            Console.WriteLine("║       VIEW ADOPTION RECORD    ║");
            Console.WriteLine("╚═══════════════════════════════╝");

            var rows = new List<(string Adopter, string Pet, string Type, string Date)>();

            try
            {
                string[] files = Directory.GetFiles(recordFolder, "*.txt");

                if (files.Length == 0)
                {
                    Console.WriteLine("No records found. Press any key...");
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }

                foreach (string file in files)
                {
                    string[] lines = File.ReadAllLines(file);
                    string adopter = "";
                    string pet = "";
                    string type = "";
                    string date = "";

                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Adopter:", StringComparison.OrdinalIgnoreCase))
                            adopter = line.Substring("Adopter:".Length).Trim();
                        else if (line.StartsWith("Pet:", StringComparison.OrdinalIgnoreCase))
                            pet = line.Substring("Pet:".Length).Trim();
                        else if (line.StartsWith("Type:", StringComparison.OrdinalIgnoreCase))
                            type = line.Substring("Type:".Length).Trim();
                        else if (line.StartsWith("Date Adopted:", StringComparison.OrdinalIgnoreCase))
                            date = line.Substring("Date Adopted:".Length).Trim();
                    }

                    rows.Add((adopter, pet, type, date));
                }

                // use shared helper to print table
                PrintRecordsTable(rows);

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);

                LogAction("manager", "Viewed all adoption records");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
            finally
            {
                Console.Out.Flush();
                Console.Clear();
            }
        }

        // -------------------- MANAGE STAFF ACCOUNTS --------------------
        public void ManageStaffAccounts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MANAGE STAFF ACCOUNTS ===");
                Console.WriteLine("1. View All Staff Accounts");
                Console.WriteLine("2. Add Staff Account");
                Console.WriteLine("3. Delete Staff Account");
                Console.WriteLine("4. Reset Staff Password");
                Console.WriteLine("5. Back to Manager Menu");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": ViewAllStaff(); break;
                    case "2": AddStaff(); break;
                    case "3": DeleteStaff(); break;
                    case "4": ResetStaffPassword(); break;
                    case "5": return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private void ViewAllStaff()
        {
            Console.Clear();
            Console.WriteLine("=== STAFF LIST ===");

            if (staffList.Count == 0)
            {
                Console.WriteLine("No staff accounts available.");
            }
            else
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"{"Full Name",-20} {"Username",-15} {"Birthday",-12} {"Contact",-13} {"Password",-10}");
                Console.WriteLine("--------------------------------------------------------------------------------");
                foreach (var s in staffList)
                {
                    Console.WriteLine($"{s.FullName,-20} {s.Username,-15} {s.Birthday,-12} {s.ContactNumber,-13} {s.Password,-10}");
                }
                Console.WriteLine("--------------------------------------------------------------------------------");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        private void AddStaff()
        {
            Console.Clear();
            Console.WriteLine("=== ADD STAFF ACCOUNT ===");

            Console.Write("Full Name: ");
            string fullName = Console.ReadLine()?.Trim();

            Console.Write("Username: ");
            string username = Console.ReadLine()?.Trim();

            Console.Write("Birthday (MM/dd/yyyy): ");
            string birthday = Console.ReadLine()?.Trim();

            Console.Write("Contact Number: ");
            string contact = Console.ReadLine()?.Trim();

            string password = "Staff@123"; // Default initial password

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(contact))
            {
                Console.WriteLine("All fields are required! Press any key...");
                Console.ReadKey(true);
                return;
            }

            staffList.Add(new Staff(fullName, username, birthday, contact, password));
            SaveStaffList(staffList);
            Console.WriteLine($"\nStaff account created successfully! Initial password: {password}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        private void DeleteStaff()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE STAFF ACCOUNT ===");
            Console.Write("Enter username to delete: ");
            string username = Console.ReadLine()?.Trim();

            var staff = staffList.Find(s => s.Username == username);
            if (staff != null)
            {
                staffList.Remove(staff);
                SaveStaffList(staffList);
                Console.WriteLine("Staff deleted! Press any key...");
            }
            else
                Console.WriteLine("Staff not found. Press any key...");

            Console.ReadKey(true);
        }

        private void ResetStaffPassword()
        {
            Console.Clear();
            Console.WriteLine("=== RESET STAFF PASSWORD ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine()?.Trim();

            var staff = staffList.Find(s => s.Username == username);
            if (staff == null)
            {
                Console.WriteLine("Staff not found. Press any key...");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Enter new password: ");
            staff.Password = Console.ReadLine()?.Trim();
            SaveStaffList(staffList);
            Console.WriteLine("Password reset successfully! Press any key...");
            Console.ReadKey(true);
        }
    }
}