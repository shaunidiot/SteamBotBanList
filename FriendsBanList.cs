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

        public void listAll () {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                    Console.WriteLine(TAG + "Banned : " + steamid.Element("steamID").Value.ToString());
                }
            }
        }

        public bool isBanned (ulong steam64) {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                    if (steamid.Element("steamID").Value.ToString().Equals(steam64.ToString())) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool addBan (ulong steam64) {
            if (fileExist()) {
                XDocument doc = XDocument.Load(fileName);
                XElement service = doc.Element("bannedIDs");
                service.Add(new XElement ("steamID", steam64.ToString()));
                doc.Save(fileName);
                // Console.WriteLine("Ban added for Steam64 :" + steam64.ToString());
            } else {
                addBan(steam64);
            }
            return true;
        }

        public bool removeBan (ulong steam64) {
            if (fileExist()) {
                XDocument bannedIDs = XDocument.Load(fileName);
                foreach (var steamid in bannedIDs.Descendants("bannedIDs")) {
                    if (steamid.Element("steamID").Value.ToString().Equals(steam64.ToString())) {
                        bannedIDs.Descendants("bannedIDs")
                        .Elements("steamID")
                        .Where(x => x.Value == steam64.ToString())
                        .Remove();
                    }
                }
                bannedIDs.Save(fileName);
            } else {
                removeBan(steam64);
            }
            return true;
        }

        public void createFile () {
            XDocument doc = new XDocument (new XElement ("bannedIDs"));
            doc.Save(fileName);
        }
    }
}

