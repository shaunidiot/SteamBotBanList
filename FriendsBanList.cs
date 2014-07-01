using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace SteamBot {

    public class FriendsBanList {
        String fileName = "FriendsBanList.xml", TAG = "[FriendBanList]";

        private bool fileExist () {
            if (!File.Exists(fileName)) {
                createFile();
                return false;
            }
            return true;
        }

        /// <summary>
        /// List all banned users' Steam64
        /// </summary>
        public void listAll () {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Descendants("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                        Console.WriteLine(TAG + " Banned : " + steamid.Element("steamID").Value.ToString());
                    }
                } else {
                    Console.WriteLine(TAG + " : No banned users.");     
                }
            }
        }

        /// <summary>
        /// Check if user is banned
        /// </summary>
        /// <returns><c>true</c>, if banned was ised, <c>false</c> otherwise.</returns>
        /// <param name="steam64">Steam64.</param>
        public bool isBanned (ulong steam64) {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Descendants("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                        if (steamid.Element("steamID").Value.ToString().Equals(steam64.ToString())) {
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
        /// <para>Example : ban.addban(Bot, OtherSID, true); -> Add ban to user + block user's Steam profile</para>
        /// <para>Example : ban.addban(Bot, OtherSID, false); -> Add ban to user ONLY</para>
        /// </summary>
        /// <returns><c>true</c>, if ban was added, <c>false</c> otherwise.</returns>
        /// <param name="bot">Bot.</param>
        /// <param name="steam64">Steam64.</param>
        /// <param name="banProfile">If set to <c>true</c> ban user Steam's profile.</param>
        public bool addBan (Bot bot, ulong steam64, bool banProfile) {
            if (fileExist()) {
                if (!isBanned(steam64)) { // if does not exist in ban list
                    XDocument doc = XDocument.Load(fileName);
                    XElement service = doc.Element("bannedIDs");
                    service.Add(new XElement ("steamID", steam64.ToString()));
                    if (banProfile) {
                        bot.SteamFriends.IgnoreFriend(steam64, true);
                    }
                    bot.log.Info(TAG + " [AddBan] : Successful");
                    doc.Save(fileName);
                }
            } else {
                addBan(bot, steam64, banProfile);
            }
            return true;
        }

        /// <summary>
        /// <para>Remove ban from user</para>
        /// <para></para>
        /// <para>Example : ban.removeBan(Bot, OtherSID, true); -> Unban user + unblock user's Steam profile</para>
        /// <para>Example : ban.removeBan(Bot, OtherSID, false); -> Unban user ONLY</para>
        /// </summary>
        /// <returns><c>true</c>, if ban was removed, <c>false</c> otherwise.</returns>
        /// <param name="bot">Bot.</param>
        /// <param name="steam64">Steam64.</param>
        /// <param name="unbanProfile">If set to <c>true</c> unban user Steam's profile.</param>
        public bool removeBan (Bot bot, ulong steam64, bool unbanProfile) {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                if (bannedIDs.Descendants("bannedIDs").Elements("steamID").Any()) {
                    foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                        if (steamid.Element("steamID").Value.ToString().Equals(steam64.ToString())) {
                            bannedIDs.Descendants("bannedIDs")
                        .Elements("steamID")
                        .Where(x => x.Value == steam64.ToString())
                        .Remove();
                            if (unbanProfile) {
                                bot.SteamFriends.IgnoreFriend(steam64, false);
                            }
                            bot.log.Info(TAG + " [RemoveBan] : Successful");
                        }
                    }
                }
                bannedIDs.Save(fileName);
            }
            return true;
        }

        public void createFile () {
            XDocument doc = new XDocument (new XElement ("bannedIDs"));
            doc.Save(fileName);
        }
    }
}

