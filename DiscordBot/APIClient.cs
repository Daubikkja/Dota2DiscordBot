//This is used to pull data from the steams API and return a string with match statistics to the caller as a big string
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class APIClient
    {




        public static string GetData(UInt32 matchID)
        {
            var steamKey = "STEAM API KEY HERE"; 
            //steamAPI URL
            var url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id=" + matchID + "&key=" + steamKey;

            // Console.WriteLine(url);
            var synClient = new WebClient();

            var content = synClient.DownloadString(url);

            //// Create the JSon serialzer and parse the respone
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MatchData));


            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
            {
                var matchData = (MatchData)serializer.ReadObject(ms);
                //Console.WriteLine("Trying to get data");
                //Console.WriteLine(matchData.result.radiant_win);
                string Winner;
                string returnData="```"; //returnData is the string that will be returned at the end of this class
                //It will be one big continously string that we add information to, due to it being easier and less error prone to send it as one string to discord.


                //Match Result
                if (matchData.result.radiant_win == true)
                {
                    Winner = "**Radiant Victory**";
                }
                else
                {
                    //Console.WriteLine("Dire Victory");
                    //GlobalVar.MyGlobals.GWinner = "Dire Victory";
                    Winner = "**Dire Victory**";
                }
                returnData += Winner+"\0\n";

                //Getting duration. Duration come from the dota API as seconds. and since 2745s isn't as readable as 45:45 (mm:ss) we change it
                double dDurationM =(matchData.result.duration* 0.0167);
                int iDurationM = (int)dDurationM;
                int iDurationS = Convert.ToInt16(60*((matchData.result.duration * 0.0166667) % matchData.result.duration)/100);
                returnData += "Duration: " + iDurationM + ":" + iDurationS;
                returnData += "\n\0" + "---------" + "\n\0"; //space making, to make the end result readable and "cleaner"

                //Player data
                returnData += "**Radiant Side** " +"\n\0"; //side
                returnData += "Heroes  | K/D/A | GPM | XPM" + "\n\0"; //starting Table
                returnData += "-------- " + "\n\0"; //divison between table header and content

                for (int i = 0; i < matchData.result.players.Count; i++)
                {
                    returnData += GetHeroName(matchData.result.players[i].hero_id) +  " | " + matchData.result.players[i].kills + " / " + matchData.result.players[i].deaths + " / " 
                        + matchData.result.players[i].assists + " | " + matchData.result.players[i].gold_per_min +
                        " | " +  matchData.result.players[i].xp_per_min + "\n\0";
                    if(i==4)
                        returnData += "\n\0" + "**Dire Side** " + "\n\0"; //after we have gone through the first 5 heroes (out of 10), we add on to the string the other side
                }


                returnData += "https://www.opendota.com/matches/" + matchID + "\n\0"; //adding a opendota link 
                returnData += "https://www.dotabuff.com/matches/" + matchID + "\n\0"; //adding dotabuff link
                returnData += "```"; //This is the markup ender in discord, to say we are done with this "block" of markup
                return returnData;
            }





        }



        public static string GetHeroName (int heroId) //Getting hero names from the hero ID
        {
            //string starts as an error in case th program doesn't find the heroId
            string HeroName = "Error";
            //Big switch loop with id -> hero names
            switch (heroId)
            {
                case 1:
                    HeroName = "Anti-Mage";
                    break;
                case 2:
                    HeroName = "Axe";
                    break;
                case 3:
                    HeroName = "Bane";
                    break;
                case 4:
                    HeroName = "Bloodseeker";
                    break;
                case 5:
                    HeroName = "Crystal Maiden";
                    break;
                case 6:
                    HeroName = "Drow Ranger";
                    break;
                case 7:
                    HeroName = "Earthshaker";
                    break;
                case 8:
                    HeroName = "Juggernaut";
                    break;
                case 9:
                    HeroName = "Mirana";
                    break;
                case 10:
                    HeroName = "Morphling";
                    break;
                case 11:
                    HeroName = "Shadow Fiend";
                    break;
                case 12:
                    HeroName = "Phantom Lancer";
                    break;
                case 13:
                    HeroName = "Puck";
                    break;
                case 14:
                    HeroName = "Pudge";
                    break;
                case 15:
                    HeroName = "Razor";
                    break;
                case 16:
                    HeroName = "Sand King";
                    break;
                case 17:
                    HeroName = "Storm Spirit";
                    break;
                case 18:
                    HeroName = "Sven";
                    break;
                case 19:
                    HeroName = "Tiny";
                    break;
                case 20:
                    HeroName = "Vengeful Spirit";
                    break;
                case 21:
                    HeroName = "Windranger";
                    break;
                case 22:
                    HeroName = "Zeus";
                    break;
                case 23:
                    HeroName = "Kunkka";
                    break;
                case 25:
                    HeroName = "Lina";
                    break;
                case 26:
                    HeroName = "Lion";
                    break;
                case 27:
                    HeroName = "Shadow Shaman";
                    break;
                case 28:
                    HeroName = "Slardar";
                    break;
                case 29:
                    HeroName = "Tidehunter";
                    break;
                case 30:
                    HeroName = "Witch Doctor";
                    break;
                case 31:
                    HeroName = "Lich";
                    break;
                case 32:
                    HeroName = "Riki";
                    break;
                case 33:
                    HeroName = "Enigma";
                    break;
                case 34:
                    HeroName = "Tinker";
                    break;
                case 35:
                    HeroName = "Sniper";
                    break;
                case 36:
                    HeroName = "Necrophos";
                    break;
                case 37:
                    HeroName = "Warlock";
                    break;
                case 38:
                    HeroName = "Beastmaster";
                    break;
                case 39:
                    HeroName = "Queen of Pain";
                    break;
                case 40:
                    HeroName = "Venomancer";
                    break;
                case 41:
                    HeroName = "Faceless Void";
                    break;
                case 42:
                    HeroName = "Wraith King";
                    break;
                case 43:
                    HeroName = "Death Prophet";
                    break;
                case 44:
                    HeroName = "Phantom Assassin";
                    break;
                case 45:
                    HeroName = "Pugna";
                    break;
                case 46:
                    HeroName = "Templar Assassin";
                    break;
                case 47:
                    HeroName = "Viper";
                    break;
                case 48:
                    HeroName = "Luna";
                    break;
                case 49:
                    HeroName = "Dragon Knight";
                    break;
                case 50:
                    HeroName = "Dazzle";
                    break;
                case 51:
                    HeroName = "Clockwerk";
                    break;
                case 52:
                    HeroName = "Leshrac";
                    break;
                case 53:
                    HeroName = "Nature's Prophet";
                    break;
                case 54:
                    HeroName = "Lifestealer";
                    break;
                case 55:
                    HeroName = "Dark Seer";
                    break;
                case 56:
                    HeroName = "Clinkz";
                    break;
                case 57:
                    HeroName = "Omniknight";
                    break;
                case 58:
                    HeroName = "Enchantress";
                    break;
                case 59:
                    HeroName = "Huskar";
                    break;
                case 60:
                    HeroName = "Night Stalker";
                    break;
                case 61:
                    HeroName = "Broodmother";
                    break;
                case 62:
                    HeroName = "Bounty Hunter";
                    break;
                case 63:
                    HeroName = "Weaver";
                    break;
                case 64:
                    HeroName = "Jakiro";
                    break;
                case 65:
                    HeroName = "Batrider";
                    break;
                case 66:
                    HeroName = "Chen";
                    break;
                case 67:
                    HeroName = "Spectre";
                    break;
                case 68:
                    HeroName = "Ancient Apparition";
                    break;
                case 69:
                    HeroName = "Doom";
                    break;
                case 70:
                    HeroName = "Ursa";
                    break;
                case 71:
                    HeroName = "Spirit Breaker";
                    break;
                case 72:
                    HeroName = "Gyrocopter";
                    break;
                case 73:
                    HeroName = "Alchemist";
                    break;
                case 74:
                    HeroName = "Invoker";
                    break;
                case 75:
                    HeroName = "Silencer";
                    break;
                case 76:
                    HeroName = "Outworld Devourer";
                    break;
                case 77:
                    HeroName = "Lycan";
                    break;
                case 78:
                    HeroName = "Brewmaster";
                    break;
                case 79:
                    HeroName = "Shadow Demon";
                    break;
                case 80:
                    HeroName = "Lone Druid";
                    break;
                case 81:
                    HeroName = "Chaos Knight";
                    break;
                case 82:
                    HeroName = "Meepo";
                    break;
                case 83:
                    HeroName = "Treant Protector";
                    break;
                case 84:
                    HeroName = "Ogre Magi";
                    break;
                case 85:
                    HeroName = "Undying";
                    break;
                case 86:
                    HeroName = "Rubick";
                    break;
                case 87:
                    HeroName = "Disruptor";
                    break;
                case 88:
                    HeroName = "Nyx Assassin";
                    break;
                case 89:
                    HeroName = "Naga Siren";
                    break;
                case 90:
                    HeroName = "Keeper of the Light";
                    break;
                case 91:
                    HeroName = "Io";
                    break;
                case 92:
                    HeroName = "Visage";
                    break;
                case 93:
                    HeroName = "Slark";
                    break;
                case 94:
                    HeroName = "Medusa";
                    break;
                case 95:
                    HeroName = "Troll Warlord";
                    break;
                case 96:
                    HeroName = "Centaur Warrunner";
                    break;
                case 97:
                    HeroName = "Magnus";
                    break;
                case 98:
                    HeroName = "Timbersaw";
                    break;
                case 99:
                    HeroName = "Bristleback";
                    break;
                case 100:
                    HeroName = "Tusk";
                    break;
                case 101:
                    HeroName = "Skywrath Mage";
                    break;
                case 102:
                    HeroName = "Abaddon";
                    break;
                case 103:
                    HeroName = "Elder Titan";
                    break;
                case 104:
                    HeroName = "Legion Commander";
                    break;
                case 105:
                    HeroName = "Techies";
                    break;
                case 106:
                    HeroName = "Ember Spirit";
                    break;
                case 107:
                    HeroName = "Earth Spirit";
                    break;
                case 108:
                    HeroName = "Underlord";
                    break;
                case 109:
                    HeroName = "Terrorblade";
                    break;
                case 110:
                    HeroName = "Phoenix";
                    break;
                case 111:
                    HeroName = "Oracle";
                    break;
                case 112:
                    HeroName = "Winter Wyvern";
                    break;
                case 113:
                    HeroName = "Arc Warden";
                    break;
                case 114:
                    HeroName = "Monkey King";
                    break;
                case 119:
                    HeroName = "Dark Willow";
                    break;
                case 120:
                    HeroName = "Pangolier";
                    break;


                default:
                    HeroName = "Error";
                    break;
            }


            return HeroName;
        }
    }



}
