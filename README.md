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
`void ListAll()` - List all banned IDs in console

`bool IsBanned(ulong steam64)` - Check if Steam64 ID is banned

`bool AddBan(Bot bot, ulong steam64, bool banProfile)` - Add ban for user

`bool RemoveBan(Bot bot, ulong steam64, bool unbanProfile)` - Remove ban for user

Example
--------------
```C#
        public override bool OnFriendAdd () {
            if (!ban.IsBanned(OtherSID)) {
                Bot.log.Success("User added me");
                return true;
            } else {
                ban.AddBan(Bot, OtherSID, true); // add to ban list + block user profile
            }
            return false;
        }
```