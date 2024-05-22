using CoreMediaUI.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMediaUI.Source.Config {
    public static class ConfigBuilder {

        private static string? AppDataFolder;
        private const string AppFolder = "CoreMediaUI";
        public static string? DatabaseFilePath { get; private set; }
        public static Setting? CurrentSetting { get; private set; }
        private static ConfigurationContext? _context;

        public static void InitializeService() {
            var pathExists = false;
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AppDataFolder == null) return;
            var directories = Directory.GetDirectories(AppDataFolder);
            var completePath = Path.Combine(AppDataFolder, AppFolder);

            pathExists = directories.Where(dir => dir == completePath).SingleOrDefault() != null;
            
            if (!pathExists) {
                Directory.CreateDirectory(completePath);
                CreateConfigFiles(completePath);
            }
            _context = new ConfigurationContext();
            //ReadOrWriteInitialConfig().Wait();
        }

        private static void CreateConfigFiles(string workingDir) {
            // Create sqlite file
            InitializeDatabase(workingDir);
        }

        private static void InitializeDatabase(string workingDir) {
            DatabaseFilePath = Path.Combine(workingDir, "Configuration.db");
            File.Open(DatabaseFilePath, FileMode.OpenOrCreate).Close();


            using (var db = new SqliteConnection($"Filename={DatabaseFilePath}")) {
                db.Open();
                
                var tableCommand = @"CREATE TABLE IF NOT EXISTS Settings (
                    settings_id INTEGET PRIMARY KEY,
                    selected_ip TEXT NOT NULL,
                    sensibility REAL DEFAULT 100.0
                )";

                var createTable = new SqliteCommand(tableCommand);
                createTable.Connection = db;
                createTable.ExecuteReader();
            }
        }
        
        private static async Task ReadOrWriteInitialConfig() {
            if (_context == null) return;
            var addresses = GetDNS.AvailableAddresses;
            Setting? gotSetting = null;

            foreach (var address in addresses) {
                var setting = await _context.Settings.Where(sett =>
                    sett.SelectedIp == address.ToString())
                    .SingleOrDefaultAsync();

                if (setting != null) {
                    gotSetting = setting;
                }

                break;
            }

            if (gotSetting == null) {
                Setting newSetting = new() {
                    SelectedIp = addresses.First().ToString(),
                };

                _context.Add(newSetting);
                await _context.SaveChangesAsync();
                CurrentSetting = newSetting;
            }

        }

    }
}
