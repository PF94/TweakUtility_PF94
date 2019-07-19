﻿using System;
using System.Linq;
using System.Management;

/// TweakUtility - Operating System Version Check (IMPORTANT NOTES)
/// WARNING: Beta support is experimental at the moment.
/// Not every beta has custom registry values, Please remember that
///
/// However, Important betas should be still supported in case
/// such as Longhorn 4074, The 3 Windows 8 Previews, etc.
///
/// The obscure Windows operating systems based on XP/2003's codebase should not be included.
/// This means that any of the Embedded (including POSready), Fundamental for Legacy PCs and Home Server
/// should not be implemented, As even if they have a different "build" number and System, WinVer still says it's 2600.
/// This also applies for Windows 7 (or Server 2008 R2)'s  MultiPoint Server 2010, Embedded 7, Home Server 2011 and Thin PC.
///
/// This should be only official Microsoft versions, Do not bloat this list with Custom Versions that uses
/// custom "build" numbers, such as Windows 2007 by Glosswired, or any other mods.
///
/// Written and updated by PF94, July 15th 2019
using TweakUtility.Attributes;

namespace TweakUtility
{
    public static class OperatingSystemVersions
    {
        private static Version _currentVersion = null;

        private static readonly Version[] _versions = new[] {
          //new Version(major, minor, build),
            new Version(5, 1),
            new Version(5, 2),
            new Version(6, 0, 4074),
            new Version(6, 0),
            new Version(6, 1, 6801),
            new Version(6, 1, 7000),
            new Version(6, 1),
            new Version(6, 2, 8102),
            new Version(6, 2, 8250),
            new Version(6, 2, 8400),
            new Version(6, 2),
            new Version(6, 3),
            new Version(6, 4),
            new Version(10, 0, 10074),
            new Version(10, 0)
        };

        public static Version GetVersion(OperatingSystemVersion version)
        {
            if (version == OperatingSystemVersion.None)
            {
                return null;
            }

            return _versions[(int)version - 1];
        }

        public static Version GetCurrentVersion()
        {
            if (_currentVersion == null)
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    string[] v = ((string)searcher.Get().Cast<ManagementObject>().FirstOrDefault().Properties["Version"].Value).Split('.');
                    _currentVersion = new Version(int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2]));
                }
            }

            return _currentVersion;
        }

        /// <summary>
        /// Checks if the current operating system version matches the <see cref="OperatingSystemSupportedAttribute"/>
        /// </summary>
        /// <param name="mininum">The minimum supported operating system version.</param>
        /// <param name="maximum">The maximum supported operating system version.</param>
        /// <returns></returns>
        public static bool IsSupported(this OperatingSystemSupportedAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return IsSupported(attribute.Mininum, attribute.Maximum);
        }

        /// <summary>
        /// Checks if the current operating system version matches the <paramref name="mininum"/> and <paramref name="maximum"/> version.
        /// </summary>
        /// <param name="mininum">The minimum supported operating system version.</param>
        /// <param name="maximum">The maximum supported operating system version.</param>
        /// <returns></returns>
        public static bool IsSupported(this Version mininum, Version maximum = null)
        {
            if (mininum is null)
            {
                throw new ArgumentNullException(nameof(mininum));
            }

            Version current = GetCurrentVersion();

            if (current < mininum)
            {
                return false;
            } //Check if current version is older than the minimum

            if (maximum != null && maximum < current)
            {
                return false;
            } //Check if there's a maximum version and if the current version is too new.

            return true;
        }
    }
}