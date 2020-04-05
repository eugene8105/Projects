using SlackBotMessages;
using SlackBotMessages.Models;
using System;

namespace NtsOnSlack
{
    class SetUp
    {
        /// <summary>
        /// need to set up a webhook url on Slack account - to send from and send to
        /// This is a link how to set up a webhook url - https://kb.itglue.com/hc/en-us/articles/228469048-Setting-up-Slack-webhook-notifications
        /// </summary>
        int num = 0;
        string WebHookUrl = "https://hooks.slack.com/services/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";

        /// <summary>
        ///     A simple example of a message which looks like it has been send by an alien
        /// </summary>

        public SetUp()
        {
            var client = new SbmClient(WebHookUrl);
            num = 1;
            var message = new Message
            {
                Username = "User",
                Text = $"Need support. Test {num}",
                IconEmoji = Emoji.AlarmClock,
            };

            //or send it fully async like this:
            //await client.SendAsync(mesdsage).ConfigureAwait(false);
            Console.ReadKey();

            client.Send(message);

        } // end of SetUp

        public void SecondSetUp()
        {
            /// <summary>
            ///     A simple message which has a custom username and image for the user,
            ///     which looks like it has been sent by Paul Seal
            /// </summary>

            var client = new SbmClient(WebHookUrl);
            num = 2;
            var message = new Message
            {
                Username = "User Name",
                Text = $"Need support. Test {num}",
                IconUrl = "Picture location"
            };

            client.Send(message);
            //or send it fully async like this:
            //await client.SendAsync(message).ConfigureAwait(false);

        } // end of SecondSetUp

        public void MultipleAttachmentsTest_1()
        {
            // example - https://github.com/prjseal/SlackBotMessages
        }

    } // end of SetUp
}
