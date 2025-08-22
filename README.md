# Frogbot 🐸

Frogbot is a **.NET 9.0 Discord bot** built with [NetCord](https://github.com/NetCord/NetCord). This was developed for the Frog Bog Discord server.

---
## Scope

* **Sleep Bonk**:

    * `/bonk @user 30m` — Assigns a temporary "sleep bonk" role for a specified duration.
    * `/unbonk @user` — Removes the role manually.
    * Background worker removes roles when their duration expires.
* **Role Background Worker**:
    * Continuously checks for expired roles.
    * Cleans up roles and data store when they are removed.
* **Guild Event Monitoring**:
    * Sends a notification when a new scheduled event is created in the server.

## Getting Started

### Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
* A Discord bot token from the [Discord Developer Portal](https://discord.com/developers/applications).

### Download Latest Release: [LINK TO RELEASE](https://dotnet.microsoft.com/)

### Or build it yourself:

1. Clone the repo:

   ```bash
   git clone https://github.com/yourusername/frogbot.git
   cd frogbot
   ```

2. Build and run the bot:

   ```bash
   dotnet run
   ```

3. On first run, the bot will prompt for your Discord bot token and generate an `appsettings.json` file in the project root:

   ```json
   {
     "Discord": {
       "Token": "YOUR_BOT_TOKEN"
     }
   }
   ```

4. Invite the bot to your server with the correct permissions (Manage Roles, Moderate Members).

---

## Structure

```
Frogbot/
├── Program.cs       # App entry
├── Admin.cs         # Administrative app commands
├── RoleWorker.cs    # Background service for sleep bonk
├── appsettings.json # App config
└── data.json        # Stores active sleep bonk victims
```

---

## Commands

| Command                   | Description                           | Example            |
| ------------------------- | ------------------------------------- | ------------------ |
| `/ping`                   | Responds with "Pong!"                 | `/ping`            |
| `/bonk <user> <duration>` | Applies a temporary "sleep bonk" role | `/bonk @froggy 2h` |
| `/unbonk <user>`          | Removes the "sleep bonk" role early   | `/unbonk @froggy`  |

**Duration format**: `30s`, `15m`, `2h`, `1d`

---

## Data

Sleep bonk victims are tracked in `data.json`
Each entry contains:

```json
{
  "GuildId": 1234567890,
  "UserId": 987654321,
  "RoleId": 1401795286116073558,
  "AddedTime": "2025-08-21T20:00:00Z",
  "ResolveTime": "2025-08-21T22:00:00Z"
}
```

The `RoleWorker` will periodically check this file and unbonk appropriately.

---

## Contributing

For latest changes, ensure you are branching from dev branch not main.