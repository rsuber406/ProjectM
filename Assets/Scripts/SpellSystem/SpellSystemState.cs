enum SpellSystemState
{
    Ready, // Can a spell be activated now?
    Activated, // Is a spell in progress blocking other spells from activating
    Offline, // No spell can be activated
}