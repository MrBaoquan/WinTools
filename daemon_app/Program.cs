using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CSHper;
namespace daemon_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("" +
                "描述: 守护进程并置顶置顶程序窗口 \n" +
                "1. 程序未启动则启动程序 \n" +
                "2. 程序挂掉则重启程序 \n" +
                "3. 程序最小化则置顶程序 \n\n" +

                "参数列表: \n" +
                "--interval=5 (执行间隔 s)\n" +
                "--process=xxx (可以为进程名 chorme 或者程序路径 C:\\chrome.exe )\n");

            //args = new string[]
            //{
            //    "--interval=10",
            //    @"--process=O:\UnityProjects\unity2019.2.5f1\VoiceRecognition\Build_Files\VoiceRecognition.exe"
            //};
            Dictionary<string, string> _params = new Dictionary<string, string>()
            {
                {"interval","5" }
            };
            args.ToList().ForEach(_arg =>
            {
                var _match = Regex.Match(_arg, @"--([a-zA-Z0-9]+)=(.*)");
                if (!_match.Success) return;
                string _key = _match.Groups[1].Value;
                string _value = _match.Groups[2].Value;
                if (!_params.ContainsKey(_key))
                    _params.Add(_key, "");
                _params[_key] = _value;
            });

            float _interval = float.Parse(_params["interval"]);
            
            Observable.Interval(TimeSpan.FromSeconds(_interval)).Subscribe(_ =>
            {
                if (!_params.ContainsKey("process"))
                    Console.WriteLine("--process parameter is required!");
                WinAPI.DaemonProcess(_params["process"]);
            });

            Console.ReadKey();
        }
    }
}
