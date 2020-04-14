using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EveProfileCopier
{
    class Program
    {
        const string UserProfileFilePattern = "core_user_([0-9]*).dat";
        const string CharacterProfileFilePattern = "core_char_([0-9]*).dat";

        static void Main(string[] args)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var profileDirectory = Path.Combine(localAppData, @"CCP\EVE\d_games_eve_sharedcache_tq_tranquility\settings_Default");

            Console.Write("Enter user ID to copy: ");
            int.TryParse(Console.ReadLine(), out int userId);

            Console.Write("Enter character ID to copy: ");
            int.TryParse(Console.ReadLine(), out int characterId);

            if (userId == 0)
            {
                Console.WriteLine("Invalid user ID");
                return;
            }

            if (characterId == 0)
            {
                Console.WriteLine("Invalid character ID");
                return;
            }

            var files = Directory.EnumerateFiles(profileDirectory, "*.dat", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x));

            var sourceUserProfile = Path.Combine(profileDirectory, $"core_user_{userId}.dat");
            var sourceCharacterProfile = Path.Combine(profileDirectory, $"core_char_{characterId}.dat");

            var userProfiles = files
                .Where(x => Regex.IsMatch(x, UserProfileFilePattern))
                .Where(x => x != Path.GetFileName(sourceUserProfile))
                .ToList();

            var characterProfiles = files
                .Where(x => Regex.IsMatch(x, CharacterProfileFilePattern))
                .Where(x => x != Path.GetFileName(sourceCharacterProfile))
                .ToList();

            foreach (var userProfile in userProfiles)
            {
                var dest = Path.Combine(profileDirectory, userProfile);
                Console.WriteLine($"Copying {sourceUserProfile} to {dest}");
                File.Copy(sourceUserProfile, dest, true);
            }

            foreach (var characterProfile in characterProfiles)
            {
                File.Copy(sourceCharacterProfile, Path.Combine(profileDirectory, characterProfile), true);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
