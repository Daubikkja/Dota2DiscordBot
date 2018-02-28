using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Dota : ModuleBase<SocketCommandContext>
    {
        [Command("dota")]
        public async Task PingAsync(uint matchId)
        {
            //await ReplyAsync("https://www.opendota.com/matches/" + matchId); //test code to test purely the "!dota" command, without using the whole steamAPI
            await ReplyAsync(DiscordBot.APIClient.GetData(matchId));

        }

    }


}
