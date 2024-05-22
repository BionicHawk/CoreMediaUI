using System;
using System.Collections.Generic;

namespace CoreMediaUI.Models;

public partial class Setting
{
    public int SettingsId { get; set; }

    public string SelectedIp { get; set; } = null!;

    public double? Sensibility { get; set; }
}
