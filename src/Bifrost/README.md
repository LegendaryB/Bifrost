# Bifrost

Bifrost is a Valheim mod that simplifies multiplayer access by saving server passwords and automatically injecting them when connecting to servers.

## Configuration

No manual setup is required initially. When you first run the mod, it automatically creates a configuration file at:
`BepInEx/config/throwingbits.valheim.Bifrost.json`

### Example Configuration
```json
{
  "Servers": {
    "dedicated_example.de_2456": "password1",
    "dedicated_example2.de_2456": "password2",
  }
}
```

# How does it work?

1. On your first connection to a server, the standard password dialog appears.
2. After successfully connecting, the password is saved automatically to the configuration file.
3. On subsequent connections, Bifrost inject the password, letting you join the server seamlessly.