/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Logger
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;
using System.Text;
using System.Timers;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LoggerService
{
    /// <summary>
    /// Class for logger that implements ILogger interface
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// App name
        /// </summary>
        private readonly string _appName;
        
        /// <summary>
        /// Log storage
        /// </summary>
        private readonly ConcurrentDictionary<DateTime?, LogInfo> _logs;

        /// <summary>
        /// Paths
        /// </summary>
        private readonly List<string> _paths;

        /// <summary>
        /// Intial path
        /// </summary>
        private string _initialPath;

        /// <summary>
        /// Timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Background worker
        /// </summary>
        private BackgroundWorker _writer;

        /// <summary>
        /// Path
        /// </summary>
        private string _path;

        /// <summary>
        /// Storing interval
        /// </summary>
        private int _storingInterval;

        /// <summary>
        /// Boolean value indicating if Logger stores
        /// </summary>
        private bool _isStoring;

        /// <summary>
        /// File maximum size
        /// </summary>
        private long _fileMaxSize;

        /// <summary>
        /// File count
        /// </summary>
        private int _fileCount;

        /// <summary>
        /// Gets app name
        /// </summary>
        public string AppName => this._appName;

        /// <summary>
        /// Gets file count
        /// </summary>
        public int FileCount => this._fileCount;

        /// <summary>
        /// Gets or sets file maximum size
        /// </summary>
        public long FileMaxSize
        {
            get => this._fileMaxSize;

            set => this._fileMaxSize = value;
        }

        /// <summary>
        /// Gets or sets path
        /// </summary>
        public string Path
        {
            get => this._path;

            set => this._path = value;
        }

        /// <summary>
        /// Gets or sets storing interval
        /// </summary>
        public int StoringInterval
        {
            get => this._storingInterval;

            set => this.InitializeTimer(value);
        }

        /// <summary>
        /// Gets or sets boolean value _isStoring which indicates
        /// wheter the Logger stores
        /// </summary>
        public bool IsStoring
        {
            get
            {
                return this._isStoring;
            }
            set
            {
                if (this._isStoring == value)
                    return;

                this._isStoring = value;

                if (this._isStoring == true)
                    this._timer.Start();
                else this._timer.Stop();
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="Logger"/>
        /// </summary>
        /// <param name="appName">App name</param>
        /// <param name="path">Path</param>
        /// <param name="storingInterval">Storing interval</param>
        public Logger(string appName, string path, int storingInterval = 60)
        {
            this._appName = appName;
            this._path = path;
            this._initialPath = path;
            this._storingInterval = storingInterval;
            this._isStoring = true;
            this._fileMaxSize = 0x4000000;

            this._logs = new ConcurrentDictionary<DateTime?, LogInfo>();
            this._paths = new List<string>
            {
                this._path
            };
            
            this.InitializeTimer(storingInterval);
            this.InitializeWriter();            
        }

        /// <summary>
        /// Logs log information
        /// </summary>
        /// <param name="logInfo">log information</param>
        public void Log(LogInfo logInfo)
        {
            var log = default(LogInfo);

            if (logInfo == null)
            {
                log = new LogInfo
                {
                    Time = DateTime.Now,
                    LogType = LogType.Default,
                    Message = "Unknown log"
                };
            }
            else log = logInfo;
            
            this._logs.TryAdd(log.Time, log);
        }

        /// <summary>
        /// Logs log information
        /// </summary>
        /// <param name="logInfo">log information</param>
        public void Log(string logInfo)
        {
            this.Log(this.ConstructLog(logInfo));
        }

        /// <summary>
        /// Clears logs cache
        /// </summary>
        public void ClearCache()
        {
            this._logs.Clear();
        }

        /// <summary>
        /// Clears logs
        /// </summary>
        public void Clear()
        {
            this.ClearCache();

            foreach (var path in this._paths)
            {
                File.Delete(path);
            }

            this._paths.Clear();

            this._fileCount = 0;
            this._path = this._initialPath;
        }

        /// <summary>
        /// Disposes logger
        /// </summary>
        public void Dispose()
        {
            this._timer.Dispose();
            this._writer.Dispose();
        }

        /// <summary>
        /// Initializes timer
        /// </summary>
        /// <param name="interval">interval</param>
        private void InitializeTimer(int interval)
        {
            if (interval < 60)
                throw new ArgumentException("Interval");

            this._storingInterval = interval;

            if (this._timer != null)
                this._timer.Elapsed -= this.ExecWhenTimerElapses;

            this._timer = new Timer(this._storingInterval * 60000);
            this._timer.Elapsed += this.ExecWhenTimerElapses;

            this._timer.Start();
        }

        /// <summary>
        /// Initializes writer
        /// </summary>
        private void InitializeWriter()
        {
            this._writer = new BackgroundWorker();
            this._writer.DoWork += Write;
        }

        /// <summary>
        /// Handler for writer
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">event argument</param>
        private void Write(object sender, DoWorkEventArgs e)
        {
            this.CheckFileSize();

            using (var stream = File.AppendText(this._path))
            {
                foreach (var log in this._logs)
                {
                    stream.WriteLine(this.GetLogAsLine(log.Value));
                }
            }

            this.ClearCache();
        }

        /// <summary>
        /// Timer elapsed handler
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event argument</param>
        private void ExecWhenTimerElapses(object sender, ElapsedEventArgs e)
        {
           if (!this._writer.IsBusy)
                this._writer.RunWorkerAsync();
        }

        /// <summary>
        /// Converts log information to string
        /// </summary>
        /// <param name="logInfo">log information</param>
        /// <returns>log as string</returns>
        private string GetLogAsLine(LogInfo logInfo)
        {          
            var time = logInfo.Time ?? DateTime.Now;
            var logType = logInfo.LogType ?? LogType.Default;
            var message = logInfo.Message ?? "";
            var exception = logInfo.Exception == null ? "" : logInfo.Exception.ToString();

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{time}    ");
            stringBuilder.Append($"{logType}    ");
            stringBuilder.Append($"{message}    ");
            stringBuilder.Append($"{exception}");

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Constructs log from string representation of log
        /// </summary>
        /// <param name="log">log info</param>
        /// <returns>log info</returns>
        private LogInfo ConstructLog(string log)
        {
            return new LogInfo
            {
                Time = DateTime.Now,
                LogType = LogType.Default,
                Message = log
            };
        }

        /// <summary>
        /// Checks file size
        /// </summary>
        private void CheckFileSize()
        {
            try
            {
                var fileInfo = new FileInfo(this._path);
                var fileSize = fileInfo.Length;

                if (fileSize > this._fileMaxSize)
                {
                    this._fileCount++;

                    var fileName = System.IO.Path.GetFileNameWithoutExtension(this._path);
                    var newPath = $"{fileName}_{this._fileCount}.{fileInfo.Extension}";

                    this._paths.Add(newPath);
                    this._path = newPath;
                }
            }
            catch
            {
                Debug.WriteLine("FileSizeCheckError");
            }
        }
    }
}