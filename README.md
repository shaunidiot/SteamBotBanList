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

`bool addban(ulong steam64)` - Add ban for user

`bool removeban(ulong steam64)` - Remove ban for user