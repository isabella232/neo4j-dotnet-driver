﻿// Copyright (c) 2002-2019 "Neo4j,"
// Neo4j Sweden AB [http://neo4j.com]
// 
// This file is part of Neo4j.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;

namespace Neo4j.Driver
{
    /// <summary>
    /// The logging levels that could be used by a logger.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Set the log level to <see cref="None"/> to avoid any logging from the driver.
        /// </summary>
        None,
        /// <summary>
        /// Set the log level to <see cref="Error"/> to only see errors logged by the driver.
        /// </summary>
        Error,
        /// <summary>
        /// Set the log level to <see cref="Info"/> to see all errors and information logged by the driver.
        /// </summary>
        Info,
        /// <summary>
        /// When setting log level to <see cref="Debug"/>, besides all logging recorded by <see cref="Error"/> and <see cref="Info"/>,
        /// all bolt messages sent between driver and server are also logged in plain text.
        /// </summary>
        Debug,
        /// <summary>
        /// When setting log level to <see cref="Trace"/>, besides all logging recorded by <see cref="Error"/>, <see cref="Info"/> and <see cref="Debug"/>,
        /// all bolt messages in hex sent between driver and server are also logged in plain text.
        /// </summary>
        Trace
    }

    /// <summary>
    /// The logger used by this driver. 
    /// This interface is expected to be implemented by users to access the logs generated by the driver.
    /// </summary>
    /// <remarks>
    /// Set the logger that you want to use via <see cref="Config"/>.
    /// If no logger is explicitly set, then a default debug logger would be used <see cref="Config.Default"/></remarks>
    public interface ILogger 
    {
        /// <summary>Log a message at <see cref="LogLevel.Error"/> level.</summary>
        /// <param name="message">The error message.</param>
        /// <param name="cause">The cause of the error.</param>
        /// <param name="restOfMessage">Any <paramref name="restOfMessage"/> parts of the message.</param>
        void Error(string message, Exception cause = null, params object[] restOfMessage);

        /// <summary>Log a message at <see cref="LogLevel.Info"/> level.</summary>
        /// <param name="message">The message.</param>
        /// <param name="restOfMessage">Any <paramref name="restOfMessage"/> parts of the message.</param>
        void Info(string message, params object[] restOfMessage);

        /// <summary>Log a message at <see cref="LogLevel.Debug"/> level.</summary>
        /// <param name="message">The message.</param>
        /// <param name="restOfMessage">Any <paramref name="restOfMessage"/> parts of the message, including (but not limited to) Sent Messages and Received Messages.</param>
        void Debug(string message, params object[] restOfMessage);

        /// <summary>Log a message at <see cref="LogLevel.Trace"/> level</summary>
        /// <param name="message">The message.</param>
        /// <param name="restOfMessage">
        /// Any <paramref name="restOfMessage"/> parts of the message, 
        /// including (but not limited to) byte buffer to send/receive data over the connection, 
        /// offset in byte buffer, the length of bytes in the buffer.
        /// </param>
        void Trace(string message, params object[] restOfMessage);

        /// <summary>
        /// Gets and sets the level of this <see cref="ILogger"/>
        /// </summary>
        LogLevel Level { get; set; }
    }
}
