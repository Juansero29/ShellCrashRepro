using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmatiKreations.Framework.Services.Alerts
{
    /// <summary>
    /// An interface defininng a simple service that can display alerts to the user
    /// </summary>
    public interface IAlertsService
    {
        /// <summary>
        /// Shows a 'feature not implemented' alert to the user. Can be awaited.
        /// </summary>
        /// <param name="messageToShow">Specify a message explaining the reason why the feature isn't available</param>
        Task ShowFeatureNotImplementedAlert(string messageToShow = null);

        /// <summary>
        /// Displays a simple alert to the user. Can be awaited.
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message presented to the user</param>
        /// <param name="cancelMessage">The message for the user to validate that he has read the alert</param>
        /// <returns>An empty task</returns>
        Task DisplayAlert(string title, string message, string cancelMessage = null);

        /// <summary>
        /// Displays an alert to the user for making a choice. Can be awaited.
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message presented to the user</param>
        /// <param name="acceptMessage">The message that prompts the user to accept</param>
        /// <param name="refuseMessage">The message that prompts the user to refuse</param>
        /// <returns>
        /// A task, containing a boolean indicating wether the response was positive (true = click on the accept message) or negative (false = click on the refuse message)
        /// </returns>
        Task<bool> DisplayAlertForResult(string title, string message, string acceptMessage, string refuseMessage);
    }
}
