//
// Copyright 2019 Dynatrace LLC
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
//

namespace Dynatrace.OneAgent.Sdk.Api
{
    /// <summary>
    /// The logging callback gets called when errors/warnings occur from OneAgent SDK API calls.
    /// Never call any SDK API, when inside one of these callback methods.
    /// The log messages are primarily intended as a development and debugging aid and are subject to change,
    /// please do not try to parse them or assert on them.
    /// </summary>
    public interface ILoggingCallback
    {
        /// <summary>
        /// Just a warning. Something is missing, but SDK is working normal.
        /// </summary>
        /// <param name="message">Warning message text. Never null</param>
        void Warn(string message);

        /// <summary>
        /// Something that should be done can't be done (e. g. path couldn't be started).
        /// </summary>
        /// <param name="message">Error message text. Never null</param>
        void Error(string message);
    }
}
