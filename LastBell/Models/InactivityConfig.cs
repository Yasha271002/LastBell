﻿using Newtonsoft.Json;

namespace LastBell.Models
{
    public record InactivityConfig(
        [property: JsonProperty("inactivityTime")] int InactivityTime,
        [property: JsonProperty("passwordInactivityTime")] int PasswordInactivityTime);
}