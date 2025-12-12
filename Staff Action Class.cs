namespace ProjectProposal1_PetAdoption
{
    public class StaffActions : FileManager
    {
        private readonly Staff staff;

        public StaffActions(Staff staff)
        {
            this.staff = staff ?? throw new ArgumentNullException(nameof(staff));
        }

        public void AddRecord()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════╗");
            Console.WriteLine("║       ADD ADOPTION RECORD     ║");
            Console.WriteLine("╚═══════════════════════════════╝");

            Console.Write("Enter adopter's name: ");
            string adopter = Console.ReadLine()?.Trim();
            Console.Write("Enter pet's name: ");
            string pet = Console.ReadLine()?.Trim();
            Console.Write("Enter pet type: ");
            string type = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(adopter) || string.IsNullOrWhiteSpace(pet) || string.IsNullOrWhiteSpace(type))
            {
                Console.WriteLine("All fields are required! Press any key...");
                Console.ReadKey(true);
                return;
            }

            var record = new PetRecord(adopter, pet, type);
            string filePath = MakeRecordFileName(adopter, pet);
            try
            {
                File.WriteAllText(filePath, record.ToRecordText());
                LogAction(staff.Username, $"Added adoption record: {adopter} adopted {pet} ({type})");
                Console.WriteLine("Record added successfully! Press any key...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding record: {ex.Message}");
                Console.WriteLine("Press any key...");
            }

            Console.ReadKey(true);
        }

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

                PrintRecordsTable(rows);

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);

                LogAction(staff.Username, "Viewed all adoption records");
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

        public void SearchRecord()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════╗");
            Console.WriteLine("║     SEARCH ADOPTION RECORD    ║");
            Console.WriteLine("╚═══════════════════════════════╝");

            string[] filesAll;
            try
            {
                filesAll = Directory.GetFiles(recordFolder, "*.txt");
                if (filesAll.Length == 0)
                {
                    Console.WriteLine("No records found. Press any key...");
                    Console.ReadKey(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing records: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                return;
            }

            // Show numbered list so user can pick by number if they want
            Console.WriteLine("All records:");
            for (int i = 0; i < filesAll.Length; i++)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(filesAll[i]) ?? "";
                int lastUnderscore = nameNoExt.LastIndexOf('_');
                string display = lastUnderscore >= 0
                    ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                    : nameNoExt.Replace('_', ' ').Trim();
                Console.WriteLine($"[{i + 1}] {display}");
            }

            Console.WriteLine();
            Console.Write("Enter record number to view, or enter adopter/pet name to search (press Enter to cancel): ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Cancelled. Press any key...");
                Console.ReadKey(true);
                return;
            }

            // If user entered a number, show that record
            if (int.TryParse(input, out int idx))
            {
                if (idx < 1 || idx > filesAll.Length)
                {
                    Console.WriteLine("Invalid number. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                string file = filesAll[idx - 1];
                try
                {
                    string[] lines = File.ReadAllLines(file);
                    Console.Clear();
                    Console.WriteLine("=== RECORD DETAILS ===\n");
                    foreach (var line in lines) Console.WriteLine(line);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    LogAction(staff.Username, $"Viewed record by number: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file: {ex.Message}");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey(true);
                }
                return;
            }

            // Otherwise treat input as keyword search (case-insensitive)
            string keyword = input.ToLowerInvariant();
            var matches = new List<string>();
            var rows = new List<(string Adopter, string Pet, string Type, string Date)>();

            foreach (var f in filesAll)
            {
                try
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(f).Replace('_', ' ');
                    string content = File.ReadAllText(f);

                    if (nameNoExt.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(f);

                        string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
                }
                catch
                {
                    // ignore unreadable files
                }
            }

            if (rows.Count == 0)
            {
                Console.WriteLine("\nNo matching record found. Press any key...");
                Console.ReadKey(true);
                return;
            }

            // Show results as table and as a numbered list so user can pick a record to view
            PrintRecordsTable(rows);

            Console.WriteLine("\nMatching Records:");
            for (int i = 0; i < matches.Count; i++)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(matches[i]) ?? "";
                int lastUnderscore = nameNoExt.LastIndexOf('_');
                string display = lastUnderscore >= 0
                    ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                    : nameNoExt.Replace('_', ' ').Trim();
                Console.WriteLine($"[{i + 1}] {display}");
            }

            Console.Write("\nEnter the number of the record to view (or press Enter to return): ");
            string choiceInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(choiceInput))
                return;

            if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > matches.Count)
            {
                Console.WriteLine("Invalid choice. Press any key...");
                Console.ReadKey(true);
                return;
            }

            string selected = matches[choice - 1];
            try
            {
                Console.Clear();
                foreach (var line in File.ReadAllLines(selected)) Console.WriteLine(line);
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                LogAction(staff.Username, $"Viewed record from search: {Path.GetFileName(selected)} (keyword: {keyword})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
            }
        }

        public void ModifyRecord()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════╗");
            Console.WriteLine("║     MODIFY ADOPTION RECORD    ║");
            Console.WriteLine("╚═══════════════════════════════╝");

            string[] filesAll;
            try
            {
                filesAll = Directory.GetFiles(recordFolder, "*.txt");
                if (filesAll.Length == 0)
                {
                    Console.WriteLine("No records found. Press any key...");
                    Console.ReadKey(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing records: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                return;
            }

            // Show all records numbered so user can pick by number if desired
            Console.WriteLine("All records:");
            for (int i = 0; i < filesAll.Length; i++)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(filesAll[i]) ?? "";
                int lastUnderscore = nameNoExt.LastIndexOf('_');
                string display = lastUnderscore >= 0
                    ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                    : nameNoExt.Replace('_', ' ').Trim();
                Console.WriteLine($"[{i + 1}] {display}");
            }

            Console.WriteLine();
            Console.Write("Enter record number to modify, or enter adopter/pet name to search (press Enter to cancel): ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Cancelled. Press any key...");
                Console.ReadKey(true);
                return;
            }

            string selectedFile = null;

            // If user entered a number, pick that file
            if (int.TryParse(input, out int idx))
            {
                if (idx < 1 || idx > filesAll.Length)
                {
                    Console.WriteLine("Invalid number. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                selectedFile = filesAll[idx - 1];
            }
            else
            {
                // treat input as keyword search (case-insensitive)
                string keyword = input;
                var matches = new List<string>();
                var rows = new List<(string Adopter, string Pet, string Type, string Date)>();

                foreach (var f in filesAll)
                {
                    try
                    {
                        string nameNoExt = Path.GetFileNameWithoutExtension(f).Replace('_', ' ');
                        string content = File.ReadAllText(f);

                        if (nameNoExt.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matches.Add(f);

                            string[] lines = File.ReadAllLines(f);
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
                    }
                    catch
                    {
                        // ignore unreadable files
                    }
                }

                if (matches.Count == 0)
                {
                    Console.WriteLine("\nNo matching record found. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                // Show table of matches and numbered list
                PrintRecordsTable(rows);

                Console.WriteLine("\nMatching Records:");
                for (int i = 0; i < matches.Count; i++)
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(matches[i]) ?? "";
                    int lastUnderscore = nameNoExt.LastIndexOf('_');
                    string display = lastUnderscore >= 0
                        ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                        : nameNoExt.Replace('_', ' ').Trim();
                    Console.WriteLine($"[{i + 1}] {display}");
                }

                Console.Write("\nEnter the number of the record to modify (or press Enter to cancel): ");
                string choiceInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(choiceInput))
                    return;

                if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > matches.Count)
                {
                    Console.WriteLine("Invalid choice. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                selectedFile = matches[choice - 1];
            }

            // selectedFile is set — proceed with existing modify flow
            if (selectedFile == null)
            {
                Console.WriteLine("No file selected. Press any key...");
                Console.ReadKey(true);
                return;
            }

            string[] linesArr;
            try
            {
                linesArr = File.ReadAllLines(selectedFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading selected file: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                return;
            }

            string currentAdopter = "";
            string currentPet = "";
            string currentType = "";
            string currentDateAdopted = "";
            foreach (var line in linesArr)
            {
                if (line.StartsWith("Adopter:", StringComparison.OrdinalIgnoreCase))
                    currentAdopter = line.Substring("Adopter:".Length).Trim();
                else if (line.StartsWith("Pet:", StringComparison.OrdinalIgnoreCase))
                    currentPet = line.Substring("Pet:".Length).Trim();
                else if (line.StartsWith("Type:", StringComparison.OrdinalIgnoreCase))
                    currentType = line.Substring("Type:".Length).Trim();
                else if (line.StartsWith("Date Adopted:", StringComparison.OrdinalIgnoreCase))
                    currentDateAdopted = line.Substring("Date Adopted:".Length).Trim();
                else if (string.IsNullOrEmpty(currentDateAdopted) && line.StartsWith("Date Modified:", StringComparison.OrdinalIgnoreCase))
                    currentDateAdopted = line.Substring("Date Modified:".Length).Trim();
            }

            if (string.IsNullOrWhiteSpace(currentDateAdopted))
                currentDateAdopted = DateTime.Now.ToString();

            Console.WriteLine($"\nCurrent adopter: {currentAdopter}");
            Console.WriteLine($"Current pet name: {currentPet}");
            Console.WriteLine($"Current pet type: {currentType}");
            Console.WriteLine($"Current date adopted: {currentDateAdopted}");

            Console.WriteLine("\nLeave a field empty to keep the current value.");
            Console.Write("Enter new adopter's name: ");
            string newAdopter = Console.ReadLine() ?? "";
            newAdopter = newAdopter.Trim();
            if (string.IsNullOrWhiteSpace(newAdopter)) newAdopter = currentAdopter;

            Console.Write("Enter new pet name: ");
            string newPet = Console.ReadLine() ?? "";
            newPet = newPet.Trim();
            if (string.IsNullOrWhiteSpace(newPet)) newPet = currentPet;

            Console.Write("Enter new pet type: ");
            string newType = Console.ReadLine() ?? "";
            newType = newType.Trim();
            if (string.IsNullOrWhiteSpace(newType)) newType = currentType;

            Console.Write("Enter new date adopted (leave empty to keep current) [e.g. MM/dd/yyyy]: ");
            string newDateInput = Console.ReadLine() ?? "";
            newDateInput = newDateInput.Trim();

            string dateToWrite;
            if (string.IsNullOrWhiteSpace(newDateInput))
            {
                dateToWrite = currentDateAdopted;
            }
            else
            {
                if (DateTime.TryParse(newDateInput, out DateTime parsed))
                    dateToWrite = parsed.ToString();
                else
                {
                    Console.WriteLine("Invalid date format. Keeping current date.");
                    dateToWrite = currentDateAdopted;
                }
            }

            string updated = "Adopter: " + newAdopter + Environment.NewLine
                           + "Pet: " + newPet + Environment.NewLine
                           + "Type: " + newType + Environment.NewLine
                           + "Date Adopted: " + dateToWrite;

            string newFilePath = MakeRecordFileName(newAdopter, newPet);
            string oldFullPath = Path.GetFullPath(selectedFile);
            string newFullPath = Path.GetFullPath(newFilePath);

            try
            {
                if (!string.Equals(oldFullPath, newFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(newFullPath))
                        File.Delete(newFullPath);

                    File.Move(oldFullPath, newFullPath);
                    File.WriteAllText(newFullPath, updated);
                }
                else
                {
                    File.WriteAllText(oldFullPath, updated);
                }

                LogAction(staff.Username, $"Modified record: {currentAdopter} -> {newAdopter}, {currentPet} -> {newPet}, {currentType} -> {newType}");
                Console.WriteLine("\nRecord updated successfully! Press any key...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError updating record: {ex.Message}");
                Console.WriteLine("Press any key...");
            }

            Console.ReadKey(true);
            Console.Clear();
        }

        public void DeleteRecord()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE RECORD ===");

            string[] filesAll;
            try
            {
                filesAll = Directory.GetFiles(recordFolder, "*.txt");
                if (filesAll.Length == 0)
                {
                    Console.WriteLine("No records found. Press any key...");
                    Console.ReadKey(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing records: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                return;
            }

            // Show all records numbered so user can pick by number if desired
            Console.WriteLine("All records:");
            for (int i = 0; i < filesAll.Length; i++)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(filesAll[i]) ?? "";
                int lastUnderscore = nameNoExt.LastIndexOf('_');
                string display = lastUnderscore >= 0
                    ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                    : nameNoExt.Replace('_', ' ').Trim();
                Console.WriteLine($"[{i + 1}] {display}");
            }

            Console.WriteLine();
            Console.Write("Enter record number to delete, or enter adopter's name to search (press Enter to cancel): ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Cancelled. Press any key...");
                Console.ReadKey(true);
                return;
            }

            // If number provided, delete that file after confirmation
            if (int.TryParse(input, out int num))
            {
                if (num < 1 || num > filesAll.Length)
                {
                    Console.WriteLine("Invalid number. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                string selectedFile = filesAll[num - 1];
                string fileNameToDelete = Path.GetFileName(selectedFile);

                Console.Write($"\nAre you sure you want to delete \"{fileNameToDelete}\"? (Y/N): ");
                string confirm = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
                if (confirm != "Y")
                {
                    Console.WriteLine("\nDeletion cancelled. Press any key...");
                    Console.ReadKey(true);
                    return;
                }

                try
                {
                    File.Delete(selectedFile);
                    LogAction(staff.Username, "Deleted record file: " + fileNameToDelete);
                    Console.WriteLine("\nRecord deleted successfully! Press any key...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError deleting file: {ex.Message}");
                    Console.WriteLine("Press any key...");
                }

                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            // Otherwise treat input as search term and show matches
            string adopter = input;
            var matches = new List<string>();

            foreach (var f in filesAll)
            {
                try
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(f).Replace('_', ' ');
                    if (nameNoExt.IndexOf(adopter, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(f);
                        continue;
                    }

                    string content = File.ReadAllText(f);
                    if (content.IndexOf(adopter, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matches.Add(f);
                    }
                }
                catch
                {
                    // ignore unreadable files
                }
            }

            if (matches.Count == 0)
            {
                Console.WriteLine("\nNo matching record found. Press any key...");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("\nMatching Records:");
            for (int i = 0; i < matches.Count; i++)
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(matches[i]) ?? "";
                int lastUnderscore = nameNoExt.LastIndexOf('_');
                string display = lastUnderscore >= 0
                    ? nameNoExt.Substring(0, lastUnderscore).Replace('_', ' ').Trim() + " - " + nameNoExt.Substring(lastUnderscore + 1).Replace('_', ' ').Trim()
                    : nameNoExt.Replace('_', ' ').Trim();
                Console.WriteLine($"[{i + 1}] {display}");
            }

            Console.WriteLine("\n[0] Cancel and return to menu");
            Console.Write("\nEnter the number of the record to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Press any key...");
                Console.ReadKey(true);
                return;
            }

            if (choice == 0)
            {
                Console.WriteLine("\nReturning to menu... Press any key...");
                Console.ReadKey(true);
                return;
            }

            if (choice < 1 || choice > matches.Count)
            {
                Console.WriteLine("Invalid choice. Press any key...");
                Console.ReadKey(true);
                return;
            }

            string selected = matches[choice - 1];
            string fileNameToDel = Path.GetFileName(selected);

            Console.Write($"\nAre you sure you want to delete \"{fileNameToDel}\"? (Y/N): ");
            string confirmDelete = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (confirmDelete != "Y")
            {
                Console.WriteLine("\nDeletion cancelled. Press any key...");
                Console.ReadKey(true);
                return;
            }

            try
            {
                File.Delete(selected);
                LogAction(staff.Username, "Deleted record file: " + fileNameToDel);
                Console.WriteLine("\nRecord deleted successfully! Press any key...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError deleting file: {ex.Message}");
                Console.WriteLine("Press any key...");
            }

            Console.ReadKey(true);
            Console.Clear();
        }
    }
}