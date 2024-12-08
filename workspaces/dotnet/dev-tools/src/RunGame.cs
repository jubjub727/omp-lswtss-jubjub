using System;
using System.Diagnostics;
using System.IO;

namespace OMP.LSWTSS;

public static class RunGame
{
    public static void Execute(string gameDirPath, CApi1.Variant cApi1Variant)
    {
        var gameProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(gameDirPath, "LEGOSTARWARSSKYWALKERSAGA_DX11.exe"),
                WorkingDirectory = gameDirPath,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
            }
        };

        if (cApi1Variant == CApi1.Variant.Steam)
        {
            gameProcess.StartInfo.Environment["SteamClientLaunch"] = "1";
            gameProcess.StartInfo.Environment["SteamNoOverlayUI"] = "1";
            gameProcess.StartInfo.Environment["SteamGameId"] = "920210";
            gameProcess.StartInfo.Environment["SteamAppId"] = "920210";
            gameProcess.StartInfo.Environment["SteamOverlayGameId"] = "920210";
            gameProcess.StartInfo.Environment["SteamEnv"] = "1";
        }
        else if (cApi1Variant == CApi1.Variant.EGS)
        {
            gameProcess.StartInfo.ArgumentList.Add("-epicapp=c390e58246ea4a778acd26473a489b48");
        }

        gameProcess.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
            }
        };

        gameProcess.Start();

        gameProcess.BeginOutputReadLine();

        gameProcess.WaitForExit();
    }
}