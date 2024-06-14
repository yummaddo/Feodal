using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Game
{
    public enum ContextDebug
    {
        Application,
        Session,
        Menu
    }
    public enum Process
    {
        Info,
        TrashHold,
        Process,
        Load,
        Action
    }
    public class Debugger
    {
        private static readonly Dictionary<Process, string[]> ProcessesColorize = new Dictionary<Process, string[]>()
        {
            { Process.Load, new[] { " Load      ", "#FF5733", "#F0EAD6", "#00C882" } }, // Orange
            { Process.Info, new[] { " Info      ", "#3498DB", "#C2EAD6", "#00C882" } }, // Blue
            { Process.Process, new[] { " Process   ", "#27AE60", "#ABEF9C", "#00C882" } }, // Green
            { Process.Action, new[] { " Action    ", "#F1C40F", "#F0EAB2", "#00C882" } }, // Yellow
            { Process.TrashHold, new[] { " TrashHold ", "#9F2000", "#9F896D", "#00C882" } },
        };

        private static readonly Dictionary<ContextDebug, string[]> ContextColorize =
            new Dictionary<ContextDebug, string[]>()
            {
                { ContextDebug.Session, new[] { " Session    ", "#289F00" } }, // Purple
                { ContextDebug.Application, new[] { " Application", "#E74C3C" } }, // Red
                { ContextDebug.Menu, new[] { " Menu       ", "#9F009F" } }, // Turquoise
            };

        private static readonly string On = "<color=#19262C>[</color>";
        private static readonly string Off = "<color=#19262C>]</color>";

        public static void Logger(string mainMessage)
        {
            var contextDebug = ContextDebug.Session;
            var process = Process.Info;
            var coloredMessage = GetBaseLog(mainMessage, contextDebug, process);
            Debug.Log(coloredMessage); //
        }

        public static void Logger(string mainMessage, string location)
        {
            var contextDebug = ContextDebug.Session;
            var process = Process.Info;
            Logger(mainMessage, location, contextDebug, process);
        }

        public static void Logger(string mainMessage, Process process)
        {
            var contextDebug = ContextDebug.Session;
            var coloredMessage = GetBaseLog(mainMessage, contextDebug, process);
            Debug.Log(coloredMessage); //
        }

        public static void Logger(string mainMessage, ContextDebug contextDebug)
        {
            var process = Process.Info;
            var coloredMessage = GetBaseLog(mainMessage, contextDebug, process);
            Debug.Log(coloredMessage); //
        }

        public static void Logger(string mainMessage, ContextDebug contextDebug, Process process)
        {
            var coloredMessage = GetBaseLog(mainMessage, contextDebug, process);
            Debug.Log(coloredMessage); //
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static void Logger(string mainMessage, string massageLocation, ContextDebug contextDebug,
            Process process)
        {
            string textColor =
                ProcessesColorize.ContainsKey(process)
                    ? ProcessesColorize[process][3]
                    : "";
            var coloredMessage = GetBaseLog(mainMessage, contextDebug, process);
            coloredMessage += $"\n{(string.IsNullOrEmpty(textColor) ? "#000021" : $"<color={textColor}>")}" +
                              $"\t[location={massageLocation}]" + "</color>";
            Debug.Log(coloredMessage); //
        }

        private static string GetBaseLog(string mainMessage, ContextDebug contextDebug, Process process)
        {
            var processName = On + (ProcessesColorize.ContainsKey(process) ? ProcessesColorize[process][0] : "") + Off;
            var contextName = On + (ContextColorize.ContainsKey(contextDebug) ? ContextColorize[contextDebug][0] : "") +
                              Off;
            string processColor =
                ProcessesColorize.ContainsKey(process)
                    ? ProcessesColorize[process][1]
                    : ""; // Визначаємо колір для процесу
            string contextColor =
                ContextColorize.ContainsKey(contextDebug)
                    ? ContextColorize[contextDebug][1]
                    : ""; // Визначаємо колір для контексту
            string textColor =
                ProcessesColorize.ContainsKey(process)
                    ? ProcessesColorize[process][2]
                    : ""; // Визначаємо колір для процесу
            string coloredMessage = $"{(string.IsNullOrEmpty(contextColor) ? "#000021" : $"<color={contextColor}>")}" +
                                    $"{contextName}" + "</color>" +
                                    $"{(string.IsNullOrEmpty(processColor) ? "#000021" : $"<color={processColor}>")}" +
                                    $"{processName}" + "</color>" +
                                    $"{(string.IsNullOrEmpty(textColor) ? "#000021" : $"<color={textColor}>")}" +
                                    $"[ {mainMessage} ]" + "</color>";
            return coloredMessage;
        }
    }
}