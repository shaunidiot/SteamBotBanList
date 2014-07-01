SteamBotBanList
=========
A SteamBot extension for managing bans.

Installation
----
Download `FriendsBanList.cs` and import it into your `SteamBot` project

Usage
-----------
Initialise `FriendsBanList ban = new FriendsBanList ();` in class.

Methods
--------------
`void listAll()` - List all banned IDs in console

`bool isBanned(ulong steam64)` - Check if Steam64 ID is banned

`bool addBan(Bot bot, ulong steam64, bool banProfile)` - Add ban for user

`bool removeBan(Bot bot, ulong steam64, bool unbanProfile)` - Remove ban for user

Example
--------------
```C#
        public override bool OnFriendAdd () {
            if (!ban.isBanned(OtherSID)) {
                Bot.log.Success("User added me");
                return true;
            } else {
                ban.addBan(Bot, OtherSID, true); // add to ban list + block user profile
            }
            return false;
        }```