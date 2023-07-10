using CommandSystem;
using Mirror;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterSL.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CustomPrimitiveObjectCommand : ICommand
    {
        //spawncustomobject <objectType> <scale x,y,z> <addCollider>
        public string Command => "spawncustomobject";
        public string[] Aliases => new string[1] { "sco" };
        public string Description => "Spawn a custom object";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);
            string[] args = arguments.Array;
            if(args.Length < 4)
            {
                response = null;
                player.SendConsoleMessage("Invalid arguments! Args: spawncustomobject <objectType> <scale x,y,z> <addCollider>", "red");
                player.SendConsoleMessage("objectTypes: \r\n Sphere,\r\n Capsule,\r\n Cylinder,\r\n Cube,\r\n Plane,\r\n Quad", "red");
            }
            PrimitiveType primitiveType = PrimitiveType.Sphere;
            try
            {
                PrimitiveType myEnum = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), args[1]);
            }
            catch (Exception ex)
            {
                player.SendConsoleMessage("Can't spawn object, Bad type!");
                player.SendConsoleMessage(ex.Message, "red");
            }
            GameObject obj = GameObject.CreatePrimitive(primitiveType);
            obj.transform.position = player.Position;
            NetworkIdentity netId = obj.AddComponent<NetworkIdentity>();
            NetworkTransform netTransform =  obj.AddComponent<NetworkTransform>();
            netTransform.syncDirection = SyncDirection.ServerToClient;
            netTransform.syncScale = true;
            obj.name = netId.netId.ToString();
            string scale = args[2];
            float[] scaleInts = { 0, 0, 0 };
            int counter = 0;
            foreach(string s in scale.Split(','))
            {
                if(!float.TryParse(s, out float i))
                {
                    player.SendConsoleMessage($"Scale parse error: '{s}' isn't a valid number.");
                }
                else
                {
                    scaleInts[counter] = i;
                }
                counter++;
            }
            Vector3 newScale = new Vector3(scaleInts[0], scaleInts[1], scaleInts[2]);
            obj.transform.localScale = newScale;
            MeshCollider col = obj.AddComponent<MeshCollider>();
            if (!bool.TryParse(args[3], out bool b))
            {
                col.enabled = false;
                player.SendConsoleMessage("Failed to parse collider bool. Assuming false.");
            }
            else
            {
                col.enabled |= b;
            }
            NetworkServer.Spawn(obj);
            player.SendConsoleMessage("Done!");
            response = null;
            return true;
        }
    }
}
