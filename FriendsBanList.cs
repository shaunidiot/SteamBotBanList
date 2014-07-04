using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace SteamBot {

    public class FriendsBanList {
        String fileName = "FriendsBanList.xml", TAG = "[FriendBanList] ";

        public FriendsBanList () {
            WriteLog(TAG + "Initialized");
        }

        private void WriteLog (String text) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private bool FileExist () {
            if (!File.Exists(fileName)) {
                CreateFile();
                return false;
            }
            return true;
        }

        /// <summary>
        /// List all banned users' Steam64
        /// </summary>
        public void ListAll () {
            if (FileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Element("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Element("bannedIDs").Elements("steamID")) {
                        WriteLog(TAG + "Banned : " + steamid.Value.ToString());
                    }
                } else {
                    WriteLog(TAG + ": No banned users.");     
                }
            }
        }

        /// <summary>
        /// Check if user is banned
        /// </summary>
        /// <returns><c>true</c>, if banned was ised, <c>false</c> otherwise.</returns>
        /// <param name="steam64">Steam64.</param>
        public bool IsBanned (ulong steam64) {
            if (FileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Element("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Element("bannedIDs").Elements("steamID")) {
                        if (steamid.Value.ToString().Equals(steam64.ToString())) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// <para>Add ban for user</para>
        /// <para></para>
        /// <para>Example : ban.Addban(Bot, OtherSID, true); -> Add ban to user + block user's Steam profile</para>
        /// <para>Example : ban.Addban(Bot, OtherSID, false); -> Add ban to user ONLY</para>
        /// </summary>
        /// <returns><c>true</c>, if ban was added, <c>false</c> otherwise.</returns>
        /// <param name="bot">Bot.</param>
        /// <param name="steam64">Steam64.</param>
        /// <param name="banProfile">If set to <c>true</c> ban user Steam's profile.</param>
        public bool AddBan (Bot bot, ulong steam64, bool banProfile) {
            if (FileExist()) {
                if (!IsBanned(steam64)) { // if does not exist in ban list
                    XDocument doc = XDocument.Load(fileName);
                    XElement service = doc.Element("bannedIDs");
                    service.Add(new XElement ("steamID", steam64.ToString()));
                    if (banProfile) {
                        bot.SteamFriends.IgnoreFriend(steam64, true);
                    }
                    WriteLog(TAG + "[AddBan] : Successful - " + steam64.ToString());
                    doc.Save(fileName);
                }
            } else {
                AddBan(bot, steam64, banProfile);
            }
            return true;
        }

        /// <summary>
        /// <para>Remove ban from user</para>
        /// <para></para>
        /// <para>Example : ban.RemoveBan(Bot, OtherSID, true); -> Unban user + unblock user's Steam profile</para>
        /// <para>Example : ban.RemoveBan(Bot, OtherSID, false); -> Unban user ONLY</para>
        /// </summary>
        /// <returns><c>true</c>, if ban was removed, <c>false</c> otherwise.</returns>
        /// <param name="bot">Bot.</param>
        /// <param name="steam64">Steam64.</param>
        /// <param name="unbanProfile">If set to <c>true</c> unban user Steam's profile.</param>
        public bool RemoveBan (Bot bot, ulong steam64, bool unbanProfile) {
            if (FileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Element("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Element("bannedIDs").Elements("steamID")) {
                        if (steamid.Value.ToString().Equals(steam64.ToString())) {
                            bannedIDs.Element("bannedIDs")
                        .Elements("steamID")
                        .Where(x => x.Value == steam64.ToString())
                        .Remove();
                            if (unbanProfile) {
                                bot.SteamFriends.IgnoreFriend(steam64, false);
                            }
                            WriteLog(TAG + "[RemoveBan] : Successful - " + steam64.ToString());
                        }
                    }
                }
                bannedIDs.Save(fileName);
            }
            return true;
        }

        public void CreateFile () {
            XDocument doc = new XDocument (new XElement ("bannedIDs"));
            doc.Save(fileName);
        }
    }
}

