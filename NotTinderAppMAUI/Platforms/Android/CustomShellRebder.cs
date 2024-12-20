﻿using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Tabs;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using AndroidApp = Android.App.Application;

namespace NotTinderAppMAUI;

public class CustomShellRenderer : ShellRenderer
{
    /// <summary>
    ///     CustomShellRenderer
    /// </summary>
    /// <param name="context"></param>
    public CustomShellRenderer(Context context) : base(context)
    {
    }

    protected override IShellTabLayoutAppearanceTracker CreateTabLayoutAppearanceTracker(ShellSection shellSection)
    {
        return new MyTabLayoutAppearanceTracker(this);
    }
}

/// <summary>
///     CustomShellItemRenderer
/// </summary>
public class MyTabLayoutAppearanceTracker : ShellTabLayoutAppearanceTracker
{
    public MyTabLayoutAppearanceTracker(IShellContext shellContext) : base(shellContext)
    {
    }

    public override void SetAppearance(TabLayout tabLayout, ShellAppearance appearance)
    {
        base.SetAppearance(tabLayout, appearance);

        tabLayout.TabMode = TabLayout.ModeFixed;
        tabLayout.TabGravity = TabLayout.GravityFill;
    }
}